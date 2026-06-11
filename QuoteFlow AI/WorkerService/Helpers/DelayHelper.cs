namespace WorkerService.Helpers
{
    public static class DelayHelper
    {
        public static DateTime CalculateScheduledDate(DateTime quoteDate, int followUpNumber)
        {
            int days = followUpNumber switch
            {
                1 => 3,
                2 => 7,
                3 => 14,
                4 => 21,
                _ => 3
            };
            return quoteDate.AddDays(days);
        }
    }
}