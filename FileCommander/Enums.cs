using System;

namespace FileCommander
{
    /// <summary>
    /// Enumerating file types
    /// </summary>
    public enum FileTypes
    {
        ParentDirectory,
        Directory,
        File,
        Empty
    }

    /// <summary>
    /// Enumerating Column Types in File View 
    /// </summary>
    public enum FileColumnTypes
    {
        FileName,
        Size,
        DateTime
    }

    /// <summary>
    /// Enumerates component layout types 
    /// </summary>
    public enum ComponentPosition
    {
        Absolute,
        Relative
    }

    /// <summary>
    /// Enumerates the direction of information output 
    /// </summary>
    public enum Direction
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// Enumerates the possible control alignments
    /// </summary>
    public enum Alignment
    {
        None,
        HorizontalCenter = 0b_0000100,
        VerticalCenter = 0b_0010000
    }

    /// <summary>
    /// Enumerates the direction in which characters are removed in text
    /// </summary>
    public enum TextRemoveDirection
    {
        Next,
        Previous
    }

    /// <summary>
    /// Enumerates the display types of lines 
    /// </summary>
    public enum LineType
    {
        None,
        Single,
        Double
    }

    /// <summary>
    /// Enumerates the result of returning from a modal window 
    /// </summary>
    public enum ModalWindowResult
    {
        None,
        Confirm,
        ConfirmAll,
        Cancel,
        Skip,
        SkipAll
    }

    /// <summary>
    /// Enumerates the possible text alignment options 
    /// </summary>
    public enum TextAlignment
    {
        None,
        Left,
        Right,
        Center,
        Width
    }

    /// <summary>
    /// Enumerates the number of characters of an abbreviation
    /// </summary>
    public enum FileSizeAcronimCutting
    {
        SingleChar,
        TwoChar
    }

}