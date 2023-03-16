﻿namespace CarNotesAPI.ViewModels
{
    public class WashingViewModel : MileageViewModel
    {
        public string? Title { get; set; }

        public string? Address { get; set; }

        public bool? IsContact { get; set; }

        public bool? IsDegreaserUsed { get; set; }

        public bool? IsPolishUsed { get; set; }

        public bool? IsAntiRainUsed { get; set; }

        public double TotalAmount { get; set; }
    }
}
