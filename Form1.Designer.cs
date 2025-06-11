using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace FoxholeSupplyCalculator
{
    partial class Form1 : Form
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnLoadFile;
        private Button btnCalculate;
        private Button btnImportItems;
        private TextBox txtSoldiers;
        private TextBox txtQuotaInput;
        private ListBox lstResults;
        private Button btnAddItem;
        private ListBox lstItems;
        private Button btnDeleteItem;
        private Button btnSaveResults;

        private TabControl tabControl;
        private TabPage tabMain;
        private TabPage tabDataBase;
        private TabPage tabCalculate;

        private Label lblMatchedItems;
        private Label lblResults;
        private Label lblCurrentQuota;
        private DataGridView dataGridQuotaView;
        private ContextMenuStrip contextMenuGrid;
        private ToolStripMenuItem toolStripMenuItemDelete;
        private ToolStripMenuItem replaceMenuItem;
        private Button btnPasteFromClipboard;



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
            btnLoadFile = new Button();
            btnCalculate = new Button();
            txtSoldiers = new TextBox();
            txtQuotaInput = new TextBox();
            lstResults = new ListBox();
            btnAddItem = new Button();
            lstItems = new ListBox();
            btnDeleteItem = new Button();
            btnSaveResults = new Button();
            tabControl = new TabControl();
            tabMain = new TabPage();
            tabDataBase = new TabPage();
            tabCalculate = new TabPage();
            btnImportItems = new Button();
            lblMatchedItems = new Label();
            lblResults = new Label();
            lblCurrentQuota = new Label();
            dataGridQuotaView = new DataGridView();
            contextMenuGrid = new ContextMenuStrip();
            toolStripMenuItemDelete = new ToolStripMenuItem("Удалить");
            replaceMenuItem = new ToolStripMenuItem("Заменить");
            btnPasteFromClipboard = new Button();

            replaceMenuItem.Click += toolStripMenuItemReplace_Click;
            contextMenuGrid.Items.Add(replaceMenuItem);

            toolStripMenuItemDelete.Click += toolStripMenuItemDelete_Click;
            dataGridQuotaView.MouseDown += dataGridQuotaView_MouseDown;

            contextMenuGrid.Items.Add(toolStripMenuItemDelete);

            // radioFromText.CheckedChanged += radioFromText_CheckedChanged;
            // txtQuotaInput.Visible = false; // Скрываем по умолчанию


            btnPasteFromClipboard.Location = new Point(420, 80);
            btnPasteFromClipboard.Name = "btnPasteFromClipboard";
            btnPasteFromClipboard.Size = new Size(120, 40);
            btnPasteFromClipboard.TabIndex = 4;
            btnPasteFromClipboard.Text = "Вставить из буфера";
            btnPasteFromClipboard.UseVisualStyleBackColor = true;
            btnPasteFromClipboard.Click += new System.EventHandler(this.btnPasteFromClipboard_Click);

            // btnLoadQuota = new Button();
            // btnLoadQuota.Location = new Point(420, 40);
            // btnLoadQuota.Name = "btnLoadQuota";
            // btnLoadQuota.Size = new Size(120, 30);
            // // btnLoadQuota.TabIndex = 3;
            // btnLoadQuota.Text = "Загрузить квоту";
            // btnLoadQuota.UseVisualStyleBackColor = true;
            // btnLoadQuota.Click += new System.EventHandler(this.btnLoadQuota_Click);

            txtQuotaInput.Name = "txtQuotaInput";
            txtQuotaInput.Multiline = true;
            txtQuotaInput.ScrollBars = ScrollBars.Vertical;
            txtQuotaInput.Location = new Point(12, 70);
            txtQuotaInput.Size = new Size(300, 400);
            txtQuotaInput.TextChanged += new System.EventHandler(this.txtQuotaInput_TextChanged);

            // txtQuotaInput.TabIndex = 2;
            txtQuotaInput.Visible = true;
            //
            //dataGridQuotaView
            //
            dataGridQuotaView.AllowUserToAddRows = false;
            dataGridQuotaView.AllowUserToDeleteRows = false;
            dataGridQuotaView.ReadOnly = true;
            dataGridQuotaView.RowHeadersVisible = false;
            dataGridQuotaView.Location = new Point(12, 200);
            dataGridQuotaView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridQuotaView.Size = new Size(570, 800);
            dataGridQuotaView.ContextMenuStrip = contextMenuGrid;

            dataGridQuotaView.Columns.Add("QuotaItem", "Лог. план");
            dataGridQuotaView.Columns.Add("MatchedItem", "База данных");

            dataGridQuotaView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridQuotaView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabMain);
            tabControl.Controls.Add(tabCalculate);
            tabControl.Controls.Add(tabDataBase);
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(600, 560);

            // 
            // tabMain
            // 
            tabMain.Text = "Главная";
            tabMain.Name = "tabMain";
            tabMain.UseVisualStyleBackColor = true;

            // 
            // tabDataBase
            // 
            tabDataBase.Text = "База Данных";
            tabDataBase.Name = "tabDataBase";
            tabDataBase.UseVisualStyleBackColor = true;

            // 
            // tabCalculate
            // 
            tabCalculate.Text = "Калькулятор";
            tabCalculate.Name = "tabCalculate";
            tabCalculate.UseVisualStyleBackColor = true;

            // 
            // btnLoadFile
            // 
            btnLoadFile.Location = new Point(12, 12);
            btnLoadFile.Name = "btnLoadFile";
            btnLoadFile.Size = new Size(120, 30);
            btnLoadFile.TabIndex = 0;
            btnLoadFile.Text = "Загрузить файл";
            btnLoadFile.UseVisualStyleBackColor = true;
            btnLoadFile.Click += new System.EventHandler(btnLoadFile_Click);

            // 
            // btnCalculate
            // 
            btnCalculate.Location = new Point(12, 85);
            btnCalculate.Name = "btnCalculate";
            btnCalculate.Size = new Size(120, 30);
            btnCalculate.TabIndex = 1;
            btnCalculate.Text = "Рассчитать";
            btnCalculate.UseVisualStyleBackColor = true;
            btnCalculate.Click += new System.EventHandler(btnCalculate_Click);

            // 
            // txtSoldiers
            // 
            txtSoldiers.Location = new Point(300, 16);
            txtSoldiers.Name = "txtSoldiers";
            txtSoldiers.Size = new Size(120, 20);
            txtSoldiers.TabIndex = 2;
            // txtSoldiers.Text = "10";
            txtSoldiers.PlaceholderText = "Количество бойцов";

            // 
            // lblSoldiers
            // 
            // lblSoldiers.AutoSize = true;
            // lblSoldiers.Location = new Point(300, 12);
            // lblSoldiers.Name = "lblSoldiers";
            // lblSoldiers.Size = new Size(125, 13);
            // lblSoldiers.TabIndex = 3;
            // lblSoldiers.Text = "Количество бойцов:";

            //
            // lstResults
            // 
            lstResults.FormattingEnabled = true;
            lstResults.Location = new Point(150, 12);
            lstResults.Name = "lstResults";
            lstResults.Size = new Size(410, 270);
            lstResults.TabIndex = 4;

            // 
            // btnAddItem
            // 
            btnAddItem.Location = new Point(12, 12);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(120, 30);
            btnAddItem.Text = "Новый предмет";
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Click += new System.EventHandler(btnAddItem_Click);

            // 
            // btnImportItems
            btnImportItems.Location = new Point(350, 12);
            btnImportItems.Name = "btnImportItems";
            btnImportItems.Size = new Size(120, 50);
            btnImportItems.Text = "Импортировать базу данных";
            btnImportItems.Click += new System.EventHandler(btnImportItems_Click);
            // 
            // btnDeleteItem
            // 
            btnDeleteItem.Location = new Point(210, 12);
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.Size = new Size(120, 30);
            btnDeleteItem.Text = "Удалить предмет";
            btnDeleteItem.Click += new System.EventHandler(btnDeleteItem_Click);


            btnSaveResults.Location = new Point(12, 120);
            btnSaveResults.Name = "btnSaveResults";
            btnSaveResults.Size = new Size(120, 50);
            btnSaveResults.Text = "📄 Сохранить результат";
            btnSaveResults.Click += new System.EventHandler(btnSaveResults_Click);

            //lblMatchedItems
            // lblMatchedItems.Location = new Point(10, 384);
            // lblMatchedItems.Name = "lblMatchedItems";
            // lblMatchedItems.Size = new Size(200, 20);
            // lblMatchedItems.Text = "Совпадения в предметах:";

            // lstItems
            // 
            lstItems.FormattingEnabled = true;
            lstItems.Location = new Point(12, 80);
            lstItems.Name = "lstItems";
            lstItems.Size = new Size(560, 500);

            // 
            // tabMain Controls
            // 
            tabMain.Controls.Add(btnLoadFile);
            tabMain.Controls.Add(txtSoldiers);
            tabMain.Controls.Add(lblMatchedItems);
            tabMain.Controls.Add(dataGridQuotaView);
            tabMain.Controls.Add(txtQuotaInput);
            tabMain.Controls.Add(btnPasteFromClipboard);


            //tabDataBase Controls

            tabDataBase.Controls.Add(btnAddItem);
            tabDataBase.Controls.Add(btnDeleteItem);
            tabDataBase.Controls.Add(btnImportItems);
            tabDataBase.Controls.Add(lstItems);

            //tabCalculate Controls

            tabCalculate.Controls.Add(btnCalculate);
            tabCalculate.Controls.Add(lstResults);
            tabCalculate.Controls.Add(btnSaveResults);
            // 
            // Form1
            // 
            ClientSize = new Size(600, 560);
            Icon = new Icon("c0d15684e6c186289b50dfe083f5c562c57e8fb6.ico");
            Controls.Add(tabControl);
            Name = "Form1";
            Text = "Калькулятор для Foxhole";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
