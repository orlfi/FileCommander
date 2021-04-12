namespace FileCommander
{   
    /// <summary>
    /// Represents information about RAM 
    /// </summary>
    public class MemoryMetrics
    {
        /// <summary>
        /// Gets or sets total memory 
        /// </summary>
        public long Total { get; set;}
        
        /// <summary>
        /// Gets or sets free memory
        /// </summary>
        /// <value></value>
        public long Free { get; set;}

        /// <summary>
        /// Gets used memory
        /// </summary>
        /// <value>Difference between total and free memory</value>
        public long Used { get => Total - Free;}
        
    }
}