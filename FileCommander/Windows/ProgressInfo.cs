using System;
namespace FileCommander
{
    /// <summary>
    /// Represents information of an operations progress
    /// </summary>
     public class ProgressInfo
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets description of the progress 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets end of operation flag 
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// Gets or sets number of operations performed 
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// Gets or sets  Number of operations required 
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Gets or sets  percentage of operations completed
        /// </summary>
        /// <value>Percentage value rounded to 1 decimal place </value>
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

        /// <summary>
        /// Gets or sets current value of operations performed 
        /// </summary>
        public double Proceded { get; set; }

        /// <summary>
        /// Gets or sets total value of required operations
        /// </summary>
        public double Total { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proceded">Current value of operations performed</param>
        /// <param name="total">Total value of required operations</param>
        /// <param name="description">Description of the progress</param>
        /// <returns></returns>
        public ProgressInfo(double proceded, double total, string description) : this(proceded, total, description, 0, 0, false) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proceded">Current value of operations performed</param>
        /// <param name="total">Total value of required operations</param>
        /// <param name="description">Description of the progress</param>
        /// <param name="count">Number of operations performed</param>
        /// <param name="totalCount">Number of operations required </param>
        /// <returns></returns>
        public ProgressInfo(double proceded, double total, string description, long count, long totalCount) : this(proceded, total, description, count, totalCount, false) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proceded">Current value of operations performed</param>
        /// <param name="total">Total value of required operations</param>
        /// <param name="description">Description of the progress</param>
        /// <param name="count">Number of operations performed</param>
        /// <param name="totalCount">Number of operations required </param>
        /// <param name="done">End of operation flag </param>
        public ProgressInfo(double proceded, double total, string description, long count, long totalCount, bool done)
        {
            Proceded = proceded;
            Total = total;
            Description = description;
            Count = count;
            TotalCount = totalCount;
            Done = done;
        }
        #endregion
    }
}