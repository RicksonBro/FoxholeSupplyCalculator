using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace FoxholeSupplyCalculator
{

    public partial class Form1 : Form
    {
        private List<Item> itemDatabase = new List<Item>();
        private List<SupplyEntry> supplyEntries = new List<SupplyEntry>();
        private int soldierCount = 10;

        public Form1()
        {
            InitializeComponent();
            LoadItemDatabase();
            // this.Icon = new System.Drawing.Icon("icon.ico");
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

                foreach (var item in items)
                {
                    string display = $"[{item.id}] {item.name} ({string.Join(", ", item.nickname)})";
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
                    supplyEntries.Clear();
                    string[] lines = File.ReadAllLines(ofd.FileName);

                    foreach (string line in lines)
                    {
                        // Попробуем найти шаблон: "Название - 18ящ" или "Название 50 ящ."
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

                    MessageBox.Show("Файл успешно загружен.");
                }
            }
        }


        private void btnAddItem_Click(object sender, EventArgs e)
        {
            var addForm = new AddItemForm();
            addForm.ShowDialog();
            LoadItemDatabase(); // Обновим базу после добавления
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

            // Сортировка по ценности
            valuedEntries = valuedEntries.OrderByDescending(e => e.ValuePerUnit).ToList();

            // Распределение предметов между бойцами
            for (int i = 0; i < soldierCount; i++)
            {
                lstResults.Items.Add($"Боец {i + 1}:");
            }

            // Хранит накопленные строки по бойцам
            var soldierResults = new List<string>[soldierCount];
            for (int i = 0; i < soldierCount; i++)
                soldierResults[i] = new List<string>();

            int soldierIndex = 0;

            foreach (var ve in valuedEntries)
            {
                int fullBatches = ve.Quantity / ve.RecommendedCrates;
                int remainder = ve.Quantity % ve.RecommendedCrates;

                for (int b = 0; b < fullBatches; b++)
                {
                    soldierResults[soldierIndex].Add($"{ve.Name} — {ve.RecommendedCrates} ящ.");
                    soldierIndex = (soldierIndex + 1) % soldierCount;
                }

                if (remainder > 0)
                {
                    // добавляем остаток к следующему бойцу
                    soldierResults[soldierIndex].Add($"{ve.Name} — {remainder} ящ. (остаток)");
                    soldierIndex = (soldierIndex + 1) % soldierCount;
                }
            }

            // Выводим все строки на форму
            lstResults.Items.Clear();
            for (int i = 0; i < soldierCount; i++)
            {
                lstResults.Items.Add($"Боец {i + 1}:");
                foreach (var line in soldierResults[i])
                {
                    lstResults.Items.Add("   " + line);
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
