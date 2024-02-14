namespace FlooringCalculator
{
    public static class Utility
    {
        public static string CreateLogMessage(IList<int> item)
        {
            string loggingMessage = string.Empty;
            for (int y = 0; y < item.Count; y++)
            {
                loggingMessage = (y == item.Count - 1) ? loggingMessage += item[y] : loggingMessage += item[y] + ",";
            }
            return loggingMessage;
        }
        public static string CreateLogMessage(ICollection<IList<int>> items)
        {
            string loggingMessage = string.Empty;
            foreach (var item in items)
            {
                for (int y = 0; y < item.Count; y++)
                {
                    loggingMessage = (y == item.Count - 1) ? loggingMessage += item[y] : loggingMessage += item[y] + ",";
                }
                loggingMessage += " | ";
            }
            return loggingMessage;
        }
    }
}
