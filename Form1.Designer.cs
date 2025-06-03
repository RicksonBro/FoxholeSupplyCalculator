using System.Windows.Forms;

namespace FoxholeSupplyCalculator
{
    partial class Form1 : Form
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnLoadFile;
        private Button btnCalculate;
        private TextBox txtSoldiers;
        private Label lblSoldiers;
        private ListBox lstResults;
        private Button btnAddItem;
        private Button btnShowItems;
        private ListBox lstItems;
        private Button btnDeleteItem;
        private TabControl tabControl;
        private TabPage tabMain;
        private TabPage tabSettings;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnLoadFile = new Button();
            this.btnCalculate = new Button();
            this.txtSoldiers = new TextBox();
            this.lblSoldiers = new Label();
            this.lstResults = new ListBox();
            this.btnAddItem = new Button();
            this.btnShowItems = new Button();
            this.lstItems = new ListBox();
            this.btnDeleteItem = new Button();
            this.tabControl = new TabControl();
            this.tabMain = new TabPage();
            this.tabSettings = new TabPage();

            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabMain);
            this.tabControl.Controls.Add(this.tabSettings);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(600, 450);

            // 
            // tabMain
            // 
            this.tabMain.Text = "Главная";
            this.tabMain.Name = "tabMain";
            this.tabMain.UseVisualStyleBackColor = true;

            // 
            // tabSettings
            // 
            this.tabSettings.Text = "Настройки";
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.UseVisualStyleBackColor = true;

            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(12, 12);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(120, 30);
            this.btnLoadFile.TabIndex = 0;
            this.btnLoadFile.Text = "Загрузить файл";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);

            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(12, 85);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(120, 30);
            this.btnCalculate.TabIndex = 1;
            this.btnCalculate.Text = "Рассчитать";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);

            // 
            // txtSoldiers
            // 
            this.txtSoldiers.Location = new System.Drawing.Point(12, 59);
            this.txtSoldiers.Name = "txtSoldiers";
            this.txtSoldiers.Size = new System.Drawing.Size(120, 20);
            this.txtSoldiers.TabIndex = 2;
            this.txtSoldiers.Text = "10";

            // 
            // lblSoldiers
            // 
            this.lblSoldiers.AutoSize = true;
            this.lblSoldiers.Location = new System.Drawing.Point(12, 43);
            this.lblSoldiers.Name = "lblSoldiers";
            this.lblSoldiers.Size = new System.Drawing.Size(125, 13);
            this.lblSoldiers.TabIndex = 3;
            this.lblSoldiers.Text = "Количество бойцов:";

            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(150, 12);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(400, 290);
            this.lstResults.TabIndex = 4;

            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(12, 155);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(120, 30);
            this.btnAddItem.Text = "Новый предмет";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);

            // 
            // btnShowItems
            // 
            this.btnShowItems.Location = new System.Drawing.Point(12, 120);
            this.btnShowItems.Name = "btnShowItems";
            this.btnShowItems.Size = new System.Drawing.Size(120, 30);
            this.btnShowItems.Text = "Предметы";
            this.btnShowItems.Click += new System.EventHandler(this.btnShowItems_Click);

            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Location = new System.Drawing.Point(12, 190);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteItem.Text = "Удалить предмет";
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);

            // 
            // lstItems
            // 
            this.lstItems.FormattingEnabled = true;
            this.lstItems.Location = new System.Drawing.Point(12, 230);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(560, 200);

            // 
            // tabMain Controls
            // 
            this.tabMain.Controls.Add(this.btnLoadFile);
            this.tabMain.Controls.Add(this.btnCalculate);
            this.tabMain.Controls.Add(this.txtSoldiers);
            this.tabMain.Controls.Add(this.lblSoldiers);
            this.tabMain.Controls.Add(this.lstResults);
            this.tabMain.Controls.Add(this.btnAddItem);
            this.tabMain.Controls.Add(this.btnShowItems);
            this.tabMain.Controls.Add(this.btnDeleteItem);
            this.tabMain.Controls.Add(this.lstItems);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.tabControl);
            this.Name = "Form1";
            this.Text = "Калькулятор для Foxhole";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
