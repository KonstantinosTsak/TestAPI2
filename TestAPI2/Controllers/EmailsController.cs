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
    public class EmailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private TestDBContext _dbcontext;

        public EmailsController(TestDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        /// <summary>
        /// Calls EmailHelper.AddDBEmailInfo method to add new Email Information
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterEmail(Email em)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmailHelper eh = new EmailHelper(_dbcontext);
                    var newemail = await eh.AddDBEmailInfo(em);
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
        /// Calls EmailHelper.GetEmailInfo method and fetches the contact information based on EmailAddress and Password
        /// </summary>
        /// <param name="emailaddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEmail(string emailaddress, string password)
        {
            try
            {
                EmailHelper emh = new EmailHelper(_dbcontext);
                return (IActionResult)Ok(await emh.GetEmailInfo(emailaddress, password));
            }
            finally
            {
                _dbcontext.Dispose();
            }
        }

        /// <summary>
        /// Calls EmailHelper.UpdateEmailInfo and updates the DB based on already existing email address and password
        /// </summary>
        /// <param name="newemail"></param>
        /// <param name="emailaddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateEmail(Email newemail, string emailaddress, string password)
        {
            if (ModelState.IsValid)
            {
                EmailHelper emh = new EmailHelper(_dbcontext);
                return (IActionResult)Ok(await emh.UpdateEmailInfo(newemail, emailaddress, password));
            }
            else
            {
                return (IActionResult)BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Deletes Email credentials in DB based on given email address and password
        /// </summary>
        /// <param name="emailaddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteEmail(string emailaddress, string password)
        {
            if (ModelState.IsValid)
            {
                EmailHelper emh = new EmailHelper(_dbcontext);
                return (IActionResult)Ok(await emh.DeleteEmailInfo(emailaddress, password));
            }
            else
            {
                return (IActionResult)BadRequest(ModelState);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbcontext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
