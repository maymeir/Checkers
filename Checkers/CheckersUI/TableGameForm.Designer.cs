using System.Drawing;
using System.Windows.Forms;
using CheckerLogic;

namespace CheckersUI
{
    internal partial class TableGameForm : Form
    {
        private System.ComponentModel.IContainer components = null;

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
            this.labelPlayer1 = new Label();
            this.labelPlayer2 = new Label();
            this.BackColor = Color.Black;
            this.SuspendLayout();
            // 
            // Player1
            // 
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Location = new Point(k_Margin, 15);
            this.labelPlayer1.Name = "Player1";
            this.labelPlayer1.Size = new Size(45, 13);
            this.labelPlayer1.TabIndex = 0;
            this.labelPlayer1.ForeColor = Color.White;
            this.labelPlayer1.Font = new Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1.Text = m_Player1Name + ": 0";
            this.Controls.Add(this.labelPlayer1);

            // 
            // Player2
            // 
            this.labelPlayer2.AutoSize = true;
            this.labelPlayer2.Location = new Point((k_Margin * m_Size) - (labelPlayer1.Left + labelPlayer1.Width + k_Margin), labelPlayer1.Location.Y);
            this.labelPlayer2.Name = "Player2";
            this.labelPlayer2.Size = new System.Drawing.Size(42, 13);
            this.labelPlayer2.TabIndex = 1;
            this.labelPlayer2.ForeColor = Color.White;
            this.labelPlayer2.Font = new Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2.Text = m_Player2Name + ": 0";
            this.Controls.Add(this.labelPlayer2);
            // 
            // TableGameForm
            // 

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "TableGameForm";
            this.Text = "Damka";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        internal void InitTable()
        {
            for (int i = 0; i < this.m_Size; i++)
            {
                for (int j = 0; j < this.m_Size; j++)
                {
                    m_Pieces[i, j] = new PieceButton(Piece.eSoliderType.Empty, i, j);
                    int yLocation = i * 50 + 50;
                    int xLocation = j * 50 - 4;
                    m_Pieces[i, j].Location = new Point(xLocation, yLocation);

                    if (i % 2 == 1)
                    {
                        if (j % 2 == 0)
                        {
                            TableEmptyPiece(i, j);
                        }
                        else
                        {
                            TableUnaccessiblePiece( i, j);
                        }
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            TableEmptyPiece(i, j);
                        }
                        else
                        {
                            TableUnaccessiblePiece(i, j);
                        }
                    }
                }
            }

            for (int i = 0; i < this.m_Size / 2 - 1; i++)
            {
                for (int j = 0; j < this.m_Size; j++)
                {
                    if (i % 2 == 1)
                    {
                        if (j % 2 == 0)
                        {
                            TableGreyPiece( i, j);
                        }
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            TableGreyPiece(i, j);
                        }
                    }
                }
            }

            for (int i = this.m_Size - 1; i > this.m_Size / 2; i--)
            {
                for (int j = 0; j < this.m_Size; j++)
                {
                    if (i % 2 == 1)
                    {
                        if (j % 2 == 0)
                        {
                            TableBlackPiece( i,  j);
                        }
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            TableBlackPiece(i, j);

                        }
                    }
                }
            }
        }

        internal void TableEmptyPiece(int i_I, int i_J)
        {
            m_Pieces[i_I, i_J].BackColor = Color.White;
            m_Pieces[i_I, i_J].Click += new System.EventHandler(button_Click);
            Controls.Add(m_Pieces[i_I, i_J]);
        }
        internal void TableUnaccessiblePiece(int i_I, int i_J)
        {
            m_Pieces[i_I, i_J].Enabled = false;
            m_Pieces[i_I, i_J].BackColor = Color.Gray;
            Controls.Add(m_Pieces[i_I, i_J
                ]);
        }
        internal void TableBlackPiece(int i_I, int i_J)
        {
            m_Pieces[i_I, i_J].Type = Piece.eSoliderType.X;
            m_Pieces[i_I, i_J].BackgroundImage = Image.FromFile("..\\..\\Resources\\blacknormal.png");
        }
        internal void TableGreyPiece(int i_I, int i_J)
        {
            m_Pieces[i_I, i_J].Type = Piece.eSoliderType.O;
            m_Pieces[i_I, i_J].BackgroundImage = Image.FromFile("..\\..\\Resources\\greynormal.png");
        }


        private Label labelPlayer1;
        private Label labelPlayer2;
    }

 

    #endregion
}