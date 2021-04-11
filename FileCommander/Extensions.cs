using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace FileCommander
{
    public static class Extensions
    {
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

        public static string Fit(this string text, int width, TextAlignment alignment = TextAlignment.Left)
        {

            return text.Substring(0, Math.Min(width, text.Length)).Align(width, alignment);
        }

        public static List<string> WrapParagraph(this string text, int lineWidth, TextAlignment alignment)
        {
            List<string> lines = new List<string>();
            foreach (var paragraph in text.Replace('\r', ' ').Split('\n'))
            {
                lines.AddRange(WrapText(paragraph, lineWidth, alignment));
            }
            return lines;
        }

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
        /// <param name="size"></param>
        /// <param name="acronymCutting">‰ÎËÌ‡ ‡·Â‚Ë‡ÚÛ˚</param>
        /// <returns></returns>
        public static string FormatFileSize(this long size, int decimals = 0, FileSizeAcronim—utting acronymCutting = FileSizeAcronim—utting.TwoChar)
        {
            string mask = "###" + (decimals == 0 ? "": $".{new string('#', decimals)}");
            long KILOBYTE = 1024;
            long MEGABYTE = KILOBYTE * 1024;
            long GIGABYTE = MEGABYTE * 1024;
            long TERABYTE = GIGABYTE * 1024;
            long PETABYTE = TERABYTE * 1024;

            if (size >= PETABYTE)
                return ((double)size / PETABYTE).ToString(mask + (acronymCutting == FileSizeAcronim—utting.SingleChar ? "P": " PB")).PadLeft(8);
            else if (size > TERABYTE && size < PETABYTE)
                return ((double)size / TERABYTE).ToString(mask + (acronymCutting == FileSizeAcronim—utting.SingleChar ? "T" : " TB")).PadLeft(8);
            else if (size > GIGABYTE && size < TERABYTE)
                return ((double)size / GIGABYTE).ToString(mask + (acronymCutting == FileSizeAcronim—utting.SingleChar ? "G" : " GB")).PadLeft(8);
            else if (size > MEGABYTE && size < GIGABYTE)
                return ((double)size / MEGABYTE).ToString(mask + (acronymCutting == FileSizeAcronim—utting.SingleChar ? "M" : " MB")).PadLeft(8);
            else if (size > KILOBYTE && size < MEGABYTE)
                return ((double)size / KILOBYTE).ToString(mask + (acronymCutting == FileSizeAcronim—utting.SingleChar ? "K" : " KB")).PadLeft(8);
            else
                return (size).ToString().PadLeft(8);
        }
    }
}