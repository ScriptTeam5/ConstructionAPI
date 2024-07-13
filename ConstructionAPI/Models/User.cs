using Microsoft.AspNetCore.Identity;

namespace ConstructionAPI.Models
{
    public class User:IdentityUser<int>
    {
        public string PhoneNumber {  get; set; }

        public int? ShopId { get; set; }
        public Shop Shop { get; set; }

        public List<Product> Products { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<Like> Likes { get; set; }

    }
}
