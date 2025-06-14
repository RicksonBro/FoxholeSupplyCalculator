using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace FoxholeSupplyCalculator
{
    public partial class AddItemForm : Form
    {
        public AddItemForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var newItem = new Item
                {
                    id = int.Parse(txtId.Text),
                    name = txtName.Text,
                    nickname = new List<string>(txtNicknames.Text.Split(',', StringSplitOptions.RemoveEmptyEntries)),
                    craftLocation = cmbcraftLocation.Text,
                    production_branch = cmbProdBranch.Text,
                    resources = new Resources
                    {
                        Bmats = int.Parse(txtBmats.Text == "" ? "0" : txtBmats.Text),
                        Rmats = int.Parse(txtRmats.Text == "" ? "0" : txtRmats.Text),
                        Emats = int.Parse(txtEmats.Text == "" ? "0" : txtEmats.Text),
                        Hemats = int.Parse(txtHemats.Text == "" ? "0" : txtHemats.Text)
                    }
                };

                // путь к items.json в корне проекта
                string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\items.json"));

                List<Item> items = new();
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    if (!string.IsNullOrWhiteSpace(json))
                        items = JsonSerializer.Deserialize<List<Item>>(json) ?? new List<Item>();
                }

                items.Add(newItem);

                string updatedJson = JsonSerializer.Serialize(items, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(path, updatedJson);

                MessageBox.Show("Предмет добавлен!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
