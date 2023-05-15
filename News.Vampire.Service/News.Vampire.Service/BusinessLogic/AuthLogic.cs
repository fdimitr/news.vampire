using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Constants;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace News.Vampire.Service.BusinessLogic
{
    public class AuthLogic : BaseLogic<User>, IAuthLogic
    {
        private readonly IConfiguration _configuration;

        public AuthLogic(IDbContextFactory<DataContext> dbContextFactory, IConfiguration configuration) : base(dbContextFactory)
        {
            _configuration = configuration;
        }

        public async Task<User> Authentificate(string login, string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_configuration.GetValue<string>(ConfigKey.HashSalt));
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            User result = null;
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            result = await (from user in dbContext.Users
                            where user.Login == login && user.Password == hashed
                            select user).FirstOrDefaultAsync();


            return result;
        }
    }
}
