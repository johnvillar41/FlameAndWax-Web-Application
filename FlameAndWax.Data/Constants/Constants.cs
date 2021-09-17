namespace FlameAndWax.Data.Constants
{
    public class Constants
    {
        public const string ALL_PRODUCTS = "All Products";
        public const string GCASH_PAYMENT_DETAILS = "After clicking 'Complete Order', you will be redirected to Pay via Credit/Debit Card & PayMaya Wallet to complete your purchase securely.";
        public const string PAYPAL_PAYMENT_DETAILS = "";
        public const string DEBIT_CREDIT_PAYMENT_DETAILS = "";

        public enum Category
        {
            Candle,
            Soap,
            Diffuser
        }
        public enum Courier
        {
            FoodPanda,
            JNT,
            NinjaVan,
            GogoExpress
        }
        public enum CustomerAccountStatus
        {
            Banned,
            Active,
            Pending
        }
        public enum EmployeeAccountStatus
        {
            Deactivated,
            Activated
        }
        public enum ModeOfPayment
        {
            Cash,
            GCash,
            DebitCreditCard,
            PayPal
        }
        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipping,
            Finished,
            Cancelled
        }
        public enum ReviewScore
        {
            Excellent = 5,
            VeryGood = 4,
            Good = 3,
            Poor = 2,
            VeryPoor = 1
        }
        public enum Roles
        {
            Customer,
            Employee,
            Administrator
        }
    }
}
