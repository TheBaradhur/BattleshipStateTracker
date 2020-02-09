using BattleshipStateTracker.Domain.Models;

namespace BattleshipStateTracker.Domain
{
    public interface IGameStateService
    {
        string GetGameStatus();

        void InitializeNewGame(string playerOneName, int totalNumberOfShips);

        GameRulesValidation ValidateAddShipRequest(int xTipPosition, int yTipPosition, string orientation, int size, bool addToPlayerOne);

        void AddShipOnPlayersBoard(int xPosition, int yPosition, string orientation, int size, bool addToPlayerOne);

        GameRulesValidation ValidateAttackPosition(int x, int y, int targetedPlayerNumber);

        string AttackPlayersPosition(int x, int y, int targetedPlayer);
    }
}