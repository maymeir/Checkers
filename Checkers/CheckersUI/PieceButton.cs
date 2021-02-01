using System.Drawing;
using System.Windows.Forms;
using CheckerLogic;
namespace CheckersUI
{

    public class PieceButton : Button
    {
        internal const int k_PieceSize = 50;
        internal Piece.eSoliderType m_Type;
        internal int m_Row;
        internal int m_Column;

        public Piece.eSoliderType Type
        {
            get
            {
                return m_Type;
            }

            set
            {
                m_Type = value;
            }
        }

        public int Row
        {
            get { return m_Row; }
        }

        public int Column
        {
            get { return m_Column; }
        }

        public PieceButton(Piece.eSoliderType i_PieceType, int i_Row, int i_Column)
        {
            this.ClientSize = new Size(k_PieceSize, k_PieceSize);
            m_Type = i_PieceType;
            m_Row = i_Row;
            m_Column = i_Column;

            switch (i_PieceType)
            {
                case Piece.eSoliderType.K:
                    this.BackgroundImage = Image.FromFile("..\\..\\Resources\\blacking.png");
                    break;
                case Piece.eSoliderType.U:
                    this.BackgroundImage = Image.FromFile("..\\..\\Resources\\greyking.png");
                    break;
                case Piece.eSoliderType.O:
                    this.BackgroundImage = Image.FromFile("..\\..\\Resources\\greynormal.png");
                    break;
                case Piece.eSoliderType.X:
                    this.BackgroundImage = Image.FromFile("..\\..\\Resources\\blacknormal.png");
                    break;
                case Piece.eSoliderType.Empty:
                    this.BackgroundImage = null;
                    break;
            }

            this.BackgroundImageLayout = ImageLayout.Stretch;

        }
    }
}
