using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Services.Interfaces
{
    public interface ICartService
    {
        Cart GetCart(Guid id);
        CartModel GetCartDetails(Guid CartId);

        int GetCartCount(Guid CartId);
        Cart AddItem(int UserId,Guid CartId,int ItemId,decimal UnitPrice,int Quantity);
        int DeleteItem(Guid cartId, int ItemId);

        int UpdateQuantity(Guid cartId, int ItemId, int Quantity);

        int UpdateCart(Guid cartId, int UserId);
    }
}
