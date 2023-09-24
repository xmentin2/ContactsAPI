using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : Controller
{
    private readonly ContactsApiDbContext _dbContext;

    public ContactsController(ContactsApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // GET
    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
        return Ok(await _dbContext.Contacts.ToListAsync());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetContact([FromRoute] Guid id)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);
        if (contact == null)
        {
            return NotFound();
        }

        return Ok(contact);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
    {
        var contact = new Contact()
        {
            Id = Guid.NewGuid(),
            Address = addContactRequest.Address,
            Email = addContactRequest.Email,
            Phone = addContactRequest.Phone,
            FullName = addContactRequest.FullName
        };
        await _dbContext.Contacts.AddAsync(contact);
        await _dbContext.SaveChangesAsync();

        return Ok(contact);
    }
    
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);
        if (contact != null)
        {
            contact.Address = updateContactRequest.Address;
            contact.Email = updateContactRequest.Email;
            contact.FullName = updateContactRequest.FullName;
            contact.Phone = updateContactRequest.Phone;

            await _dbContext.SaveChangesAsync();
            
            return Ok(contact);
        }

        return NotFound();
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);
        if (contact != null)
        {
            _dbContext.Remove(contact);
            await _dbContext.SaveChangesAsync();

            return Ok(contact);
        }

        return NotFound();
    }
}