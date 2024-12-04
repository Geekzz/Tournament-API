using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The title must be between 2 and 30 characters long.")]
        public string? Title { get; set; }
        [Required]
        public DateTime Time { get; set; }
        public int TournamentDetailsId { get; set; }
    }
}
