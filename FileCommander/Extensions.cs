using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace FileCommander
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns a new string that center-aligns the characters in this instance 
        /// by padding them with spaces on the left, for a specified total length.
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <param name="width">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters</param>
        /// <returns>A new string that is equivalent to this instance, but center-aligned and padded on the left and Right with as many spaces as needed to create a length of totalWidth</returns>
        public static string PadCenter(this string text, int width)
        {
            int marginLeft = (width - text.Length) / 2;

            if (marginLeft < 0)
                marginLeft = 0;

            int marginRigth = marginLeft;

            if ((width - text.Length) % 2 != 0)
                marginRigth++;

            return $"{"".PadRight(marginLeft)}{text}{"".PadRight(marginRigth)}";
        }

        /// <summary>
        /// Returns a new string that aligns the characters in this instance 
        /// by padding them with spaces on the left, for a specified total length.
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <param name="width">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters</param>
        /// <param name="alignment">Align type</param>
        /// <returns>A new string that is equivalent to this instance, but aligned</returns>

        public static string Align(this string text, int width, TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Left:
                    text = text.PadRight(width);
                    break;
                case TextAlignment.Right:
                    text = text.PadLeft(width);
                    break;
                case TextAlignment.Center:
                    text = text.PadCenter(width);
                    break;
                case TextAlignment.Width:
                    text = text.PadWidth(width);
                    break;
            }
            return text;
        }

        /// <summary>
        /// Returns a new string that is aligned in width by adding missing spaces
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <param name="width">The number of characters in the resulting string, equal to the number of original characters plus missing spaces</param>
        /// <returns>A new string that is equivalent to this instance, but aligned in width by adding missing spaces</returns>
        public static string PadWidth(this string text, int width)
        {
            
            string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int wordsLength = words.Sum(item => item.Length);
            int spaceCount = width - wordsLength - words.Length + 1;
            int spaceGroupCount = ((words.Length - 1) == 0)? 0 : spaceCount /(words.Length-1) + 1;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(' ');
                    if (spaceCount > 0)
                    {
                        if (i == words.Length - 1)
                            sb.Append(new string(' ', spaceCount));
                        else
                        {
                            sb.Append(new string(' ', spaceGroupCount));
                            spaceCount -= spaceGroupCount;
                            spaceGroupCount = ((words.Length - 1) == 0) ? 0 : spaceCount / (words.Length - i) + 1;
                        }
                    }
                }
                sb.Append(words[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a new string which is cropped to the width 
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <param name="width">The number of characters in the resulting string</param>
        /// <param name="alignment">Align type</param>
        /// <returns>A new string that is equivalent to this instance, but cropped to the width</returns>
        public static string Fit(this string text, int width, TextAlignment alignment = TextAlignment.Left)
        {

            return text.Substring(0, Math.Min(width, text.Length)).Align(width, alignment);
        }

        /// <summary>
        /// Returns a list of rows with word wrapping by paragraph
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <param name="lineWidth">Text instance</param>
        /// <param name="alignment">Align type</param>
        /// <returns>A list of wraped rows</returns>
        public static List<string> WrapParagraph(this string text, int lineWidth, TextAlignment alignment)
        {
            List<string> lines = new List<string>();
            foreach (var paragraph in text.Replace('\r', ' ').Split('\n'))
            {
                lines.AddRange(WrapText(paragraph, lineWidth, alignment));
            }
            return lines;
        }

        /// <summary>
        /// Returns a list of rows
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <returns>A list of rows</returns>
        public static List<string> Multiline(this string text)
        {
            return text.Replace('\r', ' ').Split('\n').ToList();
        }

        /// <summary>
        /// Returns a list of lines with word wrapping
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <param name="lineWidth">Text instance</param>
        /// <param name="alignment">Align type</param>
        /// <returns>A list of wraped lines</returns>
        public static List<string> WrapText(this string text, int lineWidth, TextAlignment alignment)
        {
            string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            List<string> lines = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach(var word in words)
            {
                if ((sb.Length + word.Length) > lineWidth-1)
                {
   
                    lines.Add(sb.ToString().Align(lineWidth,alignment));
                    sb.Clear();
                }

                if (sb.Length > 0)
                    sb.Append(' ');

                sb.Append(word);
            }
            lines.Add(sb.ToString().Align(lineWidth, alignment == TextAlignment.Width? TextAlignment.Left: alignment));
            return lines;
        }

        /// <summary>
        /// Format file size 
        /// </summary>
        /// <param name="size">Size instance in bytes</param>
        /// <param name="decimals">A number of simbols after comma</param>
        /// <param name="acronymCutting">Number of characters in the abbreviation </param>
        /// <returns>A new string</returns>
        public static string FormatFileSize(this long size, int decimals = 0, FileSizeAcronimCutting acronymCutting = FileSizeAcronimCutting.TwoChar)
        {
            string mask = "###" + (decimals == 0 ? "": $".{new string('#', decimals)}");
            long KILOBYTE = 1024;
            long MEGABYTE = KILOBYTE * 1024;
            long GIGABYTE = MEGABYTE * 1024;
            long TERABYTE = GIGABYTE * 1024;
            long PETABYTE = TERABYTE * 1024;

            if (size >= PETABYTE)
                return ((double)size / PETABYTE).ToString(mask + (acronymCutting == FileSizeAcronimCutting.SingleChar ? "P": " PB")).PadLeft(8);
            else if (size > TERABYTE && size < PETABYTE)
                return ((double)size / TERABYTE).ToString(mask + (acronymCutting == FileSizeAcronimCutting.SingleChar ? "T" : " TB")).PadLeft(8);
            else if (size > GIGABYTE && size < TERABYTE)
                return ((double)size / GIGABYTE).ToString(mask + (acronymCutting == FileSizeAcronimCutting.SingleChar ? "G" : " GB")).PadLeft(8);
            else if (size > MEGABYTE && size < GIGABYTE)
                return ((double)size / MEGABYTE).ToString(mask + (acronymCutting == FileSizeAcronimCutting.SingleChar ? "M" : " MB")).PadLeft(8);
            else if (size > KILOBYTE && size < MEGABYTE)
                return ((double)size / KILOBYTE).ToString(mask + (acronymCutting == FileSizeAcronimCutting.SingleChar ? "K" : " KB")).PadLeft(8);
            else
                return (size).ToString().PadLeft(8);
        }
    }
}