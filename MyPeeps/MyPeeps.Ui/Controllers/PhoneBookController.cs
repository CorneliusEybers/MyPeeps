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

    //PhoneBook phoneBook
    [HttpPost]
    [Route("PhoneBook/PostPhoneBook")]
    public IActionResult PostPhoneBook(PhoneBook phoneBook)
    {
      bool success = false;

      if (phoneBook.PhoneBookId < 1)
      {
        success = mc_PhoneBookRespository.CreatePhoneBook(phoneBook);
      }
      else
      {
        success =  mc_PhoneBookRespository.UpdatePhoneBook(phoneBook);
      }

      return Ok(success);
    }

    #endregion

    #region Private Methods

    #endregion
  }
}
