using ePizzaHub.Models;
using ePizzaHub.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            PaymentModel payment = new PaymentModel();
            CartModel cart = TempData.Peek<CartModel>("Cart");
            if(cart != null)
            {

                payment.Cart = cart;
                payment.GrandTotal = cart.GrandTotal;
                payment.Currency = "SAR";
                payment.Description=string.Join(",",cart.Items.Select(x => x.Name));
            }
            return View(payment);
        }
    }
}
