using static CheckerLogic.Player;

namespace CheckerLogic
{
    public class Move
    {
        public enum eMoveType
        {
            Jump,
            Diagonal,
        }

            private Piece m_CurrentPiece;
            private Piece m_TargetPiece;
            private eMoveType m_MoveType;
        public Move()
        {
            m_CurrentPiece = null;
            m_TargetPiece = null;
        }
        public Move(Piece i_CurrentPiece, Piece i_TargetPiece)
            {
                this.m_CurrentPiece = i_CurrentPiece;
                this.m_TargetPiece = i_TargetPiece;
                this.m_MoveType = eMoveType.Diagonal;
            }
            public eMoveType MoveType
            {
                get { return m_MoveType; }
                set
                {
                    m_MoveType = value;
                }
            }
            public Piece TargetPiece
            {
            get { return this.m_TargetPiece; }
            set   {
                m_TargetPiece = value;
            }
        }

            public Piece CurrentPiece
            {
            get { return this.m_CurrentPiece; }
            set
                {
                m_CurrentPiece = value;
            }
        }

            public bool IsEqualsTo(Move i_MovetoCompare)
            {
                bool isEqual = true;
                if (this.CurrentPiece.Row != i_MovetoCompare.CurrentPiece.Row || this.TargetPiece.Row != i_MovetoCompare.TargetPiece.Row)
                {
                    isEqual = false;
                }
                if (this.CurrentPiece.Column != i_MovetoCompare.CurrentPiece.Column || this.TargetPiece.Column != i_MovetoCompare.TargetPiece.Column)
                {
                    isEqual = false;
                }

                return isEqual;
            }

            internal void MoveOnTable(GameTable i_GameTable)
            {
                switch (this.MoveType)
                {
                    case (eMoveType.Diagonal):

                        if (m_CurrentPiece.Type == Piece.eSoliderType.X && m_TargetPiece.Row == 0)
                        {
                            m_TargetPiece.Type = Piece.eSoliderType.K;
                        }

                        else
                            if (m_CurrentPiece.Type == Piece.eSoliderType.O && m_TargetPiece.Row == i_GameTable.TableSize - 1)
                        {
                            m_TargetPiece.Type = Piece.eSoliderType.U;
                        }
                        else
                        {
                            m_TargetPiece.Type = m_CurrentPiece.Type;
                        }
                        m_CurrentPiece.Type = Piece.eSoliderType.Empty;
                        break;

                    case (eMoveType.Jump):
                        locatePieceOnTable(i_GameTable);
                        if (m_CurrentPiece.Type == Piece.eSoliderType.X && m_TargetPiece.Row == 0)
                        {
                            m_TargetPiece.Type = Piece.eSoliderType.K;
                        }
                        else
                        {
                            if (m_CurrentPiece.Type == Piece.eSoliderType.O && m_TargetPiece.Row == i_GameTable.TableSize - 1)
                            {
                                m_TargetPiece.Type = Piece.eSoliderType.U;
                            }
                            else
                            {
                                m_TargetPiece.Type = m_CurrentPiece.Type;
                            }
                        }
                        m_CurrentPiece.Type = Piece.eSoliderType.Empty;
                        break;
                }
            }

            internal void locatePieceOnTable(GameTable i_GameTable)
            {
                int rowOfLocatePiece = 0;
                int columnOfLocatePiece = 0;
                if (m_CurrentPiece.Row > m_TargetPiece.Row)
                {
                    rowOfLocatePiece = m_CurrentPiece.Row - 1;

                    if (m_CurrentPiece.Column > m_TargetPiece.Column)
                    {
                        columnOfLocatePiece = m_CurrentPiece.Column - 1;
                    }
                    else
                    {
                        columnOfLocatePiece = m_CurrentPiece.Column + 1;
                    }
                }
                else
                {
                    rowOfLocatePiece = m_CurrentPiece.Row + 1;

                    if (m_CurrentPiece.Column > m_TargetPiece.Column)
                    {
                        columnOfLocatePiece = m_CurrentPiece.Column - 1;
                    }
                    else
                    {
                        columnOfLocatePiece = m_CurrentPiece.Column + 1;
                    }
                }

                i_GameTable.GetSolider(rowOfLocatePiece, columnOfLocatePiece).Type = Piece.eSoliderType.Empty;
            }
            public bool CheckIsLegalMove(eShapeType i_ShapeOfPlayer)
            {
                bool isLegalMove = true;

                switch (i_ShapeOfPlayer)
                {
                    case eShapeType.X:
                        if (m_CurrentPiece.Type != Piece.eSoliderType.X && m_CurrentPiece.Type != Piece.eSoliderType.K)
                        {
                            isLegalMove = false;
                        }
                        else
                        {
                            if (m_TargetPiece.Type != Piece.eSoliderType.Empty)
                            {
                                isLegalMove = false;
                            }
                            else
                            {
                                if (m_CurrentPiece.Type == Piece.eSoliderType.X)
                                {
                                    isLegalMove = isLegalDiagonalMove(eShapeType.X);
                                }
                                else
                                {
                                    isLegalMove = isLegalDiagonalKingMove();
                                }
                            }
                        }
                        break;

                    case eShapeType.O:
                        if (m_CurrentPiece.Type != Piece.eSoliderType.O && m_CurrentPiece.Type != Piece.eSoliderType.U)
                        {
                            isLegalMove = false;

                        }
                        else
                        {
                            if (m_TargetPiece.Type != Piece.eSoliderType.Empty)
                            {
                                isLegalMove = false;
                            }
                            else
                            {
                                if (m_CurrentPiece.Type == Piece.eSoliderType.O)
                                {
                                    isLegalMove = isLegalDiagonalMove(eShapeType.O);
                                }

                                else
                                {
                                    isLegalMove = isLegalDiagonalKingMove();
                                }
                            }
                        }
                        break;
                }
                return isLegalMove;
            }

            internal bool isLegalDiagonalMove(eShapeType i_Shape)
            {
                bool isLegalMove = false;
                switch (i_Shape)
                {
                    case eShapeType.X:
                        if ((m_CurrentPiece.Row - 1 == m_TargetPiece.Row) && (m_CurrentPiece.Column - 1 == m_TargetPiece.Column))
                        {
                            isLegalMove = true;
                        }
                        if ((m_CurrentPiece.Row - 1 == m_TargetPiece.Row) && (m_CurrentPiece.Column + 1 == m_TargetPiece.Column))
                        {
                            isLegalMove = true;
                        }
                        break;
                    case eShapeType.O:
                        if ((m_CurrentPiece.Row + 1 == m_TargetPiece.Row) && (m_CurrentPiece.Column - 1 == m_TargetPiece.Column))
                        {
                            isLegalMove = true;
                        }
                        if ((m_CurrentPiece.Row + 1 == m_TargetPiece.Row) && (m_CurrentPiece.Column + 1 == m_TargetPiece.Column))
                        {
                            isLegalMove = true;
                        }
                        break;
                }
                return isLegalMove;
            }

            internal bool isLegalDiagonalKingMove()
            {
                bool isLegalKingMove = false;
                if ((m_CurrentPiece.Row - 1 == m_TargetPiece.Row) && (m_CurrentPiece.Column - 1 == m_TargetPiece.Column))
                {
                    isLegalKingMove = true;
                }
                if ((m_CurrentPiece.Row - 1 == m_TargetPiece.Row) && (m_CurrentPiece.Column + 1 == m_TargetPiece.Column))
                {
                    isLegalKingMove = true;
                }
                if ((m_CurrentPiece.Row + 1 == m_TargetPiece.Row) && (m_CurrentPiece.Column - 1 == m_TargetPiece.Column))
                {
                    isLegalKingMove = true;
                }
                if ((m_CurrentPiece.Row + 1 == m_TargetPiece.Row) && (m_CurrentPiece.Column + 1 == m_TargetPiece.Column))
                {
                    isLegalKingMove = true;
                }
                return isLegalKingMove;
            }
    }
}
