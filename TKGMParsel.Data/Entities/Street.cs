using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKGMParsel.Data.Entities
{
    
    public  class Street
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50), Column(TypeName = "Varchar(50)")]
        public string? Name { get; set; }

        [Column(TypeName = "int")]

        public int? TKGMValue { get; set; }

        [Column(TypeName = "int")]
        public int? TKGMDistrictValue { get; set; }
        [Column(TypeName = "int")]
        public int DistrictId { get; set; }
        public District District { get; set; }
    }
}
