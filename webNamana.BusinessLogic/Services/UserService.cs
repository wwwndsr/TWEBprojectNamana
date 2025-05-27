using System;
using System.Linq;
using webNamana.BusinessLogic.DBModel;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Helpers;

namespace webNamana.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;

        public UserService()
        {
            _context = new UserContext();
        }

        public UDbTable GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public bool UpdateUserProfile(string username, UDbTable updatedUser)
        {
            var user = GetUserByUsername(username);
            if (user == null)
                return false;

            user.Email = updatedUser.Email;
            user.Username = updatedUser.Username;
            // Доп. поля по необходимости

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangePassword(string username, string currentPassword, string newPassword)
        {
            var user = GetUserByUsername(username);
            if (user == null)
                return false;

            if (!VerifyPassword(currentPassword, user.Password))
                return false;

            user.Password = HashPassword(newPassword);

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateUserCredentials(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user == null)
                return false;

            return VerifyPassword(password, user.Password);
        }

        public bool CreateUser(UDbTable newUser)
        {
            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            return LoginHelper.HashGen(password);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
