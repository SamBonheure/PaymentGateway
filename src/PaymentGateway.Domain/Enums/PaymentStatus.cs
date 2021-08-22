namespace PaymentGateway.Domain.Enums
{
    /// <summary>
    /// Inspired from https://docs.checkout.com/resources/codes/response-codes
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Payment requested, waiting for confirmation
        /// </summary>
        Pending,

        /// <summary>
        /// Payment approved
        /// </summary>
        Approved,

        /// <summary>
        /// Payment declined
        /// </summary>
        Declined
    }
}
