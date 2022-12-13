using AuthenticationProject.Data;
using AuthenticationProject.Services.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationProject.Services.UserService
{
    public class UserService : IUserService
    {      
        private readonly AuthContext _context;
        private readonly IConfiguration _configuration;
        public UserService(IConfiguration configuration, AuthContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ResponseData<string>> Login(UserLogin request)
        {
            var user = await _context.User.Where(u => u.Email == request.Email).FirstOrDefaultAsync();

            if (user is null) {
                return new ResponseData<string> { 
                    Success = false,
                    Error = ResponseError.UserNotFound
                };
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new ResponseData<string>
                {
                    Success = false,
                    Error = ResponseError.IncorrectPassword
                };
               
            }
            var token = CreateToken(request);

            var userMetadata = await _context.UserMetadata.Where(um => um.UserId == user.Uid).FirstOrDefaultAsync();

            userMetadata.LastTimeSignIn = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ResponseData<string>
            {
                Success = true,
                Value = token
            };
        }

        public async Task<ResponseData<User>> Register(UserRequest request)
        {
            var emailRepeteaded = await _context.User.Where(u => u.Email == request.Email).FirstOrDefaultAsync();

            if (emailRepeteaded is not null)
            {
                return new ResponseData<User>
                {
                    Success = false,
                    Error = ResponseError.EmailAlreadyUsed
                };
            }
                        
            if (request.Password != request.ConfirmPassword)
            {
                return new ResponseData<User>
                {
                    Success = false,
                    Error = ResponseError.PasswordsAreDifferent
                };
                
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = request.Email,
            };

            _context.User.Add(user);

            var metadata = new UserMetadata
            {
                SignUpDate = DateTime.Now,
                IsLogged = true,
                LastTimeSignIn = DateTime.Now,
                HoursSpentInTheApp = 0,
                UserId = user.Uid
            };

            _context.UserMetadata.Add(metadata);

            await _context.SaveChangesAsync();
            return new ResponseData<User>
            {
                Success = true,
                Value = user
            };
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(UserLogin user)
        {
            List<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
            // Clave en bytes de nuestro token escrito en AppSettins:Token
            var symmetricKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)
            );
            // Clave anterior transformada utilizando un algoritmo de encriptacion
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature);
            // Token creado con las claims, fecha de expiracion y las credenciales
            var token = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            // Token anterior transformado en una sola cadena de texto
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
