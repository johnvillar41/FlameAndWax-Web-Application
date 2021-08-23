namespace FlameAndWax.Data.Constants
{
    public class Constants
    {
        //public const string DB_CONNECTION_STRING = @"Server=(localdb)\\MSSQLLocalDB;Database=FlameAndWaxDB;Trusted_Connection=True;";
        public const string DB_CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FlameAndWaxDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public enum ReviewScore
        {
            Excellent = 5,
            VeryGood = 4,
            Good = 3,
            Poor = 2,
            VeryPoor = 1
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
        public enum CustomerAccountStatus
        {
            Banned,
            Active
        }
        public enum EmployeeAccountStatus
        {
            Deactivated,
            Activated
        }
        public enum OrderDetailStatus
        {
            Pending,
            Finished,
            Processing,            
            Shipping,
            Cancelled
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
