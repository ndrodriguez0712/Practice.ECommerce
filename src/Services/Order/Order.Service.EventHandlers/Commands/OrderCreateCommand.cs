using MediatR;
using static Order.Common.Enums;

namespace Order.Service.EventHandlers.Commands
{
    public class OrderCreateCommand : INotification
    {
        public OrderPayment PaymentType { get; set; }
        public int ClientId { get; set; }
        public IEnumerable<OrderCreateDetail> Items { get; set; } = new List<OrderCreateDetail>();
    }    
}
