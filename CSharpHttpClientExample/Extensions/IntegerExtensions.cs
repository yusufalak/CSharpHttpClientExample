namespace Commons.Extensions
{
    public static class IntegerExtensions
    {

        public static int ToInteger(this string val)
        {
            if (string.IsNullOrEmpty(val)) { return 0; }
            try
            {
                return Int32.Parse(val);
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
