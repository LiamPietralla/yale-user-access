namespace YaleAccess.Models.Options
{
    public class CodesOptions
    {
        public const string Codes = "Codes";

        public int Home { get; set; }
        public int GuestCodeRangeStart { get; set; }
        public int GuestCodeRangeCount { get; set; }
    }
}
