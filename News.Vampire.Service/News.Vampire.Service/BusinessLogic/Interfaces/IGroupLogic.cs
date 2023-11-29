using News.Vampire.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface IGroupLogic
    {
        IQueryable<Group> GetAll();
    }
}
