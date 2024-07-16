using System.Linq;
using Xavian.DataContext;
using Xavian.DataContext.Models;
using Xavian.Services;

namespace Xavian.IQueryables
{
    public partial class IQueryables
    {
        public static IQueryable<Log> Log(XavianDbContext context, UserDto user, bool isModifyingData)
        {
            return context.Logs.Where(l => (user.dbAdmin || l.LastUpdatedUserId == user.Id || l.OwnerUserId == user.Id) && !l.Deleted);
        }
    }
}
