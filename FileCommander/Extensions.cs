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
            return text.Substring(0, Math.Min(width, text.Length));
        }

    }
}