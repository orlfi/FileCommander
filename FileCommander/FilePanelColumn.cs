using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    public class FilePanelColumn:Component
    {
        public FileColumnTypes ColumnType {get; set;}

        //public string Name  {get; set;}
        public int Flex  {get; set;}

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            throw new System.NotImplementedException();
        }

        //public int Width { get; set;}

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
        public static Dictionary<FilePanelColumn, int> GetFlexColumsWidth(List<FilePanelColumn> columns, int panelWidth)
        {
            Dictionary<FilePanelColumn, int> result = new Dictionary<FilePanelColumn, int>();
            int flexSum = columns.Sum(item => item.Flex);
            int staticWidth = GetStaticColumnWidth(columns);
            foreach(var flexColumn in columns.Where(item => item.Flex > 0))
            {
                result.Add(flexColumn, (panelWidth - staticWidth)*flexColumn.Flex/flexSum);
            }
            int remainder = panelWidth - staticWidth - result.Sum(item => item.Value);
            result[result.First().Key] += remainder;
            return result;
        }
        public FilePanelColumn(FileColumnTypes columnType, string name)
        {
            ColumnType = columnType;
            Name = name;
        }
        public static int GetStaticColumnWidth(List<FilePanelColumn> columns)
        {
            return columns.Where(item => item.Width != -1).Sum(item => item.Width);
        }


    }
}