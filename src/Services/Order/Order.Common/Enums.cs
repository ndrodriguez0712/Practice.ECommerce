namespace Order.Common
{
    public class Enums
    {
        public enum OrderStatus
        {
            Cancel,
            Pending,
            Approved
        }

        public enum OrderPayment
        {
            CreditCard,
            PayPal,
            BankTransfer
        }

        public enum ProductInStockAction
        {
            Add,
            Substract
        }
    }
}
