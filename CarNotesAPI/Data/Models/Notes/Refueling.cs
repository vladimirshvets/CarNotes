﻿using CarNotesAPI.Data.Api;
using CarNotesAPI.Services;

namespace CarNotesAPI.Data.Models.Notes
{
    public class Refueling : Note
    {
        public override string NoteType => nameof(Refueling);

        public override string NoteTitle => $"Refueling: {Volume} l. * {Price}";

        /// <summary>
        /// Amount of fuel.
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Price per unit, in national currency.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Total amount, in national currency.
        /// </summary>
        public double TotalAmount => Price * Volume;

        /// <summary>
        /// Total amount, in base currency.
        /// </summary>
        public double BaseTotalAmount =>
            CurrencyService.Convert(TotalAmount, Mileage?.Date, "USD");

        /// <summary>
        /// Trading network name.
        /// </summary>
        public string? Distributor { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Initializes a new instance of Refueling.
        /// </summary>
        public Refueling()
        {
        }

        /// <summary>
        /// Initializes a new instance of Refueling
        /// based on properties represented as a dictionary.
        /// </summary>
        /// <param name="node">Set of property names and their values</param>
        public Refueling(Dictionary<string, object> node)
        {
            Id = new Guid((string)node["id"]);
            Volume = (double)node["volume"];
            Price = (double)node["price"];
            Distributor = node.TryGetValue("distributor", out object? distributor) ? (string)distributor : null;
            Address = node.TryGetValue("address", out object? address) ? (string)address : null;
            Comment = node.TryGetValue("comment", out object? comment) ? (string)comment : null;
        }
    }
}
