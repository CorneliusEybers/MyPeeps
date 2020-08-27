// - Required Assemblies
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// - Application Assemblies
using MyPeeps.Services.DataAccess;

namespace MyPeeps.Services.Controllers
{
  [ApiController]   // - Auto Model Binding. No need for [FromBody]
  public class PhoneBookController : ControllerBase
  {
    #region ClassVariables

    protected MyPeepsDbContext mc_MyPeepsDbContext;

    #endregion

    #region Construct

    public PhoneBookController(MyPeepsDbContext myPeepsDbContext)
    {
      mc_MyPeepsDbContext = myPeepsDbContext;
    }

    #endregion

    #region PublicMethods

    [HttpGet]
    [Route("api/PhoneBook/GetPhoneBooks")]
    public async Task<ActionResult> GetPhoneBooks()
    {
      try
      {
        IEnumerable<PhoneBook> phoneBooks = await mc_MyPeepsDbContext.PhoneBooks.Include(PhnBok => PhnBok.Contacts).ToListAsync();
        return Ok(phoneBooks);
      }
      catch (Exception exception)
      {
        string message = string.Empty;
        message = "Error in PhoneBookController.GetPhoneBooks" + Environment.NewLine;
        message += exception.Message;
        return StatusCode(StatusCodes.Status500InternalServerError);
      }

    }

    [HttpGet]
    [Route("api/PhoneBook/GetPhoneBook/{phoneBookId:int}")]
    public async Task<ActionResult<PhoneBook>> GetPhoneBook(int phoneBookId)
    {
      try
      {
        PhoneBook phoneBook = await mc_MyPeepsDbContext.PhoneBooks.Include(PhnBok => PhnBok.Contacts).FirstOrDefaultAsync(PhnBok => PhnBok.PhoneBookId == phoneBookId);

        if (phoneBook == null)
        {
          return NotFound();
        }

        return Ok(phoneBook);
      }
      catch (Exception exception)
      {
        string message = string.Empty;
        message = "Error occurred in PhoneBook.GetPhoneBook." + Environment.NewLine;
        message += exception.Message;
        return StatusCode(StatusCodes.Status500InternalServerError, message);
      }
    }

    [HttpPost]
    [Route("api/PhoneBook/PostPhoneBook")]
    public async Task<ActionResult<int>> PostPhoneBook(PhoneBook phoneBook)
    {
      try
      {
        int phoneBookId = -1;

        if (phoneBook == null)
        {
          return BadRequest("Phone Book to create not specified...");
        }

        // - NEVER trust the subscriber... not even if you ARE the subscriber!!!
        var phoneBookCreated = new PhoneBook();

        phoneBookCreated.Name = phoneBook.Name;

        foreach (var contact in phoneBook.Contacts)
        {
          var contactCreated = new Contact();

          contactCreated.Name = contact.Name;
          contactCreated.Number = contact.Number;
          phoneBookCreated.Contacts.Add(contactCreated);
        }

        mc_MyPeepsDbContext.PhoneBooks.Add(phoneBookCreated);

        await mc_MyPeepsDbContext.SaveChangesAsync();

        phoneBookId = phoneBookCreated.PhoneBookId;

        return Ok(phoneBookId);
      }
      catch (Exception exception)
      {
        string message = string.Empty;
        message = "Error occurred in PhoneBook.PostPhoneBook" + Environment.NewLine;
        message += exception.Message;

        return StatusCode(StatusCodes.Status500InternalServerError, message);
      }

    }

    [HttpPut]
    [Route("api/PhoneBook/PutPhoneBook")]
    public async Task<ActionResult<int>> PutPhoneBook(PhoneBook phoneBook)
    {
      try
      {
        int phoneBookId = -1;

        if (phoneBook == null)
        {
          return BadRequest("Phone Book to be updated was not specified.");
        }

        var phoneBookExtant = await mc_MyPeepsDbContext.PhoneBooks.Include(PhnBok => PhnBok.Contacts).FirstOrDefaultAsync(PhnBok => PhnBok.PhoneBookId == phoneBook.PhoneBookId);

        if (phoneBookExtant == null)
        {
          return BadRequest("Phone Book to be updated was not found on the database...");
        }

        // - Pass back the Primary Key of the Updated PhoneBook.
        phoneBookId = phoneBookExtant.PhoneBookId;

        // - Update the PhoneBook(Parent)
        phoneBookExtant.Name = phoneBook.Name;

        // - Delete Contacts on the DbSide
        foreach (var contactExtant in phoneBookExtant.Contacts)
        {
          if (!phoneBook.Contacts.Exists(Cnt => Cnt.ContactId == contactExtant.ContactId))
          {
            mc_MyPeepsDbContext.Contacts.Remove(contactExtant);
          }
        }

        // - Contacts of the PhoneBook
        foreach (var contact in phoneBook.Contacts)
        {
          // - Get the Database Child
          var contactExtant = phoneBookExtant.Contacts.FirstOrDefault(Ext => Ext.ContactId == contact.ContactId);

          if (contactExtant == null)
          {
            // - Contact does not exit, insert.
            // - I like to use a clean object,
            //   because subscribers may pass strange data
            var contactNew = new Contact();

            contactNew.Name = contact.Name;
            contactNew.Number = contact.Number;
            contactNew.PhoneBookId = phoneBookExtant.PhoneBookId;

            phoneBookExtant.Contacts.Add(contactNew);
          }
          else
          {
            contactExtant.Name = contact.Name;
            contactExtant.Number = contact.Number;
          }
        }

        await mc_MyPeepsDbContext.SaveChangesAsync();

        return Ok(phoneBookId);
      }
      catch (Exception exception)
      {
        string message = "Error occurred in PutPhoneBook. " + Environment.NewLine;
        message += exception.Message;

        return StatusCode(StatusCodes.Status500InternalServerError, message);
      }
    }

    [HttpDelete]
    [Route("api/PhoneBook/DeletePhoneBook/{phoneBookId:int}")]
    public async Task<ActionResult<PhoneBook>> DeletePhoneBook(int phoneBookId)
    {
      try
      {
        var phoneBookDelete = await mc_MyPeepsDbContext.PhoneBooks.Include(PhnBok => PhnBok.Contacts).FirstOrDefaultAsync(PhnBok => PhnBok.PhoneBookId == phoneBookId);

        if (phoneBookDelete == null)
        {
          return NotFound("PhoneBook to Delete not found on the database... ");
        }

        // - Create a new object with the same values.
        // - This object will be passed back to the caller
        //   and can be used to undo delete...
        var phoneBookUndo = new PhoneBook();
        phoneBookUndo.Name = phoneBookDelete.Name;

        foreach (var contactDelete in phoneBookDelete.Contacts)
        {
          var contactUndo = new Contact();

          contactUndo.Name = contactDelete.Name;
          contactUndo.Number = contactDelete.Number;

          phoneBookUndo.Contacts.Add(contactUndo);
        }

        mc_MyPeepsDbContext.PhoneBooks.Remove(phoneBookDelete);
        await mc_MyPeepsDbContext.SaveChangesAsync();

        return Ok(phoneBookUndo);

      }
      catch (Exception exception)
      {
        string message = "Error occured in PhoneBook.DeletePhoneBook" + Environment.NewLine;
        message += exception.Message;

        return StatusCode(StatusCodes.Status500InternalServerError, message);
      }
    }

    [HttpDelete]
    [Route("api/PhoneBook/DeleteContact/{contactId:int}")]
    public async Task<ActionResult<Contact>> DeleteContact(int contactId)
    {
      try
      {
        var contactDelete = await mc_MyPeepsDbContext.Contacts.FirstOrDefaultAsync(Cnt => Cnt.ContactId == contactId);

        if (contactDelete == null)
        {
          NotFound("Contact to delete not found on database...");
        }

        // - Build the undo Contact for the return
        var contactUndo = new Contact();
        contactUndo.Name = contactDelete.Name;
        contactUndo.Number = contactDelete.Number;
        contactUndo.PhoneBookId = contactDelete.PhoneBookId;

        // - Get it done...
        mc_MyPeepsDbContext.Contacts.Remove(contactDelete);
        await mc_MyPeepsDbContext.SaveChangesAsync();

        // - Zero error point
        return Ok(contactUndo);

      }
      catch (Exception exception)
      {
        string message = "Error occurred in PhoneBook.DeleteContact." + Environment.NewLine;
        message += exception.Message;

        return StatusCode(StatusCodes.Status500InternalServerError, message);
      }
    }

    #endregion
  }
}