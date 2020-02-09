using System.ComponentModel.DataAnnotations;

namespace BattleshipStateTracker.Api.Models
{
    public class NewGameRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Player name requires at least 3 characters")]
        public string PlayerOneName { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "You need at least 1 ship to initiate a battleship game, no more than 10 per player")]
        public int TotalNumberOfShipsPerPlayer { get; set; }
    }
}