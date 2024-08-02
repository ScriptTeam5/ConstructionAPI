using Microsoft.AspNetCore.Mvc;
using ConstructionAPI.Models;
using ConstructionAPI.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ConstructionAPI.DTO;
using ConstructionAPI.Services;

namespace ConstructionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly FirebaseStorageService _firebaseStorageService;
        public ProductController(AppDbContext context, TokenService tokenService, FirebaseStorageService firebaseStorageService)
        {
            _context = context;
            _tokenService = tokenService;
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateProductDTO dto)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);

            //var user = await _tokenService.ValidateTokenAndGetUserAsync(token);

            //if (user == null) return NotFound("User not found.");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = new Product
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        Price = dto.Price,
                        CategoryId = dto.CategoryId,
                        ShopId = dto.ShopId,
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow,
                        Images = new List<ProductImage>()
                    };

                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();

                    foreach (var a in dto.Attributes)
                    {
                        var attribute = new ProductAttribute
                        {
                            Name = a.Name,
                            Value = a.Value,
                            ProductId = product.Id,
                            IsActive = true
                        };
                        await _context.ProductAttributes.AddAsync(attribute);
                    }

                    foreach (var file in dto.ImageFiles)
                    {
                        var fileUrl = await _firebaseStorageService.UploadFileAsync(file);
                        await _context.ProductImages.AddAsync(new ProductImage { ProductId = product.Id, ImageUrl = fileUrl, IsActive = true });
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Ok("Created.");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
