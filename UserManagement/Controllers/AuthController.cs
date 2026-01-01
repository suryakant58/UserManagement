using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserManagement.Api.DTOs;
using UserManagement.Domain.DomainServices;
using UserManagement.Domain.Entities;
using UserManagement.Domain.ValueObject;
using UserManagement.Infrastructure.Interfaces;
using UserManagement.Infrastructure.Repositories;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;

        

        public AuthController(IAuthService authService, IConfiguration configuration, IPasswordHasher passwordHasher)
        {
            _authService = authService;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }
       [HttpGet()]
        public IActionResult Index()
        {
            return Ok("AuthController is running.");
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and password are required.");

            // Create domain value objects
            Email email = Email.Create(request.Email);

            // Create password hasher instance (domain service) and hash the password
            var hashedPassword = _passwordHasher.Hash(request.Password);

            // Create domain entity using hashed password
            var user = new User(email, hashedPassword, request.FullName ?? string.Empty, request.Role ?? "User");

            // Persist to repository (synchronously to avoid requiring application restart for async change)
            _authService.AddAsync(user).GetAwaiter().GetResult();

            // Return response (could also issue JWT here)
            return Ok(new { UserId = user.Id, Message = "User registered successfully" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            // Ensure non-null values for token creation
            string userId = user.Id.ToString() ?? string.Empty;
            var issuer = _configuration["Jwt:Issuer"] ?? string.Empty;
            var audience = _configuration["Jwt:Audience"] ?? string.Empty;
            var jwtKeyBase64 = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(jwtKeyBase64);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Jwt:Key must be a Base64-encoded key with >256 bits.");
            }
            if (keyBytes.Length * 8 <= 256)
                throw new InvalidOperationException("JWT key length must be greater than 256 bits.");

            // Create JWT claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Generate JWT token
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { Token = tokenString, Expires = token.ValidTo });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.NewPassword))
                return BadRequest("Email and new password are required.");

            // Find user by email
            var user = await _authService.GetByEmailAsync(request.Email);
            if (user == null)
                return NotFound("User not found.");

            // (Optional) Validate reset token if you implement token-based reset
            // if (!await _tokenService.ValidateResetTokenAsync(user, request.ResetToken))
            //     return Unauthorized("Invalid or expired reset token.");

            // Update password
            var newPasswordHash = _passwordHasher.Hash(request.NewPassword);
            user.ChangePassword(newPasswordHash);

            await _authService.UpdateAsync(user);

            return Ok(new { Message = "Password reset successfully." });
        }
    }
}       


