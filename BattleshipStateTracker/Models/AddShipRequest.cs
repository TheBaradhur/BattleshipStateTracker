using System.ComponentModel.DataAnnotations;

namespace BattleshipStateTracker.Api.Models
{
    public class AddShipRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Board coordinates must be positive")]
        public int XPosition { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Board coordinates must be positive")]
        public int YPosition { get; set; }

        [Required]
        [RegularExpression("up|Up|down|Down|left|Left|right|Right", ErrorMessage = "Choices are only: Up, Down, Left, Right")]
        public string Orientation { get; set; }

        [Required]
        public int Size { get; set; }

        public bool AddToPlayerOne { get; set; } = true;
    }
}