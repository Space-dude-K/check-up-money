namespace check_up_money
{
    partial class Form_Main
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
            this.button_Main_Process = new System.Windows.Forms.Button();
            this.label_OperationDayValue = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem_FilePaths = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_BudgetSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_BdSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_PathSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_FileHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_About = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView_Main = new System.Windows.Forms.DataGridView();
            this.dgv_Main_FileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_FileDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_FileSizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_FileTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_BudgetTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_DocCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_SummColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_CurrencyTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Main_Process
            // 
            this.button_Main_Process.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_Main_Process.Enabled = false;
            this.button_Main_Process.Location = new System.Drawing.Point(0, 267);
            this.button_Main_Process.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Main_Process.Name = "button_Main_Process";
            this.button_Main_Process.Size = new System.Drawing.Size(1104, 27);
            this.button_Main_Process.TabIndex = 1;
            this.button_Main_Process.Text = "Обработать";
            this.button_Main_Process.UseVisualStyleBackColor = true;
            this.button_Main_Process.Click += new System.EventHandler(this.Button_Main_Process_Click);
            // 
            // label_OperationDayValue
            // 
            this.label_OperationDayValue.AutoSize = true;
            this.label_OperationDayValue.Location = new System.Drawing.Point(442, 10);
            this.label_OperationDayValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_OperationDayValue.Name = "label_OperationDayValue";
            this.label_OperationDayValue.Size = new System.Drawing.Size(0, 15);
            this.label_OperationDayValue.TabIndex = 5;
            // 
            // menuStrip
            // 
            this.menuStrip.AutoSize = false;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_FilePaths,
            this.toolStripMenuItem_FileHistory,
            this.toolStripMenuItem_About,
            this.toolStripMenuItem_Exit});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1104, 35);
            this.menuStrip.TabIndex = 6;
            this.menuStrip.Text = "menuStrip_BaseSettings";
            // 
            // toolStripMenuItem_FilePaths
            // 
            this.toolStripMenuItem_FilePaths.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_BudgetSettings,
            this.toolStripMenuItem_BdSettings,
            this.toolStripMenuItem_PathSettings});
            this.toolStripMenuItem_FilePaths.Name = "toolStripMenuItem_FilePaths";
            this.toolStripMenuItem_FilePaths.Size = new System.Drawing.Size(79, 31);
            this.toolStripMenuItem_FilePaths.Text = "Настройки";
            // 
            // toolStripMenuItem_BudgetSettings
            // 
            this.toolStripMenuItem_BudgetSettings.Name = "toolStripMenuItem_BudgetSettings";
            this.toolStripMenuItem_BudgetSettings.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem_BudgetSettings.Text = "Бюджеты";
            this.toolStripMenuItem_BudgetSettings.Click += new System.EventHandler(this.ToolStripMenuItem_BudgetSettings_Click);
            // 
            // toolStripMenuItem_BdSettings
            // 
            this.toolStripMenuItem_BdSettings.Name = "toolStripMenuItem_BdSettings";
            this.toolStripMenuItem_BdSettings.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem_BdSettings.Text = "Базы данных";
            this.toolStripMenuItem_BdSettings.Click += new System.EventHandler(this.ToolStripMenuItem_BdSettings_Click);
            // 
            // toolStripMenuItem_PathSettings
            // 
            this.toolStripMenuItem_PathSettings.Name = "toolStripMenuItem_PathSettings";
            this.toolStripMenuItem_PathSettings.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItem_PathSettings.Text = "Пути к файлам";
            this.toolStripMenuItem_PathSettings.Click += new System.EventHandler(this.ToolStripMenuItem_PathSettings_Click);
            // 
            // toolStripMenuItem_FileHistory
            // 
            this.toolStripMenuItem_FileHistory.Name = "toolStripMenuItem_FileHistory";
            this.toolStripMenuItem_FileHistory.Size = new System.Drawing.Size(170, 31);
            this.toolStripMenuItem_FileHistory.Text = "Журнал обработки файлов";
            this.toolStripMenuItem_FileHistory.Click += new System.EventHandler(this.ToolStripMenuItem_FileHistory_Click);
            // 
            // toolStripMenuItem_About
            // 
            this.toolStripMenuItem_About.Name = "toolStripMenuItem_About";
            this.toolStripMenuItem_About.Size = new System.Drawing.Size(94, 31);
            this.toolStripMenuItem_About.Text = "О программе";
            this.toolStripMenuItem_About.Click += new System.EventHandler(this.ToolStripMenuItem_About_Click);
            // 
            // toolStripMenuItem_Exit
            // 
            this.toolStripMenuItem_Exit.Name = "toolStripMenuItem_Exit";
            this.toolStripMenuItem_Exit.Size = new System.Drawing.Size(54, 31);
            this.toolStripMenuItem_Exit.Text = "Выход";
            this.toolStripMenuItem_Exit.Click += new System.EventHandler(this.ToolStripMenuItem_Exit_Click);
            // 
            // dataGridView_Main
            // 
            this.dataGridView_Main.AllowUserToAddRows = false;
            this.dataGridView_Main.AllowUserToDeleteRows = false;
            this.dataGridView_Main.AllowUserToResizeRows = false;
            this.dataGridView_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_Main_FileNameColumn,
            this.dgv_Main_FileDateColumn,
            this.dgv_Main_FileSizeColumn,
            this.dgv_Main_FileTypeColumn,
            this.dgv_Main_BudgetTypeColumn,
            this.dgv_Main_DocCountColumn,
            this.dgv_Main_SummColumn,
            this.dgv_Main_CurrencyTypeColumn});
            this.dataGridView_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Main.Location = new System.Drawing.Point(0, 35);
            this.dataGridView_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView_Main.Name = "dataGridView_Main";
            this.dataGridView_Main.Size = new System.Drawing.Size(1104, 232);
            this.dataGridView_Main.TabIndex = 7;
            // 
            // dgv_Main_FileNameColumn
            // 
            this.dgv_Main_FileNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgv_Main_FileNameColumn.DataPropertyName = "FileName";
            this.dgv_Main_FileNameColumn.HeaderText = "Имя файла";
            this.dgv_Main_FileNameColumn.Name = "dgv_Main_FileNameColumn";
            this.dgv_Main_FileNameColumn.ReadOnly = true;
            this.dgv_Main_FileNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgv_Main_FileDateColumn
            // 
            this.dgv_Main_FileDateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_FileDateColumn.DataPropertyName = "FileDate";
            this.dgv_Main_FileDateColumn.HeaderText = "Дата файла";
            this.dgv_Main_FileDateColumn.Name = "dgv_Main_FileDateColumn";
            this.dgv_Main_FileDateColumn.ReadOnly = true;
            this.dgv_Main_FileDateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Main_FileDateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_FileDateColumn.Width = 96;
            // 
            // dgv_Main_FileSizeColumn
            // 
            this.dgv_Main_FileSizeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_FileSizeColumn.DataPropertyName = "FileSizeFormatted";
            this.dgv_Main_FileSizeColumn.HeaderText = "Размер";
            this.dgv_Main_FileSizeColumn.Name = "dgv_Main_FileSizeColumn";
            this.dgv_Main_FileSizeColumn.ReadOnly = true;
            this.dgv_Main_FileSizeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_FileSizeColumn.Width = 70;
            // 
            // dgv_Main_FileTypeColumn
            // 
            this.dgv_Main_FileTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_FileTypeColumn.DataPropertyName = "CheckUpFileTypeString";
            this.dgv_Main_FileTypeColumn.HeaderText = "Тип файла";
            this.dgv_Main_FileTypeColumn.Name = "dgv_Main_FileTypeColumn";
            this.dgv_Main_FileTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Main_FileTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_FileTypeColumn.Width = 230;
            // 
            // dgv_Main_BudgetTypeColumn
            // 
            this.dgv_Main_BudgetTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_BudgetTypeColumn.DataPropertyName = "CheckUpBudgetTypeString";
            this.dgv_Main_BudgetTypeColumn.HeaderText = "Бюджет";
            this.dgv_Main_BudgetTypeColumn.Name = "dgv_Main_BudgetTypeColumn";
            this.dgv_Main_BudgetTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_BudgetTypeColumn.Width = 106;
            // 
            // dgv_Main_DocCountColumn
            // 
            this.dgv_Main_DocCountColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_DocCountColumn.DataPropertyName = "TotalDocsInCheckUpDataStruct";
            this.dgv_Main_DocCountColumn.HeaderText = "Кол-во док-ов";
            this.dgv_Main_DocCountColumn.Name = "dgv_Main_DocCountColumn";
            this.dgv_Main_DocCountColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgv_Main_SummColumn
            // 
            this.dgv_Main_SummColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_SummColumn.DataPropertyName = "TotalSumm";
            this.dgv_Main_SummColumn.HeaderText = "Сумма";
            this.dgv_Main_SummColumn.Name = "dgv_Main_SummColumn";
            this.dgv_Main_SummColumn.ReadOnly = true;
            this.dgv_Main_SummColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_SummColumn.Width = 140;
            // 
            // dgv_Main_CurrencyTypeColumn
            // 
            this.dgv_Main_CurrencyTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_CurrencyTypeColumn.DataPropertyName = "CurrencyType";
            this.dgv_Main_CurrencyTypeColumn.HeaderText = "Тип валюты";
            this.dgv_Main_CurrencyTypeColumn.Name = "dgv_Main_CurrencyTypeColumn";
            this.dgv_Main_CurrencyTypeColumn.ReadOnly = true;
            this.dgv_Main_CurrencyTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_CurrencyTypeColumn.Width = 60;
            // 
            // Form_Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 294);
            this.Controls.Add(this.dataGridView_Main);
            this.Controls.Add(this.label_OperationDayValue);
            this.Controls.Add(this.button_Main_Process);
            this.Controls.Add(this.menuStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form_Main";
            this.Text = "Check up money";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Main_Process;
        private System.Windows.Forms.Label label_OperationDayValue;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_FilePaths;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_PathSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_BdSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_BudgetSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_About;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Exit;
        private System.Windows.Forms.DataGridView dataGridView_Main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_FileHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_FileNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_FileDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_FileSizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_FileTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_BudgetTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_DocCountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_SummColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_CurrencyTypeColumn;
    }
}

