namespace LocalizationApp.Models
{
    public class Resource
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Culture Culture { get; set; }
    }
}
