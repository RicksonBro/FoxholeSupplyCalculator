using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class MoveItemForm : Form
{
    public int SelectedSubgroupIndex { get; private set; }
    public int QuantityToMove { get; private set; }

    private ComboBox cbSubgroups;
    private NumericUpDown nudQuantity;
    private Button btnOk;
    private Button btnCancel;

    public MoveItemForm(int subgroupCount, int maxQuantity)
    {

        Text = "Переместить предмет";
        Size = new Size(300, 200);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        Label lblGroup = new Label { Text = "В какую подгруппу:", Location = new Point(10, 20), AutoSize = true };
        cbSubgroups = new ComboBox { Location = new Point(150, 15), Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
        for (int i = 0; i < subgroupCount; i++)
            cbSubgroups.Items.Add($"Подгруппа {i + 1}");
        if (cbSubgroups.Items.Count > 0)
            cbSubgroups.SelectedIndex = 0;

        Label lblQuantity = new Label { Text = "Количество ящиков:", Location = new Point(10, 60), AutoSize = true };
        nudQuantity = new NumericUpDown { Location = new Point(150, 55), Width = 100, Minimum = 1, Maximum = maxQuantity, Value = 1 };

        btnOk = new Button { Text = "OK", Location = new Point(50, 110), DialogResult = DialogResult.OK };
        btnCancel = new Button { Text = "Отмена", Location = new Point(150, 110), DialogResult = DialogResult.Cancel };

        btnOk.Click += (s, e) =>
        {
            SelectedSubgroupIndex = cbSubgroups.SelectedIndex;
            QuantityToMove = (int)nudQuantity.Value;
            DialogResult = DialogResult.OK;
            Close();
        };


        Controls.Add(lblGroup);
        Controls.Add(cbSubgroups);
        Controls.Add(lblQuantity);
        Controls.Add(nudQuantity);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
    }
}
