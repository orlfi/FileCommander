using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    /// <summary>
    /// Contains a description of the file pane view columns 
    /// </summary>
    public class FilePanelColumn
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets the column type 
        /// </summary>
        public FileColumnTypes ColumnType {get; set;}

        /// <summary>
        /// Gets or sets flex of the column
        /// Each child item with a flex property will be flexed horizontally in view 
        /// according to each item's relative flex value compared to the sum of all items with a flex value specified. 
        /// Any child items that have a flex = 0 will not be 'flexed' (the initial width will not be changed)
        /// </summary>
        public int Flex  {get; set;}
        
        /// <summary>
        /// Gets or sets column name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the initial width of the column
        /// </summary>
        public int Width { get; set;}
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="columnType">Column type /param>
        /// <param name="name">Column name</param>
        public FilePanelColumn(FileColumnTypes columnType, string name)
        {
            ColumnType = columnType;
            Name = name;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns the column width calculated from the flex property.
        /// If flex = 0, then it returns the initial width 
        /// </summary>
        /// <param name="columns">Column list</param>
        /// <param name="panelWidth">Parent control width </param>
        /// <returns></returns>
        public int GetWidth(List<FilePanelColumn> columns, int panelWidth)
        {
            if (Flex > 0)
            {
                var widthDic = GetFlexColumsWidth(columns, panelWidth);
                return widthDic[this];
            }
            else
            {
                return Width;
            }
        }

        /// <summary>
        /// Returns the width of columns with a flex property of 0 
        /// </summary>
        /// <param name="columns">Column list</param>
        /// <returns></returns>
        public static int GetStaticColumnWidth(List<FilePanelColumn> columns)
        {
            return columns.Where(item => item.Flex == 0).Sum(item => item.Width);
        }

        /// <summary>
        /// Returns a dictionary containing the column widths based on the flex property 
        /// </summary>
        /// <param name="columns">Column list</param>
        /// <param name="panelWidth">Parent control width </param>
        /// <returns></returns>
        public static Dictionary<FilePanelColumn, int> GetFlexColumsWidth(List<FilePanelColumn> columns, int panelWidth)
        {
            Dictionary<FilePanelColumn, int> result = new Dictionary<FilePanelColumn, int>();
            int flexSum = columns.Sum(item => item.Flex);
            int staticWidth = GetStaticColumnWidth(columns);
            foreach (var flexColumn in columns.Where(item => item.Flex > 0))
            {
                result.Add(flexColumn, (panelWidth - staticWidth) * flexColumn.Flex / flexSum);
            }
            int remainder = panelWidth - staticWidth - result.Sum(item => item.Value);
            result[result.First().Key] += remainder;
            return result;
        }
        #endregion        
    }
}