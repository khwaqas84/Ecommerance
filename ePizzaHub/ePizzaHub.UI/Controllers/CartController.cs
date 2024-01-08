using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ePizzaHub.UI.Controllers
{
    public class CartController : BaseController
    {
        ICartService _cartService;

        Guid CartId
        {
            get
            {
                Guid Id;
                string CID = Request.Cookies["CID"];
                if (string.IsNullOrEmpty(CID))
                {
                    Id = Guid.NewGuid();
                    Response.Cookies.Append("CID", Id.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(1) });

                }
                else
                { 
                    Id=Guid.Parse(CID);
                }
                return Id;
            }
        }
        

        public CartController(ICartService cartService)
        {
            _cartService= cartService;
        }
        public IActionResult Index()
        {
            CartModel cart =_cartService.GetCartDetails(CartId);
            return View(cart);
        }
        [Route("/Cart/AddToCart/{ItemId}/{UnitPrice}/{Quantity}")]
        public IActionResult AddToCart(int ItemId,decimal UnitPrice,int Quantity)
        {
           int UserId=CurrentUser!=null?CurrentUser.Id:0;
            if (ItemId > 0 && Quantity > 0) 
            { 
                Cart cart = _cartService.AddItem(UserId,CartId,ItemId,UnitPrice,Quantity);
                JsonSerializerOptions serializationOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                };

                var data= System.Text.Json.JsonSerializer.Serialize(cart, serializationOptions);
                return Json(data);
            }
            else
            { return Json(""); }
        }
        [Route("/Cart/UpdateQuantity/{Id}/{Quantity}")]
        public IActionResult UpdateQuantity(int Id,  int Quantity)
        {
            int count = _cartService.UpdateQuantity(CartId, Id, Quantity);
                return Json(count);
            
        }

        public IActionResult DeleteItem(int Id)
        {
            int count = _cartService.DeleteItem(CartId, Id);
            return Json(count);

        }

        public IActionResult GetCartCount()
        {
            int count = _cartService.GetCartCount(CartId);
            return Json(count);

        }
        //[HttpPost]
        //public IActionResult CheckOut(AddressModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        CartModel cart = _cartService.GetCartDetails(CartId);
        //        if (cart != null && CurrentUser != null)
        //        {
        //            _cartService.UpdateCart(cart.Id, CurrentUser.Id);
        //        }
        //        TempData.Set("Cart", cart);
        //        TempData.Set("Address", model);
        //        return RedirectToAction("Index", "Payment");
        //    }
        //    return View();
        //}
        public IActionResult CheckOut()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CheckOut( AddressModel model)
        {
            if (ModelState.IsValid)
            { 
            CartModel cart =_cartService.GetCartDetails(CartId);
                if (cart != null&& CurrentUser!=null) {
                 _cartService.UpdateCart(cart.Id,CurrentUser.Id);
                }
                TempData.Set("Cart", cart);
                TempData.Set("Address", model);
                return RedirectToAction("Index","Payment");
            }
            
            return View(model);
        }

    }
}
