namespace WpfAppWithRedisCache.Models
{
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class Entity
    {
        public int Id { get; set; }

    }
}
