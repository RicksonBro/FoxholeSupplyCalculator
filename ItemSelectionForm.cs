using System;
using System.Collections.Generic;
using System.Windows.Forms;

public partial class ItemSelectionForm : Form
{
    private List<ItemQuota> items;
    public ItemQuota? SelectedItem { get; private set; }

    private ListBox listBox;

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

        listBox = new ListBox() { Dock = DockStyle.Fill };
        listBox.DoubleClick += ListBox_DoubleClick;
        this.Controls.Add(listBox);
    }

    private void LoadItems()
    {
        listBox.Items.Clear();
        foreach (var item in items)
        {
            listBox.Items.Add($"{item.Name} — {item.CrateCount} ящ.");
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


    public class ItemQuota
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int CrateCount { get; set; }

    }

}
