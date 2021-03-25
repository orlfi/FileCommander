using System;
namespace FileCommander
{
    public class ProgressInfo
    {
        public string Description { get; set; }
        public double Progress
        {
            get
            {
                return Math.Round(Proceded / Total * 100, 1, MidpointRounding.AwayFromZero);
            }
        }
        public double Proceded { get; set; }
        public double Total { get; set; }

        public ProgressInfo(double proceded, double total, string description)
        {
            Proceded = proceded;
            Total = total;
            Description = description;
        }
    }
}