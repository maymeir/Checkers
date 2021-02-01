
namespace CheckerLogic
{
    public class Piece
    {
        public eSoliderType m_SoliderType;
        private readonly int r_Row;
        private readonly int r_Column;
        public enum eSoliderType
        {
            Illegal,
            Empty,
            O,
            U,
            X,
            K,
        }
        public Piece(eSoliderType i_Type, int i_Row, int i_Column)
        {
            this.m_SoliderType = i_Type;
            this.r_Row = i_Row;
            this.r_Column = i_Column;
        }
        public Piece(int i_Row, int i_Column)
        {
            this.r_Row = i_Row;
            this.r_Column = i_Column;
        }
        public eSoliderType Type
        {
            get { return m_SoliderType; }
            set { m_SoliderType = value; }
        }
        public int Row
        {
            get { return r_Row; }
        }
        public int Column
        {
            get { return r_Column; }
        }
        public static string SoliderTypeToString(eSoliderType i_Type)
        {
            string soliderTypeString = "";
            switch (i_Type)
            {
                case (eSoliderType.Empty):
                    soliderTypeString = "   ";
                    break;
                case (eSoliderType.O):
                    soliderTypeString = " O ";
                    break;
                case (eSoliderType.X):
                    soliderTypeString = " X ";
                    break;
                case (eSoliderType.K):
                    soliderTypeString = " K ";
                    break;
                case (eSoliderType.U):
                    soliderTypeString = " U ";
                    break;
            }
            return soliderTypeString;
        }
    }
}
