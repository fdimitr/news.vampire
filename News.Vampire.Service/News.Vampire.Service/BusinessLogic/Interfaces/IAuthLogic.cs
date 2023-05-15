using News.Vampire.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface IAuthLogic
    {
        public Task<User> Authentificate(string login, string password);
    }
}
