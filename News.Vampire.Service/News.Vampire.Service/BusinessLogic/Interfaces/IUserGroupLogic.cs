using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface IUserGroupLogic: IBaseLogic<UserGroup>
    {
        IQueryable<UserGroup> GetAllByUserAsync(int userId);
    }
}
