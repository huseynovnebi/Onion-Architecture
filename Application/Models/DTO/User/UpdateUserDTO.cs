using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTO.User
{
    public class UpdateUserDTO
    {
        [Required]
        [Column(TypeName = "number")]

        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string Age { get; set; }
    }
}
