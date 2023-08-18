using System.ComponentModel.DataAnnotations;

namespace DomainLibrary
{
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class Entity
    {
        [Key]
        public int Id { get; set; }

    }
}
