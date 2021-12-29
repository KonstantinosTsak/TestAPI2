using System.Linq;
using System.Net;
using System.Net.Http;
using TestAPI2.Models;
using Microsoft.Extensions.Configuration;
using TestAPI2.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfosController : ControllerBase
    {
        private readonly TestDBContext _dbcontext;
        public ContactInfosController(TestDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// Calls ContactInfoHelper.AddContactInfo to add the new contact information
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterContactInformation(Models.ContactInfo ci)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContactInfoHelper cih = new ContactInfoHelper(_dbcontext);
                    var newcontactinfo = await cih.AddContactInfo(ci);

                    //Εδω θα χρησιμοποιουσα τον ελεγχο της Success , τωρα δεν κανω ελεγχο καν αν υπαρχει

                    //if (newcontactinfo.Success)
                    //      return Ok(newcontactinfo.ReturnObject);
                    //else
                    //       return BadRequest(newcontactinfo.Message);

                    return (IActionResult)Ok();
                }
                finally
                {
                    _dbcontext.Dispose();
                }
            }
            else
            {
                return (IActionResult)BadRequest();
            }
        }

        /// <summary>
        /// Checks the First Name and Last name given and checks in ContactInfoHelper.GetContactInfo if they exist
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetContactInformation(string firstname, string lastname)
        {
            try
            {
                ContactInfoHelper cih = new ContactInfoHelper(_dbcontext);
                return (IActionResult)Ok(await cih.GetContactInfo(firstname, lastname));
            }
            finally
            {
                _dbcontext.Dispose();
            }


        }

        /// <summary>
        /// Checks in ContactInfoHelper.UpdateContactInfo if the contacts exists and replaces the information
        /// </summary>
        /// <param name="newcontactinfo"></param>
        /// <param name="cID"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateContactInformation(ContactInfo newcontactinfo, string firstname, string lastname)
        {
            if (ModelState.IsValid)
            {
                ContactInfoHelper cih = new ContactInfoHelper(_dbcontext);
                return (IActionResult)Ok(await cih.UpdateContactInfo(newcontactinfo, firstname, lastname));
            }
            else
            {
                return (IActionResult)BadRequest(ModelState);
            }

        }


        /// <summary>
        /// Deletes the contact information based on Contact Number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteContactInformation(int contactnumber)
        {
            if (ModelState.IsValid)
            {
                ContactInfoHelper cih = new ContactInfoHelper(_dbcontext);
                return (IActionResult)Ok(await cih.DeleteContactInfo(contactnumber));
            }
            else
            {
                return (IActionResult)BadRequest(ModelState);
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _dbcontext.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
