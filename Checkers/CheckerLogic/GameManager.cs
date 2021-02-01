using System;
using System.Collections.Generic;
using static CheckerLogic.Move;
using static CheckerLogic.Player;

namespace CheckerLogic
{
    public class GameManager
    {
        public event EventHandler InvalidMove;

        public event EventHandler MakeMove;

        public event EventHandler EndGameRound;

        private readonly GameStatus r_GameStatus;
        private bool v_TurnPlayer1;
        private readonly GameTable r_GameTable;
        private readonly short r_TableSize;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private readonly static Random r_Random = new Random();
        private List<Move> m_LegalJumps;
        public GameManager(string i_Player1, string i_Player2, short i_TableSize)
        {
            r_GameStatus = new GameStatus(GameStatus.eGameStatus.StillPlaying);
            v_TurnPlayer1 = true;
            r_Player1 = new Player(Player.eShapeType.X, i_Player1, Player.ePlayerType.Person);
            if(i_Player2.Equals("Computer"))
            {
                r_Player2 = new Player(Player.eShapeType.O, "Computer", Player.ePlayerType.Computer);
            }
            else
            {
                r_Player2 = new Player(Player.eShapeType.O, i_Player2, Player.ePlayerType.Person);
            }
            r_TableSize = i_TableSize;
            r_GameTable = new GameTable(r_TableSize);
            r_GameTable.BuildTable();
            m_LegalJumps = new List<Move>();
        }

        public Player GetPlayer1()
        {
            return this.r_Player1;
        }

        public Player GetPlayer2()
        {
            return this.r_Player2;
        }

        public GameTable GetGameTable()
        {
            return this.r_GameTable;
        }
        public GameStatus.eGameStatus StatusGame
        {
            get { return this.r_GameStatus.m_StatusType; }
            set { this.r_GameStatus.m_StatusType = value; }
        }
        public void GameRound(Move i_CurrentMove)
        {
            i_CurrentMove.CurrentPiece = r_GameTable.GetSolider(i_CurrentMove.CurrentPiece.Row, i_CurrentMove.CurrentPiece.Column);
            i_CurrentMove.TargetPiece = r_GameTable.GetSolider(i_CurrentMove.TargetPiece.Row, i_CurrentMove.TargetPiece.Column);
            if (this.r_GameStatus.m_StatusType == GameStatus.eGameStatus.StillPlaying)
            {
                if (v_TurnPlayer1)
                {
                    playCurrentPlayerTurn(i_CurrentMove, r_Player1, r_Player2);
                }
                else
                {
                    if (r_Player2.PlayerType == Player.ePlayerType.Person)
                    {
                        playCurrentPlayerTurn(i_CurrentMove, r_Player2, r_Player1);
                    }
                }

                checkGameStatus();
                if (this.r_GameStatus.m_StatusType == GameStatus.eGameStatus.StillPlaying)
                {
                    if (!v_TurnPlayer1)
                    {
                        if (r_Player2.PlayerType == Player.ePlayerType.Computer)
                        {
                            PlayComputerTurn();
                        }
                    }
                }
            }

            checkGameStatus();
            if (this.r_GameStatus.m_StatusType != GameStatus.eGameStatus.StillPlaying)
            {
                if (this.r_GameStatus.m_StatusType == GameStatus.eGameStatus.Winner)
                {
                    EndGameRound.Invoke(GetPlayer1(), EventArgs.Empty);
                }
                else
                {
                    EndGameRound.Invoke(GetPlayer2(), EventArgs.Empty);
                }
            }
        }

        internal void PlayComputerTurn()
        {
            List<Move> computerJumpsMoves = r_GameTable.GetPlayerJumpsList(eShapeType.O);
            int lengthOfJumpsList = computerJumpsMoves.Count;
            Move currentMoveForComputer = null;
            if (lengthOfJumpsList > 0)
            {
                while (lengthOfJumpsList > 0)
                {
                    int indexOfJumplMove = r_Random.Next(0, lengthOfJumpsList);
                    currentMoveForComputer = computerJumpsMoves[indexOfJumplMove];
                    currentMoveForComputer.MoveType = eMoveType.Jump;
                    MakeMove.Invoke(currentMoveForComputer, EventArgs.Empty);
                    currentMoveForComputer.MoveOnTable(r_GameTable);
                    r_Player2.IsJumpTurn = true;

                    if (hasAnotherJump(currentMoveForComputer, r_Player2))
                    {
                        computerJumpsMoves = getListOfJumpsForPiece(r_Player2.GetShapeType(), currentMoveForComputer.TargetPiece);
                        lengthOfJumpsList = computerJumpsMoves.Count;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                List<Move> computerDiagonalMoves = r_GameTable.GetListOfPlayerDiagonalMoves(eShapeType.O);
                int lengthOfListDiagonal = computerDiagonalMoves.Count;
                int indexOfDiagonalMove = r_Random.Next(0, lengthOfListDiagonal);
                    currentMoveForComputer = computerDiagonalMoves[indexOfDiagonalMove];
                    currentMoveForComputer.MoveType = eMoveType.Diagonal;
                    MakeMove.Invoke(currentMoveForComputer, EventArgs.Empty);
                    currentMoveForComputer.MoveOnTable(r_GameTable);
              
            }
            v_TurnPlayer1 = !v_TurnPlayer1;
        }
  
        private void player1Win()
        {
            r_GameStatus.m_StatusType = GameStatus.eGameStatus.Winner;
            r_Player1.Points = r_GameTable.GetPlayerPoints(r_Player1.GetShapeType()) - r_GameTable.GetPlayerPoints(r_Player2.GetShapeType());
        }
        private void player2Win()
        {
            r_GameStatus.m_StatusType = GameStatus.eGameStatus.Looser;
            r_Player2.Points = r_GameTable.GetPlayerPoints(r_Player2.GetShapeType()) - r_GameTable.GetPlayerPoints(r_Player1.GetShapeType());
        }
        private void checkGameStatus()
        {
            List<Move> diagonalMovesOfPlayer1 = r_GameTable.GetListOfPlayerDiagonalMoves(eShapeType.X);
            List<Move> diagonalMovesOfPlayer2 = r_GameTable.GetListOfPlayerDiagonalMoves(eShapeType.O);
            List<Move> jumpsMovesOfPlayer1 = r_GameTable.GetPlayerJumpsList(eShapeType.X);
            List<Move> jumpsMovesOfPlayer2 = r_GameTable.GetPlayerJumpsList(eShapeType.O);
            if (diagonalMovesOfPlayer1.Count == 0 && diagonalMovesOfPlayer2.Count == 0 && jumpsMovesOfPlayer1.Count == 0 && jumpsMovesOfPlayer2.Count == 0)
            {
                this.r_GameStatus.m_StatusType = GameStatus.eGameStatus.Tie;
            }
            else
            {
                if (diagonalMovesOfPlayer1.Count == 0 && jumpsMovesOfPlayer1.Count == 0 || r_GameTable.GetPlayerPoints(r_Player1.GetShapeType()) == 0)
                {
                    player2Win();
                }
                else
                {
                    if (diagonalMovesOfPlayer2.Count == 0 && jumpsMovesOfPlayer2.Count == 0 || r_GameTable.GetPlayerPoints(r_Player2.GetShapeType()) == 0)
                    {
                        player1Win();
                    }
                }
            }
        }

        private void playCurrentPlayerTurn(Move i_CurrentMove, Player i_PlayerTurn, Player i_NotPlayerTurn)
        {
            bool isLegal = isLegalMove(i_CurrentMove, i_PlayerTurn);
            if (!isLegal)
            {
                InvalidMove.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MakeMove.Invoke(i_CurrentMove, EventArgs.Empty);
                i_CurrentMove.MoveOnTable(r_GameTable);

                if (i_PlayerTurn.IsJumpTurn)
                {
                    if (hasAnotherJump(i_CurrentMove, i_PlayerTurn))
                    {
                        m_LegalJumps = getListOfJumpsForPiece(i_PlayerTurn.GetShapeType(), i_CurrentMove.TargetPiece);
                    }
                    else
                    {
                        v_TurnPlayer1 = !v_TurnPlayer1;
                        i_PlayerTurn.IsJumpTurn = false;
                    }
                }
                else
                {
                    v_TurnPlayer1 = !v_TurnPlayer1;
                }
            }
        }

        private bool hasAnotherJump(Move i_CurrentMove, Player i_PlayerTurn)
        {
            List<Move> playerSecondJumps = getListOfJumpsForPiece(i_PlayerTurn.GetShapeType(), i_CurrentMove.TargetPiece);
            return (playerSecondJumps.Count > 0) ? true : false;
        }

        public bool isLegalMove(Move i_currentMove, Player i_PlayerTurn)
        {
            bool isLegal = false;

            if (i_PlayerTurn.IsJumpTurn)
            {
                if (isContainsMoveElement(m_LegalJumps, i_currentMove))
                {
                    isLegal = true;
                    i_currentMove.MoveType = eMoveType.Jump;
                }
            }
            else
            {
                List<Move> playerJumpMoves = r_GameTable.GetPlayerJumpsList(i_PlayerTurn.GetShapeType());
                if (playerJumpMoves.Count > 0)
                {
                    if (isContainsMoveElement(playerJumpMoves, i_currentMove))
                    {
                        isLegal = true;
                        i_currentMove.MoveType = eMoveType.Jump;
                        i_PlayerTurn.IsJumpTurn = true;
                    }
                    else
                    {
                        i_PlayerTurn.IsJumpTurn = false;
                    }
                }
                else
                {
                    if (i_currentMove.CheckIsLegalMove(i_PlayerTurn.GetShapeType()))
                    {
                        isLegal = true;
                        i_currentMove.MoveType = eMoveType.Diagonal;
                    }
                }
            }
            return isLegal;
        }
        private static bool isContainsMoveElement(List<Move> i_ListOfMoves, Move i_currentMove)
        {
            bool isContainsMove = false;
            foreach (Move m in i_ListOfMoves)
            {
                if (i_currentMove.IsEqualsTo(m))
                {
                    isContainsMove = true;
                    break;
                }
            }
            return isContainsMove;
        }

        private List<Move> getListOfJumpsForPiece(eShapeType i_Shape, Piece i_Solider)
        {
            int SoliderRow = i_Solider.Row;
            int SoliderColumn = i_Solider.Column;
            Move currentMove;
            List<Move> leggalJumpsForPiece = r_GameTable.GetPlayerJumpsList(i_Shape);
            for (int i = 0; i < leggalJumpsForPiece.Count; i++)
            {
                currentMove = leggalJumpsForPiece[i];

                if (currentMove.CurrentPiece.Row != SoliderRow || currentMove.CurrentPiece.Column != SoliderColumn)
                {
                    leggalJumpsForPiece.Remove(currentMove);
                }
            }
            return leggalJumpsForPiece;
        }
    }
}
