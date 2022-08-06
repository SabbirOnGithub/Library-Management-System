using App.Web.Models;
using App.Web.Models.Data;
using App.Web.Models.Entities;
using App.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Web.Providers.Repositories
{
    public interface IUserRepository
    {
        UserItem Register(RegisterViewModel model);
        UserItem Validate(LoginViewModel model);

    }
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public UserItem Register(RegisterViewModel model)
        {
            var salt = Hasher.GenerateSalt();
            var hashedPassword = Hasher.GenerateHash(model.Password, salt);

            var user = new Users
            {
                Id = Guid.NewGuid(),
                EmailAddress = model.EmailAddress,
                PasswordHash = hashedPassword,
                Salt = salt,
                Name = model.Name,
                CreatedUtc = DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return new UserItem
            {
                UserId = user.Id,
                EmailAddress = user.EmailAddress,
                Name = user.Name,
                CreatedUtc = user.CreatedUtc
            };
        }

        public UserItem Validate(LoginViewModel model)
        {
            var emailRecords = _dbContext.Users.Where(x => x.EmailAddress == model.EmailAddress);

            var results = emailRecords.AsEnumerable()
            .Where(m => m.PasswordHash == Hasher.GenerateHash(model.Password, m.Salt))
            .Select(m => new UserItem
            {
                UserId = m.Id,
                EmailAddress = m.EmailAddress,
                Name = m.Name,
                CreatedUtc = m.CreatedUtc
            });

            return results.FirstOrDefault();
        }
    }
}
