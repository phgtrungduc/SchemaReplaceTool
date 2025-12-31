namespace SqlSchemaReplacer.Models
{
    public class ReplaceSummary
    {
        public int Total { get; set; }
        public int Success { get; set; }
        public int Failed => Total - Success;
    }
}
