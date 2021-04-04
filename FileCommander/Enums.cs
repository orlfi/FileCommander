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
    public enum WindowButton
    {
        Cancel = 0b_0000_0001,
        Copy = 0b_0000_0010,
        Move = 0b_0000_0100,
        Delete = 0b_0000_1000
    }

    public enum Alignment
    {
        None,
        HorizontalLeft = 0b_0000001,
        HorizontalRight = 0b_0000010,
        HorizontalCenter = 0b_0000100,
        VerticalLeft = 0b_0001000,
        VerticalRight = 0b_0010000,
        VerticalCenter = 0b_0010000
    }

    public enum MoveCursorDirection
    {
        Left,
        Right,
        LeftWord,
        RightWord
    }

    public enum TextRemoveDirection
    {
        Next,
        Previous
    }

    public enum LineType
    {
        None,
        Single,
        Double
    }

    public enum ModalWindowResult
    {
        None,
        Ok,
        Confirm,
        ConfirmAll,
        Yes,
        No,
        Break,
        Cancel,
        Skip,
        SkipAll
    }

}