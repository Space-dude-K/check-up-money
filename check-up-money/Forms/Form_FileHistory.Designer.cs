
namespace check_up_money.Forms
{
    partial class Form_FileHistory
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
            this.dataGridView_FileHistory = new System.Windows.Forms.DataGridView();
            this.button_FileHistory_Proceed = new System.Windows.Forms.Button();
            this.dateTimePicker_FileHistory_From = new System.Windows.Forms.DateTimePicker();
            this.label_FileHistory_From = new System.Windows.Forms.Label();
            this.dateTimePicker_FileHistory_To = new System.Windows.Forms.DateTimePicker();
            this.label_FileHistory_To = new System.Windows.Forms.Label();
            this.dgv_Main_FileDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_FileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_FileTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_BudgetTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_DocCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_SummColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Main_CurrencyTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_FileHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_FileHistory
            // 
            this.dataGridView_FileHistory.AllowUserToAddRows = false;
            this.dataGridView_FileHistory.AllowUserToDeleteRows = false;
            this.dataGridView_FileHistory.AllowUserToResizeRows = false;
            this.dataGridView_FileHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_FileHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_Main_FileDateColumn,
            this.dgv_Main_FileNameColumn,
            this.dgv_Main_FileTypeColumn,
            this.dgv_Main_BudgetTypeColumn,
            this.dgv_Main_DocCountColumn,
            this.dgv_Main_SummColumn,
            this.dgv_Main_CurrencyTypeColumn});
            this.dataGridView_FileHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView_FileHistory.Location = new System.Drawing.Point(0, 84);
            this.dataGridView_FileHistory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView_FileHistory.Name = "dataGridView_FileHistory";
            this.dataGridView_FileHistory.Size = new System.Drawing.Size(974, 236);
            this.dataGridView_FileHistory.TabIndex = 8;
            // 
            // button_FileHistory_Proceed
            // 
            this.button_FileHistory_Proceed.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_FileHistory_Proceed.Location = new System.Drawing.Point(284, 55);
            this.button_FileHistory_Proceed.Name = "button_FileHistory_Proceed";
            this.button_FileHistory_Proceed.Size = new System.Drawing.Size(406, 23);
            this.button_FileHistory_Proceed.TabIndex = 9;
            this.button_FileHistory_Proceed.Text = "Выбрать";
            this.button_FileHistory_Proceed.UseVisualStyleBackColor = true;
            this.button_FileHistory_Proceed.Click += new System.EventHandler(this.Button_FileHistory_Proceed_Click);
            // 
            // dateTimePicker_FileHistory_From
            // 
            this.dateTimePicker_FileHistory_From.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_FileHistory_From.Location = new System.Drawing.Point(173, 28);
            this.dateTimePicker_FileHistory_From.Name = "dateTimePicker_FileHistory_From";
            this.dateTimePicker_FileHistory_From.Size = new System.Drawing.Size(311, 23);
            this.dateTimePicker_FileHistory_From.TabIndex = 10;
            this.dateTimePicker_FileHistory_From.ValueChanged += new System.EventHandler(this.dateTimePicker_FileHistory_From_ValueChanged);
            // 
            // label_FileHistory_From
            // 
            this.label_FileHistory_From.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_FileHistory_From.AutoSize = true;
            this.label_FileHistory_From.Location = new System.Drawing.Point(173, 10);
            this.label_FileHistory_From.Name = "label_FileHistory_From";
            this.label_FileHistory_From.Size = new System.Drawing.Size(143, 15);
            this.label_FileHistory_From.TabIndex = 11;
            this.label_FileHistory_From.Text = "Обработанные файлы с:";
            // 
            // dateTimePicker_FileHistory_To
            // 
            this.dateTimePicker_FileHistory_To.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_FileHistory_To.Location = new System.Drawing.Point(490, 28);
            this.dateTimePicker_FileHistory_To.Name = "dateTimePicker_FileHistory_To";
            this.dateTimePicker_FileHistory_To.Size = new System.Drawing.Size(311, 23);
            this.dateTimePicker_FileHistory_To.TabIndex = 12;
            this.dateTimePicker_FileHistory_To.ValueChanged += new System.EventHandler(this.dateTimePicker_FileHistory_To_ValueChanged);
            // 
            // label_FileHistory_To
            // 
            this.label_FileHistory_To.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_FileHistory_To.AutoSize = true;
            this.label_FileHistory_To.Location = new System.Drawing.Point(490, 10);
            this.label_FileHistory_To.Name = "label_FileHistory_To";
            this.label_FileHistory_To.Size = new System.Drawing.Size(24, 15);
            this.label_FileHistory_To.TabIndex = 13;
            this.label_FileHistory_To.Text = "по:";
            // 
            // dgv_Main_FileDateColumn
            // 
            this.dgv_Main_FileDateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_FileDateColumn.DataPropertyName = "FileDate";
            this.dgv_Main_FileDateColumn.HeaderText = "Дата и время обработки";
            this.dgv_Main_FileDateColumn.Name = "dgv_Main_FileDateColumn";
            this.dgv_Main_FileDateColumn.ReadOnly = true;
            this.dgv_Main_FileDateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Main_FileDateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_FileDateColumn.Width = 162;
            // 
            // dgv_Main_FileNameColumn
            // 
            this.dgv_Main_FileNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_FileNameColumn.DataPropertyName = "FileName";
            this.dgv_Main_FileNameColumn.HeaderText = "Имя файла";
            this.dgv_Main_FileNameColumn.Name = "dgv_Main_FileNameColumn";
            this.dgv_Main_FileNameColumn.ReadOnly = true;
            this.dgv_Main_FileNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgv_Main_FileTypeColumn
            // 
            this.dgv_Main_FileTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_FileTypeColumn.DataPropertyName = "FileType";
            this.dgv_Main_FileTypeColumn.HeaderText = "Тип файла";
            this.dgv_Main_FileTypeColumn.Name = "dgv_Main_FileTypeColumn";
            this.dgv_Main_FileTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Main_FileTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_FileTypeColumn.Width = 200;
            // 
            // dgv_Main_BudgetTypeColumn
            // 
            this.dgv_Main_BudgetTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_BudgetTypeColumn.DataPropertyName = "BudgetType";
            this.dgv_Main_BudgetTypeColumn.HeaderText = "Бюджет";
            this.dgv_Main_BudgetTypeColumn.Name = "dgv_Main_BudgetTypeColumn";
            this.dgv_Main_BudgetTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgv_Main_BudgetTypeColumn.Width = 140;
            // 
            // dgv_Main_DocCountColumn
            // 
            this.dgv_Main_DocCountColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_Main_DocCountColumn.DataPropertyName = "TotalDocs";
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
            this.dgv_Main_SummColumn.Width = 115;
            // 
            // dgv_Main_CurrencyTypeColumn
            // 
            this.dgv_Main_CurrencyTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgv_Main_CurrencyTypeColumn.DataPropertyName = "CurrencyType";
            this.dgv_Main_CurrencyTypeColumn.HeaderText = "Тип валюты";
            this.dgv_Main_CurrencyTypeColumn.Name = "dgv_Main_CurrencyTypeColumn";
            this.dgv_Main_CurrencyTypeColumn.ReadOnly = true;
            this.dgv_Main_CurrencyTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Form_FileHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 320);
            this.Controls.Add(this.label_FileHistory_To);
            this.Controls.Add(this.dateTimePicker_FileHistory_To);
            this.Controls.Add(this.label_FileHistory_From);
            this.Controls.Add(this.dateTimePicker_FileHistory_From);
            this.Controls.Add(this.button_FileHistory_Proceed);
            this.Controls.Add(this.dataGridView_FileHistory);
            this.Name = "Form_FileHistory";
            this.Text = "Журнал обработки файлов";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_FileHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_FileHistory;
        private System.Windows.Forms.Button button_FileHistory_Proceed;
        private System.Windows.Forms.DateTimePicker dateTimePicker_FileHistory_From;
        private System.Windows.Forms.Label label_FileHistory_From;
        private System.Windows.Forms.DateTimePicker dateTimePicker_FileHistory_To;
        private System.Windows.Forms.Label label_FileHistory_To;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_FileDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_FileNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_FileTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_BudgetTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_DocCountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_SummColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Main_CurrencyTypeColumn;
    }
}