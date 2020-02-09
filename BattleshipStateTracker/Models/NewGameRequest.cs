using System.ComponentModel.DataAnnotations;

namespace BattleshipStateTracker.Api.Models
{
    public class NewGameRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Player name requires at least 3 characters")]
        public string PlayerOneName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You need at least 1 ship to initiate a battleship game")]
        public int TotalNumberOfShipsPerPlayer { get; set; }
    }
}