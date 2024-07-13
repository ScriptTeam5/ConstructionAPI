using ConstructionAPI.Models;

namespace ConstructionAPI.DTO
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsVIP { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int? ShopId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        //il
    }
}
