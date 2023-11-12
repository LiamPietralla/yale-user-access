namespace YaleAccess.Models.Options
{
    public class ZWaveOptions
    {
        public const string ZWave = "ZWave";

        public string Url { get; set; } = null!;
        public int SchemaVersion { get; set; }
    }
}
