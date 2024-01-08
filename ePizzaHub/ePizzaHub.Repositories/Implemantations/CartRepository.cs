using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Implemantations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext db) : base(db)
        {
        }

        public int DeleteItem(Guid cartId, int ItemId)
        {
            var item=_db.CartItems.Where(c=>c.CartId==cartId && c.Id==ItemId).FirstOrDefault();
            if(item!=null)
            {
                _db.CartItems.Remove(item);
               return _db.SaveChanges();
            }
            return 0;
        }

        public Cart GetCart(Guid id)
        {
            return _db.Carts.Include(i=>i.CartItems).Where(c => c.Id == id && c.IsActive==true).FirstOrDefault();
        }

        public CartModel GetCartDetails(Guid CartId)
        {
            var model = (from cart in _db.Carts
                         where cart.Id == CartId && cart.IsActive == true
                         select new CartModel
                         {
                             Id = cart.Id,
                             UserId = cart.UserId,
                             CreatedDate = cart.CreatedDate,
                             Items = (from cartItem in _db.CartItems
                                      join item in _db.Items
                                      on cartItem.ItemId equals item.Id
                                      where cartItem.CartId == CartId
                                      select new ItemModel
                                      {
                                          Id=cartItem.Id,
                                          Quantity = cartItem.Quantity,
                                          UnitPrice = cartItem.UnitPrice,
                                          ItemId = item.Id,
                                          Name=item.Name,
                                          Description=item.Description,
                                          ImageUrl=item.ImageUrl

                                      }).ToList()

                         }).FirstOrDefault();

            return model;
        }

        public int UpdateCart(Guid cartId, int UserId)
        {
           Cart cart = GetCart(cartId);
            cart.UserId = UserId;
            return _db.SaveChanges();
        }

        public int UpdateQuantity(Guid CartId, int ItemId, int Quantity)
        {
            bool flag = false;
            var cart = GetCart(CartId);
            if (cart != null)
            {
                var cartItems = cart.CartItems.ToList();
                for (int i = 0; i < cartItems.Count; i++)
                {
                    if (cartItems[i].Id == ItemId)
                    {
                        flag = true;
                        cartItems[i].Quantity += (Quantity);
                        break;
                    }
                }
                if (flag)
                {
                    cart.CartItems = cartItems;
                    return _db.SaveChanges();
                }
            }
            return 0;
        }
    }
}
