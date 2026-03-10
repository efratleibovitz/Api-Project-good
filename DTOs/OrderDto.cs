namespace DTOs
{
    public record OrderDto
    (
        int orderId,
        DateOnly? OrderDate,

        double OrderSum,

        int UserId,
        ICollection<OrderItemDto> OrderItems

    //    ICollection<OrderItem> OrderItems= new List<OrderItem>()

    )
    {
        public OrderDto() : this( default, default, default, default, default) { }

    }


}
