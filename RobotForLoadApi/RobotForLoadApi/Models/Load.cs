using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RobotForLoadApi.Models
{
    public class Load
    {
        [Required]
        public double? X { get; set; }
        [Required]
        public double? Y { get; set; }
    }
}
