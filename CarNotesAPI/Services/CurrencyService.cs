namespace CarNotesAPI.Services
{
    public class CurrencyService
    {
        public static double Convert(
            double amount, DateOnly? date, string currency = "USD")
        {
            return amount / 2.815;
        }
    }
}

