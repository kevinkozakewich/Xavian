using Xavian.DataContext;
using Microsoft.Extensions.Options;
using Xavian.DataContext.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xavian.DataContext.Models.Template;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xavian.DTOs;

namespace Xavian.Services.Template
{
    public abstract class DefaultService<T> where T : class, IDefaultBase
    {
        XavianDbContext _context;
        IAuthenticationService _authenticationService;
        IOptions<Configurations> _config;
        Func<XavianDbContext, UserDto, bool, IQueryable<T>> _serviceModelQuery;
        public DbSet<T> _serviceTable;

        public DefaultService(XavianDbContext context, IAuthenticationService authenticationService, IOptions<Configurations> config, Func<XavianDbContext, UserDto, bool, IQueryable<T>> serviceModelQuery) 
        {
            _context = context;
            _authenticationService = authenticationService;
            _config = config;
            _serviceModelQuery = serviceModelQuery;
            _serviceTable = _context.Set<T>();
        }

        public async Task<List<T>> Get(int year, int month)
        {
            var user = await _authenticationService.ValidateUser();
            try
            {
                var rows = await _serviceModelQuery(_context, user, false)
                    .Where(o => o.LastUpdatedDateTime.Year == year && o.LastUpdatedDateTime.Month == month)
                    .ToListAsync();

                return rows;
            }
            catch(Exception ex)
            {
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = $"Failed on Get({year}, {month}).",
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> GetNew()
        {
            var user = await _authenticationService.ValidateUser();
            try
            {
                var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

                var rows = await _serviceModelQuery(_context, user, false)
                    .Where(o => o.LastUpdatedDateTime >= sevenDaysAgo)
                    .ToListAsync();

                return rows;
            }
            catch (Exception ex)
            {
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = $"Failed on GetNew().",
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetCount()
        {
            var user = await _authenticationService.ValidateUser();

            try
            {
                var count = await _serviceModelQuery(_context, user, false)
                    .CountAsync();

                return count;
            }
            catch (Exception ex)
            {
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = $"Failed on GetCount().",
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<long>> GetAllIds()
        {
            var user = await _authenticationService.ValidateUser();

            try
            {
                var rows = await _serviceModelQuery(_context, user, false)
                    .Select(o => o.Id)
                    .ToListAsync();

                return rows;
            }
            catch (Exception ex)
            {
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = $"Failed on GetAllIds().",
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> GetByIds(List<long> ids)
        {
            var user = await _authenticationService.ValidateUser();

            try
            {
                var rows = await _serviceModelQuery(_context, user, false)
                    .Where(o => ids.Contains(o.Id))
                    .ToListAsync();

                return rows;
            }
            catch (Exception ex)
            {
                string message = $"Failed on GetByIds({JsonConvert.SerializeObject(ids)}).";
                string messageCleaned = message.Length > 10000 ? "Import contained more than 10000 characters. Truncating error message size. " + Environment.NewLine + message.Substring(0, 10000) : message;
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = $"Failed on GetByIds({messageCleaned}).",
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                throw new Exception(ex.Message);
            }
        }

        // Overwritten in DefaultService, for records with OwnerUserId.
        public async Task<List<ResultsDto>> Insert(List<T> rows, bool hasPicture)
        {
            var user = await _authenticationService.ValidateUser();
            var resultsDto = new List<ResultsDto>();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                foreach (var row in rows)
                {
                    var insertedRow = await Insert(row, user);
                    resultsDto.Add(insertedRow);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    string message = $"Failed on Insert({JsonConvert.SerializeObject(rows)}, {hasPicture}).";
                    string messageCleaned = message.Length > 10000 ? "Import contained more than 10000 characters. Truncating error message size. " + Environment.NewLine + message.Substring(0, 10000) : message;
                    await _context.Logs.AddAsync(new Log
                    {
                        Level = LogLevel.Error,
                        Class = nameof(DefaultService<T>),
                        Message = messageCleaned,
                        FurtherDetails = ex.Message,
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedUserId = user.Id,
                        LastUpdatedDateTime = DateTime.UtcNow,
                        LastUpdatedUserId = user.Id,
                        OwnerUserId = user.Id,
                        Deleted = false
                    });
                    await _context.SaveChangesAsync();

                    throw new Exception(ex.Message);
                }

                await VerifyInsertPermissions(resultsDto, user);

                await transaction.CommitAsync();
            }

            return resultsDto;
        }

        public async Task<ResultsDto> Insert(T newValues, UserDto user)
        {
            if(user == null)
            { 
                user = await _authenticationService.ValidateUser();
            }

            if(newValues.Deleted)
            {
                string errorMessage = "Unable to insert deleted records. Validation views will return blank.";
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = $"Failed on Insert({JsonConvert.SerializeObject(newValues)}).",
                    FurtherDetails = errorMessage,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                return new ResultsDto { Id = newValues.Id, ErrorMessage = errorMessage };
            }

            try
            {
                newValues.LastUpdatedDateTime = DateTime.UtcNow;
                newValues.LastUpdatedUserId = user.Id;
                newValues.OwnerUserId = user.Id;

                var result = (await _serviceTable.AddAsync(newValues)).Entity;

                await _context.SaveChangesAsync();

                return new ResultsDto { Id = result.Id };
            }
            catch (Exception ex)
            {
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = $"Failed on Insert({JsonConvert.SerializeObject(newValues)}).",
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                return new ResultsDto { Id = null, ErrorMessage = $"Exception while inserting row: {ex.Message}" };
            }
        }

        // Update always pre-retrieves.
        private async Task VerifyInsertPermissions(List<ResultsDto> resultsDto, UserDto user)
        {
            try
            {
                var paramIds = resultsDto.Where(p => p.ErrorMessage == null && p.Id != null).Select(r => r.Id.Value).ToList();
                List<long> recordsWithPermissionToView = (await GetByIds(paramIds)).Select(t => t.Id).ToList();

                // If any found which shouldn't have been inserted, delete and report.
                foreach (var resultDto in resultsDto)
                {
                    if(resultDto.ErrorMessage == null)
                    {
                        var id = resultDto.Id.Value;
                        if (!recordsWithPermissionToView.Contains(id))
                        { 
                            resultDto.Id = id;
                            resultDto.ErrorMessage = "Missing permissions to view inserted record. Possibly due to foreign-key insertion order.";

                            string errorMessage = "Missing permissions to view inserted record. Possibly due to foreign-key insertion order.";
                            string serializedRow = JsonConvert.SerializeObject(resultDto);
                            string furtherDetails = serializedRow.Length > 10000 ? "Import contained more than 10000 characters. Truncating error message size. " + Environment.NewLine + serializedRow.Substring(0, 10000) : serializedRow;
                            await _context.Logs.AddAsync(new Log
                            {
                                Level = LogLevel.Error,
                                Class = nameof(AuthenticationService),
                                Message = errorMessage,
                                FurtherDetails = furtherDetails,
                                CreatedDateTime = DateTime.UtcNow,
                                CreatedUserId = user.Id,
                                LastUpdatedDateTime = DateTime.UtcNow,
                                LastUpdatedUserId = user.Id,
                                OwnerUserId = user.Id,
                                Deleted = false
                            });

                            // Undo transaction.
                            throw new Exception(errorMessage);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"Failed on GetByIds({JsonConvert.SerializeObject(resultsDto)}).";
                string messageCleaned = message.Length > 10000 ? "Import contained more than 10000 characters. Truncating error message size. " + Environment.NewLine + message.Substring(0, 10000) : message;
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = messageCleaned,
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<T>> UpdateRetrieve(UserDto user, List<T> rows)
        {
            try
            {
                var rowIds = rows.Select(r => r.Id).ToList();
                var recordsFound = await _serviceModelQuery(_context, user, true)
                        .Where(o => rowIds.Contains(o.Id))
                        .ToListAsync();

                return recordsFound;
            }
            catch (Exception ex)
            {
                string message = $"Failed on UpdateRetrieve({JsonConvert.SerializeObject(rows)}).";
                string messageCleaned = message.Length > 10000 ? "Import contained more than 10000 characters. Truncating error message size. " + Environment.NewLine + message.Substring(0, 10000) : message;
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = messageCleaned,
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                throw new Exception(ex.Message);
            }
        }

        // Overwritten in DefaultService, for records with OwnerUserId.
        public async Task<List<ResultsDto>> Update(List<T> rows)
        {
            var user = await _authenticationService.ValidateUser();
            var resultsDto = new List<ResultsDto>();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                // Also validates the user has permissions.
                var recordsFound = await UpdateRetrieve(user, rows);

                foreach (var row in rows)
                {
                    row.LastUpdatedDateTime = DateTime.UtcNow;
                    row.LastUpdatedUserId = user.Id;

                    var recordFound = recordsFound.Where(o => o.Id == row.Id).FirstOrDefault();
                    if (recordFound == null)
                    {
                        resultsDto.Add(new ResultsDto { ErrorMessage = "Failed to find record.", Id = row.Id });
                    }
                    else
                    {
                        resultsDto.Add(await Update(recordFound, row, user));
                    }
                }
                try
                {
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }

                catch (Exception ex)
                {
                    string message = $"Failed on Update({JsonConvert.SerializeObject(rows)}).";
                    string messageCleaned = message.Length > 10000 ? "Import contained more than 10000 characters. Truncating error message size. " + Environment.NewLine + message.Substring(0, 10000) : message;
                    await _context.Logs.AddAsync(new Log
                    {
                        Level = LogLevel.Error,
                        Class = nameof(DefaultService<T>),
                        Message = messageCleaned,
                        FurtherDetails = ex.Message,
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedUserId = user.Id,
                        LastUpdatedDateTime = DateTime.UtcNow,
                        LastUpdatedUserId = user.Id,
                        OwnerUserId = user.Id,
                        Deleted = false
                    });
                    await _context.SaveChangesAsync();

                    throw new Exception(ex.Message);
                }
            }

            return resultsDto;
        }

        public async Task<ResultsDto> Update(T recordFound, T newValues, UserDto user)
        {
            if (user == null)
            {
                user = await _authenticationService.ValidateUser();
            }
            try
            {
                _context.Entry(recordFound).CurrentValues.SetValues(newValues);

                return new ResultsDto { Id = recordFound.Id };
            }
            catch (Exception ex)
            {
                string message = $"Failed on UpdateRetrieve({JsonConvert.SerializeObject(recordFound)}, {JsonConvert.SerializeObject(newValues)}).";
                string messageCleaned = message.Length > 10000 ? "Import contained more than 10000 characters. Truncating error message size. " + Environment.NewLine + message.Substring(0, 10000) : message;
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = messageCleaned,
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                return new ResultsDto { Id = recordFound.Id, ErrorMessage = $"Exception while updating row: {ex.Message}" };
            }
        }
        public async Task<List<ResultsDto>> Delete(List<long> ids)
        {
            var user = await _authenticationService.ValidateUser();
            
            try 
            {
                var resultsDto = new List<ResultsDto>();

                var recordsFound = await _serviceModelQuery(_context, user, true)
                        .Where(o => ids.Contains(o.Id))
                        .ToListAsync();

                foreach (var id in ids)
                {
                    var recordFound = recordsFound.Where(o => o.Id == id).FirstOrDefault();
                    if (recordFound == null)
                    {
                        resultsDto.Add(new ResultsDto { Id = id, ErrorMessage = $"Unable to find record while deactivating row {id}." });
                    }
                    else
                    {
                        recordFound.Deleted = true;
                        recordFound.LastUpdatedDateTime = DateTime.UtcNow;
                        recordFound.LastUpdatedUserId = user.Id;

                        _serviceTable.Update(recordFound);

                        resultsDto.Add(new ResultsDto { Id = id });
                    }
                }

                await _context.SaveChangesAsync();
                return resultsDto;
            }
            catch (Exception ex)
            {
                string message = $"Failed on Delete({JsonConvert.SerializeObject(ids)}).";
                string messageCleaned = message.Length > 10000 ? "Import contained more than 10000 characters. Truncating error message size. " + Environment.NewLine + message.Substring(0, 10000) : message;
                await _context.Logs.AddAsync(new Log
                {
                    Level = LogLevel.Error,
                    Class = nameof(DefaultService<T>),
                    Message = messageCleaned,
                    FurtherDetails = ex.Message,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = user.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = user.Id,
                    OwnerUserId = user.Id,
                    Deleted = false
                });
                await _context.SaveChangesAsync();

                throw new Exception(ex.Message);
            }
        }
    }
}
