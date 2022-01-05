namespace Client.Methods
{
    public class Check
    {
        public static bool IsFieldWithData(string first, string second)
        {
            return !string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second);
        }
    }
}