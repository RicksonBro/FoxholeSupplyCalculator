using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public partial class ItemSelectionForm : Form
{
    private List<ItemQuota> items;
    public ItemQuota? SelectedItem { get; private set; }

    private ListBox listBox;
    private TextBox txtSearch;

    public ItemSelectionForm(List<ItemQuota> items)
    {
        this.items = items;

        InitializeComponent();
        LoadItems();
    }

    private void InitializeComponent()
    {
        this.Text = "Выберите предмет";
        this.Width = 300;
        this.Height = 400;

        listBox = new ListBox() { Dock = DockStyle.Bottom };
        listBox.Size = new Size(100, 300);
        listBox.DoubleClick += ListBox_DoubleClick;

        txtSearch = new TextBox() { Dock = DockStyle.Top };
        // txtSearch.Location = new Point(12, 10);
        txtSearch.Size = new Size(100, 30);
        txtSearch.PlaceholderText = "🔍Поиск";
        txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

        this.Controls.Add(listBox);
        Controls.Add(txtSearch);
    }

    private void LoadItems()
    {
        listBox.Items.Clear();
        foreach (var item in items)
        {
            listBox.Items.Add($"{item.Name}");
        }
    }


    private void ListBox_DoubleClick(object? sender, EventArgs e)
    {
        if (listBox.SelectedItem != null)
        {
            string selectedLine = listBox.SelectedItem.ToString();
            // Получаем имя до " — "
            string selectedName = selectedLine.Split('—')[0].Trim();
            SelectedItem = items.Find(i => i.Name == selectedName);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string search = txtSearch.Text.Trim().ToLower();

        listBox.Items.Clear();

        foreach (var item in items)
        {
            if (item.Name.ToLower().Contains(search))
            {
                listBox.Items.Add($"{item.Name}");
            }
        }
    }


    public class ItemQuota
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int CrateCount { get; set; }

    }

}
