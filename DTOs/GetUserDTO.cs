using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record GetUserDTO
    (
        int Id,
        string UserEmail,
        string FirstName,
        string LastName
     
    );
}
