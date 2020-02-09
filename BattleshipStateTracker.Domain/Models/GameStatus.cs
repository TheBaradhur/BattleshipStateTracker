namespace BattleshipStateTracker.Domain.Models
{
    public enum GameStatus
    {
        NoGame,
        Initiated,
        ShipSetup,
        ShipSetupCompleted,
        OnGoing,
        Finished
    }
}