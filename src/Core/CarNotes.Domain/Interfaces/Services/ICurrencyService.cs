namespace CarNotes.Domain.Interfaces.Services
{
    public interface ICurrencyService
    {
        static double Convert(double amount, DateOnly? date, string currency = "USD")
        {
            // ToDo: requires implementation.
            return amount / 2.75;
        }
    }
}

