namespace ElectronicJournalAPI.Utilities
{
    public static class WordFormulator
    {
        public static string GetForm(int count, string[] forms)
        {
            if (count % 100 >= 11 && count % 100 <= 19)
                return forms[0];

            switch (count % 10)
            {
                case 1:
                    return forms[1];
                case 2:
                case 3:
                case 4:
                    return forms[2];
                default:
                    return forms[0];
            }
        }
    }
}
