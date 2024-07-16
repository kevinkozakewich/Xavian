using Xavian.DataContext;
using Xavian.DataContext.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Xavian.Services
{
    public interface IAuthenticationService
    {
        Task<UserDto> ValidateUser();
    }

    public class AuthenticationService : IAuthenticationService
    {
        XavianDbContext _context;
        //IHttpContextAccessor _contextAccessor;
        public AuthenticationService(XavianDbContext context/*, IHttpContextAccessor contextAccessor*/)
        {
            context = _context;
            //contextAccessor = _contextAccessor;
        }

        // Presently users have a Google account and a Microsoft account, but no way to combine them.
        // We should auto-combine them, because when a user creates a google account with the same email, they are asked to verify they actually own it.
        public async Task<UserDto> ValidateUser()
        {
            return null;

            /*var user = _contextAccessor.HttpContext?.User;

            // Only used if the application is web-based.
            // Will need a different form of token retrieval if desktop.
            var authenticationMethod = user.FindFirstValue("amr") ?? user.FindFirstValue("iss");
            var authenticationId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = user.FindFirstValue("name");
            var email = user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue(ClaimTypes.Upn);

            bool IsGoogleAuthentication = false;
            bool IsMicrosoftAuthentication = false;

            if (authenticationMethod == "https://accounts.google.com")
            {
                IsGoogleAuthentication = true;
            }
            else if (authenticationMethod.BeginsWith("https://sts.windows.net/"))
            {
                IsMicrosoftAuthentication = true;
            }
            else
            {
                throw new Exception($"Unable to identify authentication method: {authenticationMethod}");
            }

            var userRecord = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            // Update
            if (userRecord != null)
            {
                if(userRecord.Deleted)
                {
                    string errorMessage = "User account has been deactivated. Please email admin for assistance.";

                    await _context.Logs.AddAsync(new Log
                    {
                        Level = LogLevel.Error,
                        Class = nameof(AuthenticationService),
                        Message = "ValidateUser()",
                        FurtherDetails = errorMessage,
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedUserId = userRecord.Id,
                        LastUpdatedDateTime = DateTime.UtcNow,
                        LastUpdatedUserId = userRecord.Id,
                        OwnerUserId = userRecord.Id,
                        Deleted = false
                    });
                    await _context.SaveChangesAsync();

                    throw new Exception("User account has been deactivated. Please email admin for assistance.");
                }
                if (IsGoogleAuthentication)
                {
                    if(userRecord.GoogleAuthenticationId == null)
                    { 
                        userRecord.GoogleAuthenticationId = authenticationId;
                    }
                    else if (userRecord.GoogleAuthenticationId != authenticationId)
                    {
                        string errorMessage = "Google Authentication Id doesn't match the one attached to this email. Please email admin to check logs.";

                        await _context.Logs.AddAsync(new Log 
                        {
                            Level = LogLevel.Error,
                            Class = nameof(AuthenticationService),
                            Message = "ValidateUser() -> " + errorMessage,
                            FurtherDetails = $"New Authentication Id: \"{authenticationId}\"",
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedUserId = userRecord.Id,
                            LastUpdatedDateTime = DateTime.UtcNow,
                            LastUpdatedUserId = userRecord.Id,
                            OwnerUserId = userRecord.Id,
                            Deleted = false
                        });
                        await _context.SaveChangesAsync();

                        throw new Exception(errorMessage);
                    }
                }
                else if (IsMicrosoftAuthentication)
                {
                    if(userRecord.MicrosoftAuthenticationId == null)
                    { 
                        userRecord.MicrosoftAuthenticationId = authenticationId;
                    }
                    else if (userRecord.MicrosoftAuthenticationId != authenticationId)
                    {
                        string errorMessage = "Microsoft Authentication Id doesn't match the one attached to this email. Please email admin to check logs.";

                        await _context.Logs.AddAsync(new Log
                        {
                            Level = LogLevel.Error,
                            Class = nameof(AuthenticationService),
                            Message = "ValidateUser() -> " + errorMessage,
                            FurtherDetails = $"New Authentication Id: \"{authenticationId}\"",
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedUserId = userRecord.Id,
                            LastUpdatedDateTime = DateTime.UtcNow,
                            LastUpdatedUserId = userRecord.Id,
                            OwnerUserId = userRecord.Id,
                            Deleted = false
                        });
                        await _context.SaveChangesAsync();

                        throw new Exception(errorMessage);
                    }
                }

                userRecord.LastLoggedInDateTime = DateTime.UtcNow;
                userRecord.LastUpdatedDateTime = DateTime.UtcNow;
                userRecord.LastUpdatedUserId = userRecord.Id;
                userRecord.OwnerUserId = userRecord.Id;

                userRecord = _context.Users.Update(userRecord).Entity;
                await _context.SaveChangesAsync();

                return new UserDto
                {
                    Id = userRecord.Id,
                    Name = userRecord.Name,
                    Email = userRecord.Email,
                    dbAdmin = userRecord.DbAdmin
                };
            } 

            // Insert
            else
            {
                if (name == null)
                {
                    throw new Exception("Unable to retrieve name from user");
                }
                if (email == null)
                {
                    throw new Exception("Unable to retrieve email from user");
                }
                if (!email.Contains('@') || !email.Contains('.'))
                {
                    throw new Exception($"Invalid email from user: {email}");
                }

                userRecord = new User
                {
                    GoogleAuthenticationId = IsGoogleAuthentication ? authenticationId : null,
                    MicrosoftAuthenticationId = IsMicrosoftAuthentication ? authenticationId : null,
                    Name = name,
                    Email = email,
                    Deleted = false,
                    WebAccess = false,
                    LastLoggedInDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    LastUpdatedUserId = null,
                    DbAdmin = false
                };

                userRecord = _context.Users.Add(userRecord).Entity;
                await _context.SaveChangesAsync();

                userRecord.LastLoggedInDateTime = DateTime.UtcNow;
                userRecord.LastUpdatedDateTime = DateTime.UtcNow;
                userRecord.LastUpdatedUserId = userRecord.Id;
                userRecord.OwnerUserId = userRecord.Id;

                userRecord = _context.Users.Update(userRecord).Entity;
                await _context.SaveChangesAsync();

                return new UserDto
                {
                    Id = userRecord.Id,
                    Name = userRecord.Name,
                    Email = userRecord.Email,
                    dbAdmin = false
                };
            }*/
        }
    }

    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public bool dbAdmin { get; set; }
    }
}
