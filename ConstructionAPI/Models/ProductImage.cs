using ConstructionAPI.DAL;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructionAPI.Models
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
