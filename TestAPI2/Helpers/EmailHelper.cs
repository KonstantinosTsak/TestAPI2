using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestAPI2.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using TestAPI2.Helpers;

namespace TestAPI2.Helpers
{
    public class EmailHelper
    {
        private readonly TestDBContext _dbcontext;
        public EmailHelper(TestDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// Adds Email address and password in the DB
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public async Task<Email> AddDBEmailInfo(Email em)
        {
            if (em != null)
            {
                //check if the user with the same Email address already exists
                var emailexists = await _dbcontext.Email.Where(x => x.EmailAddress == em.EmailAddress).FirstOrDefaultAsync();
                if (emailexists != null)
                {
                    return emailexists;
                }
                else
                {
                    _dbcontext.Email.Add(em);
                    await _dbcontext.SaveChangesAsync();
                    return em;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets Email credentials based on both Email address and Password
        /// </summary>
        /// <param name="emailaddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<List<Models.ContactInfo>> GetEmailInfo(string emailaddress, string password)
        {
            var infoexist = _dbcontext.Email.AnyAsync(u => u.EmailAddress == emailaddress && u.Password == password);
            if (infoexist != null)
            {
                return _dbcontext.ContactInfo
                                             .Include(u => u.FirstName)
                                             .Include(u => u.LastName)
                                             .Include(u => u.ContactNumber)
                                             .Select(u => new ContactInfo
                                             {
                                                 FirstName = u.FirstName,
                                                 LastName = u.LastName,
                                                 ContactNumber = u.ContactNumber
                                             })
                                             .ToListAsync();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updates Email credentials based on both Email address and Password
        /// </summary>
        /// <param name="emailcheck"></param>
        /// <param name="emailaddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Email> UpdateEmailInfo(Email emailcheck, string emailaddress, string password)
        {
            Email newemail = new Email
            {
                Id = emailcheck.Id,
                EmailAddress = emailcheck.EmailAddress,
                Password = emailcheck.Password
            };
            bool emailsearch = _dbcontext.ChangeTracker.Entries<Email>().Any(p => p.Entity.EmailAddress == emailcheck.EmailAddress && p.Entity.Password == emailcheck.Password);
            if (!emailsearch)
            {
                _dbcontext.Email.Update(newemail);
            }
            await _dbcontext.SaveChangesAsync();
            return newemail;
        }

        /// <summary>
        /// Deletes Emails credentials based on both Email and Password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEmailInfo(string email, string password)
        {
            var em = await _dbcontext.Email.FindAsync(email, password);
            if (em != null)
            {
                _dbcontext.Email.Remove(em);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
