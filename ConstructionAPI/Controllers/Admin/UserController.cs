using ConstructionAPI.Models;
using ConstructionAPI.DTO;
using ConstructionAPI.DAL;
using ConstructionAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConstructionAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;


        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager, AppDbContext context, TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<List<IdentityUser>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            //var user = await _tokenService.ValidateTokenAndGetUserAsync(token);

            //if (user == null) return Unauthorized(new { Message = "Invalid token" });

            //if (!await _userManager.IsInRoleAsync(user, "admin")) return Forbid();

            var removeUser = await _userManager.Users
                .Include(u => u.Products)
                .Include(u => u.Shop)
                .Include(u => u.Likes)
                .Include(u => u.Favorites)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (removeUser == null) return NotFound(new { Message = "User Not Found" });

            //_context.Products.RemoveRange(removeUser.Products);
            //_context.Shops.RemoveRange(removeUser.Shops);
            //_context.Likes.RemoveRange(removeUser.Likes);
            //_context.SavedBlogs.RemoveRange(removeUser.SavedBlogs);

            var result = await _userManager.DeleteAsync(removeUser);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "User deleted successfully" });
        }

        //[HttpPost("{userId}")]
        //public async Task<IActionResult> AssignRole([FromRoute] int userId, [FromBody] List<AssignRoleDTO> list, [FromHeader(Name = "Authorization")] string token)
        //{
        //    var user = await _tokenService.ValidateTokenAndGetUserAsync(token);

        //    if (user == null) return Unauthorized(new { Message = "Invalid token" });

        //    if (!await _userManager.IsInRoleAsync(user, "admin")) return Forbid();

        //    var targetUser = await _userManager.FindByIdAsync(userId.ToString());
        //    if (targetUser == null) return NotFound(new { Message = "User Not Found" });

        //    foreach (var item in list)
        //    {
        //        IdentityResult result;
        //        if (item.Status)
        //        {
        //            result = await _userManager.AddToRoleAsync(targetUser, item.Name);
        //        }
        //        else
        //        {
        //            result = await _userManager.RemoveFromRoleAsync(targetUser, item.Name);
        //        }

        //        if (!result.Succeeded)
        //        {
        //            return BadRequest(result.Errors);
        //        }
        //    }

        //    return Ok(new { Message = "Roles updated successfully" });
        //}

        #region Role

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost("role/create")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            var appRole = new Role
            {
                Name = role
            };

            var result = await _roleManager.CreateAsync(appRole);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetRoles), null); // HTTP 201 Created
            }
            else
            {
                return BadRequest(result.Errors); // HTTP 400 Bad Request
            }
        }

        [HttpPut("role/edit/{id}")]
        public async Task<IActionResult> EditRole([FromRoute] int id, [FromBody] string roleName)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound($"Role with ID {id} not found");
            }

            role.Name = roleName;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        [HttpDelete("role/delete/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok("Role removed.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        #endregion
    }

}
