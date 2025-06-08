using System.Windows.Forms;


namespace FoxholeSupplyCalculator
{
    partial class AddItemForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtId, txtName, txtNicknames, txtBmats, txtRmats, txtEmats, txtHemats;
        private ComboBox cmbFabric, cmbProdBranch;
        private Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtId = new TextBox();
            this.txtName = new TextBox();
            this.txtNicknames = new TextBox();
            this.txtBmats = new TextBox();
            this.txtRmats = new TextBox();
            this.txtEmats = new TextBox();
            this.txtHemats = new TextBox();
            this.cmbFabric = new ComboBox();
            this.cmbProdBranch = new ComboBox();
            this.btnSave = new Button();

            this.SuspendLayout();

            // ID
            this.txtId.Location = new System.Drawing.Point(12, 12);
            this.txtId.PlaceholderText = "ID";

            // Name
            this.txtName.Location = new System.Drawing.Point(12, 40);
            this.txtName.PlaceholderText = "Название";

            // Nicknames
            this.txtNicknames.Location = new System.Drawing.Point(12, 68);
            this.txtNicknames.PlaceholderText = "Синонимы через запятую";

            // Bmats
            this.txtBmats.Location = new System.Drawing.Point(12, 96);
            this.txtBmats.PlaceholderText = "Bmats";

            // Rmats
            this.txtRmats.Location = new System.Drawing.Point(12, 124);
            this.txtRmats.PlaceholderText = "Rmats";

            // Emats
            this.txtEmats.Location = new System.Drawing.Point(12, 152);
            this.txtEmats.PlaceholderText = "Emats";

            // Hemats
            this.txtHemats.Location = new System.Drawing.Point(12, 180);
            this.txtHemats.PlaceholderText = "Hemats";

            // ComboBox
            this.cmbFabric.Location = new System.Drawing.Point(12, 208);
            this.cmbFabric.Items.AddRange(new string[] { "mass", "common", "recycler" });
            this.cmbFabric.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFabric.SelectedIndex = 0;

            this.cmbProdBranch.Location = new System.Drawing.Point(12, 236);
            this.cmbProdBranch.Items.AddRange(new string[] { "light", "heavy", "shell", "engineering", "supply", "medicine", "uniform", "vehicle", "yard" });
            this.cmbProdBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProdBranch.SelectedIndex = 0;

            // Save button
            this.btnSave.Location = new System.Drawing.Point(12, 260);
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(250, 280);
            this.Controls.AddRange(new Control[] {
                txtId, txtName, txtNicknames, txtBmats, txtRmats, txtEmats, txtHemats, cmbFabric, cmbProdBranch, btnSave
            });

            this.Text = "Новый предмет";
            this.ResumeLayout(false);
        }
    }
}
