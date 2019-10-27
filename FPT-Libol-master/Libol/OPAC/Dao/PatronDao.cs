using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using OPAC.Models;

namespace OPAC.Dao
{
    public class PatronDao
    {
        /// <summary>
        /// Update password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userID"></param>
        public void UpdatePassword(string password, int userID)
        {
            using (var dbContext = new OpacEntities())
            {
                var account = (from a in dbContext.CIR_PATRON where a.ID == userID select a).FirstOrDefault();

                if (account != null)
                {
                    account.Password = password;
                    dbContext.Entry(account).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update password via Email
        /// </summary>
        /// <param name="password"></param>
        /// <param name="email"></param>
        public void UpdatePasswordByEmail(string password, string email)
        {
            using (var dbContext = new OpacEntities())
            {
                var account = (from a in dbContext.CIR_PATRON where a.Email.Equals(email) select a).FirstOrDefault();
                if (account != null)
                {
                    account.Password = password;
                    dbContext.Entry(account).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Random new password with the length of 7
        /// </summary>
        /// <returns></returns>
        public string RandomPassword()
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int passwordLength = 7;
            StringBuilder newPassword = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (passwordLength-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    newPassword.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return newPassword.ToString().Trim();
        }
    }
}