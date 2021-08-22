namespace PaymentGateway.Domain.Enums
{
    /// <summary>
    /// Inspired by https://docs.checkout.com/resources/codes/response-codes
    /// </summary>
    public enum PaymentDeclinedReasonCode
    {
        /// <summary>
        /// The payment request timed out
        /// </summary>
        Timeout,

        /// <summary>
        /// The aquiring bank is unavailable
        /// </summary>
        BankUnavailable,

        /// <summary>
        /// The API gateway encountered a internal error
        /// </summary>
        GatewayError,

        /// <summary>
        /// The merchant provided is not registered
        /// </summary>
        InvalidMerchant,

        /// <summary>
        /// The card details provided are invalid
        /// </summary>
        InvalidCardDetails,

        /// <summary>
        /// Insufficient funds in bank account to perform transaction
        /// </summary>
        InsufficientFunds,
    }
}
