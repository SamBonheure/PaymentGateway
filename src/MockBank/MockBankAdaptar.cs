using PaymentGateway.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace MockBank
{
    public class MockBankAdaptar : IBankAdaptar
    {
        /// <summary>
        /// Process a payment through a mock bank
        /// </summary>
        /// <param name="request">the payment request</param>
        /// <returns>the payment response from the bank</returns>
        public async Task<BankPaymentResponse> ProcessPaymentAsync(BankPaymentRequest request)
        {
            // Put random delay to simulate bank behaviour
            Random random = new Random();
            var delayInMilliseconds = random.Next(5, 15) * 1000;
            System.Threading.Thread.Sleep(delayInMilliseconds);

            // Automatically decline when over 1000
            if(request.Amount > 1000)
                return await Task.FromResult(new BankPaymentResponse(BankPaymentStatus.Declined, request.PaymentId, PaymentDeclinedReasonCode.PotentialFraudulentPayment));

            // Randomize approval status
            Array statusValues = Enum.GetValues(typeof(BankPaymentStatus));
            BankPaymentStatus randomPaymentStatus = (BankPaymentStatus)statusValues.GetValue(random.Next(statusValues.Length));
            PaymentDeclinedReasonCode? randomDeclinedReason = null;

            if (randomPaymentStatus == BankPaymentStatus.Declined)
            {
                // Randomize decline reason
                Array reasonValues = Enum.GetValues(typeof(PaymentDeclinedReasonCode));
                randomDeclinedReason = (PaymentDeclinedReasonCode)reasonValues.GetValue(random.Next(reasonValues.Length));
            }

            return await Task.FromResult(new BankPaymentResponse(randomPaymentStatus, request.PaymentId, randomDeclinedReason));
        }
    }
}
