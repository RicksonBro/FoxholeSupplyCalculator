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
        private Button btnDarkMode;
        private CheckBox checkbxEdit;
        private CheckBox checkbxShowQuota;
        private Button btnImportItems;
        private TextBox txtQuotaInput;
        private TextBox txtSearchDB;
        private ListBox lstResults;
        private Button btnAddItem;
        private ListBox lstItems;
        private Button btnSaveResults;
        private Button btnAddSubgroup;
        private Button btnRemoveSubgroup;
        private Button copyResultsFromClipboard;
        private Button btnDeleteQuota;
        private Button btnReloadDG;

        private TabControl tabControl;
        private TabPage tabMain;
        private TabPage tabDataBase;
        private TabPage tabCalculate;

        private Label lblMatchedItems;
        private Label lblResults;
        private Label lblCurrentQuota;
        private Label lblSubgroups;

        private DataGridView dataGridQuotaView;
        private ContextMenuStrip contextMenuGrid;
        private ContextMenuStrip resultContextMenu;
        private ContextMenuStrip lstItemsMenu;
        private ToolStripMenuItem toolStripItemsDelete;
        private ToolStripMenuItem toolStripItemsEdit;
        private ToolStripMenuItem toolStripMenuItemDelete;
        private ToolStripMenuItem replaceMenuItem;
        private ToolStripMenuItem replaceSaveItem;
        private Button btnPasteFromClipboard;
        private FlowLayoutPanel panelSubgroups;
        private Button btnApplySubgroupCount;
        private NumericUpDown numericSubgroupCount;
        private Button btnAddItemDG;


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
            btnDarkMode = new Button();
            txtQuotaInput = new TextBox();
            lstResults = new ListBox();
            btnAddItem = new Button();
            lstItems = new ListBox();
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
            lstItemsMenu = new ContextMenuStrip();
            toolStripItemsDelete = new ToolStripMenuItem("Удалить");
            toolStripItemsEdit = new ToolStripMenuItem("Редактировать");
            toolStripMenuItemDelete = new ToolStripMenuItem("Удалить");
            replaceMenuItem = new ToolStripMenuItem("Заменить");
            replaceSaveItem = new ToolStripMenuItem("Заменить и сохранить");
            btnPasteFromClipboard = new Button();
            checkbxEdit = new CheckBox();
            checkbxShowQuota = new CheckBox();
            btnAddSubgroup = new Button();
            panelSubgroups = new FlowLayoutPanel();
            lblSubgroups = new Label();
            btnRemoveSubgroup = new Button();
            numericSubgroupCount = new NumericUpDown();
            resultContextMenu = new ContextMenuStrip();
            txtSearchDB = new TextBox();
            var moveItem = new ToolStripMenuItem("Переместить");
            copyResultsFromClipboard = new Button();
            btnDeleteQuota = new Button();
            btnReloadDG = new Button();
            btnAddItemDG = new Button();
            moveItem.Click += MoveItem_Click;
            toolStripItemsDelete.Click += btnDeleteItem_Click;
            toolStripItemsEdit.Click += toolStripItemsEdit_Click;
            resultContextMenu.Items.Add(moveItem);
            lstItemsMenu.Items.Add(toolStripItemsDelete);

            lstItemsMenu.Items.Add(toolStripItemsEdit);
            lstResults.MouseDown += lstResults_MouseDown;

            lstResults.ContextMenuStrip = resultContextMenu;

            // NumericUpDown (ввод количества)
            btnDarkMode.Size = new Size(40, 40);
            // btnDarkMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDarkMode.Location = new Point(500, 12);
            btnDarkMode.Name = "btnDarkMode";
            btnDarkMode.Click += new System.EventHandler(this.btnDarkMode_Click);
            btnDarkMode.ImageAlign = ContentAlignment.MiddleCenter;
            btnDarkMode.Text = "";
            btnDarkMode.Image = Image.FromFile("img/moon-icon.png");

            btnReloadDG.Location = new Point(150, -330);
            btnReloadDG.Size = new Size(25, 25);
            btnReloadDG.Name = "btnReloadDG";
            btnReloadDG.Click += new System.EventHandler(this.btnReloadDG_Click);
            btnReloadDG.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnReloadDG.Image = Image.FromFile("img/reload-icon.png");

            btnAddItemDG.Location = new Point(12, -340);
            btnAddItemDG.Size = new Size(130, 30);
            btnAddItemDG.Name = "btnAddItemDG";
            btnAddItemDG.Text = "Добавить предмет";
            btnAddItemDG.Click += new System.EventHandler(this.btnAddItemDG_Click);
            btnAddItemDG.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            // btnAddItemDG.Image = Image.FromFile("img/reload-icon.png");

            txtSearchDB.Location = new Point(0, -421);
            txtSearchDB.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            txtSearchDB.Size = new Size(300, 30);
            txtSearchDB.PlaceholderText = "🔍 Поиск";
            txtSearchDB.TextChanged += new System.EventHandler(this.txtSearchDB_TextChanged);


            numericSubgroupCount.Location = new Point(140, -235);
            numericSubgroupCount.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            numericSubgroupCount.Size = new Size(50, 26);
            numericSubgroupCount.Minimum = 1;
            numericSubgroupCount.Maximum = 100;
            numericSubgroupCount.Value = 1;
            numericSubgroupCount.Name = "numericSubgroupCount";

            copyResultsFromClipboard.Location = new Point(12, 180);
            copyResultsFromClipboard.Size = new Size(120, 50);
            copyResultsFromClipboard.Name = "copyResultsFromClipboard";
            copyResultsFromClipboard.Text = "Скопировать в буфер обмена";
            copyResultsFromClipboard.Click += new System.EventHandler(this.copyResultsFromClipboard_Click);

            // Текстовое поле для ввода количества подгрупп

            btnDeleteQuota.Location = new Point(420, 140);
            btnDeleteQuota.Size = new Size(120, 40);
            btnDeleteQuota.Text = "Удалить квоту";
            btnDeleteQuota.Name = "btnDeleteQuota";
            btnDeleteQuota.Click += new System.EventHandler(this.btnDeleteQuota_Click);

            // Кнопка "Применить"
            btnApplySubgroupCount = new Button();
            btnApplySubgroupCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnApplySubgroupCount.Location = new Point(numericSubgroupCount.Right + 10, -235);
            btnApplySubgroupCount.Size = new Size(80, 26);
            btnApplySubgroupCount.Name = "btnApplySubgroupCount";
            btnApplySubgroupCount.Text = "Применить";
            btnApplySubgroupCount.Click += new System.EventHandler(this.btnApplySubgroupCount_Click);


            replaceMenuItem.Click += toolStripMenuItemReplace_Click;
            contextMenuGrid.Items.Add(replaceMenuItem);

            replaceSaveItem.Click += toolStripMenuItemReplaceSave_Click;
            contextMenuGrid.Items.Add(replaceSaveItem);

            toolStripMenuItemDelete.Click += toolStripMenuItemDelete_Click;
            dataGridQuotaView.MouseDown += dataGridQuotaView_MouseDown;
            dataGridQuotaView.CellValueChanged += dataGridQuotaView_CellValueChanged;
            // dataGridQuotaView.CurrentCellDirtyStateChanged += dataGridQuotaView_CurrentCellDirtyStateChanged;


            contextMenuGrid.Items.Add(toolStripMenuItemDelete);

            // radioFromText.CheckedChanged += radioFromText_CheckedChanged;
            // txtQuotaInput.Visible = false; // Скрываем по умолчанию
            btnRemoveSubgroup.Location = new Point(10, -240);
            btnRemoveSubgroup.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnRemoveSubgroup.Name = "btnRemoveSubgroup";
            btnRemoveSubgroup.Size = new Size(60, 30);
            btnRemoveSubgroup.TextAlign = ContentAlignment.MiddleCenter;
            btnRemoveSubgroup.TabIndex = 0;
            btnRemoveSubgroup.Text = "-";
            btnRemoveSubgroup.UseVisualStyleBackColor = true;
            btnRemoveSubgroup.Click += new System.EventHandler(this.btnRemoveSubgroup_Click);

            lblSubgroups.AutoSize = true;
            lblSubgroups.Location = new Point(10, -260);
            lblSubgroups.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            lblSubgroups.Name = "lblSubgroups";
            lblSubgroups.Size = new Size(125, 13);
            lblSubgroups.TabIndex = 3;
            lblSubgroups.Text = "Кол-во подгрупп:";

            btnAddSubgroup.Location = new Point(75, -240);
            btnAddSubgroup.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnAddSubgroup.Name = "btnAddSubgroup";
            btnAddSubgroup.Size = new Size(60, 30);
            btnAddSubgroup.TextAlign = ContentAlignment.MiddleCenter;
            btnAddSubgroup.TabIndex = 0;
            btnAddSubgroup.Text = "+";
            btnAddSubgroup.UseVisualStyleBackColor = true;
            btnAddSubgroup.Click += new System.EventHandler(this.btnAddSubgroup_Click);

            // 
            // panelSubgroups
            // 
            this.panelSubgroups.Dock = DockStyle.Bottom;
            panelSubgroups.AutoScroll = true;
            panelSubgroups.FlowDirection = FlowDirection.TopDown | FlowDirection.LeftToRight;
            panelSubgroups.Location = new Point(12, 330);
            panelSubgroups.Name = "panelSubgroups";
            panelSubgroups.Size = new Size(400, 300);
            panelSubgroups.TabIndex = 1;
            panelSubgroups.WrapContents = false;
            panelSubgroups.Padding = new Padding(3);
            panelSubgroups.BackColor = Color.WhiteSmoke;


            // 
            // Form1
            // 
            // this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            // this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            // this.ClientSize = new Size(384, 440); // или больше, если нужно


            this.Name = "Form1";
            this.Text = "Подгруппы распределения";
            this.ResumeLayout(false);


            btnPasteFromClipboard.Location = new Point(420, 80);
            btnPasteFromClipboard.Name = "btnPasteFromClipboard";
            btnPasteFromClipboard.Size = new Size(120, 40);
            btnPasteFromClipboard.TabIndex = 4;
            btnPasteFromClipboard.Text = "Вставить из буфера";
            btnPasteFromClipboard.UseVisualStyleBackColor = true;
            btnPasteFromClipboard.Click += new System.EventHandler(this.btnPasteFromClipboard_Click);

            checkbxEdit.Location = new Point(200, 12);
            checkbxEdit.Size = new Size(190, 25);
            checkbxEdit.Name = "checkbxEdit";
            checkbxEdit.Text = "Редактировать текст квоты";
            checkbxEdit.Checked = false;
            checkbxEdit.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChangedEdit);
            checkbxEdit.Enabled = true;

            checkbxShowQuota.Size = new Size(150, 30);
            checkbxShowQuota.Location = new Point(200, 36);
            checkbxShowQuota.Name = "checkbxShowQuota";
            checkbxShowQuota.Checked = true;
            checkbxShowQuota.Text = "Показать текст квоты";
            checkbxShowQuota.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChangedQShow);


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
            txtQuotaInput.Size = new Size(400, 100);
            txtQuotaInput.ReadOnly = true;
            txtQuotaInput.TextChanged += new System.EventHandler(this.txtQuotaInput_TextChanged);
            txtQuotaInput.Visible = true;
            //
            //dataGridQuotaView
            //
            dataGridQuotaView.AllowUserToAddRows = false;
            dataGridQuotaView.AllowUserToDeleteRows = false;
            dataGridQuotaView.ReadOnly = false;
            dataGridQuotaView.RowHeadersVisible = false;
            dataGridQuotaView.Dock = DockStyle.Bottom;
            // dataGridQuotaView.Location = new Point(12, 250);
            dataGridQuotaView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridQuotaView.Size = new Size(570, 400);
            dataGridQuotaView.ContextMenuStrip = contextMenuGrid;
            dataGridQuotaView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridQuotaView.ScrollBars = ScrollBars.Vertical;
            dataGridQuotaView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridQuotaView.AllowUserToAddRows = false;
            dataGridQuotaView.CellClick += dataGridQuotaView_CellClick;


            dataGridQuotaView.Columns.Add("Quantity", "Кол-во ящиков");
            dataGridQuotaView.Columns.Add("QuotaItem", "Лог. план");
            dataGridQuotaView.Columns.Add("MatchedItem", "База данных");
            dataGridQuotaView.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                HeaderText = "✔",
                Name = "SelectColumn",
                Width = 30,
                ReadOnly = false,           // ВАЖНО: столбец должен быть редактируемым
                TrueValue = true,
                FalseValue = false,
            });
            dataGridQuotaView.EditMode = DataGridViewEditMode.EditOnEnter;

            foreach (DataGridViewColumn column in dataGridQuotaView.Columns)
            {
                column.ReadOnly = column.Index != 3;
            }

            dataGridQuotaView.Columns[0].ReadOnly = false;
            dataGridQuotaView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // dataGridQuotaView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridQuotaView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridQuotaView.Columns[0].Width = 50;
            dataGridQuotaView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridQuotaView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridQuotaView.Columns[3].Width = 50;

            dataGridQuotaView.CellPainting += dataGridQuotaView_CellPainting;
            dataGridQuotaView.CellBeginEdit += dataGridQuotaView_CellBeginEdit;


            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabMain);
            tabControl.Controls.Add(tabCalculate);
            tabControl.Controls.Add(tabDataBase);
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            // tabControl.Size = new Size(ClientSize.Width, ClientSize.Height);
            tabControl.SelectedIndex = 0;
            tabControl.Dock = DockStyle.Fill;
            // tabControl.Margin = new Padding(10);
            // tabControl.Padding = new Point(5, 6);
            // tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            // tabControl.Size = new Size(600, 700);

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
            btnLoadFile.Anchor = AnchorStyles.Top | AnchorStyles.Left;
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
            lstResults.Location = new Point(-150, -250);
            lstResults.Anchor = AnchorStyles.Right;
            lstResults.Name = "lstResults";
            lstResults.Size = new Size(320, 200);
            // lstResults.TabIndex = 4;

            // 
            // btnAddItem
            // 
            btnAddItem.Location = new Point(12, 12);
            btnAddItem.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(120, 30);
            btnAddItem.Text = "Новый предмет";
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Click += new System.EventHandler(btnAddItem_Click);

            //
            // 
            // btnImportItems
            btnImportItems.Location = new Point(150, 12);
            btnImportItems.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            btnImportItems.Name = "btnImportItems";
            btnImportItems.Size = new Size(200, 30);
            btnImportItems.Text = "Импортировать базу данных";
            btnImportItems.Click += new System.EventHandler(btnImportItems_Click);
            // 
            // btnDeleteItem
            // 


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
            lstItems.ContextMenuStrip = lstItemsMenu;
            // lstItems.Location = new Point(12, 80);
            lstItems.Dock = DockStyle.Bottom;
            lstItems.Name = "lstItems";
            lstItems.Size = new Size(560, 500);

            // 
            // tabMain Controls
            //

            // Image newImage = Image.FromFile("mqdefault.jpg");

            // Create Point for upper-left corner of image.
            Point ulCorner = new Point(100, 100);

            tabMain.Controls.Add(btnLoadFile);
            tabMain.Controls.Add(lblMatchedItems);
            tabMain.Controls.Add(dataGridQuotaView);
            tabMain.Controls.Add(txtQuotaInput);
            tabMain.Controls.Add(btnPasteFromClipboard);
            tabMain.Controls.Add(checkbxEdit);
            tabMain.Controls.Add(checkbxShowQuota);
            tabMain.Controls.Add(btnDarkMode);
            tabMain.Controls.Add(btnDeleteQuota);
            tabMain.Controls.Add(btnReloadDG);
            tabMain.Controls.Add(btnAddItemDG);
            // tabMain.BackgroundImage = newImage;


            //tabDataBase Controls

            tabDataBase.Controls.Add(btnAddItem);
            tabDataBase.Controls.Add(btnImportItems);
            tabDataBase.Controls.Add(txtSearchDB);
            tabDataBase.Controls.Add(lstItems);

            //tabCalculate Controls

            tabCalculate.Controls.Add(btnCalculate);
            tabCalculate.Controls.Add(lstResults);
            tabCalculate.Controls.Add(btnSaveResults);
            tabCalculate.Controls.Add(btnAddSubgroup);
            tabCalculate.Controls.Add(btnRemoveSubgroup);
            tabCalculate.Controls.Add(panelSubgroups);
            tabCalculate.Controls.Add(lblSubgroups);
            tabCalculate.Controls.Add(btnApplySubgroupCount);
            tabCalculate.Controls.Add(btnApplySubgroupCount);
            tabCalculate.Controls.Add(numericSubgroupCount);
            tabCalculate.Controls.Add(copyResultsFromClipboard);

            // 
            // Form1

            AutoSize = true;
            StartPosition = FormStartPosition.CenterScreen;
            //         this.SetBounds((Screen.GetBounds(this).Width / 2) - (this.Width / 2),
            //    (Screen.GetBounds(this).Height / 2) - (this.Height / 2),
            //    this.Width, this.Height, BoundsSpecified.Location);
            MinimumSize = new Size(600, 700);
            ClientSize = new Size(600, 700);
            Icon = new Icon("img/icon.ico");
            Controls.Add(tabControl);
            Name = "Form1";
            Text = "Калькулятор для Foxhole";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
