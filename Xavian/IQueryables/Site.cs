using System.Linq;
using Xavian.DataContext;
using Xavian.DataContext.Models;
using Xavian.Services;

namespace Xavian.IQueryables
{
    public partial class IQueryables
    {
        public static IQueryable<Site> Site(XavianDbContext context, UserDto user, bool isModifyingData)
        {
            // isModifyingData: Filters on Teams, only returning records which one owns/manages, via the Manager column in dbo.TeamUser.
            var teamRows = IQueryables.SitePermission(context, user, isModifyingData)
                .Join(context.Sites, sp => sp.SiteId, s => s.Id, (sp, s) => s);

            var ownedRows = context.Sites
                .Where(s => s.OwnerUserId == user.Id || user.dbAdmin);

            return teamRows.Union(ownedRows).Where(o => !o.Deleted);
        }
    }
}
