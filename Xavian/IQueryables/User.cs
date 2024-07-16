using System.Linq;
using Xavian.DataContext;
using Xavian.DataContext.Models;
using Xavian.Services;

namespace Xavian.IQueryables
{
    public partial class IQueryables
    {
        public static IQueryable<User> User(XavianDbContext context, UserDto user, bool isModifyingData)
        {
            if (isModifyingData && user.dbAdmin)
            {
                return context.Users;
            }
            
            var ownerUser = context.Users.Where(u => u.Id == user.Id || user.dbAdmin);

            // isModifyingData: Filters on Teams, only returning Users which one owns/manages, via the Manager column in dbo.TeamUser.
            var teamUsers = IQueryables.TeamUser(context, user, isModifyingData)
                .Join(context.Users, tu => tu.UserId, u => u.Id, (tu, u) => u);

            return ownerUser.Union(teamUsers).Where(o => !o.Deleted);
        }
    }
}
