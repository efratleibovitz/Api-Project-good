using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
     public record LoginDTO
    (
        [Required]
        string UserEmail,
        [Required]
        string Password
    );
}
