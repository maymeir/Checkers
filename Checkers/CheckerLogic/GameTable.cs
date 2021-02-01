using System.Collections.Generic;
using static CheckerLogic.Player;

namespace CheckerLogic
{
    public class GameTable
    {

        private readonly Piece[,] r_Table;
        private readonly short r_TableSize;
        internal GameTable(short i_TableSize)
        {
            this.r_TableSize = i_TableSize;
            r_Table = new Piece[this.r_TableSize, this.r_TableSize];
            for (int i = 0; i < this.r_TableSize; i++)
            {
                for (int j = 0; j < this.r_TableSize; j++)
                {
                    r_Table[i, j] = new Piece(Piece.eSoliderType.Empty, i, j);
                }
            }
        }

        internal short TableSize
        {
            get { return this.r_TableSize; }
        }
        internal Piece GetSolider(int i_Row, int i_Column)
        {
            return this.r_Table[i_Row, i_Column];
        }
        internal void BuildTable()
        {
            for (int i = 0; i < this.r_TableSize / 2 - 1; i++)
            {
                for (int j = 0; j < this.r_TableSize; j++)
                {
                    if (i % 2 == 1)
                    {
                        if (j % 2 == 0)
                        {
                            r_Table[i, j].Type = Piece.eSoliderType.O;
                        }
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            r_Table[i, j].Type = Piece.eSoliderType.O;
                        }
                    }
                }
            }
            for (int i = this.r_TableSize - 1; i > this.r_TableSize / 2; i--)
            {
                for (int j = 0; j < this.r_TableSize; j++)
                {
                    if (i % 2 == 1)
                    {
                        if (j % 2 == 0)
                        {
                            r_Table[i, j].Type = Piece.eSoliderType.X;
                        }
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            r_Table[i, j].Type = Piece.eSoliderType.X;
                        }
                    }
                }
            }
        }

        internal int GetPlayerPoints(eShapeType i_Piece)
        {
            int PlayerPoints = 0;
            Piece.eSoliderType soliderType = Piece.eSoliderType.X;
            Piece.eSoliderType kingType = Piece.eSoliderType.K;
            if (i_Piece.Equals(eShapeType.O))
            {
                soliderType = Piece.eSoliderType.O;
                kingType = Piece.eSoliderType.U;
            }
            for (int i = 0; i < this.r_TableSize; i++)
            {
                for (int j = 0; j < this.r_TableSize; j++)
                {
                    Piece currentPiece = this.r_Table[i, j];
                    if (currentPiece.Type == soliderType)
                    {
                        PlayerPoints += 1;
                    }
                    if (currentPiece.Type == kingType)
                    {
                        PlayerPoints += 4;
                    }
                }
            }
            return PlayerPoints;
        }
        private Piece.eSoliderType getSoliderTypeInTable(int i_Row, int i_Column)
        {
            Piece.eSoliderType typeToReturn;
            if (i_Row < 0 || i_Row > this.TableSize - 1 || i_Column < 0 || i_Column > this.TableSize - 1)
            {
                typeToReturn = Piece.eSoliderType.Illegal;
            }
            else
            {
                typeToReturn = this.r_Table[i_Row, i_Column].Type;
            }

            return typeToReturn;
        }
        internal List<Move> GetListOfPlayerDiagonalMoves(eShapeType i_Shape)
        {
            List<Move> legalMoves = new List<Move>();
            short TableSize = this.TableSize;
            int diagonalMove = 1;
            for (int r = 0; r < TableSize; r++)
            {
                for (int c = 0; c < TableSize; c++)
                {
                    switch (i_Shape)
                    {
                        case eShapeType.O:
                            if ((getSoliderTypeInTable(r, c) == Piece.eSoliderType.O)
                                || (getSoliderTypeInTable(r, c) == Piece.eSoliderType.U)) //regular piece of O
                            {
                                MoveDiagonal(r, c, ref legalMoves , diagonalMove);
                            }
                            if (getSoliderTypeInTable(r, c) == Piece.eSoliderType.U) //King of O
                            {

                                MoveDiagonal(r, c, ref legalMoves , (diagonalMove * -1));
                            }
                            break;
                        case eShapeType.X:
                            if ((getSoliderTypeInTable(r, c) == Piece.eSoliderType.X)
                                || (getSoliderTypeInTable(r, c) == Piece.eSoliderType.K)) //regular piece of X
                            {
                                MoveDiagonal(r, c, ref legalMoves , (diagonalMove * -1));
                            }
                            if (getSoliderTypeInTable(r, c) == Piece.eSoliderType.K) //King of X
                            {
                                MoveDiagonal(r, c, ref legalMoves , diagonalMove);


                            }
                            break;
                    }
                }
            }
            return legalMoves;
        }

        internal void MoveDiagonal(int i_R, int i_C, ref List<Move> i_LegalMoves , int i_DiagonalMove)
        {
            if ((getSoliderTypeInTable(i_R + i_DiagonalMove, i_C - 1) == Piece.eSoliderType.Empty))
            {
                i_LegalMoves.Add(new Move(GetSolider(i_R, i_C), GetSolider(i_R + i_DiagonalMove, i_C - 1)));
            }

            if ((getSoliderTypeInTable(i_R +  i_DiagonalMove, i_C + 1) == Piece.eSoliderType.Empty))
            {
                i_LegalMoves.Add(new Move(GetSolider(i_R, i_C), GetSolider(i_R +  i_DiagonalMove, i_C + 1)));
            }
        }
        internal List<Move> GetPlayerJumpsList(eShapeType i_Shape)
        {
            List<Move> legalJumps = new List<Move>();
            short TableSize = this.TableSize;
            int jumpMove = 1;
            Piece.eSoliderType soliderType = Piece.eSoliderType.X;
            Piece.eSoliderType kingType = Piece.eSoliderType.K;
            for (int r = 0; r < TableSize; r++)
            {
                for (int c = 0; c < TableSize; c++)
                {
                    switch (i_Shape)
                    {
                        case eShapeType.O:

                            if ((getSoliderTypeInTable(r, c) == Piece.eSoliderType.O) || (getSoliderTypeInTable(r, c) == Piece.eSoliderType.U))//regular piece of O
                            {
                                JumpMove(r, c, ref legalJumps ,soliderType ,kingType , jumpMove);
                            }
                            if (getSoliderTypeInTable(r, c) == Piece.eSoliderType.U) //King of O
                            {
                                JumpMove(r, c, ref legalJumps, soliderType, kingType , (jumpMove * -1));
                            }
                            break;
                        case eShapeType.X:
                            soliderType = Piece.eSoliderType.O;
                            kingType = Piece.eSoliderType.U;
                            if ((getSoliderTypeInTable(r, c) == Piece.eSoliderType.X) || (getSoliderTypeInTable(r, c) == Piece.eSoliderType.K))//regular piece of X
                            {
                                JumpMove(r, c, ref legalJumps, soliderType, kingType ,(jumpMove * -1));
                            }
                            if (getSoliderTypeInTable(r, c) == Piece.eSoliderType.K) //king of X
                            {
                                JumpMove(r, c, ref legalJumps, soliderType, kingType , jumpMove);
                            }
                            break;
                    }
                }
            }
            return legalJumps;
        }

        internal void JumpMove(int i_R, int i_C, ref List<Move> i_LegalJumps, Piece.eSoliderType i_SoliderType, Piece.eSoliderType i_KingType, int i_JumpMove)
        {
            if ((getSoliderTypeInTable(i_R + (2 * i_JumpMove), i_C - 2) == Piece.eSoliderType.Empty) && (getSoliderTypeInTable(i_R +  i_JumpMove, i_C - 1) == i_SoliderType || getSoliderTypeInTable(i_R + i_JumpMove, i_C - 1) == i_KingType))
            {
                i_LegalJumps.Add(new Move(GetSolider(i_R, i_C), GetSolider(i_R +( 2 * i_JumpMove), i_C - 2)));
            }
            if ((getSoliderTypeInTable(i_R +( 2 * i_JumpMove), i_C + 2) == Piece.eSoliderType.Empty) && (getSoliderTypeInTable(i_R + i_JumpMove, i_C + 1) == i_SoliderType || getSoliderTypeInTable(i_R + i_JumpMove, i_C + 1) == i_KingType))
            {
                i_LegalJumps.Add(new Move(GetSolider(i_R, i_C), GetSolider(i_R +( 2 * i_JumpMove), i_C + 2)));
            }

        }
    }
}
