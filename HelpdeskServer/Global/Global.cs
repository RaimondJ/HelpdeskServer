namespace HelpdeskServer.Global
{
    public class Global
    {
        public static string timespanToString(TimeSpan timeSpan)
        {
            string val = string.Empty;
            val += timeSpan.Days > 0 ? (timeSpan.Days + (timeSpan.Days > 1 ? " päeva, " : " päev, ")) : "";
            val += timeSpan.Hours > 0 ? (timeSpan.Hours + (timeSpan.Hours > 1 ? " tundi, " : " tund, ")) : "";
            val += timeSpan.Minutes > 0 ? (timeSpan.Minutes + (timeSpan.Minutes > 1 ? " minutit, " : " minut, ")) : "";
            if (val.Length > 2)
                val = val.Substring(0, val.Length - 2);
            if (val.Equals(""))
                val = "vähem kui 1 minut";
            return val;
        }
    }
}
