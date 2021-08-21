namespace FlameAndWax.Data.Constants
{
    public class Constants
    {
        //public const string DB_CONNECTION_STRING = @"Server=(localdb)\\MSSQLLocalDB;Database=FlameAndWaxDB;Trusted_Connection=True;";
        public const string DB_CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FlameAndWaxDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public enum ReviewScore
        {
            Excellent,
            VeryGood,
            Good,
            Poor,
            VeryPoor
        }
        public enum ModeOfPayment
        {
            Cash,
            Cheque
        }
        public enum Courier
        {
            FoodPanda,
            JNT,
            NinjaVan,
            GogoExpress
        }
        public enum AccountStatus
        {
            Deactivated,
            Activated
        }
        public enum Category
        {
            Candle,
            Soap,
            Diffuser
        }
        public enum Roles
        {
            Customer,
            Employee,
            Administrator
        }
    }
}
