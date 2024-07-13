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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public ProductsController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDTO dto, [FromHeader(Name = "Authorization")] string token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _tokenService.ValidateTokenAndGetUserAsync(token);

            if (user == null) return NotFound("User not found.");

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
                        UserId=user.Id,
                        CreatedAt = DateTime.UtcNow,
                        Attributes = dto.Attributes,
                        Images = new List<ProductImage>()
                    };

                    if (dto.ImageFiles != null && dto.ImageFiles.Any())
                    {
                        foreach (var file in dto.ImageFiles)
                        {
                            if (file.Length > 0)
                            {
                                var filePath = Path.Combine("wwwroot/images/products", file.FileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                product.Images.Add(new ProductImage { ImageUrl = $"/images/products/{file.FileName}" });
                            }
                        }
                    }

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
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
