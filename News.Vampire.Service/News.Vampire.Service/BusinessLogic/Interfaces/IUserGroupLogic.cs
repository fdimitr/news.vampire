using News.Vampire.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface IUserGroupLogic: IBaseLogic<UserGroup>
    {
        Task<IList<UserGroup>> GetAllByUserAsync(long userId);
    }
}
