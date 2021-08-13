namespace FlameAndWax.Data.Constants
{
    public class Constants
    {
        public const string DB_CONNECTION_STRING = @"Server=(localdb)\\MSSQLLocalDB;Database=FlameAndWaxDB;Trusted_Connection=True;";
        public enum ReviewScore
        {
            Good,
            Bad,
            Meh
        }
        public enum ModeOfPayment
        {
            Cash,
            Cheque
        }
        public enum Courier
        {
            FoodPanda,
            JNT
        }
    }
}
