using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ihospital.API.Data;
using Ihospital.API.DTOs;
using Ihospital.API.Models;
using Ihospital.API.Services;

namespace Ihospital.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            
            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateStaffDto staffDto)
        {
            // Check if username already exists
            if (await _context.Staff.AnyAsync(s => s.UserName == staffDto.UserName))
                return BadRequest(new { message = "Username already exists" });

            var staff = new Staff
            {
                UserName = staffDto.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(staffDto.Password)
            };

            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();

            var token = _authService.GenerateJwtToken(staff.StaffId, staff.UserName);

            return Ok(new LoginResponseDto
            {
                StaffId = staff.StaffId,
                UserName = staff.UserName,
                Token = token
            });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var staffIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (staffIdClaim == null)
                return Unauthorized();

            var staffId = int.Parse(staffIdClaim.Value);
            var staff = await _context.Staff.FindAsync(staffId);

            if (staff == null)
                return NotFound();

            // Verify old password
            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, staff.Password))
                return BadRequest(new { message = "Invalid old password" });

            // Update password
            staff.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully" });
        }

        [Authorize]
        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            return Ok(new { message = "Token is valid" });
        }
    }
}
