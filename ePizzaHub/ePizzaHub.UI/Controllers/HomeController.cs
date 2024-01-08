using ePizzaHub.Services.Implementations;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace ePizzaHub.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IItemService _itemservice;
        IMemoryCache _cache;
        public HomeController(ILogger<HomeController> logger, IItemService itemservice, IMemoryCache cache)
        {
            _logger = logger;
            _itemservice = itemservice;
            _cache = cache;
        }

        public IActionResult Index()
        {
            //string key = "catalog";
            string key = "catalog";
            var items = _cache.GetOrCreate(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(15);
                return _itemservice.GetItems();
            });
            //var items= _itemservice.GetItems();
            //try
            //{
            //    int x = 6;int y = 0;
            //    int z = x / y;

            //}catch(Exception ex)
            //{
            //    _logger.LogError(ex.Message,ex);
            //}
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
