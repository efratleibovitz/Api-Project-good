using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record GetUserDTO
    (
        int Id,
        [EmailAddress]
        [Required]
        string UserEmail,
        string FirstName,
        string LastName
     
    );
}
