using System.Linq;
using Xavian.DataContext;
using Xavian.DataContext.Models;
using Xavian.Services;

namespace Xavian.IQueryables
{
    public partial class IQueryables
    {
        public static IQueryable<SitePermission> SitePermission(XavianDbContext context, UserDto user, bool isModifyingData)
        {
            var parentRows = IQueryables.Team(context, user, isModifyingData)
                .Join(context.SitePermissions, t => t.Id, sp => sp.TeamId, (t, sp) => sp);

            var ownedRows = context.SitePermissions.Where(o => o.OwnerUserId == user.Id || user.dbAdmin);

            return parentRows.Union(ownedRows).Where(o => !o.Deleted);
        }
    }
}
