using Xavian.DataContext;
using Microsoft.Extensions.Options;
using Xavian.DTOs;
using Xavian.Services.Template;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Xavian.Services
{
    public interface IAppVersionService
    {
        Task<int> GetAppVersion();
    }

    public class AppVersionService : IAppVersionService
    {
        XavianDbContext _context;
        IAuthenticationService _authenticationService;
        IOptions<Configurations> _config;

        public AppVersionService(XavianDbContext context, IAuthenticationService authenticationService, IOptions<Configurations> config)
        { 
            _context = context;
            _authenticationService = authenticationService;
            _config = config;
        }

        public async Task<int> GetAppVersion() 
        {
            return await _context.AppVersions.Select(v => v.Version).FirstOrDefaultAsync();
        }
    }
}
