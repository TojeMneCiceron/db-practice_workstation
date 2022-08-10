namespace arm
{
    partial class Tables
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tControl = new System.Windows.Forms.TabControl();
            this.tpApp = new System.Windows.Forms.TabPage();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.tpProc = new System.Windows.Forms.TabPage();
            this.dgvProcedures = new System.Windows.Forms.DataGridView();
            this.tpPets = new System.Windows.Forms.TabPage();
            this.dgvPets = new System.Windows.Forms.DataGridView();
            this.tpSpecies = new System.Windows.Forms.TabPage();
            this.dgvSpecies = new System.Windows.Forms.DataGridView();
            this.tpUsers = new System.Windows.Forms.TabPage();
            this.lv = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tpNameSearch = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bNameSearchCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bNameSearch = new System.Windows.Forms.Button();
            this.tbNameSearch = new System.Windows.Forms.TextBox();
            this.lb404name = new System.Windows.Forms.Label();
            this.dgvNameSearch = new System.Windows.Forms.DataGridView();
            this.tpSpeciesSearch = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cmbSpeciesSearch = new System.Windows.Forms.ComboBox();
            this.bSpeciesSearchCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bSpeciesSearch = new System.Windows.Forms.Button();
            this.lb404species = new System.Windows.Forms.Label();
            this.dgvSpeciesSearch = new System.Windows.Forms.DataGridView();
            this.tControl.SuspendLayout();
            this.tpApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.tpProc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcedures)).BeginInit();
            this.tpPets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPets)).BeginInit();
            this.tpSpecies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpecies)).BeginInit();
            this.tpUsers.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tpNameSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNameSearch)).BeginInit();
            this.tpSpeciesSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpeciesSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // tControl
            // 
            this.tControl.Controls.Add(this.tpApp);
            this.tControl.Controls.Add(this.tpProc);
            this.tControl.Controls.Add(this.tpPets);
            this.tControl.Controls.Add(this.tpSpecies);
            this.tControl.Controls.Add(this.tpUsers);
            this.tControl.Controls.Add(this.tpNameSearch);
            this.tControl.Controls.Add(this.tpSpeciesSearch);
            this.tControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tControl.Location = new System.Drawing.Point(0, 0);
            this.tControl.Name = "tControl";
            this.tControl.SelectedIndex = 0;
            this.tControl.Size = new System.Drawing.Size(800, 472);
            this.tControl.TabIndex = 0;
            this.tControl.SelectedIndexChanged += new System.EventHandler(this.tControl_SelectedIndexChanged);
            // 
            // tpApp
            // 
            this.tpApp.Controls.Add(this.dgvAppointments);
            this.tpApp.Location = new System.Drawing.Point(4, 22);
            this.tpApp.Name = "tpApp";
            this.tpApp.Padding = new System.Windows.Forms.Padding(3);
            this.tpApp.Size = new System.Drawing.Size(792, 446);
            this.tpApp.TabIndex = 0;
            this.tpApp.Text = "Приемы";
            this.tpApp.UseVisualStyleBackColor = true;
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAppointments.Location = new System.Drawing.Point(3, 3);
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.Size = new System.Drawing.Size(786, 440);
            this.dgvAppointments.TabIndex = 0;
            this.dgvAppointments.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAppointments_CellValueChanged);
            this.dgvAppointments.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAppointments_CurrentCellDirtyStateChanged);
            this.dgvAppointments.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvAppointments_RowValidating);
            this.dgvAppointments.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvAppointments_UserDeletingRow);
            this.dgvAppointments.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvAppointments_PreviewKeyDown);
            // 
            // tpProc
            // 
            this.tpProc.Controls.Add(this.dgvProcedures);
            this.tpProc.Location = new System.Drawing.Point(4, 22);
            this.tpProc.Name = "tpProc";
            this.tpProc.Padding = new System.Windows.Forms.Padding(3);
            this.tpProc.Size = new System.Drawing.Size(768, 422);
            this.tpProc.TabIndex = 1;
            this.tpProc.Text = "Процедуры";
            this.tpProc.UseVisualStyleBackColor = true;
            // 
            // dgvProcedures
            // 
            this.dgvProcedures.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProcedures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProcedures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProcedures.Location = new System.Drawing.Point(3, 3);
            this.dgvProcedures.Name = "dgvProcedures";
            this.dgvProcedures.Size = new System.Drawing.Size(762, 416);
            this.dgvProcedures.TabIndex = 0;
            this.dgvProcedures.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProcedures_CellValueChanged);
            this.dgvProcedures.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvProcedures_RowValidating);
            this.dgvProcedures.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvProcedures_UserDeletingRow);
            this.dgvProcedures.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvProcedures_PreviewKeyDown);
            // 
            // tpPets
            // 
            this.tpPets.Controls.Add(this.dgvPets);
            this.tpPets.Location = new System.Drawing.Point(4, 22);
            this.tpPets.Name = "tpPets";
            this.tpPets.Padding = new System.Windows.Forms.Padding(3);
            this.tpPets.Size = new System.Drawing.Size(768, 422);
            this.tpPets.TabIndex = 2;
            this.tpPets.Text = "Питомцы";
            this.tpPets.UseVisualStyleBackColor = true;
            // 
            // dgvPets
            // 
            this.dgvPets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPets.Location = new System.Drawing.Point(3, 3);
            this.dgvPets.Name = "dgvPets";
            this.dgvPets.Size = new System.Drawing.Size(762, 416);
            this.dgvPets.TabIndex = 0;
            this.dgvPets.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPets_CellValueChanged);
            this.dgvPets.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvPets_CurrentCellDirtyStateChanged);
            this.dgvPets.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvPets_RowValidating);
            this.dgvPets.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvPets_UserDeletingRow);
            this.dgvPets.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvPets_PreviewKeyDown);
            // 
            // tpSpecies
            // 
            this.tpSpecies.Controls.Add(this.dgvSpecies);
            this.tpSpecies.Location = new System.Drawing.Point(4, 22);
            this.tpSpecies.Name = "tpSpecies";
            this.tpSpecies.Padding = new System.Windows.Forms.Padding(3);
            this.tpSpecies.Size = new System.Drawing.Size(768, 422);
            this.tpSpecies.TabIndex = 3;
            this.tpSpecies.Text = "Виды";
            this.tpSpecies.UseVisualStyleBackColor = true;
            // 
            // dgvSpecies
            // 
            this.dgvSpecies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSpecies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSpecies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSpecies.Location = new System.Drawing.Point(3, 3);
            this.dgvSpecies.Name = "dgvSpecies";
            this.dgvSpecies.Size = new System.Drawing.Size(762, 416);
            this.dgvSpecies.TabIndex = 0;
            this.dgvSpecies.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSpecies_CellValueChanged);
            this.dgvSpecies.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvSpecies_RowValidating);
            this.dgvSpecies.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvSpecies_UserDeletingRow);
            this.dgvSpecies.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvSpecies_PreviewKeyDown);
            // 
            // tpUsers
            // 
            this.tpUsers.Controls.Add(this.lv);
            this.tpUsers.Controls.Add(this.menuStrip1);
            this.tpUsers.Location = new System.Drawing.Point(4, 22);
            this.tpUsers.Name = "tpUsers";
            this.tpUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tpUsers.Size = new System.Drawing.Size(768, 422);
            this.tpUsers.TabIndex = 4;
            this.tpUsers.Text = "Пользователи";
            this.tpUsers.UseVisualStyleBackColor = true;
            // 
            // lv
            // 
            this.lv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv.FullRowSelect = true;
            this.lv.HideSelection = false;
            this.lv.Location = new System.Drawing.Point(3, 27);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(762, 392);
            this.lv.TabIndex = 0;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьToolStripMenuItem,
            this.редактироватьToolStripMenuItem,
            this.удалитьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(762, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // добавитьToolStripMenuItem
            // 
            this.добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
            this.добавитьToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.добавитьToolStripMenuItem.Text = "Добавить";
            this.добавитьToolStripMenuItem.Click += new System.EventHandler(this.добавитьToolStripMenuItem_Click);
            // 
            // редактироватьToolStripMenuItem
            // 
            this.редактироватьToolStripMenuItem.Name = "редактироватьToolStripMenuItem";
            this.редактироватьToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.редактироватьToolStripMenuItem.Text = "Редактировать";
            this.редактироватьToolStripMenuItem.Click += new System.EventHandler(this.редактироватьToolStripMenuItem_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // tpNameSearch
            // 
            this.tpNameSearch.Controls.Add(this.splitContainer1);
            this.tpNameSearch.Location = new System.Drawing.Point(4, 22);
            this.tpNameSearch.Name = "tpNameSearch";
            this.tpNameSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpNameSearch.Size = new System.Drawing.Size(768, 422);
            this.tpNameSearch.TabIndex = 5;
            this.tpNameSearch.Text = "Поиск записей на прием по имени питомца";
            this.tpNameSearch.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.bNameSearchCancel);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.bNameSearch);
            this.splitContainer1.Panel1.Controls.Add(this.tbNameSearch);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lb404name);
            this.splitContainer1.Panel2.Controls.Add(this.dgvNameSearch);
            this.splitContainer1.Size = new System.Drawing.Size(762, 416);
            this.splitContainer1.SplitterDistance = 32;
            this.splitContainer1.TabIndex = 0;
            // 
            // bNameSearchCancel
            // 
            this.bNameSearchCancel.Location = new System.Drawing.Point(538, 5);
            this.bNameSearchCancel.Name = "bNameSearchCancel";
            this.bNameSearchCancel.Size = new System.Drawing.Size(75, 23);
            this.bNameSearchCancel.TabIndex = 3;
            this.bNameSearchCancel.Text = "Сбросить";
            this.bNameSearchCancel.UseVisualStyleBackColor = true;
            this.bNameSearchCancel.Click += new System.EventHandler(this.bNameSearchCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(150, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Введите имя питомца";
            // 
            // bNameSearch
            // 
            this.bNameSearch.Location = new System.Drawing.Point(457, 5);
            this.bNameSearch.Name = "bNameSearch";
            this.bNameSearch.Size = new System.Drawing.Size(75, 23);
            this.bNameSearch.TabIndex = 1;
            this.bNameSearch.Text = "Найти";
            this.bNameSearch.UseVisualStyleBackColor = true;
            this.bNameSearch.Click += new System.EventHandler(this.bNameSearch_Click);
            // 
            // tbNameSearch
            // 
            this.tbNameSearch.Location = new System.Drawing.Point(274, 6);
            this.tbNameSearch.Name = "tbNameSearch";
            this.tbNameSearch.Size = new System.Drawing.Size(177, 20);
            this.tbNameSearch.TabIndex = 0;
            // 
            // lb404name
            // 
            this.lb404name.AutoSize = true;
            this.lb404name.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lb404name.Location = new System.Drawing.Point(322, 15);
            this.lb404name.Name = "lb404name";
            this.lb404name.Size = new System.Drawing.Size(109, 13);
            this.lb404name.TabIndex = 4;
            this.lb404name.Text = "Записи отсутствуют";
            this.lb404name.Visible = false;
            // 
            // dgvNameSearch
            // 
            this.dgvNameSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNameSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNameSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNameSearch.Location = new System.Drawing.Point(0, 0);
            this.dgvNameSearch.Name = "dgvNameSearch";
            this.dgvNameSearch.Size = new System.Drawing.Size(762, 380);
            this.dgvNameSearch.TabIndex = 0;
            this.dgvNameSearch.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvNameSearch_UserDeletingRow);
            // 
            // tpSpeciesSearch
            // 
            this.tpSpeciesSearch.Controls.Add(this.splitContainer2);
            this.tpSpeciesSearch.Location = new System.Drawing.Point(4, 22);
            this.tpSpeciesSearch.Name = "tpSpeciesSearch";
            this.tpSpeciesSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpSpeciesSearch.Size = new System.Drawing.Size(768, 422);
            this.tpSpeciesSearch.TabIndex = 6;
            this.tpSpeciesSearch.Text = "Поиск питомцев по виду";
            this.tpSpeciesSearch.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cmbSpeciesSearch);
            this.splitContainer2.Panel1.Controls.Add(this.bSpeciesSearchCancel);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.bSpeciesSearch);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lb404species);
            this.splitContainer2.Panel2.Controls.Add(this.dgvSpeciesSearch);
            this.splitContainer2.Size = new System.Drawing.Size(762, 416);
            this.splitContainer2.SplitterDistance = 32;
            this.splitContainer2.TabIndex = 1;
            // 
            // cmbSpeciesSearch
            // 
            this.cmbSpeciesSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSpeciesSearch.FormattingEnabled = true;
            this.cmbSpeciesSearch.Location = new System.Drawing.Point(254, 6);
            this.cmbSpeciesSearch.Name = "cmbSpeciesSearch";
            this.cmbSpeciesSearch.Size = new System.Drawing.Size(177, 21);
            this.cmbSpeciesSearch.TabIndex = 4;
            this.cmbSpeciesSearch.SelectedValueChanged += new System.EventHandler(this.cmbSpeciesSearch_SelectedValueChanged);
            // 
            // bSpeciesSearchCancel
            // 
            this.bSpeciesSearchCancel.Location = new System.Drawing.Point(518, 5);
            this.bSpeciesSearchCancel.Name = "bSpeciesSearchCancel";
            this.bSpeciesSearchCancel.Size = new System.Drawing.Size(75, 23);
            this.bSpeciesSearchCancel.TabIndex = 3;
            this.bSpeciesSearchCancel.Text = "Сбросить";
            this.bSpeciesSearchCancel.UseVisualStyleBackColor = true;
            this.bSpeciesSearchCancel.Click += new System.EventHandler(this.bSpeciesSearchCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Выберите вид";
            // 
            // bSpeciesSearch
            // 
            this.bSpeciesSearch.Location = new System.Drawing.Point(437, 5);
            this.bSpeciesSearch.Name = "bSpeciesSearch";
            this.bSpeciesSearch.Size = new System.Drawing.Size(75, 23);
            this.bSpeciesSearch.TabIndex = 1;
            this.bSpeciesSearch.Text = "Найти";
            this.bSpeciesSearch.UseVisualStyleBackColor = true;
            this.bSpeciesSearch.Click += new System.EventHandler(this.bSpeciesSearch_Click);
            // 
            // lb404species
            // 
            this.lb404species.AutoSize = true;
            this.lb404species.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lb404species.Location = new System.Drawing.Point(322, 15);
            this.lb404species.Name = "lb404species";
            this.lb404species.Size = new System.Drawing.Size(109, 13);
            this.lb404species.TabIndex = 4;
            this.lb404species.Text = "Записи отсутствуют";
            this.lb404species.Visible = false;
            // 
            // dgvSpeciesSearch
            // 
            this.dgvSpeciesSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSpeciesSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSpeciesSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSpeciesSearch.Location = new System.Drawing.Point(0, 0);
            this.dgvSpeciesSearch.Name = "dgvSpeciesSearch";
            this.dgvSpeciesSearch.Size = new System.Drawing.Size(762, 380);
            this.dgvSpeciesSearch.TabIndex = 0;
            this.dgvSpeciesSearch.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvSpeciesSearch_UserDeletingRow);
            // 
            // Tables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 472);
            this.Controls.Add(this.tControl);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Tables";
            this.Text = "[вставьте название ветклиники]";
            this.tControl.ResumeLayout(false);
            this.tpApp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.tpProc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcedures)).EndInit();
            this.tpPets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPets)).EndInit();
            this.tpSpecies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpecies)).EndInit();
            this.tpUsers.ResumeLayout(false);
            this.tpUsers.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tpNameSearch.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNameSearch)).EndInit();
            this.tpSpeciesSearch.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpeciesSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tControl;
        private System.Windows.Forms.TabPage tpApp;
        private System.Windows.Forms.TabPage tpProc;
        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.TabPage tpPets;
        private System.Windows.Forms.DataGridView dgvPets;
        private System.Windows.Forms.TabPage tpSpecies;
        private System.Windows.Forms.DataGridView dgvSpecies;
        private System.Windows.Forms.TabPage tpUsers;
        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.TabPage tpSpeciesSearch;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem добавитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.TabPage tpNameSearch;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bNameSearch;
        private System.Windows.Forms.TextBox tbNameSearch;
        private System.Windows.Forms.DataGridView dgvNameSearch;
        private System.Windows.Forms.Button bNameSearchCancel;
        private System.Windows.Forms.Label lb404name;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ComboBox cmbSpeciesSearch;
        private System.Windows.Forms.Button bSpeciesSearchCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bSpeciesSearch;
        private System.Windows.Forms.Label lb404species;
        private System.Windows.Forms.DataGridView dgvSpeciesSearch;
        private System.Windows.Forms.DataGridView dgvProcedures;
    }
}