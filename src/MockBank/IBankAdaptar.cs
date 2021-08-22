using System.Threading.Tasks;

namespace MockBank
{
    public interface IBankAdaptar
    {
        Task<BankPaymentResponse> ProcessPaymentAsync(BankPaymentRequest request);
    }
}
