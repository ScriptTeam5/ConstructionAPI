using ConstructionAPI.DAL;

namespace ConstructionAPI.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsVIP { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; } 
        public int? ShopId { get; set; }
        public Shop Shop { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<ProductAttribute> Attributes { get; set; }
        public List<Like> Likes { get; set; }
        public List<Favorite> Favorites { get; set; }
        public int LikeCount => Likes?.Count ?? 0;
        public int FavoriteCount => Favorites?.Count ?? 0;


    }
}
