namespace check_up_money.Forms
{
    partial class SecurityTextBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_InputBox = new System.Windows.Forms.TextBox();
            this.pictureBox_SecurityTextBox_PasswordSwitch = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SecurityTextBox_PasswordSwitch)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_InputBox
            // 
            this.textBox_InputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_InputBox.Location = new System.Drawing.Point(0, 0);
            this.textBox_InputBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_InputBox.Name = "textBox_InputBox";
            this.textBox_InputBox.Size = new System.Drawing.Size(313, 23);
            this.textBox_InputBox.TabIndex = 0;
            this.textBox_InputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputBox_KeyPress);
            // 
            // pictureBox_SecurityTextBox_PasswordSwitch
            // 
            this.pictureBox_SecurityTextBox_PasswordSwitch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_SecurityTextBox_PasswordSwitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox_SecurityTextBox_PasswordSwitch.InitialImage = null;
            this.pictureBox_SecurityTextBox_PasswordSwitch.Location = new System.Drawing.Point(288, 3);
            this.pictureBox_SecurityTextBox_PasswordSwitch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox_SecurityTextBox_PasswordSwitch.Name = "pictureBox_SecurityTextBox_PasswordSwitch";
            this.pictureBox_SecurityTextBox_PasswordSwitch.Size = new System.Drawing.Size(21, 18);
            this.pictureBox_SecurityTextBox_PasswordSwitch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_SecurityTextBox_PasswordSwitch.TabIndex = 1;
            this.pictureBox_SecurityTextBox_PasswordSwitch.TabStop = false;
            // 
            // SecurityTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox_SecurityTextBox_PasswordSwitch);
            this.Controls.Add(this.textBox_InputBox);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SecurityTextBox";
            this.Size = new System.Drawing.Size(313, 27);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SecurityTextBox_PasswordSwitch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_InputBox;
        private System.Windows.Forms.PictureBox pictureBox_SecurityTextBox_PasswordSwitch;

    }
}
