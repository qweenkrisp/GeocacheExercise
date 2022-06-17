using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeocacheAPI.Models
{
    public class Item
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public String Name { get; set; }

        public int? Geocache { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Active { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Inactive { get; set; }
    }
}
