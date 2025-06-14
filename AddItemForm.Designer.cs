using System.Windows.Forms;


namespace FoxholeSupplyCalculator
{
    partial class AddItemForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtId, txtName, txtNicknames, txtBmats, txtRmats, txtEmats, txtHemats;
        private ComboBox cmbcraftLocation, cmbProdBranch;
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
            this.cmbcraftLocation = new ComboBox();
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
            this.cmbcraftLocation.Location = new System.Drawing.Point(12, 208);
            this.cmbcraftLocation.Items.AddRange(new string[] { "mpf", "factory", "refinery" });
            this.cmbcraftLocation.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbcraftLocation.SelectedIndex = 0;

            this.cmbProdBranch.Location = new System.Drawing.Point(12, 236);
            this.cmbProdBranch.Items.AddRange(new string[] { "small_arms", "heavy_arms", "heavy_ammunition", "utilities", "medical", "supplies", "shipables", "vehicles", "uniforms" });
            this.cmbProdBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProdBranch.SelectedIndex = 0;

            // Save button
            this.btnSave.Location = new System.Drawing.Point(12, 260);
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(250, 280);
            this.Controls.AddRange(new Control[] {
                txtId, txtName, txtNicknames, txtBmats, txtRmats, txtEmats, txtHemats, cmbcraftLocation, cmbProdBranch, btnSave
            });

            this.Text = "Новый предмет";
            this.ResumeLayout(false);
        }
    }
}
