using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record productDto
    (
         string ProductName,

         double Price,

         int? CategoryId,

         string Description,

         string Category_Name

    )
    {
        public productDto() : this(default, default, default, default, default) { }
    }
}
