using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using KaraokeAPI.Data;
using KaraokeAPI.DTOs;
using KaraokeAPI.Models;

namespace KaraokeAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ApiResponseDto<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if username exists
                if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                {
                    return new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = "Tên đăng nhập đã tồn tại",
                        Errors = { "Username already exists" }
                    };
                }

                // Check if email exists
                if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                {
                    return new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = "Email đã được sử dụng",
                        Errors = { "Email already exists" }
                    };
                }

                // Create new user
                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    FullName = registerDto.FullName,
                    PhoneNumber = registerDto.PhoneNumber,
                    Role = "User",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Generate JWT token
                var token = GenerateJwtToken(user);
                var expiresAt = DateTime.Now.AddHours(24);

                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role,
                    ExpiresAt = expiresAt
                };

                return new ApiResponseDto<AuthResponseDto>
                {
                    Success = true,
                    Message = "Đăng ký thành công",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<AuthResponseDto>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi đăng ký",
                    Errors = { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = "Tên đăng nhập hoặc mật khẩu không đúng",
                        Errors = { "Invalid credentials" }
                    };
                }

                // Update last login
                user.LastLoginAt = DateTime.Now;
                await _context.SaveChangesAsync();

                // Generate JWT token
                var token = GenerateJwtToken(user);
                var expiresAt = DateTime.Now.AddHours(24);

                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role,
                    ExpiresAt = expiresAt
                };

                return new ApiResponseDto<AuthResponseDto>
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<AuthResponseDto>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi đăng nhập",
                    Errors = { ex.Message }
                };
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "YourVeryLongSecretKeyHere123456789");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"] ?? "KaraokeAPI",
                Audience = jwtSettings["Audience"] ?? "KaraokeAPI"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}