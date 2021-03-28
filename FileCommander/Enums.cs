using System;

namespace FileCommander
{
    public enum FileTypes
    {
        ParentDirectory,
        Directory,
        File,
        Empty
    }

    public enum FileColumnTypes
    {
        FileName,
        Size,
        DateTime    
    }

    public enum ComponentPosition
    {
        Absolute,
        Relative
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }

    [Flags]
    public enum WindowButtons
    {
        OK = 0b_0000001,
        Cancel = 0b_0000010
    }

    public enum Alignment
    {
        None,
        HorizontalLeft =    0b_0000001,
        HorizontalRight =   0b_0000010,
        HorizontalCenter =  0b_0000100,
        VerticalLeft =      0b_0001000,
        VerticalRight =     0b_0010000,
        VerticalCenter =    0b_0010000
    }
}