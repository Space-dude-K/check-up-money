namespace check_up_money.Forms
{
    partial class Form_DataBaseSettings
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
            System.Security.SecureString secureString1 = new System.Security.SecureString();
            this.label_DBS_User = new System.Windows.Forms.Label();
            this.label_DBS_Pass = new System.Windows.Forms.Label();
            this.label_DBS_Name = new System.Windows.Forms.Label();
            this.label_DBS_ServName = new System.Windows.Forms.Label();
            this.button_DBS_Save = new System.Windows.Forms.Button();
            this.button_DBS_Cancel = new System.Windows.Forms.Button();
            this.textBox_DBS_Name = new System.Windows.Forms.TextBox();
            this.textBox_DBS_ServName = new System.Windows.Forms.TextBox();
            this.textBox_DBS_User = new System.Windows.Forms.TextBox();
            this.listBox_DBS_LoadedDbSettings = new System.Windows.Forms.ListBox();
            this.button_DBS_Delete = new System.Windows.Forms.Button();
            this.label_DBS_LoadedDbSettings = new System.Windows.Forms.Label();
            this.button_DBS_Add = new System.Windows.Forms.Button();
            this.textBox_DBS_Driver = new System.Windows.Forms.TextBox();
            this.label_DBS_Driver = new System.Windows.Forms.Label();
            this.textBox_DBS_Instance = new System.Windows.Forms.TextBox();
            this.label_DBS_Instance = new System.Windows.Forms.Label();
            this.securityTextBox_DBS_Password = new check_up_money.Forms.SecurityTextBox();
            this.SuspendLayout();
            // 
            // label_DBS_User
            // 
            this.label_DBS_User.AutoSize = true;
            this.label_DBS_User.Location = new System.Drawing.Point(650, 261);
            this.label_DBS_User.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_DBS_User.Name = "label_DBS_User";
            this.label_DBS_User.Size = new System.Drawing.Size(84, 15);
            this.label_DBS_User.TabIndex = 0;
            this.label_DBS_User.Text = "Пользователь";
            // 
            // label_DBS_Pass
            // 
            this.label_DBS_Pass.AutoSize = true;
            this.label_DBS_Pass.Location = new System.Drawing.Point(965, 261);
            this.label_DBS_Pass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_DBS_Pass.Name = "label_DBS_Pass";
            this.label_DBS_Pass.Size = new System.Drawing.Size(49, 15);
            this.label_DBS_Pass.TabIndex = 0;
            this.label_DBS_Pass.Text = "Пароль";
            // 
            // label_DBS_Name
            // 
            this.label_DBS_Name.AutoSize = true;
            this.label_DBS_Name.Location = new System.Drawing.Point(465, 261);
            this.label_DBS_Name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_DBS_Name.Name = "label_DBS_Name";
            this.label_DBS_Name.Size = new System.Drawing.Size(105, 15);
            this.label_DBS_Name.TabIndex = 0;
            this.label_DBS_Name.Text = "Имя базы данных";
            // 
            // label_DBS_ServName
            // 
            this.label_DBS_ServName.AutoSize = true;
            this.label_DBS_ServName.Location = new System.Drawing.Point(159, 261);
            this.label_DBS_ServName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_DBS_ServName.Name = "label_DBS_ServName";
            this.label_DBS_ServName.Size = new System.Drawing.Size(73, 15);
            this.label_DBS_ServName.TabIndex = 0;
            this.label_DBS_ServName.Text = "Адрес хоста";
            // 
            // button_DBS_Save
            // 
            this.button_DBS_Save.Location = new System.Drawing.Point(559, 321);
            this.button_DBS_Save.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DBS_Save.Name = "button_DBS_Save";
            this.button_DBS_Save.Size = new System.Drawing.Size(88, 27);
            this.button_DBS_Save.TabIndex = 8;
            this.button_DBS_Save.Text = "Сохранить";
            this.button_DBS_Save.UseVisualStyleBackColor = true;
            this.button_DBS_Save.Click += new System.EventHandler(this.Button_DBS_Save_Click);
            // 
            // button_DBS_Cancel
            // 
            this.button_DBS_Cancel.Location = new System.Drawing.Point(743, 321);
            this.button_DBS_Cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DBS_Cancel.Name = "button_DBS_Cancel";
            this.button_DBS_Cancel.Size = new System.Drawing.Size(88, 27);
            this.button_DBS_Cancel.TabIndex = 10;
            this.button_DBS_Cancel.Text = "Отменить";
            this.button_DBS_Cancel.UseVisualStyleBackColor = true;
            this.button_DBS_Cancel.Click += new System.EventHandler(this.Button_DBS_Cancel_Click);
            // 
            // textBox_DBS_Name
            // 
            this.textBox_DBS_Name.Location = new System.Drawing.Point(465, 279);
            this.textBox_DBS_Name.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_DBS_Name.Name = "textBox_DBS_Name";
            this.textBox_DBS_Name.Size = new System.Drawing.Size(178, 23);
            this.textBox_DBS_Name.TabIndex = 4;
            // 
            // textBox_DBS_ServName
            // 
            this.textBox_DBS_ServName.Location = new System.Drawing.Point(159, 279);
            this.textBox_DBS_ServName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_DBS_ServName.Name = "textBox_DBS_ServName";
            this.textBox_DBS_ServName.Size = new System.Drawing.Size(194, 23);
            this.textBox_DBS_ServName.TabIndex = 2;
            // 
            // textBox_DBS_User
            // 
            this.textBox_DBS_User.Location = new System.Drawing.Point(651, 279);
            this.textBox_DBS_User.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_DBS_User.Name = "textBox_DBS_User";
            this.textBox_DBS_User.Size = new System.Drawing.Size(308, 23);
            this.textBox_DBS_User.TabIndex = 5;
            // 
            // listBox_DBS_LoadedDbSettings
            // 
            this.listBox_DBS_LoadedDbSettings.FormattingEnabled = true;
            this.listBox_DBS_LoadedDbSettings.ItemHeight = 15;
            this.listBox_DBS_LoadedDbSettings.Location = new System.Drawing.Point(21, 24);
            this.listBox_DBS_LoadedDbSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBox_DBS_LoadedDbSettings.Name = "listBox_DBS_LoadedDbSettings";
            this.listBox_DBS_LoadedDbSettings.Size = new System.Drawing.Size(1257, 169);
            this.listBox_DBS_LoadedDbSettings.TabIndex = 12;
            // 
            // button_DBS_Delete
            // 
            this.button_DBS_Delete.Location = new System.Drawing.Point(651, 321);
            this.button_DBS_Delete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DBS_Delete.Name = "button_DBS_Delete";
            this.button_DBS_Delete.Size = new System.Drawing.Size(88, 27);
            this.button_DBS_Delete.TabIndex = 9;
            this.button_DBS_Delete.Text = "Удалить";
            this.button_DBS_Delete.UseVisualStyleBackColor = true;
            this.button_DBS_Delete.Click += new System.EventHandler(this.Button_DBS_Delete_Click);
            // 
            // label_DBS_LoadedDbSettings
            // 
            this.label_DBS_LoadedDbSettings.AutoSize = true;
            this.label_DBS_LoadedDbSettings.Location = new System.Drawing.Point(18, 6);
            this.label_DBS_LoadedDbSettings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_DBS_LoadedDbSettings.Name = "label_DBS_LoadedDbSettings";
            this.label_DBS_LoadedDbSettings.Size = new System.Drawing.Size(146, 15);
            this.label_DBS_LoadedDbSettings.TabIndex = 14;
            this.label_DBS_LoadedDbSettings.Text = "Загруженные настройки:";
            // 
            // button_DBS_Add
            // 
            this.button_DBS_Add.Location = new System.Drawing.Point(466, 321);
            this.button_DBS_Add.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DBS_Add.Name = "button_DBS_Add";
            this.button_DBS_Add.Size = new System.Drawing.Size(88, 27);
            this.button_DBS_Add.TabIndex = 7;
            this.button_DBS_Add.Text = "Добавить";
            this.button_DBS_Add.UseVisualStyleBackColor = true;
            this.button_DBS_Add.Click += new System.EventHandler(this.Button_DBS_Add_Click);
            // 
            // textBox_DBS_Driver
            // 
            this.textBox_DBS_Driver.Location = new System.Drawing.Point(21, 279);
            this.textBox_DBS_Driver.Name = "textBox_DBS_Driver";
            this.textBox_DBS_Driver.Size = new System.Drawing.Size(131, 23);
            this.textBox_DBS_Driver.TabIndex = 1;
            // 
            // label_DBS_Driver
            // 
            this.label_DBS_Driver.AutoSize = true;
            this.label_DBS_Driver.Location = new System.Drawing.Point(21, 260);
            this.label_DBS_Driver.Name = "label_DBS_Driver";
            this.label_DBS_Driver.Size = new System.Drawing.Size(54, 15);
            this.label_DBS_Driver.TabIndex = 0;
            this.label_DBS_Driver.Text = "Драйвер";
            // 
            // textBox_DBS_Instance
            // 
            this.textBox_DBS_Instance.Location = new System.Drawing.Point(360, 279);
            this.textBox_DBS_Instance.Name = "textBox_DBS_Instance";
            this.textBox_DBS_Instance.Size = new System.Drawing.Size(98, 23);
            this.textBox_DBS_Instance.TabIndex = 3;
            // 
            // label_DBS_Instance
            // 
            this.label_DBS_Instance.AutoSize = true;
            this.label_DBS_Instance.Location = new System.Drawing.Point(360, 261);
            this.label_DBS_Instance.Name = "label_DBS_Instance";
            this.label_DBS_Instance.Size = new System.Drawing.Size(53, 15);
            this.label_DBS_Instance.TabIndex = 0;
            this.label_DBS_Instance.Text = "Инстанс";
            // 
            // securityTextBox_DBS_Password
            // 
            this.securityTextBox_DBS_Password.IsPasswordChanged = false;
            this.securityTextBox_DBS_Password.Location = new System.Drawing.Point(965, 279);
            this.securityTextBox_DBS_Password.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.securityTextBox_DBS_Password.Name = "securityTextBox_DBS_Password";
            this.securityTextBox_DBS_Password.PasswordChar = '●';
            this.securityTextBox_DBS_Password.SecureString = secureString1;
            this.securityTextBox_DBS_Password.Size = new System.Drawing.Size(313, 23);
            this.securityTextBox_DBS_Password.TabIndex = 6;
            // 
            // Form_DataBaseSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1296, 384);
            this.Controls.Add(this.securityTextBox_DBS_Password);
            this.Controls.Add(this.label_DBS_Instance);
            this.Controls.Add(this.textBox_DBS_Instance);
            this.Controls.Add(this.label_DBS_Driver);
            this.Controls.Add(this.textBox_DBS_Driver);
            this.Controls.Add(this.button_DBS_Add);
            this.Controls.Add(this.label_DBS_LoadedDbSettings);
            this.Controls.Add(this.button_DBS_Delete);
            this.Controls.Add(this.listBox_DBS_LoadedDbSettings);
            this.Controls.Add(this.textBox_DBS_User);
            this.Controls.Add(this.textBox_DBS_ServName);
            this.Controls.Add(this.textBox_DBS_Name);
            this.Controls.Add(this.button_DBS_Cancel);
            this.Controls.Add(this.button_DBS_Save);
            this.Controls.Add(this.label_DBS_ServName);
            this.Controls.Add(this.label_DBS_Name);
            this.Controls.Add(this.label_DBS_Pass);
            this.Controls.Add(this.label_DBS_User);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form_DataBaseSettings";
            this.Text = "Form_DataBaseSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_DBS_User;
        private System.Windows.Forms.Label label_DBS_Pass;
        private System.Windows.Forms.Label label_DBS_Name;
        private System.Windows.Forms.Label label_DBS_ServName;
        private System.Windows.Forms.Button button_DBS_Save;
        private System.Windows.Forms.Button button_DBS_Cancel;
        private System.Windows.Forms.TextBox textBox_DBS_Name;
        private System.Windows.Forms.TextBox textBox_DBS_ServName;
        private SecurityTextBox securityTextBox_DBS_Password;
        private System.Windows.Forms.TextBox textBox_DBS_User;
        private System.Windows.Forms.ListBox listBox_DBS_LoadedDbSettings;
        private System.Windows.Forms.Button button_DBS_Delete;
        private System.Windows.Forms.Label label_DBS_LoadedDbSettings;
        private System.Windows.Forms.Button button_DBS_Add;
        private System.Windows.Forms.TextBox textBox_DBS_Driver;
        private System.Windows.Forms.Label label_DBS_Driver;
        private System.Windows.Forms.TextBox textBox_DBS_Instance;
        private System.Windows.Forms.Label label_DBS_Instance;
    }
}