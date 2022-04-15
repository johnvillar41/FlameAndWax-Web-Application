namespace FlameAndWax.Data.Constants
{
    public class Constants
    {
        public const string ALL_PRODUCTS = "All Products";
        public const string GCASH_PAYMENT_DETAILS = "After clicking 'Complete Order', you will be redirected to Pay via Credit/Debit Card & PayMaya Wallet to complete your purchase securely.";
        public const string PAYPAL_PAYMENT_DETAILS = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        public const string DEBIT_CREDIT_PAYMENT_DETAILS = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?";

        public const string BASE_URL_CUSTOMER = "https://localhost:44385";
        public const string BASE_URL_API_IMAGES = "https://localhost:44353";
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
