using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beitostolen_live_api.Models
{
    public class Response
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime Registered { get; set; }

        public virtual Question Question { get; set; }

        public virtual Alternative Answear { get; set; }
    }
}
