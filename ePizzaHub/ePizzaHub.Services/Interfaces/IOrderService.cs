using ePizzaHub.Core.Entities;
using ePizzaHub.Models;

namespace ePizzaHub.Services.Interfaces
{
    public interface IOrderService: IService<Order>
    {
        OrderModel GetOrderDetails(string id);
        IEnumerable<Order> GetUserOrder(int UserId);

        int PlaceOrder(int userId, string orderId, string paymentId, CartModel cart, AddressModel address);
    }
}
