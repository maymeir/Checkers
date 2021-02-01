
namespace CheckerLogic
{
   public class Player
    {
        public enum eShapeType
        {
            X,
            O,
        }
        public enum ePlayerType
        {
            Person,
            Computer,
        }
            private readonly string r_PlayerName;
            private readonly eShapeType r_Shape;
            private ePlayerType m_PlayerType;
            private bool v_JumpTurn;
            private int m_Points;
            public Player(eShapeType i_Shape, string i_PlayerName, ePlayerType i_PlayerType)
            {
                this.r_Shape = i_Shape;
                this.r_PlayerName = i_PlayerName;
                v_JumpTurn = false;
                this.m_PlayerType = i_PlayerType;
                this.m_Points = 0;
            }
            internal ePlayerType PlayerType
            {
                get
                {
                    return m_PlayerType;
                }
                set
                {
                    m_PlayerType = value;
                }
            }
            public int Points
            {
                get
                {
                    return m_Points;
                }
                set
                {
                    m_Points = value;
                }
            }
            internal bool IsJumpTurn
            {
                get
                {
                    return v_JumpTurn;
                }
                set
                {
                    v_JumpTurn = value;
                }
            }
            public string Name
            {
                get
                {
                    return r_PlayerName;
                }
            }
            internal eShapeType GetShapeType()
            {
                return r_Shape;
            }
    }
}
