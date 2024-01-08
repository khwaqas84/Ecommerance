using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

using ePizzaHub.Repositories;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
namespace ePizzaHub.Services.Implementations
{
    public class CartService : Service<Cart>, ICartService
    {
        ICartRepository _cartRepo;
        IRepository<CartItem> _cartItem;
        private readonly IConfiguration _configuration;
        public CartService(ICartRepository cartRepo,IRepository<CartItem> cartItem,IConfiguration config):
            base(cartRepo)
        {
            _cartItem = cartItem;
            _configuration = config;
            _cartRepo = cartRepo;
        }

        public int DeleteItem(Guid cartId, int ItemId)
        {
           return _cartRepo.DeleteItem(cartId,ItemId);
        }

        public Cart GetCart(Guid id)
        {
            return _cartRepo.GetCart(id);
        }

        public CartModel GetCartDetails(Guid CartId)
        {
            var model= _cartRepo.GetCartDetails(CartId);
            if(model != null&& model.Items.Count>0)
            {
                decimal subTotal = 0;
                foreach(var item in model.Items)
                {
                    item.Total=item.UnitPrice*item.Quantity;
                    subTotal = subTotal + item.Total;
                }
                model.Total=subTotal;
                model.Tax=Math.Round(model.Total* Convert.ToDecimal(_configuration["TAX:GST"])/100, 2);
                model.GrandTotal=model.Total+model.Tax;
            }
            return model;
        }

        public int UpdateCart(Guid cartId, int UserId)
        {
            return _cartRepo.UpdateCart(cartId, UserId);
        }

        public int UpdateQuantity(Guid cartId, int ItemId, int Quantity)
        {
            return _cartRepo.UpdateQuantity(cartId, ItemId,Quantity);
        }

        public Cart AddItem(int UserId, Guid CartId, int ItemId, decimal UnitPrice, int Quantity)
        {
            try
            {
                var cart = GetCart(CartId);
                if (cart == null)
                {
                    cart = new Cart();
                    cart.Id = CartId;
                    cart.UserId = UserId;
                    cart.CreatedDate = DateTime.Now;
                    cart.IsActive = true;
                    CartItem item = new CartItem { CartId = CartId, ItemId = ItemId, Quantity = Quantity, UnitPrice = UnitPrice };

                    cart.CartItems.Add(item);
                    _cartRepo.Add(cart);
                    _cartRepo.SaveChanges();
                }
                else
                {
                    CartItem item = cart.CartItems.Where(ci=>ci.ItemId==ItemId).FirstOrDefault();
                    if (item != null)
                    {
                        item.Quantity += Quantity;
                        _cartItem.Add(item);
                        _cartItem.SaveChanges();

                    }
                    else
                    {
                        item = new CartItem { CartId = CartId, ItemId = ItemId, Quantity = Quantity, UnitPrice = UnitPrice };

                        cart.CartItems.Add(item);
                        _cartItem.Update(item);
                        _cartItem.SaveChanges();
                    }


                }
                return cart;

            }
            catch (Exception ex)
            {
                return null;
                
            }
        }

        public int GetCartCount(Guid CartId)
        {
            var cart=_cartRepo.GetCart(CartId);
            return cart!=null?cart.CartItems.Count : 0;
        }

       
    }
}
