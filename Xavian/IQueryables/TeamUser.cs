using System.Linq;
using Xavian.DataContext;
using Xavian.DataContext.Models;
using Xavian.Services;

namespace Xavian.IQueryables
{
    public partial class IQueryables
    {
        public static IQueryable<TeamUser> TeamUser(XavianDbContext context, UserDto user, bool isModifyingData)
        {
            // No need for a separate line to also pull TeamUsers with one's Id, as Team already returns all users within a team.
            var parentRows = IQueryables.Team(context, user, isModifyingData)
                .Join(context.TeamUsers, t => t.Id, tu => tu.TeamId, (t, tu) => tu);

            var ownedRows = context.TeamUsers.Where(o => o.OwnerUserId == user.Id || user.dbAdmin);

            return parentRows.Union(ownedRows).Where(o => !o.Deleted);
        }
    }
}