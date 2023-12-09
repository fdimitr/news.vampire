using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Constants;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;
using System.Text;

namespace News.Vampire.Service.BusinessLogic
{
    public class AuthLogic : BaseLogic<Reader>, IAuthLogic
    {
        private readonly IConfiguration _configuration;

        public AuthLogic(DataContext dbContext, DbContextOptions<DataContext> dbContextOptions, IConfiguration configuration) : base(dbContext, dbContextOptions)
        {
            _configuration = configuration;
        }

        public async Task<Reader?> Authenticate(string login, string password)
        {
            await using var dbContextTransactional = new DataContext(DbContextOptions);

            string? saltSource = _configuration.GetValue<string>(ConfigKey.HashSalt);
            if (saltSource is null) return null;

            byte[] salt = Encoding.ASCII.GetBytes(saltSource);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            Reader? result = await (from user in dbContextTransactional.Readers
                                where user.Login == login && user.Password == hashed
                                select user).FirstOrDefaultAsync();

            return result;
        }
    }
}
