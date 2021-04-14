using System;
using System.IO;

namespace FileCommander
{
    /// <summary>
    /// Represents a container that contains system information 
    /// </summary>
    public class InfoPanel: Panel
    {
        #region Constants
        /// <summary>
        /// Date format template
        /// </summary>
        private const string DATE_FORMAT = "dd.MM.yyyy HH.mm.ss";
        #endregion

        #region Fields && Properties
        /// <summary>
        /// Gets or sets the Label control for displaying computer name
        /// </summary>
        public Label ComputerName { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying current user name
        /// </summary>
        public Label UserName { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying drive name of the focused panel
        /// </summary>
        public Label DriveName { get; set; }
        
        /// <summary>
        /// Gets or sets the Label control for displaying drives size
        /// </summary>
        public Label DriveTotalSize { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying drives free space
        /// </summary>
        public Label DriveSpaceFree { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying drives volume label
        /// </summary>
        public Label DriveVolumeLabel { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying total memory installed
        /// in bytes
        /// </summary>
        public Label MemoryTotal { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying used memory
        /// in bytes
        /// </summary>
        public Label MemoryUsed { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying free memory
        /// in bytes
        /// </summary>
        public Label MemoryFree { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying file name selected in focused panel
        /// </summary>
        public Label FileName { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying size of file selected in focused panel
        /// in bytes
        /// </summary>
        public Label FileSize { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying creation date and time of file selected in focused panel
        /// </summary>
        public Label FileDateCreated { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying modification date and time of file selected in focused panel
        /// </summary>
        public Label FileDateModified { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying last access date date and time of file selected in focused panel
        /// </summary>
        public Label FileLastAccessTime { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying path of focused panel
        /// </summary>
        public Label FilePath { get; set; }
        #endregion

        #region Constructors        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <returns></returns>
        public InfoPanel(string rectangle, Size size) : this(rectangle, size, Alignment.None) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        /// <returns></returns>
        public InfoPanel(string rectangle, Size size, Alignment alignment) : base(rectangle, size, alignment)
        {
            Name = "Information";
            Border = LineType.Single;
            Fill = true;
            ForegroundColor = Theme.FilePanelForegroundColor;
            BackgroundColor = Theme.FilePanelBackgroundColor;

            int y = 1;
            ComputerName = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "ComputerName", "Computer Name");
            ComputerName.TextAlignment = TextAlignment.Right;
            ComputerName.UseParentForegroundColor = false;
            ComputerName.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(ComputerName);

            UserName = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "UserName", "File Destination Path");
            UserName.TextAlignment = TextAlignment.Right;
            UserName.UseParentForegroundColor = false;
            UserName.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(UserName);

            DriveName = new Label($"1, {y++}, 100%-1, 1", Size, Alignment.HorizontalCenter, "DriveName", "Drive Name");
            DriveName.TextAlignment = TextAlignment.Center;
            Add(DriveName);

            DriveTotalSize = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "TotalSize", "TotalSize");
            DriveTotalSize.TextAlignment = TextAlignment.Right;
            DriveTotalSize.UseParentForegroundColor = false;
            DriveTotalSize.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(DriveTotalSize);

            DriveSpaceFree = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "SpaceFree", "SpaceFree");
            DriveSpaceFree.TextAlignment = TextAlignment.Right;
            DriveSpaceFree.UseParentForegroundColor = false;
            DriveSpaceFree.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(DriveSpaceFree);

            DriveVolumeLabel = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "VolumeLabel", "VolumeLabel");
            DriveVolumeLabel.TextAlignment = TextAlignment.Right;
            DriveVolumeLabel.UseParentForegroundColor = false;
            DriveVolumeLabel.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(DriveVolumeLabel);

            var memory = CommandManager.GetWindowsMetrics();
            y++;

            MemoryTotal = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "MemoryTotal", memory.Total.ToString());
            MemoryTotal.TextAlignment = TextAlignment.Right;
            MemoryTotal.UseParentForegroundColor = false;
            MemoryTotal.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(MemoryTotal);

            MemoryUsed = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "MemoryUsed", memory.Used.ToString());
            MemoryUsed.TextAlignment = TextAlignment.Right;
            MemoryUsed.UseParentForegroundColor = false;
            MemoryUsed.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(MemoryUsed);

            MemoryFree = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "Free", memory.Free.ToString());
            MemoryFree.TextAlignment = TextAlignment.Right;
            MemoryFree.UseParentForegroundColor = false;
            MemoryFree.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(MemoryFree);

            FileName = new Label($"2, {y++}, 100%-4, 1", Size, Alignment.HorizontalCenter, "FileName", "FileName");
            FileName.TextAlignment = TextAlignment.Center;
            Add(FileName);

            FileSize = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "FileSize", "FileSize");
            FileSize.TextAlignment = TextAlignment.Right;
            FileSize.UseParentForegroundColor = false;
            FileSize.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(FileSize);

            FileDateCreated = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "FileDateCreated", "FileDateCreated");
            FileDateCreated.TextAlignment = TextAlignment.Right;
            FileDateCreated.UseParentForegroundColor = false;
            FileDateCreated.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(FileDateCreated);

            FileDateModified = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "FileDateModified", "FileDateModified");
            FileDateModified.TextAlignment = TextAlignment.Right;
            FileDateModified.UseParentForegroundColor = false;
            FileDateModified.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(FileDateModified);

            FileLastAccessTime = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "FileLastAccessTime", "FileLastAccessTime");
            FileLastAccessTime.TextAlignment = TextAlignment.Right;
            FileLastAccessTime.UseParentForegroundColor = false;
            FileLastAccessTime.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(FileLastAccessTime);

            FilePath = new Label($"16, {y++}, 100%-17, 1", Size, Alignment.None, "FilePath", "FilePath");
            FilePath.TextAlignment = TextAlignment.Right;
            FilePath.UseParentForegroundColor = false;
            FilePath.ForegroundColor = Theme.FilePanelColumnForegroundColor;
            Add(FilePath);
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Positions and resizes the panel according to the passed area 
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        public void SetRectangle(string rectangle, Size size)
        {
            _rectangleString = rectangle;
            SetRectangle(size);
        }

        /// <summary>
        /// Updates information in child controls when the path changes
        /// </summary>
        /// <param name="path">Path</param>
        public void OnPathChange(string path)
        {
            SetBaseInfoValues();
            SetDriveInfoValues(path);
            SetMemoryInfoValues(); 
            SetFileInfoValues(path);
            Update();
        }

        /// <summary>
        /// Updates information in child controls when focus is changed in the file panel
        /// </summary>
        /// <param name="sender">File panel</param>
        /// <param name="item">Focused file panel item</param>
        public void OnSelectFile(Control sender, FileItem item)
        {
            if (!Disabled && Visible)
            {
                SetBaseInfoValues();
                SetDriveInfoValues(item.Path);
                SetFileInfoValues(item.Path);
            }
            Update();
        }

        /// <summary>
        /// Sets information about username and computer name
        /// </summary>
        private void SetBaseInfoValues()
        {
            UserName.SetText(Environment.UserName);
            ComputerName.SetText(Environment.MachineName);
        }

        /// <summary>
        /// Sets memory information
        /// </summary>
        private void SetMemoryInfoValues()
        {
            var memory = CommandManager.GetWindowsMetrics();
            MemoryTotal.SetText(memory.Total.ToString("#,#"), false);
            MemoryUsed.SetText(memory.Used.ToString("#,#"), false);
            MemoryFree.SetText(memory.Free.ToString("#,#"), false);
        }

        /// <summary>
        /// Sets drive information
        /// </summary>
        private void SetDriveInfoValues(string path)
        {
            DriveInfo di = new DriveInfo(System.IO.Path.GetPathRoot(path));           
            DriveName.SetText($"{ di.DriveType} {di.Name} ({ di.DriveFormat})", false);
            DriveTotalSize.SetText(di.TotalSize.FormatFileSize(0, FileSizeAcronimCutting.TwoChar), false);
            DriveSpaceFree.SetText(di.AvailableFreeSpace.FormatFileSize(0, FileSizeAcronimCutting.TwoChar), false);
            DriveVolumeLabel.SetText(di.VolumeLabel, false);
        }

        /// <summary>
        /// Sets file information
        /// </summary>
        private void SetFileInfoValues(string path)
        {
            FileName.SetText(FileItem.GetFitName(System.IO.Path.GetFileName(path), Width), false);
            string FileSizeText = "";
            string dateCreated = "";
            string dateModified = "";
            string lastAccess = "";
            string directory = "";

            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                dateCreated = di.CreationTime.ToString(DATE_FORMAT);
                dateModified = di.LastWriteTime.ToString(DATE_FORMAT);
                lastAccess = di.LastAccessTime.ToString(DATE_FORMAT);
                directory = di.Parent == null ? di.Root.Name : di.Parent.FullName;
            }

            if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                FileSizeText = fi.Length.ToString("#,#");
                dateCreated = fi.CreationTime.ToString(DATE_FORMAT);
                dateModified = fi.LastWriteTime.ToString(DATE_FORMAT);
                lastAccess = fi.LastAccessTime.ToString(DATE_FORMAT);
                directory = fi.DirectoryName;
            }
            FileSize.SetText(FileSizeText, false);
            FileDateCreated.SetText(dateCreated, false);
            FileDateModified.SetText(dateModified, false);
            FileLastAccessTime.SetText(lastAccess, false);
            FilePath.SetText(directory, false);
        }

        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            int y = 1;
            if (Border != LineType.None || Fill)
            {
                var box = new Box(X, Y, Width, Height, Border, Fill);
                box.foregroundColor = ForegroundColor;
                box.backgroundColor = BackgroundColor;
                box.Draw(buffer, targetX, targetY);
            }

            string centerText = $" {Name} ";
            buffer.WriteAt(centerText, targetX + X + Width / 2 - centerText.Length/2, targetY + Y, ForegroundColor, BackgroundColor);
            buffer.WriteAt("Computer name", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("User name", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);

            var line = new Line(X, Y + (y++), Width, 1, Direction.Horizontal, LineType.Single);
            line.FirstChar = Border == LineType.Single? '├' : '╟';
            line.LastChar = Border == LineType.Single ? '┤' : '╢';
            line.ForegroundColor = ForegroundColor;
            line.BackgroundColor = BackgroundColor;
            line.Draw(buffer, targetX, targetY);
            buffer.WriteAt("Total size", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Space free", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Volume label", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);

            line.Y = Y + y;
            line.Draw(buffer, targetX, targetY);
            centerText = " Memory ";
            buffer.WriteAt(centerText, targetX + X + Width / 2 - centerText.Length / 2, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Total", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Used", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Free", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);

            line.Y = Y + (y++);
            line.Draw(buffer, targetX, targetY);
            buffer.WriteAt("File size", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Date created", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Date modified", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Last access time", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            buffer.WriteAt("Path", targetX + X + 1, targetY + Y + (y++), ForegroundColor, BackgroundColor);
            DrawChildren(buffer, targetX, targetY);
        }
        #endregion
    }
}