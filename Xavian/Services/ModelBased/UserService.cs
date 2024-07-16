using _serviceModel = Xavian.DataContext.Models.User;

using Xavian.DataContext;
using Xavian.Services.Template;
using Microsoft.Extensions.Options;
using Xavian.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Xavian.Services
{
    public interface IUserService
    {
        Task<UserDto> Login();
        Task<bool> BeginUpdate();
        Task<bool> FinishUpdate();
        Task<List<_serviceModel>> Get(int year, int month);
        Task<List<_serviceModel>> GetNew();
        Task<int> GetCount();
        Task<List<long>> GetAllIds();
        Task<List<_serviceModel>> GetByIds(List<long> ids);
        Task<List<ResultsDto>> Insert(List<_serviceModel> rows);
        Task<List<ResultsDto>> Update(List<_serviceModel> rows);
        Task<List<ResultsDto>> Delete(List<long> ids);
    }

    public class UserService : DefaultService<_serviceModel>, IUserService
    {
        XavianDbContext _context;
        IAuthenticationService _authenticationService;
        IOptions<Configurations> _config;

        public UserService(XavianDbContext context, IAuthenticationService authenticationService, IOptions<Configurations> config) : base(context, authenticationService, config, IQueryables.IQueryables.User)
        {
            _context = context;
            _authenticationService = authenticationService;
            _config = config;
        }

        public async Task<UserDto> Login()
        {
            var user = await _authenticationService.ValidateUser();

            var signedInUser = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

            signedInUser.LastUpdatedDateTime = DateTime.UtcNow;
            signedInUser.LastLoggedInDateTime = DateTime.UtcNow;
            signedInUser.LastUpdatedUserId = signedInUser.Id;

            _context.Users.Update(signedInUser);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> BeginUpdate()
        {
            var user = await _authenticationService.ValidateUser();

            var updateUser = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

            if (updateUser == null) { throw new Exception("This error should be impossible to happen. User returned null on BeginUpdate()."); }
            else
            {
                updateUser.LastUpdatedDateTime = DateTime.UtcNow;
                updateUser.LastUpdatedUserId = user.Id;

                updateUser = _context.Users.Update(updateUser).Entity;

                int rowsAffected = await _context.SaveChangesAsync();
                if (rowsAffected == 0) { throw new Exception("Failed to update User on BeginUpdate(). No rows affected."); }

                return rowsAffected > 0;
            }
        }

        public async Task<bool> FinishUpdate()
        {
            var user = await _authenticationService.ValidateUser();

            var updateUser = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

            if (updateUser == null) { throw new Exception("This error should be impossible to happen. User returned null on FinishUpdate()."); }
            else
            {
                updateUser.LastUpdatedDateTime = DateTime.UtcNow;
                updateUser.LastUpdatedUserId = user.Id;

                updateUser = _context.Users.Update(updateUser).Entity;

                int rowsAffected = await _context.SaveChangesAsync();
                if(rowsAffected == 0) { throw new Exception("Failed to update User on FinishUpdate(). No rows affected."); }

                return rowsAffected > 0;
            }
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
