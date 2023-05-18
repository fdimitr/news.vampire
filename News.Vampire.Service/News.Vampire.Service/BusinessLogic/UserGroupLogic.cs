﻿using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic
{
    public class UserGroupLogic : BaseLogic<UserGroup>, IUserGroupLogic
    {
        public UserGroupLogic(IDbContextFactory<DataContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<IList<UserGroup>> GetAllByUserAsync(int userId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await (from userGroup in dbContext.UserGroups
                          join subscribe in dbContext.Subscriptions on userGroup.Id equals subscribe.UserGroupId
                          where subscribe.UserId == userId
                          select userGroup).Distinct().Include(ug => ug.Subscriptions).ThenInclude(s => s.Source).ToListAsync();
        }
    }
}
