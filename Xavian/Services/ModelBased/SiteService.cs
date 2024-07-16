using _serviceModel = Xavian.DataContext.Models.Site;

using Xavian.DataContext;
using Microsoft.Extensions.Options;
using Xavian.DTOs;
using Xavian.Services.Template;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Xavian.Services
{
    public interface ISiteService
    {
        Task<List<_serviceModel>> Get(int year, int month);
        Task<List<_serviceModel>> GetNew();
        Task<int> GetCount();
        Task<List<long>> GetAllIds();
        Task<List<_serviceModel>> GetByIds(List<long> ids);
        Task<List<ResultsDto>> Insert(List<_serviceModel> rows);
        Task<List<ResultsDto>> Update(List<_serviceModel> rows);
        Task<List<ResultsDto>> Delete(List<long> ids);
    }

    public class SiteService : DefaultService<_serviceModel>, ISiteService
    {
        XavianDbContext _context;
        IAuthenticationService _authenticationService;
        IOptions<Configurations> _config;

        public SiteService(XavianDbContext context, IAuthenticationService authenticationService, IOptions<Configurations> config) : base(context, authenticationService, config, IQueryables.IQueryables.Site)
        {
            _context = context;
            _authenticationService = authenticationService;
            _config = config;
        }
        public new async Task<List<_serviceModel>> Get(int year, int month)
        {
            return await base.Get(year, month);
        }

        public new async Task<List<_serviceModel>> GetNew()
        {
            return await base.GetNew();
        }

        public new async Task<int> GetCount()
        {
            return await base.GetCount();
        }

        public new async Task<List<long>> GetAllIds()
        {
            return await base.GetAllIds();
        }

        public new async Task<List<_serviceModel>> GetByIds(List<long> ids)
        {
            return await base.GetByIds(ids);
        }

        public async Task<List<ResultsDto>> Insert(List<_serviceModel> rows)
        {
            return await base.Insert(rows, false);
        }

        public new async Task<List<ResultsDto>> Update(List<_serviceModel> rows)
        {
            return await base.Update(rows);
        }

        public new async Task<List<ResultsDto>> Delete(List<long> ids)
        {
            return await base.Delete(ids);
        }
    }
}
