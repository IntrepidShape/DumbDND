namespace DumbDnD
{
    public class GameplayState
    {
        public int NumOfDeadPlayers { get; set;}
        public int Round { get; set;}
        public string LastPlayer { get; set;}
        public bool VictoryConditionMet { get; set;}
        public bool UntilLastPlayerDead { get; set;}
        public int NumberOfPlayers { get; set;}
        
    }
}