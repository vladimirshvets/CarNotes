namespace CarNotesAPI.Models
{
    public class Car
	{
		public int Id { get; set; }

		public string UserId { get; set; }

		public string Make { get; set; }

		public string Model { get; set; }

		public string? Generation { get; set; }

		public string? VIN { get; set; }

		public int? Year { get; set; }
	}
}
