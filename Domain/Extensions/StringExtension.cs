namespace Domain.Extensions
{
    public static class StringExtension
    {
        public static bool IsAllLettersOrDigits(this string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }
    }
}
