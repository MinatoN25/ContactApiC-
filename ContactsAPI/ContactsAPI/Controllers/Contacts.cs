using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsApiDBContext dbContext;

        public ContactsController(ContactsApiDBContext dBContext)
        {
            this.dbContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());

        }
        
        [HttpGet]
        [Route("/jenkins")]
        public ActionResult<string> DeployedViaJenkins()
        {
            return Ok("Deployed via jenkins");
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone,
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact.Id);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContract([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;

                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return BadRequest("Your contact doesn't exist");
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContactById([FromRoute] Guid id)
        {
            if (id ==Guid.Empty)
            {
                return BadRequest("Enter a contact id");
            }
            else
            {
                var contact = await dbContext.Contacts.FindAsync(id);
                if (contact != null)
                {
                    return Ok(contact);
                }
                return BadRequest("Contact Not exist");
            }
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContactById([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Please enter contact id");
            }
            else
            {
                var contact = await dbContext.Contacts.FindAsync(id);

                if (contact != null)
                {
                    dbContext.Remove(contact);
                    await dbContext.SaveChangesAsync();
                    return Ok("Deleted");
                }
                return BadRequest("Your contact does not exist");
            }
        }






    }
}
