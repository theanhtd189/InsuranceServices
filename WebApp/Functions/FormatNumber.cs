namespace WebApp.Functions
{
    public static class FormatNumber
    {
        public static string Split(decimal money)
        {
            return String.Format("{0:0.0}", money);
        }

        public static string Split(string money)
        {
            if (!string.IsNullOrEmpty(money))
            {
                return String.Format("{0:0.0}", decimal.Parse(money));
            }
            else
                return "0";
            
        }
    }
}
