using System;
using System.Windows.Forms;

namespace CheckersUI
{
    public partial class StartGameForm : Form
    {
        private bool m_ValidStartGameForm = false;

        public StartGameForm()
        {
            InitializeComponent();
        }

        public CheckBox CheckBoxPlayer2
        {
            get
            {
                return this.checkBoxPlayer2;
            }
        }

        public string TextPlayer1
        {
            get
            {
                return this.textBoxPlayer1.Text;
            }
        }

        public string TextPlayer2
        {
            get
            {
                return this.textBoxPlayer2.Text;
            }
        }

        public short TableSize
        {
            get
            {
                short tableSize = 0;

                if (radioButtonSize6.Checked == true)
                {
                    tableSize = 6;
                }
                else
                {
                    if (radioButtonSize8.Checked == true)
                    {
                        {
                            tableSize = 8;
                        }
                    }
                    else if (radioButtonSize10.Checked == true)
                    {
                        tableSize = 10;
                    }
                }

                return tableSize;
            }
        }

        private void checkBoxPlayer2_Click(object sender, EventArgs e)
        {
            if (checkBoxPlayer2.Checked)
            {
                textBoxPlayer2.Enabled = true;
                textBoxPlayer2.Text = string.Empty;
            }
            else
            {
                textBoxPlayer2.Enabled = false;
                textBoxPlayer2.Text = "[Computer]";
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            if (ensureLoggedIn())
            {
                TableGameForm tableGameForm = new TableGameForm(this);
                this.Hide();
                tableGameForm.ShowDialog();
                this.Close();
            }
        }

        private bool ensureLoggedIn()
        {
            if (!m_ValidStartGameForm)
            {
                if (IsValidSizePlayersName(textBoxPlayer1) && IsValidSizePlayersName(textBoxPlayer2) && IsValidSizeRadioButtons())
                {
                    m_ValidStartGameForm = true;
                }
                else
                {
                    if (MessageBox.Show("The form is invalid. Try again", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.Retry)
                    {
                        ensureLoggedIn();
                    }
                }
            }

            return m_ValidStartGameForm;
        }

        private bool IsValidSizeRadioButtons()
        {
            return radioButtonSize6.Checked || radioButtonSize8.Checked || radioButtonSize10.Checked;
        }

        private bool IsValidSizePlayersName(TextBox i_PlayerTextBox)
        {
            const short k_PlayerNameValidLength = 20;

            return !((i_PlayerTextBox.Text.Length > k_PlayerNameValidLength) || i_PlayerTextBox.Text.Contains(" ") || i_PlayerTextBox.Text.Length == 0);
        }
    }
}
