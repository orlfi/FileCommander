using System;
namespace FileCommander
{
    public static class Extensions
    {
        public static string PadCenter(this string text, int width)
        {
            int marginLeft = (width - text.Length) / 2;
            int marginRigth = marginLeft;

            if ((width - text.Length) % 2 != 0)
                marginRigth++;

            return $"{"".PadRight(marginLeft)}{text}{"".PadRight(marginRigth)}";
        }

    }
}