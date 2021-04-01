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

        public static string[] LineBreak(this string text, int lineLength)
        {
            int lines = text.Length / lineLength + 1;
            string[] result = new string[lines];
            for (int i = 0; i < lines; i++)
                result[i] = text.Substring(i*lineLength, Math.Min(lineLength, text.Length - i * lineLength));
            return result;
        }
    }
}