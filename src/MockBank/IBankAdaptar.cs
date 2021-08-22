using System.Threading.Tasks;

namespace MockBank
{
    public interface IBankAdaptar
    {
        /// <summary>
        /// Process a payment through a bank
        /// </summary>
        /// <param name="request">the payment request</param>
        /// <returns>the payment response from the bank</returns>
        Task<BankPaymentResponse> ProcessPaymentAsync(BankPaymentRequest request);
    }
}
