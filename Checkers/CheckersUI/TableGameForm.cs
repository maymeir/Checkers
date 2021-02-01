using System;
using System.Drawing;
using CheckerLogic;
using System.Windows.Forms;

namespace CheckersUI
{
    internal partial class TableGameForm : Form
    {
        internal const int k_Margin = 50;
        private GameManager m_Game;
        private short m_Size;
        private PieceButton[,] m_Pieces;
        private Move m_CurrentMove;
        private readonly StartGameForm r_StartGameForm;
        private readonly bool r_IsComputerGame;
        internal string m_Player1Name;
        internal string m_Player2Name;

        public TableGameForm(StartGameForm i_StartGameForm)
        {
            r_StartGameForm = i_StartGameForm;
            m_Size = r_StartGameForm.TableSize;
            m_Player1Name = r_StartGameForm.TextPlayer1;
            m_Player2Name = r_StartGameForm.TextPlayer2;
            m_CurrentMove = null;
            m_Pieces = new PieceButton[this.m_Size, this.m_Size];
            r_IsComputerGame = !r_StartGameForm.CheckBoxPlayer2.Checked;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(SizeTable * k_Margin - 8, SizeTable * k_Margin + k_Margin);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeComponent();
            InitTable();

            if (r_IsComputerGame)
            {
                m_Game = new GameManager(r_StartGameForm.TextPlayer1, "Computer",r_StartGameForm.TableSize);
            }
            else
            {
                m_Game = new GameManager(r_StartGameForm.TextPlayer1, r_StartGameForm.TextPlayer2, r_StartGameForm.TableSize);
            }

            registerEvents();
        }

        public StartGameForm GetStartGameForm
        {
            get
            {
                return r_StartGameForm;
            }
        }

        public Move CurrentMove
        {
            get { return m_CurrentMove; }
            set { m_CurrentMove = value; }
        }

        public PieceButton[,] Pieces
        {
            get
            {
                return m_Pieces;
            }

            set
            {
                m_Pieces = value;
            }
        }

        public short SizeTable
        {
            get
            {
                return m_Size;
            }

            set
            {
                m_Size = value;
            }
        }

        private void registerEvents()
        {
            m_Game.InvalidMove += new EventHandler(invalidMove);
            m_Game.MakeMove += new EventHandler(MakeMove);
            m_Game.EndGameRound += new EventHandler(OnEndRoundGame);
        }

        public void button_Click(Object sender, EventArgs e)
        {
            PieceButton button = (PieceButton)sender;
            int row = button.Row;
            int col = button.Column;

            if (CurrentMove == null)
            {
                CurrentMove = new Move();
            }

            if (CurrentMove.CurrentPiece == null)
            {
                button.BackColor = ColorTranslator.FromHtml("#5c7fa1");
                CurrentMove.CurrentPiece = new Piece(row, col);
            }
            else
            {
                if (CurrentMove.CurrentPiece.Row == row && CurrentMove.CurrentPiece.Column == col)
                {
                    button.BackColor = Color.White;
                    CurrentMove.CurrentPiece = null;
                }
                else
                {
                    CurrentMove.TargetPiece = new Piece(row, col);
                }
            }

            if ((CurrentMove.CurrentPiece != null) && (CurrentMove.TargetPiece != null))
            {
                m_Game.GameRound(CurrentMove);
                Pieces[CurrentMove.CurrentPiece.Row, CurrentMove.CurrentPiece.Column].BackColor = Color.White;
                CurrentMove.CurrentPiece = null;
                CurrentMove.TargetPiece = null;
                CurrentMove = null;
            }
        }

        public void MakeMove(object sender, EventArgs e)
        {
            Move currentMove = sender as Move;
            PieceButton targetButton = Pieces[currentMove.TargetPiece.Row, currentMove.TargetPiece.Column];
            PieceButton currentButton = Pieces[currentMove.CurrentPiece.Row, currentMove.CurrentPiece.Column];
            Piece CurrentPiece = currentMove.CurrentPiece;
            Piece TargetPiece = currentMove.TargetPiece;

            if (currentMove.MoveType == CheckerLogic.Move.eMoveType.Jump)
            {
                int captureRow = currentButton.Row > targetButton.Row ? currentButton.Row - 1 : currentButton.Row + 1;
                int captureColumn = currentButton.Column > targetButton.Column ? currentButton.Column - 1 : currentButton.Column + 1;
                Pieces[captureRow, captureColumn].BackgroundImage = null;
            }

            if (CurrentPiece.Type == Piece.eSoliderType.X && TargetPiece.Row == 0)
            {
                targetButton.BackgroundImage = Image.FromFile("..\\..\\Resources\\blackking.png");
            }
            else
            {
                if ((CurrentPiece.Type == Piece.eSoliderType.O) && TargetPiece.Row == m_Size - 1)
                {
                    targetButton.BackgroundImage = Image.FromFile("..\\..\\Resources\\greyking.png");

                }
                else
                {
                    targetButton.BackgroundImage = currentButton.BackgroundImage;
                }
            }

            currentButton.BackgroundImage = null;
        }

        private void invalidMove(object sender, EventArgs e)
        {
            MessageBox.Show("Invalid Move!" + Environment.NewLine + "Please choose a valid move", "Damka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void OnEndRoundGame(object sender, EventArgs e)
        {
            DialogResult isAnotherRound = DialogResult.None;

            if (m_Game.StatusGame== GameStatus.eGameStatus.Tie)
            {
                string drawMsg = "Tie!" + Environment.NewLine + "Another Round?";
                isAnotherRound = MessageBox.Show(drawMsg, "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                string winMsg = (sender as Player).Name + " Won!" + Environment.NewLine + "Another Round?";
                isAnotherRound = MessageBox.Show(winMsg, "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (isAnotherRound == DialogResult.No)
            {
                this.Close();
                Application.Exit();
            }

            if (isAnotherRound == DialogResult.Yes)
            {
                playAnotherRound();
            }
        }

        private void playAnotherRound()
        {
            int player1Points = m_Game.GetPlayer1().Points;
            int player2Points = m_Game.GetPlayer2().Points;
            this.Controls.Clear();
            this.OnLoad(EventArgs.Empty);
            this.labelPlayer1.Text = m_Player1Name + ": " + player1Points;
            this.labelPlayer2.Text = m_Player2Name + ": " + player2Points;
        }
    }
}
