using ConstructionAPI.DAL;

namespace ConstructionAPI.Models
{
    public class ProductAttribute : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
