// - Required Assemblies
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// - Application Assemblies
using MyPeeps.Ui.Models;
using MyPeeps.Ui.Repositories;

namespace MyPeeps.Ui.Controllers
{
  public class PhoneBookController : Controller
  {
    #region ClassVariables

    private readonly IPhoneBookRepository mc_PhoneBookRespository;

    #endregion

    #region Construct

    public PhoneBookController(IPhoneBookRepository phoneBookRepository)
    {
      mc_PhoneBookRespository = phoneBookRepository;
    }

    #endregion

    #region PublicMethods

    [HttpGet]
    [Route("PhoneBook/GetPhoneBooks")]
    public IActionResult GetPhoneBooks()
    {
      var result = mc_PhoneBookRespository.ReadPhoneBooks();

      return Ok(result);
    }

    [HttpGet]
    [Route("PhoneBook/GetPhoneBook/{phoneBookId}")]
    public IActionResult GetPhoneBook(int phoneBookId)
    {
      var result = mc_PhoneBookRespository.ReadPhoneBook(phoneBookId);

      if (result == null)
      {
        return NotFound();
      }

      return Ok(result);
    }

    [HttpPost]
    [Route("PhoneBook/PostPhoneBook")]
    public IActionResult PostPhoneBook(PhoneBook phoneBook)
    {
      int phoneBookId = -1;

      if (phoneBook.PhoneBookId < 1)
      {
        phoneBookId = mc_PhoneBookRespository.CreatePhoneBook(phoneBook);
      }
      else
      {
        phoneBookId =  mc_PhoneBookRespository.UpdatePhoneBook(phoneBook);
      }

      if (phoneBookId < 1)
      {
        return NotFound();
      }

      return Ok(phoneBookId);
    }

    [HttpDelete]
    [Route("PhoneBook/DeletePhoneBook/{phoneBookId}")]
    public IActionResult DeletePhoneBook(int phoneBookId)
    {
      var contactUndo = mc_PhoneBookRespository.DeletePhoneBook(phoneBookId);

      if (contactUndo == null)
      {
        return NotFound();
      }

      return Ok(contactUndo);
    }

    [HttpDelete]
    [Route("PhoneBook/DeleteContact/{contactId}")]
    public IActionResult DeleteContact(int contactId)
    {
      var contactUndo = mc_PhoneBookRespository.DeleteContact(contactId);

      if (contactUndo == null)
      {
        return NotFound();
      }

      return Ok(contactUndo);
    }

    #endregion

    #region Private Methods

    #endregion
  }
}
