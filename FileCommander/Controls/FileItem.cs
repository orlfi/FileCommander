using System;
using System.IO;
using System.Text;

using System.Collections.Generic;

namespace FileCommander
{
    /// <summary>
    /// File panel list item 
    /// </summary>
    public class FileItem : Control
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets the flag of the selected item
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets a link to the file panel 
        /// </summary>
        public FilePanel FilePanel => (FilePanel)Parent.Parent;

        /// <summary>
        ///  Gets or sets an instance of the FileSystemInfo class 
        /// </summary>
        FileSystemInfo FileSystemInfo { get; set; }

        /// <summary>
        /// Gets or sets the type of the element 
        /// </summary>
        public FileTypes ItemType { get; set; }

        /// <summary>
        /// Gets or sets a list of file pane view columns 
        /// </summary>
        public List<FilePanelColumn> Columns { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">File structure path</param>
        /// <param name="fileSystemInfo">An instance of the FileSystemInfo class</param>
        /// <param name="width">Item Width </param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="itemType">The type of the element</param>
        /// <returns></returns>
        public FileItem(string path, FileSystemInfo fileSystemInfo, string width, Size size, FileTypes itemType = FileTypes.File) : 
            this($"0, 0, {width}, 1", size, path, fileSystemInfo, itemType) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="path">File structure path </param>
        /// <param name="fileSystemInfo">An instance of the FileSystemInfo class</param>
        /// <param name="itemType">The type of the element</param>
        /// <returns></returns>
        public FileItem(string rectangle, Size size, string path, FileSystemInfo fileSystemInfo, FileTypes itemType = FileTypes.File) : base(rectangle, size)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            ItemType = itemType;
            FileSystemInfo = fileSystemInfo;
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            ConsoleColor foreground = GetItemForegroundColor();
            int x = X;
            for (int i = 0; i < Columns.Count; i++)
            {
                int columnWidth = Columns[i].GetWidth(Columns, Width - Columns.Count + 1);
                string text = new string(' ', columnWidth);
                switch (Columns[i].ColumnType)
                {
                    case FileColumnTypes.FileName:
                        text = GetFitName(Name, columnWidth).PadRight(columnWidth);
                        break;
                    case FileColumnTypes.Size:
                        if (ItemType == FileTypes.File)
                        {
                            FileInfo fi = new FileInfo(Path);
                            text = fi.Exists ? fi.Length.FormatFileSize(3, FileSizeAcronimCutting.SingleChar) : "";
                        }
                        break;
                    case FileColumnTypes.DateTime:
                        if (ItemType == FileTypes.File)
                        {
                            text = File.Exists(Path) ? File.GetLastWriteTime(Path).ToString("dd.MM.yy HH:mm").PadLeft(columnWidth) : "";
                        }
                        break;
                }

                buffer.WriteAt(text, x + targetX, Y + targetY, foreground,
                    Focused && FilePanel.Focused ? Theme.FilePanelFocusedBackgroundColor : Theme.FilePanelItemBackgroundColor);

                x += columnWidth;
                if (i < Columns.Count - 1)
                {
                    buffer.WriteAt('│', x + targetX, Y + targetY, Theme.FilePanelForegroundColor, Focused && FilePanel.Focused ? Theme.FilePanelFocusedBackgroundColor : Theme.FilePanelBackgroundColor);
                    x++;
                }

            }
        }

        /// <summary>
        /// Returns the foreground color of an element based on the current state 
        /// </summary>
        /// <returns>ConsoleColor</returns>
        public ConsoleColor GetItemForegroundColor()
        {
            ConsoleColor result = ConsoleColor.Cyan;
            switch (ItemType)
            {
                case FileTypes.ParentDirectory:
                    result = ConsoleColor.Cyan;
                    break;
                case FileTypes.Directory:
                    result = Theme.FilePanelDirectoryForegroundColor;
                    if (Selected)
                        result = Theme.FilePanelSelectedForegroundColor;
                    else if (Focused && FilePanel.Focused)
                        result = Theme.FilePanelFocusedForegroundColor;
                    break;
                case FileTypes.File:
                    result = Theme.FilePanelFileForegroundColor;
                    if (Selected)
                        result = Theme.FilePanelSelectedForegroundColor;
                    else if (Focused && FilePanel.Focused)
                        result = Theme.FilePanelFocusedForegroundColor;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Returns a string containing the filename and formatted to fit the width of the element 
        /// </summary>
        /// <param name="name">File name</param>
        /// <param name="width">The width for which you want to format the string</param>
        /// <returns></returns>
        public static string GetFitName(string name, int width)
        {
            string result = name;
            if (name.Length > width)
            {
                int extensionIndex = name.LastIndexOf('.');
                if (extensionIndex > 0)
                {
                    string extension = name.Substring(extensionIndex);
                    result = $"{name.Substring(0, (width - extension.Length - 1) < 0 ? 0 : (width - extension.Length - 1))}~{extension}";
                }
                else
                    result = $"{name.Substring(0, width - 1)}~";
            }
            return result;
        }
        #endregion
    }
}