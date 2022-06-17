using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeocacheAPI.Models
{
    public class Geocache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public String Name { get; set; }

        [StringLength(50)]
        public String Coordinates { get; set; }
    }
}
