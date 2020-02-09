namespace BattleshipStateTracker.Domain.Models
{
    public class GameRulesValidation
    {
        public bool IsValid { get; set; }

        public string ErrorCode { get; set; }

        public string Error { get; set; }

        public GameRulesValidation()
        { }

        public GameRulesValidation(bool isValid)
        {
            IsValid = isValid;
        }

        public GameRulesValidation(bool isValid, string errorCode, string error)
        {
            IsValid = isValid;
            ErrorCode = errorCode;
            Error = error;
        }
    }
}