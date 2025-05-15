using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using webNamana.BusinessLogic.DBModel;
using webNamana.Domain.Entities.User;
using webNamana.BusinessLogic.Interfaces;

namespace webNamana.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;

        public UserService()
        {
            _context = new UserContext();
        }

        public UDbTable GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool UpdateUserProfile(string currentEmail, string newUsername, string newEmail)
        {
            try
            {
                var user = GetUserByEmail(currentEmail);
                if (user == null) return false;

                // Check if new email is already taken by another user
                if (newEmail != currentEmail && _context.Users.Any(u => u.Email == newEmail))
                {
                    return false;
                }

                user.Username = newUsername;
                user.Email = newEmail;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangePassword(string email, string currentPassword, string newPassword)
        {
            try
            {
                var user = GetUserByEmail(email);
                if (user == null) return false;

                // Use VerifyPassword instead of direct comparison
                if (!VerifyPassword(user.Password, currentPassword))
                {
                    return false;
                }

                user.Password = HashPassword(newPassword);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Combine password and salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            // Hash the combined bytes
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                // Combine salt and hash for storage
                byte[] hashWithSalt = new byte[hashBytes.Length + salt.Length];
                Buffer.BlockCopy(hashBytes, 0, hashWithSalt, 0, hashBytes.Length);
                Buffer.BlockCopy(salt, 0, hashWithSalt, hashBytes.Length, salt.Length);
                return Convert.ToBase64String(hashWithSalt);
            }
        }

        public bool VerifyPassword(string storedPassword, string providedPassword)
        {
            try
            {
                byte[] hashWithSalt = Convert.FromBase64String(storedPassword);
                byte[] salt = new byte[16];
                byte[] hash = new byte[hashWithSalt.Length - 16];

                // Extract salt and hash
                Buffer.BlockCopy(hashWithSalt, hashWithSalt.Length - 16, salt, 0, 16);
                Buffer.BlockCopy(hashWithSalt, 0, hash, 0, hashWithSalt.Length - 16);

                // Hash the provided password with the same salt
                byte[] passwordBytes = Encoding.UTF8.GetBytes(providedPassword);
                byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

                using (var sha256 = SHA256.Create())
                {
                    byte[] computedHash = sha256.ComputeHash(combinedBytes);
                    return computedHash.SequenceEqual(hash);
                }
            }
            catch
            {
                return false;
            }
        }
    }
} 