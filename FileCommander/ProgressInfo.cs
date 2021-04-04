using System;
namespace FileCommander
{
    public class ProgressInfo
    {
        public string Description { get; set; }

        public bool Done { get; set; }

        public long Count { get; set; }

        public long TotalCount { get; set; }

        public double Procent
        {
            get
            {
                if (Total == 0)
                    return 100;
                else
                    return Math.Round(Proceded / Total * 100, 1, MidpointRounding.AwayFromZero);
            }
        }

        public double Proceded { get; set; }

        public double Total { get; set; }

        public ProgressInfo(double proceded, double total, string description) : this(proceded, total, description, 0, 0, false) { }

        public ProgressInfo(double proceded, double total, string description, long count, long totalCount) : this(proceded, total, description, count, totalCount, false) { }

        public ProgressInfo(double proceded, double total, string description, long count, long totalCount, bool done)
        {
            Proceded = proceded;
            Total = total;
            Description = description;
            Count = count;
            TotalCount = totalCount;
            Done = done;
        }
    }
}