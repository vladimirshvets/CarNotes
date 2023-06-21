using CarNotes.Domain.Interfaces.Services;

namespace CarNotes.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        public static double Convert(
            double amount, DateOnly? date, string currency = "USD")
        {
            return amount / 2.815;
        }
    }
}

