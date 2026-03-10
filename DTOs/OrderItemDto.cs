using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record OrderItemDto
    (
        int OrderId,
        [Range(0.01, double.MaxValue, ErrorMessage = "הכמות חייבת להיות גדולה מאפס")]
        double? Quantity,

        int ProductId
    )
    {
        public OrderItemDto() : this(default, default, default) { }

    }
}
