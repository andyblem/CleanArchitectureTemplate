using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities
{
    public class GeneralAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime InsertedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "json")]
        public string Data { get; set; }

        [StringLength(20)]
        public string User { get; set; }
    }
}
