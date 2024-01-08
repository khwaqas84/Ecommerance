

using ePizzaHub.Core.Entities;
using Razorpay.Api;

namespace ePizzaHub.Services.Interfaces
{
    public interface IPaymentService
    {
        string CreateOrder(decimal amount, string Currency, string receipt);
        Payment GetPaymentDetails(string paymentId);

        bool VerifySignature(string signature,string orderId, string paymentId);

        int SavePaymentDetails(PaymentDetail model);
    }
}
