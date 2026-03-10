using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record OrderItemDto
    (
        int OrderId,

        double? Quantity,

        string ProductName
    )
    {
        public OrderItemDto() : this( default, default, default) { }

    }
}
