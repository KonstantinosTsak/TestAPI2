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
    public class ContactInfoHelper
    {
        private readonly TestDBContext _dbcontext;
        public ContactInfoHelper(TestDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// Registers the Contact information for the first time // Adds email and password in Email table in the DB
        /// </summary>
        /// <param name="newinfo"></param>
        /// <returns></returns>
        public async Task<ContactInfo> AddContactInfo(ContactInfo newinfo)
        {
            bool registered = false;
            try
            {
                ContactInfoHelper cih = new ContactInfoHelper(_dbcontext);
                EmailHelper emh = new EmailHelper(_dbcontext);

                //checking if the Contact number already exists => contact information already exist
                var infoexists = _dbcontext.ContactInfo.Where(u => u.ContactNumber == newinfo.ContactNumber)
                    .Select(u => new ContactInfo
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        ContactNumber = u.ContactNumber
                    })
                    .FirstOrDefault();
                if (infoexists == null)
                {
                    ContactInfo ci = new ContactInfo
                    {
                        FirstName = newinfo.FirstName,
                        LastName = newinfo.LastName,
                        ContactNumber = newinfo.ContactNumber
                    };
                    await AddDBContactInfo(ci);

                    Email em = new Email();

                    //add email to the contact information
                    if (ci.ContactNumber != null)
                    {
                        em = new Email
                        {
                            EmailAddress = em.EmailAddress,
                            Password = em.Password
                        };
                        await emh.AddDBEmailInfo(em);
                    }

                    registered = true;
                    return ci;
                    //return new Response<ContactInfo>
                    //{
                    //    Message = "The contact information have been successfully committed",
                    //    ReturnObject = newinfo,
                    //    Success = true

                }
                else
                {
                    registered = false;
                    return infoexists;
                    //            return new Response<ContactInfo>
                    //            {
                    //                Message = "The contact information already exist",
                    //                ReturnObject = infoexists,
                    //                Success = false
                    //            }
                }
            }
            finally
            {
                _dbcontext.Dispose();
            }
        }

        /// <summary>
        /// Adds Contact Information in the DB
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        public async Task<ContactInfo> AddDBContactInfo(ContactInfo ci)
        {
            if (ci != null)
            {
                //check if the user with the same First Name and Last Name already exists
                var alreadyexists = await _dbcontext.ContactInfo.Where(cont => cont.FirstName == ci.FirstName && cont.LastName == ci.LastName).FirstOrDefaultAsync();
                if (alreadyexists != null)
                {
                    return alreadyexists;
                }
                else
                {
                    _dbcontext.ContactInfo.Add(ci);
                    await _dbcontext.SaveChangesAsync();
                    return ci;
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Gets the Contact Information based on First and Last Name
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public Task<List<Models.ContactInfo>> GetContactInfo(string firstname, string lastname)
        {
            return _dbcontext.ContactInfo
                                                    .Include(p => p.FirstName)
                                                    .Include(p => p.LastName)
                                                    .Include(p => p.ContactNumber)
                                                    .Where(p => p.FirstName == firstname && p.LastName == lastname)
                                                    .Select(p => new ContactInfo
                                                    {
                                                        FirstName = p.FirstName,
                                                        LastName = p.LastName,
                                                        ContactNumber = p.ContactNumber
                                                    })
                                                    .ToListAsync();
        }

        /// <summary>
        /// Updates the contact information based on first and last name
        /// </summary>
        /// <param name="infocheck"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        public async Task<ContactInfo> UpdateContactInfo(ContactInfo infocheck, string firstname, string lastname)
        {
            ContactInfo newcontact = new ContactInfo
            {
                Id=infocheck.Id,
                FirstName = infocheck.FirstName,
                LastName = infocheck.LastName,
                ContactNumber = infocheck.ContactNumber
            };
            bool idsearch = _dbcontext.ChangeTracker.Entries<ContactInfo>().Any(p => p.Entity.FirstName == infocheck.FirstName && p.Entity.LastName == infocheck.LastName);

            if (!idsearch)
            {
                _dbcontext.ContactInfo.Update(newcontact);
            }
            await _dbcontext.SaveChangesAsync();
            return newcontact;
        }
        /// <summary>
        /// Deletes a contact information from the DB based on Contact Number
        /// </summary>
        /// <param name="contactnumber"></param>
        /// <returns></returns>
        public async Task<bool> DeleteContactInfo(int contactnumber)
        {
            var ci = await _dbcontext.ContactInfo.FindAsync(contactnumber);
            if (ci != null)
            {
                _dbcontext.ContactInfo.Remove(ci);
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
