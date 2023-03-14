using System.Windows.Forms;
using System.Security;
using System.Drawing;
using check_up_money.Properties;
using System.Text;
using System;
using System.Diagnostics;

namespace check_up_money.Forms
{
    public partial class SecurityTextBox : UserControl
    {
        int passwordDummyLenght = 8;
        char passwordChar = '●';
        bool isInit = false;
        private bool isPasswordChanged = false;

        public bool IsPasswordChanged
        {
            get { return isPasswordChanged; }
            set { isPasswordChanged = value; }
        }

        SecureString secureString = new SecureString();

        TextBox tb;

        public TextBox Tb
        {
            get { return tb; }
            set { tb = value; }
        }
        PictureBox _pb;

        public PictureBox Pb
        {
            get { return _pb; }
            set { _pb = value; }
        }

        public SecureString SecureString
        {
            get { return secureString; }
            set { secureString = value; }
        }

        public char PasswordChar
        {
            get { return passwordChar; }
            set { passwordChar = value; }
        }

        public SecurityTextBox()
        {
            InitializeComponent();
            InitPictureBox();
        }
        private void InitPictureBox()
        {
            Debug.WriteLine("Init pb!");
            _pb = pictureBox_SecurityTextBox_PasswordSwitch;
            tb = this.textBox_InputBox;

            this.pictureBox_SecurityTextBox_PasswordSwitch.Image = Resources.PasswordEyeImage;
            this.pictureBox_SecurityTextBox_PasswordSwitch.BackColor = Color.Transparent;
            this.pictureBox_SecurityTextBox_PasswordSwitch.Hide();
        }
        private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
                ProcessBackspace();
            else
                ProcessNewCharacter(e.KeyChar);

            e.Handled = true;
        }

        public void ProcessNewCharacter(char character)
        {
            bool isDummyPassword = secureString.Length == 0 && this.textBox_InputBox.Text.Length > 0;

            if (textBox_InputBox.SelectionLength > 0)
            {
                RemoveSelectedCharacters();
            }

            // Check if password was default (loaded from config)
            if(isDummyPassword)
            {
                this.textBox_InputBox.Clear();
                ProcessNewCharacter(character);
            }
            else
            {
                secureString.InsertAt(textBox_InputBox.SelectionStart, character);
                ResetDisplayCharacters(textBox_InputBox.SelectionStart + 1);
                isPasswordChanged = true;
            }
        }
        private void RemoveSelectedCharacters()
        {
            if(secureString.Length > 0)
            {
                secureString.Clear();

                /*
                for (int i = 0; i < textBox_InputBox.SelectionLength; i++)
                {
                    secureString.RemoveAt(textBox_InputBox.SelectionStart);
                }
                */
            }
        }

        private void ResetDisplayCharacters(int caretPosition)
        {
            textBox_InputBox.Text = new string(passwordChar, secureString.Length);
            textBox_InputBox.SelectionStart = caretPosition;
        }

        private void ProcessBackspace()
        {
            if (secureString.Length == 0)
                textBox_InputBox.Text = string.Empty;

            if (textBox_InputBox.SelectionLength > 0)
            {
                RemoveSelectedCharacters();
                ResetDisplayCharacters(textBox_InputBox.SelectionStart);
            }
            else if (textBox_InputBox.SelectionStart > 0)
            {
                textBox_InputBox.SelectionStart = secureString.Length;
                secureString.RemoveAt(textBox_InputBox.SelectionStart - 1);
                ResetDisplayCharacters(textBox_InputBox.SelectionStart - 1);
            }

            isPasswordChanged = true;
        }
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ProcessDelete();
                e.Handled = true;
            }
            else if (IsIgnorableKey(e.KeyCode))
            {
                e.Handled = true;
            }
        }
        private bool IsIgnorableKey(Keys key)
        {
            return key == Keys.Escape || key == Keys.Enter;
        }

        private void ProcessDelete()
        {
            if (textBox_InputBox.SelectionLength > 0)
            {
                RemoveSelectedCharacters();
            }
            else if (textBox_InputBox.SelectionStart < textBox_InputBox.Text.Length)
            {
                secureString.RemoveAt(textBox_InputBox.SelectionStart);
            }

            ResetDisplayCharacters(textBox_InputBox.SelectionStart);
        }
        // Set fake password for loaded by default password (only visible, textBox.Text)
        public void SetPasswordDummy(int dummyLenght, bool isInit = false)
        {
            StringBuilder sb = new StringBuilder();

            passwordDummyLenght = dummyLenght > 0 ? dummyLenght : passwordDummyLenght;

            Debug.WriteLine("Dl: " + passwordDummyLenght);

            for (int i = 0; i < passwordDummyLenght; i++)
            {
                sb.Append(passwordChar);
            }

            this.textBox_InputBox.Text = sb.ToString();

            // Subscirbe to text input events after initialize
            if (dummyLenght == 0)
            {
                this.textBox_InputBox.TextChanged += new System.EventHandler(this.textBox_InputBox_TextChanged);
                this.isInit = isInit;
            }
        }

        private void textBox_InputBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!this.pictureBox_SecurityTextBox_PasswordSwitch.Visible && this.textBox_InputBox.Text.Length > 0)
            {
                this.pictureBox_SecurityTextBox_PasswordSwitch.Show();
            }
            else
            {
                if(this.textBox_InputBox.Text.Length == 0)
                    this.pictureBox_SecurityTextBox_PasswordSwitch.Hide();
            }
        }
    }
}