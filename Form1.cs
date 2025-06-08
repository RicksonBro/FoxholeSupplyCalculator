using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Threading;


namespace FoxholeSupplyCalculator
{

    public partial class Form1 : Form
    {
        private List<Item> itemDatabase = new List<Item>();
        private List<SupplyEntry> supplyEntries = new List<SupplyEntry>();
        private int soldierCount = 10;
        private DataGridViewRow selectedRow = null;

        public Form1()
        {
            InitializeComponent();
            LoadItemDatabase();
            btnShowItems_Click(null, null);
        }

        private void dataGridQuotaView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dataGridQuotaView.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0)
                {
                    dataGridQuotaView.ClearSelection();
                    dataGridQuotaView.Rows[hit.RowIndex].Selected = true;
                    selectedRow = dataGridQuotaView.Rows[hit.RowIndex];
                }
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                dataGridQuotaView.Rows.Remove(selectedRow);
                selectedRow = null;
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
                var items = JsonSerializer.Deserialize<List<Item>>(json);

                if (items == null || items.Count == 0)
                {
                    MessageBox.Show("Нет предметов в базе.");
                    return;
                }
                int count = 1;
                foreach (var item in items)
                {
                    string display = $"[{count}] {item.name} ({string.Join(", ", item.nickname)})";
                    count++;
                    lstItems.Items.Add(display);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке предметов: " + ex.Message);
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите предмет для удаления.");
                return;
            }

            string selected = lstItems.SelectedItem.ToString();

            // Пытаемся получить ID из начала строки
            int idStart = selected.IndexOf('[') + 1;
            int idEnd = selected.IndexOf(']');
            if (idStart < 0 || idEnd < 0 || idEnd <= idStart)
            {
                MessageBox.Show("Не удалось определить ID предмета.");
                return;
            }

            string idStr = selected.Substring(idStart, idEnd - idStart);
            if (!int.TryParse(idStr, out int id))
            {
                MessageBox.Show("Неверный ID.");
                return;
            }

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

                var itemToRemove = items.FirstOrDefault(i => i.id == id);
                if (itemToRemove != null)
                {
                    items.Remove(itemToRemove);

                    string updatedJson = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, updatedJson);

                    MessageBox.Show("Предмет удалён.");
                    btnShowItems_Click(null, null); // Перезагрузим список
                }
                else
                {
                    MessageBox.Show("Предмет не найден.");
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
                        string quotaText = File.ReadAllText(ofd.FileName);

                        // Очистка старых данных
                        supplyEntries.Clear();
                        dataGridQuotaView.Rows.Clear();

                        string[] lines = quotaText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string line in lines)
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(line, @"(.+?)[\s\-–]+(\d+)\s*ящ\.?", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            if (match.Success)
                            {
                                string name = match.Groups[1].Value.Trim();
                                string quantityStr = match.Groups[2].Value.Trim();

                                if (int.TryParse(quantityStr, out int quantity))
                                {
                                    supplyEntries.Add(new SupplyEntry { Name = name, Quantity = quantity });
                                }
                            }
                        }

                        MessageBox.Show("Файл успешно загружен и квота отображена.");

                        // Загрузка базы данных
                        string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\items.json"));
                        if (!File.Exists(path))
                        {
                            MessageBox.Show("Файл items.json не найден.");
                            return;
                        }

                        string json = File.ReadAllText(path);
                        var allItems = JsonSerializer.Deserialize<List<Item>>(json);
                        if (allItems == null) return;

                        // Добавление в таблицу
                        foreach (var entry in supplyEntries)
                        {
                            var matched = allItems.FirstOrDefault(item =>
                                item.nickname != null &&
                                item.nickname.Any(nick =>
                                    string.Equals(nick.Trim(), entry.Name.Trim(), StringComparison.OrdinalIgnoreCase)));

                            string quotaInfo = $"{entry.Name} — {entry.Quantity} ящ.";
                            string matchedInfo = matched != null
                                ? $"{matched.name}"
                                : "❌ Не найдено";

                            int rowIndex = dataGridQuotaView.Rows.Add(quotaInfo, matchedInfo);

                            // Подсветка, если не найдено
                            if (matched == null)
                            {
                                dataGridQuotaView.Rows[rowIndex].Cells[1].Style.ForeColor = Color.Red;
                            }
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
                // Загружаем импортируемые предметы
                string importJson = File.ReadAllText(importPath);
                var importedItems = JsonSerializer.Deserialize<List<Item>>(importJson);

                if (importedItems == null || importedItems.Count == 0)
                {
                    MessageBox.Show("Импортируемый файл не содержит предметов.");
                    return;
                }

                // Загружаем текущие предметы
                string jsonPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\items.json"));
                var currentItems = File.Exists(jsonPath)
                    ? JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(jsonPath)) ?? new List<Item>()
                    : new List<Item>();

                int replacedCount = 0;
                int addedCount = 0;

                foreach (var imported in importedItems)
                {
                    var existing = currentItems.FirstOrDefault(i => i.name == imported.name);
                    if (existing != null)
                    {
                        // Заменяем существующий предмет
                        currentItems.Remove(existing);
                        currentItems.Add(imported);
                        replacedCount++;
                    }
                    else
                    {
                        // Добавляем новый
                        currentItems.Add(imported);
                        addedCount++;
                    }
                }

                // Сохраняем обновлённую базу
                File.WriteAllText(jsonPath, JsonSerializer.Serialize(currentItems, new JsonSerializerOptions { WriteIndented = true }));

                MessageBox.Show($"Импорт завершён.\nЗаменено: {replacedCount}, добавлено новых: {addedCount}.");
                LoadItemDatabase();
                btnShowItems_Click(null, null); // Обновить отображение
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

        private int GetRecommendedCrateCount(string? factoryType)
        {
            if (string.IsNullOrEmpty(factoryType))
                return 1;

            return factoryType.ToLower() switch
            {
                "mass" => 9,
                "common" => 4, // или 8 — уточни, если надо учитывать оба
                _ => 1 // по умолчанию
            };
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSoldiers.Text, out soldierCount) || soldierCount <= 0)
            {
                MessageBox.Show("Введите корректное количество бойцов.");
                return;
            }

            lstResults.Items.Clear();
            var valuedEntries = new List<ValuedEntry>();

            foreach (var entry in supplyEntries)
            {
                var matchedItem = itemDatabase.FirstOrDefault(item =>
                    item.nickname != null &&
                    item.nickname.Any(nick => entry.Name.Contains(nick, StringComparison.OrdinalIgnoreCase)));

                if (matchedItem == null || matchedItem.resources == null)
                    continue;

                int total = (matchedItem.resources.Bmats * BMAT_C) +
                            (matchedItem.resources.Rmats * RMAT_C) +
                            (matchedItem.resources.Emats * EMAT_C) +
                            (matchedItem.resources.Hemats * HMAT_C);

                double valuePerUnit = total / 4.0;
                double totalValue = valuePerUnit * entry.Quantity;
                int recommendedCrates = GetRecommendedCrateCount(matchedItem.fabric_type);

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
            var soldierResults = new List<string>[soldierCount];
            var soldierValues = new double[soldierCount];

            for (int i = 0; i < soldierCount; i++)
                soldierResults[i] = new List<string>();

            foreach (var ve in valuedEntries)
            {
                int fullBatches = ve.Quantity / ve.RecommendedCrates;
                int remainder = ve.Quantity % ve.RecommendedCrates;
                double batchValue = ve.ValuePerUnit * ve.RecommendedCrates;

                // Раздаём полные партии
                for (int b = 0; b < fullBatches; b++)
                {
                    int targetSoldier = GetSoldierWithLowestValue(soldierValues);
                    soldierResults[targetSoldier].Add($"{ve.Name} — {ve.RecommendedCrates} ящ.");
                    soldierValues[targetSoldier] += batchValue;
                }

                // Раздаём остаток — как есть
                if (remainder > 0)
                {
                    double remainderValue = ve.ValuePerUnit * remainder;
                    int targetSoldier = GetSoldierWithLowestValue(soldierValues);
                    soldierResults[targetSoldier].Add($"{ve.Name} — {remainder} ящ. (остаток)");
                    soldierValues[targetSoldier] += remainderValue;
                }
            }

            // Вывод
            lstResults.Items.Clear();
            for (int i = 0; i < soldierCount; i++)
            {
                lstResults.Items.Add($"Боец {i + 1}:");
                var grouped = soldierResults[i]
    .GroupBy(x => x)
    .Select(g => new { Text = g.Key, Count = g.Count() });

                foreach (var entry in grouped)
                {
                    if (entry.Count > 1)
                        lstResults.Items.Add($"   {entry.Text} × {entry.Count}");
                    else
                        lstResults.Items.Add("   " + entry.Text);
                }

                lstResults.Items.Add($"   Общая ценность: {soldierValues[i]:F2}");
            }
        }

        // Вспомогательная функция для определения бойца с наименьшей ценностью
        private int GetSoldierWithLowestValue(double[] values)
        {
            double min = values[0];
            int index = 0;
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min)
                {
                    min = values[i];
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



    public class Item
    {
        public int id { get; set; }
        public string? name { get; set; }
        public List<string>? nickname { get; set; }
        public Resources? resources { get; set; }
        public string? fabric_type { get; set; }
        public string? production_branch { get; set; }
    }

    public class Resources
    {
        public int Bmats { get; set; }
        public int Rmats { get; set; }
        public int Emats { get; set; }
        public int Hemats { get; set; }
    }

    public class SupplyEntry
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
    }
}
