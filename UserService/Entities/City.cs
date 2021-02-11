using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class City
    {
        [Column("CityId")]
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid CityId { get; set; }
   
        [Column("CityName")]
        [StringLength(50)]
        [Required]
        public String CityName { get; set; }
    }
}
