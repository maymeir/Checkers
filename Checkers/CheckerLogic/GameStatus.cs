
namespace CheckerLogic
{
    public class GameStatus
    {
        public enum eGameStatus
        {
            Winner,
            Looser,
            Tie,
            StillPlaying,
        }
        internal eGameStatus m_StatusType;
        internal GameStatus(eGameStatus i_StatusType)
        {
            this.m_StatusType = i_StatusType;
        }
    }
}
