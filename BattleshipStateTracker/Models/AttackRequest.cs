using System.ComponentModel.DataAnnotations;

namespace BattleshipStateTracker.Api.Models
{
    public class AttackRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Board coordinates must be positive")]
        public int XAttackCoordinate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Board coordinates must be positive")]
        public int YAttackCoordinate { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Please select player 1 or 2")]
        public int TargetedUser { get; set; }
    }
}