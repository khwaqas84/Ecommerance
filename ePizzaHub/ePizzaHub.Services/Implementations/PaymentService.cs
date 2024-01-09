using ePizzaHub.Core.Entities;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace ePizzaHub.Services.Implementations
{
    public class PaymentService : Service<PaymentDetail>, IPaymentService
    {
        private readonly RazorpayClient _client;
        IRepository<PaymentDetail> paymrepository;
        ICartRepository cartRepository;
        IConfiguration configuration;
        public PaymentService(IConfiguration _configuration,IRepository<PaymentDetail> _paymentrepo,ICartRepository _cartRepo) : base(_paymentrepo)
        {
            paymrepository = _paymentrepo;
            cartRepository = _cartRepo;
            configuration = _configuration;
            if (_client == null)
            {
                string Key      = configuration["RazorPay:Key"];
                string Secret   = configuration["RazorPay:Secret"];

                _client = new RazorpayClient(Key, Secret);
            }

        }

        public string CreateOrder(decimal amount, string Currency, string receipt)
        {
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", amount); // amount in the smallest currency unit
            options.Add("receipt", receipt);
            options.Add("currency", Currency);
            //options.Add("amount", 50000); // amount in the smallest currency unit
            //options.Add("receipt", "order_rcptid_11");
            //options.Add("currency", "INR");
            Razorpay.Api.Order order = _client.Order.Create(options);
            return order["id"].ToString();
        }

        public Payment GetPaymentDetails(string paymentId)
        {

            return _client.Payment.Fetch(paymentId);
        }

        public int SavePaymentDetails(PaymentDetail model)
        {
            paymrepository.Add(model);
            Cart cart = cartRepository.Find(model.CartId);
            cart.IsActive = false;
            return paymrepository.SaveChanges();
        }

        public bool VerifySignature(string signature, string orderId, string paymentId)
        {
            string payload = string.Format("{0}|{1}", orderId, paymentId);
            string secret = RazorpayClient.Secret;
            string actualSignature = getActualSignature(payload, secret);
            return actualSignature.Equals(signature);
        }

        private static string getActualSignature(string payload, string secret)
        {
            byte[] secretBytes = StringEncode(secret);
            HMACSHA256 hashHmac = new HMACSHA256(secretBytes);
            var bytes = StringEncode(payload);

            return HashEncode(hashHmac.ComputeHash(bytes));
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
