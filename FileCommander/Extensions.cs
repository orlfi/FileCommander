using System;
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

        public static string Fit(this string text, int width)
        {
            return text.Substring(0, Math.Min(width, text.Length)).PadRight(width);
        }

        public static string[] LineBreak(this string text, int lineLength)
        {
            int lines = text.Length / lineLength + 1;
            string[] result = new string[lines];
            for (int i = 0; i < lines; i++)
                result[i] = text.Substring(i*lineLength, Math.Min(lineLength, text.Length - i * lineLength));
            return result;
        }


        public static string FormatFileSize(this long size)
        {
            long KILOBYTE = 1024;
            long MEGABYTE = KILOBYTE * 1024;
            long GIGABYTE = MEGABYTE * 1024;
            long TERABYTE = GIGABYTE * 1024;
            long PETABYTE = TERABYTE * 1024;

            if (size >= PETABYTE)
                return ((double)size / PETABYTE).ToString("###.###P").PadLeft(8);
            else if (size > TERABYTE && size < PETABYTE)
                return ((double)size / TERABYTE).ToString("###.###T").PadLeft(8);
            else if (size > GIGABYTE && size < TERABYTE)
                return ((double)size / GIGABYTE).ToString("###.###G").PadLeft(8);
            else if (size > MEGABYTE && size < GIGABYTE)
                return ((double)size / MEGABYTE).ToString("###.###M").PadLeft(8);
            else if (size > KILOBYTE && size < MEGABYTE)
                return ((double)size / KILOBYTE).ToString("###.###K").PadLeft(8);
            else
                return (size).ToString().PadLeft(8);
        }
    }
}