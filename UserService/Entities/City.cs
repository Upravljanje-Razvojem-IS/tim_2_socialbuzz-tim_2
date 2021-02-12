using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Entity class which models cities 
    /// </summary>
    public class City
    {
        /// <summary>
        /// Unique identifier for a city
        /// </summary>
        [Column("CityId")]
        [Key]
        [Required]
        public Guid CityId { get; set; }
   
        /// <summary>
        /// City name 
        /// </summary>
        [Column("CityName")]
        [StringLength(50)]
        [Required]
        public String CityName { get; set; }
    }
}
