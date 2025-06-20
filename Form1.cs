using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Threading;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.ComponentModel;


namespace FoxholeSupplyCalculator
{

    public partial class Form1 : Form
    {
        private List<Item> itemDatabase = new List<Item>();
        private List<SupplyEntry> supplyEntries = new List<SupplyEntry>();
        private DataGridViewRow? selectedRow = null;
        private string? quotaText;
        private string quotaFilePath;
        private int clickedColumnIndex = -1; // добавь это поле в Form1, если его еще нет
        private int subgroupCounter = 0;
        private List<TextBox> subgroupTextBoxes = new List<TextBox>();
        private bool isUpdatingPlaceholders = false;
        private List<Item> currentItems = new();
        private bool isDarkMode = false;


        public Form1()
        {
            InitializeComponent();
            LoadItemDatabase();
            btnShowItems_Click(null, null);
            AllowDrop = true;
            DragEnter += new DragEventHandler(Form1_DragEnter);
            DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        private void btnApplySubgroupCount_Click(object sender, EventArgs e)
        {
            int targetCount = (int)numericSubgroupCount.Value;

            while (subgroupTextBoxes.Count < targetCount)
            {
                btnAddSubgroup_Click(null, null);
            }

            while (subgroupTextBoxes.Count > targetCount)
            {
                btnRemoveSubgroup_Click(null, null);
            }

            RecalculatePlaceholders();
        }

        private ColorScheme GetLightScheme()
        {
            return new ColorScheme
            {
                FormBG = Color.White,
                FlowLayoutPanelBG = Color.White,
                FlowLayoutPanelFG = Color.Black,
                ButtonBG = Color.Gainsboro,
                ButtonFG = Color.Black,
                TextBoxBG = Color.White,
                TextBoxFG = Color.Black,
                TabControlBG = Color.White,
                TabControlFG = Color.Black,
                TabPageBG = Color.White,
                TabPageFG = Color.Black,
                dataGridQuotaViewBG = Color.White,
                dataGridQuotaViewFG = Color.Black
            };
        }

        private ColorScheme GetDarkScheme()
        {
            return new ColorScheme
            {
                FormBG = Color.FromArgb(20, 20, 20),
                FlowLayoutPanelBG = Color.FromArgb(30, 30, 30),
                FlowLayoutPanelFG = Color.White,
                ButtonBG = Color.FromArgb(45, 45, 45),
                ButtonFG = Color.White,
                TextBoxBG = Color.FromArgb(40, 40, 40),
                TextBoxFG = Color.White,
                TabControlBG = Color.FromArgb(30, 30, 30),
                TabControlFG = Color.White,
                TabPageBG = Color.FromArgb(35, 35, 35),
                TabPageFG = Color.White,
                dataGridQuotaViewBG = Color.FromArgb(35, 35, 35),
                dataGridQuotaViewFG = Color.White,

            };
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            try
            {
                if (!string.IsNullOrEmpty(quotaText))
                {
                    DialogResult result = MessageBox.Show(
                                        $"Таблица с квотой уже содержит данные. Заменить содержимое новой квотой из ${files[0]}?",
                                        "Предупреждение",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.Yes)
                    {
                        quotaFilePath = files[0];
                        quotaText = File.ReadAllText(files[0]);

                        LoadQuotaFromText(quotaText);
                        txtQuotaInput.Text = txtQuotaInput.Text = quotaText.Replace("\n", "\r\n"); ; ;
                        // Очистка старых данных
                        MessageBox.Show("Файл успешно загружен и квота отображена.");
                    }
                }
                else
                {
                    quotaFilePath = files[0];
                    quotaText = File.ReadAllText(files[0]);

                    LoadQuotaFromText(quotaText);
                    txtQuotaInput.Text = quotaText.Replace("\n", "\r\n"); ;
                    // Очистка старых данных
                    MessageBox.Show("Файл успешно загружен и квота отображена.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }

        private void lstResults_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = lstResults.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    lstResults.SelectedIndex = index; // выделяем кликнутый элемент
                }
            }
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;

            ColorScheme scheme = isDarkMode ? GetDarkScheme() : GetLightScheme();
            ChangeTheme(scheme, this.Controls);
        }



        public void ChangeTheme(ColorScheme scheme, Control.ControlCollection container)
        {
            foreach (Control component in container)
            {
                if (component.HasChildren)
                    ChangeTheme(scheme, component.Controls); // рекурсивно

                // component.BackColor = Color.Black;
                // component.ForeColor = Color.White;
                if (component is FlowLayoutPanel)
                {
                    component.BackColor = scheme.FlowLayoutPanelBG;
                    component.ForeColor = scheme.FlowLayoutPanelFG;
                }
                else if (component is Button)
                {
                    component.BackColor = scheme.ButtonBG;
                    component.ForeColor = scheme.ButtonFG;
                }
                else if (component is TextBox)
                {
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                }
                else if (component is TabPage)
                {
                    component.BackColor = scheme.dataGridQuotaViewBG;
                    component.ForeColor = scheme.dataGridQuotaViewFG;
                }
                else if (component is DataGridView)
                {
                    component.BackColor = scheme.TabPageBG;
                    component.ForeColor = scheme.TabPageFG;
                }
                else if (component is TabControl tabControl)
                {
                    if (isDarkMode)
                    {
                        tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
                        tabControl.DrawItem -= DrawDarkTabHeader;
                        tabControl.DrawItem += DrawDarkTabHeader;
                        btnDarkMode.Image = Image.FromFile("sun-icon.png");
                    }
                    else
                    {
                        tabControl.DrawMode = TabDrawMode.Normal;
                        tabControl.DrawItem -= DrawDarkTabHeader;
                        btnDarkMode.Image = Image.FromFile("moon-icon.png");
                    }

                    tabControl.BackColor = scheme.TabControlBG;
                    tabControl.ForeColor = scheme.TabControlFG;
                }

                else if (component is Label || component is CheckBox || component is ComboBox || component is ListBox)
                {
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                }
            }

            this.BackColor = scheme.FormBG;
        }

        private void DrawDarkTabHeader(object sender, DrawItemEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabPage = tabControl?.TabPages[e.Index];
            var bounds = e.Bounds;

            using (SolidBrush backBrush = new SolidBrush(Color.FromArgb(40, 40, 40)))
            using (SolidBrush foreBrush = new SolidBrush(Color.White))
            using (Font font = new Font("Segoe UI", 9, FontStyle.Bold))
            {
                e.Graphics.FillRectangle(backBrush, bounds);
                e.Graphics.DrawString(tabPage.Text, font, foreBrush, bounds.X + 6, bounds.Y + 4);
            }
        }

        private void copyResultsFromClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstResults.Items.Count != 0)
                {
                    Clipboard.SetText(string.Join(Environment.NewLine, lstResults.Items.OfType<string>()));
                }
                else
                {
                    MessageBox.Show("Список результатов пустой");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: ", ex.Message);
            }
        }

        private void MoveItem_Click(object sender, EventArgs e)
        {
            if (lstResults.SelectedItem == null) return;

            string selectedText = lstResults.SelectedItem.ToString();

            // Проверка, что это предмет (а не строка заголовка типа "Подгруппа X:")
            if (!selectedText.Contains("ящ.")) return;

            // Получение текущей подгруппы
            int currentSubgroup = -1;
            for (int i = lstResults.SelectedIndex - 1; i >= 0; i--)
            {
                string line = lstResults.Items[i].ToString();
                if (line.StartsWith("Подгруппа"))
                {
                    if (int.TryParse(line.Split(' ')[1].TrimEnd(':'), out int parsed))
                    {
                        currentSubgroup = parsed - 1; // индекс с нуля
                        break;
                    }
                }
            }

            if (currentSubgroup < 0)
            {
                MessageBox.Show("Не удалось определить подгруппу.");
                return;
            }

            // Извлекаем название предмета и количество ящиков
            string lineText = selectedText.Trim();
            string itemName;
            int qty;

            // Пример: "   Винтовка — 4 ящ."
            var match = Regex.Match(lineText, @"(.+?)—\s*(\d+)\s*ящ");
            if (match.Success)
            {
                itemName = match.Groups[1].Value.Trim();
                qty = int.Parse(match.Groups[2].Value);
            }
            else
            {
                MessageBox.Show("Не удалось распознать предмет или количество.");
                return;
            }

            if (qty <= 0)
            {
                MessageBox.Show("Невозможно переместить этот предмет.");
                return;
            }

            MessageBox.Show(itemName);

            // Открываем форму перемещения
            using (var moveForm = new MoveItemForm(subgroupTextBoxes.Count, qty))
            {
                if (moveForm.ShowDialog() == DialogResult.OK)
                {
                    int targetSubgroup = moveForm.SelectedSubgroupIndex;
                    int amountToMove = moveForm.QuantityToMove;

                    if (targetSubgroup == currentSubgroup)
                    {
                        MessageBox.Show("Нельзя переместить предмет в ту же подгруппу.");
                        return;
                    }

                    int selectedIndex = lstResults.SelectedIndex; // Сохраняем индекс до удаления

                    // Удаляем старую запись
                    lstResults.Items.RemoveAt(selectedIndex);

                    // Обновляем старую подгруппу, вычитая перемещённое количество
                    if (qty - amountToMove > 0)
                    {
                        string updatedOld = $"   {itemName} — {qty - amountToMove} ящ.";
                        lstResults.Items.Insert(selectedIndex, updatedOld);
                    }

                    // Находим позицию подгруппы назначения
                    int insertIndex = -1;
                    for (int i = 0; i < lstResults.Items.Count; i++)
                    {
                        string text = lstResults.Items[i].ToString();

                        if (text.StartsWith($"Подгруппа {targetSubgroup + 1}"))
                        {
                            insertIndex = i + 1;
                            break;
                        }
                    }
                    MessageBox.Show($"insertIndex: {insertIndex}");

                    if (insertIndex != -1)
                    {
                        lstResults.Items.Insert(insertIndex, $"   {itemName} — {amountToMove} ящ. (перемещено)");
                    }
                    else
                    {
                        MessageBox.Show("Целевая подгруппа не найдена.");
                    }
                }

            }
        }



        private void btnAddSubgroup_Click(object sender, EventArgs e)
        {
            subgroupCounter++;

            var subgroupPanel = new Panel
            {
                Height = 28,
                Width = panelSubgroups.ClientSize.Width - 25,
                Margin = new Padding(1),
                BackColor = Color.Transparent
            };

            var label = new Label
            {
                Text = $"Подгруппа {subgroupCounter}",
                AutoSize = true,
                Location = new Point(5, 4)
            };

            var textBox = new TextBox
            {
                Width = 60,
                Height = 25,
                Location = new Point(label.Width + 10, 3), // Справа от панели
                PlaceholderText = "25%", // Пример плейсхолдера
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            textBox.TextChanged += OnSubgroupTextChanged;

            // Добавляем контролы в панель и на форму
            subgroupPanel.Controls.Add(label);
            subgroupPanel.Controls.Add(textBox);
            panelSubgroups.Controls.Add(subgroupPanel);

            subgroupTextBoxes.Add(textBox);

            RecalculatePlaceholders(); // функция автозаполнения
        }

        private void OnSubgroupTextChanged(object sender, EventArgs e)
        {
            if (isUpdatingPlaceholders) return;
            isUpdatingPlaceholders = true;

            var manualInputs = new Dictionary<TextBox, int>();
            var remainingBoxes = new List<TextBox>();

            // Собираем кто что ввёл
            foreach (var tb in subgroupTextBoxes)
            {
                if (int.TryParse(tb.Text, out int value) && value >= 0 && value <= 100)
                {
                    manualInputs[tb] = value;
                }
                else
                {
                    remainingBoxes.Add(tb);
                }
            }

            int totalManual = manualInputs.Values.Sum();
            int remainingPercent = 100 - totalManual;
            int countRemaining = remainingBoxes.Count;

            if (countRemaining > 0)
            {
                double evenShare = (double)remainingPercent / countRemaining;
                List<int> intValues = new List<int>();
                double sumRounded = 0;

                for (int i = 0; i < countRemaining; i++)
                {
                    int rounded = (int)Math.Floor(evenShare);
                    intValues.Add(rounded);
                    sumRounded += rounded;
                }

                int missing = remainingPercent - (int)sumRounded;
                for (int i = 0; i < countRemaining && missing > 0; i++)
                {
                    intValues[i]++;
                    missing--;
                }

                for (int i = 0; i < countRemaining; i++)
                {
                    remainingBoxes[i].PlaceholderText = intValues[i] + " %";
                }
            }


            isUpdatingPlaceholders = false;
        }



        private void btnRemoveSubgroup_Click(object sender, EventArgs e)
        {
            if (panelSubgroups.Controls.Count > 0)
            {
                Control last = panelSubgroups.Controls[panelSubgroups.Controls.Count - 1];

                if (last.Controls.OfType<TextBox>().FirstOrDefault() is TextBox tb)
                {
                    tb.TextChanged -= OnSubgroupTextChanged;
                    subgroupTextBoxes.Remove(tb);
                }

                panelSubgroups.Controls.Remove(last);

                subgroupCounter--;

                RecalculatePlaceholders();
            }
        }


        private void RecalculatePlaceholders()
        {
            int total = 100;
            int count = subgroupTextBoxes.Count;
            if (count == 0) return;

            // Шаг 1: считаем точные доли (double)
            double evenShare = (double)total / count;
            var rawValues = new List<double>();
            var intValues = new List<int>();

            double sumRounded = 0;

            for (int i = 0; i < count; i++)
            {
                double raw = evenShare;
                int rounded = (int)Math.Floor(raw);
                rawValues.Add(raw);
                intValues.Add(rounded);
                sumRounded += rounded;
            }

            // Шаг 2: распределим остаток (до 100%)
            int missing = total - (int)sumRounded;
            for (int i = 0; i < count && missing > 0; i++)
            {
                intValues[i]++;
                missing--;
            }

            // Шаг 3: применяем
            for (int i = 0; i < count; i++)
            {
                subgroupTextBoxes[i].PlaceholderText = intValues[i] + " %";
            }

            OnSubgroupTextChanged(null, null);
        }

        private void checkBox_CheckedChangedEdit(object sender, EventArgs e)
        {
            txtQuotaInput.ReadOnly = checkbxEdit.Checked == true ? false : true;
        }

        private void checkBox_CheckedChangedQShow(object sender, EventArgs e)
        {
            txtQuotaInput.Visible = checkbxShowQuota.Checked == true ? true : false;
            checkbxEdit.Enabled = checkbxShowQuota.Checked == true ? true : false;
        }

        private void dataGridQuotaView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dataGridQuotaView.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0 && hit.ColumnIndex >= 0)
                {
                    dataGridQuotaView.ClearSelection();
                    dataGridQuotaView.Rows[hit.RowIndex].Selected = true;
                    selectedRow = dataGridQuotaView.Rows[hit.RowIndex];
                    clickedColumnIndex = hit.ColumnIndex; // сохраняем индекс колонки
                }
            }
        }

        // private void dataGridQuotaView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        // {
        //     if (dataGridQuotaView.IsCurrentCellDirty)
        //     {
        //         dataGridQuotaView.CommitEdit(DataGridViewDataErrorContexts.Commit);
        //     }
        // }

        private void dataGridQuotaView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что изменили именно 1-й столбец (кол-во ящиков)
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridQuotaView.Rows[e.RowIndex];

                string name = row.Tag?.ToString();
                if (name == null) return;

                // Новое количество
                if (int.TryParse(row.Cells[0].Value?.ToString(), out int newQuantity))
                {
                    // Обновляем в supplyEntries
                    var entry = supplyEntries.FirstOrDefault(x => x.Name == name);
                    if (entry != null)
                    {
                        entry.Quantity = newQuantity;
                    }

                    // Обновляем в quotaText
                    UpdateCrateCountInQuotaText(name, newQuantity);

                    // Обновляем текстовое поле, если оно используется
                    if (txtQuotaInput != null)
                    {
                        txtQuotaInput.Text = quotaText;
                    }
                }
            }
        }

        private void UpdateCrateCountInQuotaText(string itemName, int newCrateCount)
        {
            string[] lines = quotaText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                // Совпадение формата "число ящ. - название"
                var match = Regex.Match(lines[i], @"^(\d+)\s*ящ\.?\s*[-–]\s*(.+)$", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string nameInText = match.Groups[2].Value.Trim();
                    if (string.Equals(nameInText, itemName, StringComparison.OrdinalIgnoreCase))
                    {
                        lines[i] = $"{newCrateCount} ящ. - {itemName}";
                        lines[i].Replace("\n", "\r\n");
                        break;
                    }
                }
            }
            quotaText = string.Join("\r\n", lines);

            // Также можно сохранить в файл, если загружали файл:
            if (!string.IsNullOrEmpty(quotaFilePath))
            {
                File.WriteAllText(quotaFilePath, quotaText);
            }
        }



        private void ReplaceItemInQuotaText(string oldLine, string newName, int newCrates)
        {
            string[] lines = quotaText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string newLine = $"{newCrates} ящ. - {newName}";
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim() == oldLine.Trim())
                {
                    lines[i] = newLine;
                    break;
                }
            }

            quotaText = string.Join("\n", lines);

            // Обновляем текстовое поле, если пользователь вставил текст вручную
            if (txtQuotaInput != null)
            {
                txtQuotaInput.Text = quotaText.Replace("\n", "\r\n"); ;
            }

            // File.WriteAllText(quotaFilePath, quotaText);
        }


        private void toolStripMenuItemReplace_Click(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                // Получаем старое имя предмета из Tag
                string oldItemName = selectedRow.Tag?.ToString();

                // Находим количество ящиков из текущего supplyEntries
                int previousCrateCount = 0;
                var existingEntry = supplyEntries.FirstOrDefault(e => e.Name == oldItemName);
                if (existingEntry != null)
                {
                    previousCrateCount = existingEntry.Quantity;
                }

                // Формируем список предметов для выбора
                var itemQuotaList = itemDatabase.Select(item => new ItemSelectionForm.ItemQuota
                {
                    Name = item.itemName,
                    CrateCount = previousCrateCount // <-- вот тут устанавливаем старое количество
                }).ToList();

                ItemSelectionForm selectionForm = new ItemSelectionForm(itemQuotaList);
                if (selectionForm.ShowDialog() == DialogResult.OK)
                {
                    var selectedItem = selectionForm.SelectedItem;
                    if (selectedItem != null)
                    {
                        // Устанавливаем выбранное количество (оно будет равно предыдущему)
                        selectedItem.CrateCount = previousCrateCount;

                        // Обновляем таблицу
                        string newQuotaText = $"{selectedItem.Name}";
                        selectedRow.Cells[1].Value = newQuotaText;

                        var matched = itemDatabase.FirstOrDefault(i => i.itemName == selectedItem.Name);
                        selectedRow.Cells[2].Value = matched != null ? matched.itemName : "❌ Не найдено";
                        selectedRow.Cells[2].Style.ForeColor = matched != null ? Color.Black : Color.Red;

                        // Обновляем Tag
                        selectedRow.Tag = selectedItem.Name;

                        // Обновляем текст квоты
                        ReplaceItemInQuotaText($"{oldItemName} —", selectedItem.Name, selectedItem.CrateCount);

                        // Обновляем запись в supplyEntries
                        if (existingEntry != null)
                        {
                            existingEntry.Name = selectedItem.Name;
                            existingEntry.Quantity = selectedItem.CrateCount;
                        }
                    }
                }
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                if (clickedColumnIndex == 2) // Правая колонка — удаляем только найденный предмет
                {
                    selectedRow.Cells[2].Value = "❌ Не найдено";
                    selectedRow.Cells[2].Style.ForeColor = Color.Red;
                }
                else // Левая колонка — удаляем всю строку как раньше
                {
                    string itemName = selectedRow.Tag?.ToString();

                    if (!string.IsNullOrWhiteSpace(itemName))
                    {
                        dataGridQuotaView.Rows.Remove(selectedRow);
                        selectedRow = null;

                        var entryToRemove = supplyEntries.FirstOrDefault(entry =>
                            entry.Name != null && entry.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

                        if (entryToRemove != null)
                        {
                            supplyEntries.Remove(entryToRemove);
                        }
                    }
                }
            }
        }

        private void txtQuotaInput_TextChanged(object sender, EventArgs e)
        {
            string input = txtQuotaInput.Text;

            // Если текст пустой — очищаем таблицу
            if (string.IsNullOrWhiteSpace(input))
            {
                dataGridQuotaView.Rows.Clear();
                supplyEntries.Clear();
                return;
            }

            quotaText = input; // сохраняем как текущий текст квоты
            LoadQuotaFromText(input);
        }

        private void LoadQuotaFromText(string text)
        {
            // Очистка старых данных
            supplyEntries.Clear();
            dataGridQuotaView.Rows.Clear();

            string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                line.Replace("–", "-");
                var match = Regex.Match(line, @"(\d+)\s*ящ\.?\s*[-–]\s*(.+)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string quantityStr = match.Groups[1].Value.Trim(); // сначала число
                    string name = match.Groups[2].Value.Trim();        // потом имя

                    if (int.TryParse(quantityStr, out int quantity))
                    {
                        var entry = new SupplyEntry { Name = name, Quantity = quantity };
                        supplyEntries.Add(entry);

                        var matched = itemDatabase.FirstOrDefault(item =>
                            item.nickname != null &&
                            item.nickname.Any(nick =>
                                string.Equals(nick.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase)));

                        string quotaInfo = $"{name}";
                        string matchedInfo = matched != null ? matched.itemName : "❌ Не найдено";
                        try
                        {
                            bool? showCheckbox = matched.craftLocation?.Contains("refinery") == true ? false : (bool?)null;

                            int rowIndex = dataGridQuotaView.Rows.Add(quantity, quotaInfo, matchedInfo, showCheckbox);

                            dataGridQuotaView.Rows[rowIndex].Tag = name;

                            if (matched == null)
                            {
                                dataGridQuotaView.Rows[rowIndex].Cells[2].Style.ForeColor = Color.Red;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: ", ex);
                        }
                    }
                }
            }
        }
        private void dataGridQuotaView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == dataGridQuotaView.Columns["SelectColumn"].Index && e.RowIndex >= 0)
            {
                var cellValue = dataGridQuotaView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (cellValue == null || cellValue == DBNull.Value)
                {
                    e.PaintBackground(e.CellBounds, true); // только фон
                    e.Handled = true; // отменить отрисовку чекбокса
                }
            }
        }


        private void dataGridQuotaView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridQuotaView.Columns[e.ColumnIndex].Name == "SelectColumn")
            {
                var value = dataGridQuotaView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (value == null || value == DBNull.Value)
                {
                    e.Cancel = true; // запрещаем редактировать ячейку
                }
            }
        }


        private void btnPasteFromClipboard_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                if (!string.IsNullOrEmpty(quotaText))
                {
                    DialogResult result = MessageBox.Show(
                                        "Таблица с квотой уже содержит данные. Заменить содержимое новой квотой из буфера обмена?",
                                        "Предупреждение",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.Yes)
                    {
                        string clipboardText = Clipboard.GetText().Replace("\n", "\r\n");
                        txtQuotaInput.Text = clipboardText;
                        quotaText = clipboardText;
                        LoadQuotaFromText(quotaText); // функция парсинга и загрузки
                    }
                }
                else
                {
                    string clipboardText = Clipboard.GetText().Replace("\n", "\r\n");
                    txtQuotaInput.Text = clipboardText;
                    quotaText = clipboardText;
                    LoadQuotaFromText(quotaText);
                }
            }
            else
            {
                MessageBox.Show("Буфер обмена не содержит текста.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void LoadItemDatabase()
        {
            try
            {
                string jsonPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\items.json"));

                if (File.Exists(jsonPath))
                {
                    string jsonContent = File.ReadAllText(jsonPath);
                    itemDatabase = JsonSerializer.Deserialize<List<Item>>(jsonContent);
                }
                else
                {
                    MessageBox.Show($"Файл items.json не найден по пути:\n{jsonPath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки базы данных предметов: " + ex.Message);
            }
        }

        private void btnShowItems_Click(object sender, EventArgs e)
        {
            lstItems.Items.Clear();

            try
            {
                string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\items.json"));
                if (!File.Exists(path))
                {
                    MessageBox.Show("Файл items.json не найден.");
                    return;
                }

                string json = File.ReadAllText(path);
                currentItems = JsonSerializer.Deserialize<List<Item>>(json) ?? new List<Item>();

                if (currentItems.Count == 0)
                {
                    MessageBox.Show("Нет предметов в базе.");
                    return;
                }

                DisplayFilteredItems(""); // Показать всё изначально
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке предметов: " + ex.Message);
            }
        }

        private void DisplayFilteredItems(string filter)
        {
            lstItems.Items.Clear();

            int count = 1;
            foreach (var item in currentItems)
            {
                string name = item.itemName?.ToLower() ?? "";
                string nicks = string.Join(", ", item.nickname ?? new List<string>()).ToLower();
                string full = name + " " + nicks;

                if (full.Contains(filter.ToLower()))
                {
                    lstItems.Items.Add($"[{count}] {item.itemName} ({string.Join(", ", item.nickname ?? new List<string>())})");
                }

                count++;
            }
        }

        private void txtSearchDB_TextChanged(object sender, EventArgs e)
        {
            DisplayFilteredItems(txtSearchDB.Text);
        }


        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите предмет для удаления.");
                return;
            }

            string selected = lstItems.SelectedItem.ToString();

            // Извлекаем имя между "]" и "("
            int endOfId = selected.IndexOf("]") + 1;
            int startOfNicks = selected.LastIndexOf("(");

            if (endOfId <= 0 || startOfNicks == -1 || startOfNicks <= endOfId)
            {
                MessageBox.Show("Не удалось извлечь имя предмета.");
                return;
            }

            string namePart = selected.Substring(endOfId, startOfNicks - endOfId).Trim();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\items.json"));
            if (!File.Exists(path))
            {
                MessageBox.Show("Файл items.json не найден.");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var items = JsonSerializer.Deserialize<List<Item>>(json);

                var itemToRemove = items.FirstOrDefault(i =>
                    string.Equals(i.itemName, namePart, StringComparison.OrdinalIgnoreCase) ||
                    (i.nickname != null && i.nickname.Any(nick =>
                        string.Equals(nick.Trim(), namePart, StringComparison.OrdinalIgnoreCase))));

                if (itemToRemove != null)
                {
                    items.Remove(itemToRemove);

                    string updatedJson = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, updatedJson);

                    MessageBox.Show("Предмет удалён.");
                    btnShowItems_Click(null, null); // Обновить список
                }
                else
                {
                    MessageBox.Show("Предмет не найден по имени или никнейму.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }



        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(quotaText))
                        {
                            DialogResult result = MessageBox.Show(
                                                $"Таблица с квотой уже содержит данные. Заменить содержимое новой квотой из ${ofd.FileName}?",
                                                "Предупреждение",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Information,
                                                MessageBoxDefaultButton.Button1,
                                                MessageBoxOptions.DefaultDesktopOnly);
                            if (result == DialogResult.Yes)
                            {
                                quotaFilePath = ofd.FileName;
                                quotaText = File.ReadAllText(ofd.FileName);

                                LoadQuotaFromText(quotaText);
                                txtQuotaInput.Text = txtQuotaInput.Text = quotaText.Replace("\n", "\r\n"); ; ;
                                // Очистка старых данных
                                MessageBox.Show("Файл успешно загружен и квота отображена.");
                            }
                        }
                        else
                        {
                            quotaFilePath = ofd.FileName;
                            quotaText = File.ReadAllText(ofd.FileName);

                            LoadQuotaFromText(quotaText);
                            txtQuotaInput.Text = quotaText.Replace("\n", "\r\n"); ;
                            // Очистка старых данных
                            MessageBox.Show("Файл успешно загружен и квота отображена.");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при загрузке: " + ex.Message);
                    }
                }
            }
        }


        private void ImportItemDatabase(string importPath)
        {
            if (!File.Exists(importPath))
            {
                MessageBox.Show("Указанный файл не существует.");
                return;
            }

            try
            {
                string importJson = File.ReadAllText(importPath);
                var parsedJson = JsonDocument.Parse(importJson);

                if (parsedJson.RootElement.ValueKind != JsonValueKind.Array)
                {
                    MessageBox.Show("Импортируемый JSON должен быть массивом объектов.");
                    return;
                }

                var validItems = new List<Item>();

                foreach (var element in parsedJson.RootElement.EnumerateArray())
                {
                    try
                    {
                        string itemName = element.GetProperty("itemName").GetString() ?? "";
                        string[]? craftLocations = element.TryGetProperty("craftLocation", out var locs) && locs.ValueKind == JsonValueKind.Array
                            ? locs.EnumerateArray().Select(l => l.GetString() ?? "").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray()
                            : null;

                        List<string> nicknames = new() { itemName };

                        Cost cost = new Cost
                        {
                            bmat = element.GetProperty("cost").TryGetProperty("bmat", out var bmat) && bmat.ValueKind == JsonValueKind.Number ? bmat.GetInt32() : 0,
                            rmat = element.GetProperty("cost").TryGetProperty("rmat", out var rmat) && rmat.ValueKind == JsonValueKind.Number ? rmat.GetInt32() : 0,
                            emat = element.GetProperty("cost").TryGetProperty("emat", out var emat) && emat.ValueKind == JsonValueKind.Number ? emat.GetInt32() : 0,
                            hemat = element.GetProperty("cost").TryGetProperty("hemat", out var hemat) && hemat.ValueKind == JsonValueKind.Number ? hemat.GetInt32() : 0,
                        };

                        string category = element.TryGetProperty("itemCategory", out var cat) ? cat.GetString() : null;

                        var item = new Item
                        {
                            itemName = itemName,
                            nickname = nicknames,
                            craftLocation = craftLocations,
                            itemCategory = category,
                            cost = cost
                        };

                        validItems.Add(item);
                    }
                    catch
                    {
                        // Игнорируем ошибочные элементы
                        continue;
                    }
                }

                if (validItems.Count == 0)
                {
                    MessageBox.Show("Не удалось извлечь ни одного предмета из файла.");
                    return;
                }

                // Загрузка текущей базы
                string jsonPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\items.json"));
                var currentItems = File.Exists(jsonPath)
                    ? JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(jsonPath)) ?? new List<Item>()
                    : new List<Item>();

                int replaced = 0, added = 0;

                foreach (var imported in validItems)
                {
                    var existing = currentItems.FirstOrDefault(i => i.itemName == imported.itemName);
                    if (existing != null)
                    {
                        currentItems.Remove(existing);
                        currentItems.Add(imported);
                        replaced++;
                    }
                    else
                    {
                        currentItems.Add(imported);
                        added++;
                    }
                }

                File.WriteAllText(jsonPath, JsonSerializer.Serialize(currentItems, new JsonSerializerOptions { WriteIndented = true }));

                MessageBox.Show($"Импорт завершён.\nЗаменено: {replaced}, добавлено новых: {added}.");
                LoadItemDatabase();
                btnShowItems_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при импорте базы данных: " + ex.Message);
            }
        }


        private void btnImportItems_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ImportItemDatabase(ofd.FileName);
                }
            }
        }


        private void btnAddItem_Click(object sender, EventArgs e)
        {
            var addForm = new AddItemForm();
            addForm.ShowDialog();
            LoadItemDatabase(); // Обновим базу после добавления
            btnShowItems_Click(null, null);

        }

        const int BMAT_C = 1;
        const int RMAT_C = 10;
        const int EMAT_C = 2;
        const int HMAT_C = 2;

        private List<double> GetSubgroupPercentages()
        {
            var percentages = new List<double>();
            double total = 0;

            foreach (var tb in subgroupTextBoxes)
            {
                if (double.TryParse(tb.Text, out double val))
                {
                    percentages.Add(val);
                    total += val;
                }
                else if (tb.PlaceholderText.EndsWith("%") &&
                         double.TryParse(tb.PlaceholderText.Replace("%", "").Trim(), out double placeholderVal))
                {
                    percentages.Add(placeholderVal);
                    total += placeholderVal;
                }
                else
                {
                    percentages.Add(0);
                }
            }

            // Нормализация на случай, если сумма не 100
            if (Math.Abs(total - 100) > 0.01 && total > 0)
            {
                for (int i = 0; i < percentages.Count; i++)
                    percentages[i] = percentages[i] / total * 100;
            }

            return percentages;
        }


        private int GetRecommendedCrateCount(string? factoryType)
        {
            if (string.IsNullOrEmpty(factoryType))
                return 1;

            return factoryType.ToLower() switch
            {
                "mpf" => 9,
                "factory" => 4, // или 8 — уточни, если надо учитывать оба
                _ => 1 // по умолчанию
            };
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            int subgroupCount = subgroupTextBoxes.Count;
            if (subgroupCount == 0)
            {
                MessageBox.Show("Добавьте хотя бы одну подгруппу.");
                return;
            }


            lstResults.Items.Clear();
            var valuedEntries = new List<ValuedEntry>();

            foreach (var entry in supplyEntries)
            {
                var matchedItem = itemDatabase.FirstOrDefault(item =>
                    item.nickname != null &&
                    item.nickname.Any(nick => entry.Name.Contains(nick, StringComparison.OrdinalIgnoreCase)));

                if (matchedItem == null || matchedItem.cost == null)
                    continue;

                int bmats = matchedItem.cost.bmat ?? 0;
                int rmats = matchedItem.cost.rmat ?? 0;
                int emats = matchedItem.cost.emat ?? 0;
                int hemats = matchedItem.cost.hemat ?? 0;

                int total = (bmats * BMAT_C) +
                            (rmats * RMAT_C) +
                            (emats * EMAT_C) +
                            (hemats * HMAT_C);

                double valuePerUnit = total / 4.0;
                double totalValue = valuePerUnit * entry.Quantity;
                int recommendedCrates;
                if (matchedItem.craftLocation.Any("mpf".Contains))
                {
                    recommendedCrates = GetRecommendedCrateCount("mpf");
                }
                else
                {
                    recommendedCrates = GetRecommendedCrateCount(matchedItem.craftLocation[0]);
                }

                valuedEntries.Add(new ValuedEntry
                {
                    Name = entry.Name,
                    Quantity = entry.Quantity,
                    ValuePerUnit = valuePerUnit,
                    TotalValue = totalValue,
                    RecommendedCrates = recommendedCrates
                });
            }


            // Сортировка по убыванию ценности
            valuedEntries = valuedEntries.OrderByDescending(e => e.ValuePerUnit).ToList();

            // Подготовка структуры: солдаты и их инвентарь
            var subgroupPercentages = GetSubgroupPercentages();

            var subgroupResults = new List<string>[subgroupCount];
            var subgroupValues = new double[subgroupCount];

            for (int i = 0; i < subgroupCount; i++)
                subgroupResults[i] = new List<string>();

            double totalQuotaValue = valuedEntries.Sum(e => e.TotalValue);
            var subgroupBudgets = subgroupPercentages
    .Select(p => p / 100.0 * totalQuotaValue)
    .ToArray();

            foreach (var ve in valuedEntries)
            {
                int fullBatches = ve.Quantity / ve.RecommendedCrates;
                int remainder = ve.Quantity % ve.RecommendedCrates;
                double batchValue = ve.ValuePerUnit * ve.RecommendedCrates;

                for (int b = 0; b < fullBatches; b++)
                {
                    int target = GetSubgroupWithMostRemainingBudget(subgroupValues, subgroupBudgets);
                    subgroupResults[target].Add($"{ve.Name} — {ve.RecommendedCrates} ящ.");
                    subgroupValues[target] += batchValue;
                }

                if (remainder > 0)
                {
                    double remainderValue = ve.ValuePerUnit * remainder;
                    int target = GetSubgroupWithMostRemainingBudget(subgroupValues, subgroupBudgets);
                    subgroupResults[target].Add($"{ve.Name} — {remainder} ящ. (остаток)");
                    subgroupValues[target] += remainderValue;
                }
            }

            lstResults.Items.Clear();
            for (int i = 0; i < subgroupCount; i++)
            {
                lstResults.Items.Add($"Подгруппа {i + 1} ({subgroupPercentages[i]:F1}%):");
                var grouped = subgroupResults[i]
                    .GroupBy(x => x)
                    .Select(g => new { Text = g.Key, Count = g.Count() });

                foreach (var entry in grouped)
                {
                    if (entry.Count > 1)
                        lstResults.Items.Add($"   {entry.Text} × {entry.Count}");
                    else
                        lstResults.Items.Add("   " + entry.Text);
                }

                lstResults.Items.Add($"   Общая ценность: {subgroupValues[i]:F2}");
            }

        }

        // Вспомогательная функция для определения бойца с наименьшей ценностью
        private int GetSubgroupWithMostRemainingBudget(double[] current, double[] budgets)
        {
            double maxRemaining = budgets[0] - current[0];
            int index = 0;

            for (int i = 1; i < current.Length; i++)
            {
                double remaining = budgets[i] - current[i];
                if (remaining > maxRemaining)
                {
                    maxRemaining = remaining;
                    index = i;
                }
            }

            return index;
        }



        private void btnSaveResults_Click(object sender, EventArgs e)
        {
            if (lstResults.Items.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files (*.txt)|*.txt";
                sfd.FileName = $"Распределение_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(sfd.FileName))
                        {
                            foreach (var item in lstResults.Items)
                            {
                                writer.WriteLine(item.ToString());
                            }
                        }

                        MessageBox.Show("Результат успешно сохранён!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при сохранении файла: " + ex.Message);
                    }
                }
            }
        }


    }

    public class ValuedEntry
    {
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public double ValuePerUnit { get; set; }
        public double TotalValue { get; set; }
        public int RecommendedCrates { get; set; }
    }



    public class ColorScheme
    {
        public Color dataGridQuotaViewBG = Color.Black;
        public Color dataGridQuotaViewFG = Color.White;

        public Color FormBG = Color.Black;

        public Color FlowLayoutPanelBG = Color.FromArgb(30, 30, 30);
        public Color FlowLayoutPanelFG = Color.White;

        public Color ButtonBG = Color.FromArgb(45, 45, 45);
        public Color ButtonFG = Color.White;

        public Color TextBoxBG = Color.FromArgb(40, 40, 40);
        public Color TextBoxFG = Color.White;

        public Color TabControlBG = Color.FromArgb(30, 30, 30);
        public Color TabControlFG = Color.White;

        public Color TabPageBG = Color.FromArgb(35, 35, 35);
        public Color TabPageFG = Color.White;
    }



    public class Item
    {
        public string? itemName { get; set; }
        public List<string>? nickname { get; set; }
        public Cost? cost { get; set; }
        public string?[] craftLocation { get; set; }
        public string? itemCategory { get; set; }
    }

    public class Cost
    {
        public int? bmat { get; set; }
        public int? rmat { get; set; }
        public int? emat { get; set; }
        public int? hemat { get; set; }
    }

    public class SupplyEntry
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
    }
}
