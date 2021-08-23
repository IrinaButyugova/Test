namespace WebApplication2.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Company Company { get; set; }
        public int Price { get; set; }
    }
}
