using System;
using System.Threading.Tasks;
using webNamana.Models;
using webNamana.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace webNamana.Services
{
    public interface IUserService
    {
        Task<UserDashboard> GetUserDashboardAsync(string username);
        Task<UserProfileEdit> GetUserProfileForEditAsync(string username);
        Task<bool> UpdateUserProfileAsync(string username, UserProfileEdit model);
        Task<bool> ChangePasswordAsync(string username, PasswordChange model);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDashboard> GetUserDashboardAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return null;

            return new UserDashboard
            {
                Username = user.Username,
                Email = user.Email,
                CartItems = user.CartItems.Select(ci => ci.Name).ToList(),
                LastLogin = user.LastLogin,
                MembershipStatus = user.MembershipStatus,
                RecentActivities = user.RecentActivities
            };
        }

        public async Task<UserProfileEdit> GetUserProfileForEditAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return null;

            return new UserProfileEdit
            {
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
        }

        public async Task<bool> UpdateUserProfileAsync(string username, UserProfileEdit model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return false;

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(string username, PasswordChange model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return false;

            if (!VerifyPassword(model.CurrentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(model.NewPassword);
            user.LastPasswordChange = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return false;

            return VerifyPassword(password, user.PasswordHash);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
} 