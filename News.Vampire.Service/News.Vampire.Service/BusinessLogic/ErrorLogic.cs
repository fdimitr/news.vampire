using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic
{
    public class ErrorLogic : BaseLogic<Error>, IErrorLogic
    {
        public ErrorLogic(DataContext dbContext, DbContextOptions<DataContext> dbContextOptions) : base(dbContext, dbContextOptions)
        {
        }
    }
}
