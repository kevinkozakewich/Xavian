using System.Linq;
using Xavian.DataContext;
using Xavian.DataContext.Models;
using Xavian.Services;

namespace Xavian.IQueryables
{
    public partial class IQueryables
    {
        public static IQueryable<Team> Team(XavianDbContext context, UserDto user, bool isModifyingData)
        {
            // isModifyingData: Filters on Teams, only returning records which one owns/manages, via the Manager column in dbo.TeamUser.
            return context.Teams
                .Join(context.TeamUsers, t => t.Id, tu => tu.TeamId, (t, tu) => new { t, tu })
                .Where(o => o.t.OwnerUserId == user.Id || (o.tu.UserId == user.Id && (!isModifyingData || o.tu.Manager)) || user.dbAdmin)
                .Select(o => o.t)
                .Where(o => !o.Deleted);
        }
    }
}
