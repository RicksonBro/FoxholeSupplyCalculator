using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FoxholeSupplyCalculator;

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
        StartPosition = FormStartPosition.CenterParent;
    }

    private void InitializeComponent()
    {
        Text = "Выберите предмет";
        Width = 300;
        Height = 400;

        listBox = new ListBox() { Dock = DockStyle.Bottom };
        listBox.Size = new Size(100, 300);
        listBox.DoubleClick += ListBox_DoubleClick;

        txtSearch = new TextBox() { Dock = DockStyle.Top };
        // txtSearch.Location = new Point(12, 10);
        txtSearch.Size = new Size(100, 30);
        txtSearch.PlaceholderText = "🔍Поиск";
        txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);

        Controls.Add(listBox);
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
            if (listBox.SelectedIndex >= 0)
            {
                SelectedItem = items[listBox.SelectedIndex];
                DialogResult = DialogResult.OK;
                Close();
            }
            DialogResult = DialogResult.OK;
            Close();
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
        public Item? SourceItem { get; set; }

    }

}
