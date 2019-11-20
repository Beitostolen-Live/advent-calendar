using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beitostolen_live_api.Models
{
    public class Winner
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Comment { get; set; }

        public WinnerStatus Status { get; set; }

        public bool OnlyWithCorrectAnswer { get; set; }

        public virtual Response Alternative { get; set; }
    }

    public enum WinnerStatus
    {
        InProgress,
        Completed
    }
}
