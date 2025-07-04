using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FoxholeSupplyCalculator;

public class EditItemForm : Form
{
    private Item item;

    private TextBox txtName;
    private TextBox txtNicknames;
    private NumericUpDown numBmat, numRmat, numEmat, numHemat;
    private CheckedListBox clbCraftLocations;
    private TextBox txtCategory;
    private Button btnSave;
    private Button btnCancel;

    private readonly string[] allCraftLocations = { "factory", "mpf", "refinery" };

    public EditItemForm(Item item)
    {
        this.item = item;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Редактировать предмет";
        Size = new Size(400, 500);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;

        Label lblName = new Label { Text = "Название:", Location = new Point(10, 10) };
        txtName = new TextBox { Location = new Point(120, 10), Width = 240, Text = item.itemName };

        Label lblNick = new Label { Text = "Никнеймы (через запятую):", Location = new Point(10, 40) };
        txtNicknames = new TextBox
        {
            Location = new Point(10, 60),
            Width = 350,
            Text = string.Join(", ", item.nickname ?? new List<string>())
        };

        Label lblCost = new Label { Text = "Ресурсы:", Location = new Point(10, 100) };
        numBmat = CreateNumeric("Bmat", 10, 120, item.cost?.bmat ?? 0);
        numRmat = CreateNumeric("Rmat", 10, 150, item.cost?.rmat ?? 0);
        numEmat = CreateNumeric("Emat", 10, 180, item.cost?.emat ?? 0);
        numHemat = CreateNumeric("Hemat", 10, 210, item.cost?.hemat ?? 0);

        Label lblLocation = new Label { Text = "Локации крафта:", Location = new Point(200, 100) };
        clbCraftLocations = new CheckedListBox { Location = new Point(200, 120), Width = 150, Height = 100 };
        clbCraftLocations.Items.AddRange(allCraftLocations);

        if (item.craftLocation != null)
        {
            for (int i = 0; i < clbCraftLocations.Items.Count; i++)
            {
                string loc = clbCraftLocations.Items[i].ToString();
                if (item.craftLocation.Contains(loc))
                {
                    clbCraftLocations.SetItemChecked(i, true);
                }
            }
        }

        Label lblCategory = new Label { Text = "Категория:", Location = new Point(10, 250) };
        txtCategory = new TextBox { Location = new Point(120, 250), Width = 240, Text = item.itemCategory };

        btnSave = new Button { Text = "Сохранить", Location = new Point(80, 300), DialogResult = DialogResult.OK };
        btnCancel = new Button { Text = "Отмена", Location = new Point(200, 300), DialogResult = DialogResult.Cancel };

        btnSave.Click += BtnSave_Click;

        Controls.AddRange(new Control[]
        {
            lblName, txtName,
            lblNick, txtNicknames,
            lblCost, numBmat, numRmat, numEmat, numHemat,
            lblLocation, clbCraftLocations,
            lblCategory, txtCategory,
            btnSave, btnCancel
        });
    }

    private NumericUpDown CreateNumeric(string label, int x, int y, int value)
    {
        Controls.Add(new Label { Text = label + ":", Location = new Point(x, y) });
        var nud = new NumericUpDown { Location = new Point(x + 60, y), Width = 60, Minimum = 0, Maximum = 999, Value = value };
        Controls.Add(nud);
        return nud;
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        item.itemName = txtName.Text.Trim();
        item.nickname = txtNicknames.Text.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

        item.cost.bmat = (int)numBmat.Value;
        item.cost.rmat = (int)numRmat.Value;
        item.cost.emat = (int)numEmat.Value;
        item.cost.hemat = (int)numHemat.Value;

        item.craftLocation = clbCraftLocations.CheckedItems.Cast<string>().ToArray();

        item.itemCategory = txtCategory.Text.Trim();
        DialogResult = DialogResult.OK;
        Close();
    }
}
