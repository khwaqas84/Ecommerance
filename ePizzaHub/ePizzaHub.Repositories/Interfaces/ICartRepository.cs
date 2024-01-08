using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using System;


namespace ePizzaHub.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetCart(Guid id);
        CartModel GetCartDetails(Guid CartId);

        int DeleteItem(Guid cartId,int ItemId);

        int UpdateQuantity(Guid cartId, int ItemId,int Quantity);

        int UpdateCart(Guid cartId, int UserId);
    }
}
