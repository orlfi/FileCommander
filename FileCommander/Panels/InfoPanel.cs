using System;
using System.IO;

namespace FileCommander
{
    public class InfoPanel: Panel
    {
        private const string DATE_FORMAT = "dd.MM.yyyy HH.mm.ss";
        public Label ComputerName { get; set; }

        public Label UserName { get; set; }

        public Label DriveName { get; set; }
        
        public Label DriveTotalSize { get; set; }
        
        public Label DriveSpaceFree { get; set; }

        public Label DriveVolumeLabel { get; set; }
      
        public Label MemoryTotal { get; set; }

        public Label MemoryUsed { get; set; }

        public Label MemoryFree { get; set; }
        
        public Label FileName { get; set; }

        public Label FileSize { get; set; }

        public Label FileDateCreated { get; set; }

        public Label FileDateModified { get; set; }
        
        public Label FileLastAccessTime { get; set; }

        public Label FilePath { get; set; }

        
        public InfoPanel(string rectangle, Size size) : this(rectangle, size, Alignment.None) { }

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

        public void SetRectangle(string rectangle, Size size)
        {
            _rectangleString = rectangle;
            SetRectangle(size);
        }

        public void OnPathChange(string path)
        {
            SetBaseInfoValues();
            SetDriveInfoValues(path);
            SetMemoryInfoValues(); 
            SetFileInfoValues(path);
            Update();
        }

        public void OnSelectFile(Component sender, FileItem item)
        {
            if (!Disabled && Visible)
            {
                SetBaseInfoValues();
                SetDriveInfoValues(item.Path);
                SetFileInfoValues(item.Path);
            }
            Update();
        }

        private void SetBaseInfoValues()
        {
            UserName.SetText(Environment.UserName);
            ComputerName.SetText(Environment.MachineName);
        }
        private void SetMemoryInfoValues()
        {
            var memory = CommandManager.GetWindowsMetrics();
            MemoryTotal.SetText(memory.Total.ToString("#,#"), false);
            MemoryUsed.SetText(memory.Used.ToString("#,#"), false);
            MemoryFree.SetText(memory.Free.ToString("#,#"), false);
        }

        private void SetDriveInfoValues(string path)
        {
            DriveInfo di = new DriveInfo(System.IO.Path.GetPathRoot(path));           
            DriveName.SetText($"{ di.DriveType} {di.Name} ({ di.DriveFormat})", false);
            DriveTotalSize.SetText(di.TotalSize.FormatFileSize(0, FileSizeAcronimСutting.TwoChar), false);
            DriveSpaceFree.SetText(di.AvailableFreeSpace.FormatFileSize(0, FileSizeAcronimСutting.TwoChar), false);
            DriveVolumeLabel.SetText(di.VolumeLabel, false);
        }

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
    }
}