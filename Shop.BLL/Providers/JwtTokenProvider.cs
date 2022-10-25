using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop.BLL.Providers.Interfaces;
using Shop.DAL.Data.EF;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace Shop.BLL.Providers
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly static string SECRET_KEY = "18c9ce8f3466f0d84ab657133530cc1431e044223a3cf7cedac653d7f25b6c3e";
        public readonly static SymmetricSecurityKey SIGNIN_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public JwtTokenProvider(ApplicationDBContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        public async Task<string> GenerateTokenAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User id can't be null");

            Guid idUser = Guid.Empty;
            if (Guid.TryParse(userId, out idUser) == false)
            {
                throw new Exception("User id is not valid");
            }
            var currentUser = await _context.Users.Where(x => x.Id == idUser).FirstOrDefaultAsync();

            if (currentUser == null)
                throw new Exception("Not found current user");

            var credentials = new SigningCredentials(SIGNIN_KEY, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            DateTime Expiry = DateTime.UtcNow.AddMinutes(1);
            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

            var payload = new JwtPayload
            {
                { "name", currentUser.UserName },
                { "email", currentUser.Email },
                { "userid", currentUser.Id },
                { "exp", ts },
                { "iss","https://localhost:7102" },
                { "aud","https://localhost:7117"}
            };
            var jwtsecToken = new JwtSecurityToken(header, payload);

            var jwtSecTokenHandler = new JwtSecurityTokenHandler();

            var token = jwtSecTokenHandler.WriteToken(jwtsecToken);

            return token;
        }
    }
}
