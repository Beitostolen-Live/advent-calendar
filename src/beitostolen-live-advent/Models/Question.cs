using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beitostolen_live_api.Models
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        public DateTime QuestionForDate { get; set; }

        public string Image { get; set; }

        public string Text { get; set; }

        public virtual ICollection<Alternative> Alternatives { get; set; }
    }
}
