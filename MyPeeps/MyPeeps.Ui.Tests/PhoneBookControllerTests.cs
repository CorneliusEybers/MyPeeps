// - Required Assemblies

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// - Application Assemblies
using MyPeeps.Ui.Controllers;
using MyPeeps.Ui.Models;
using MyPeeps.Ui.Repositories;

namespace MyPeeps.Ui.Tests
{
  [TestClass]
  public class PhoneBookControllerTests
  {
    public class Resources
    {
      #region Properties

      public IPhoneBookRepository PhoneBookRepository { get; set; }

      public PhoneBookController Controller { get; set; }

      #endregion

      #region Construct

      public Resources()
      {
        PhoneBookRepository = new PhoneBookRepositoryMock();
        Controller = new PhoneBookController(PhoneBookRepository);
      }

      #endregion
    }

    [TestMethod]
    public void GetPhoneBook_Success()
    {
      // - Given
      var resources = new Resources();
      var phoneBookId = 4;
      var phoneBookExpected = resources.PhoneBookRepository.ReadPhoneBook(phoneBookId);

      // - When
      var result = resources.Controller.GetPhoneBook(phoneBookId) as OkObjectResult;

      // - Then
      Assert.IsNotNull(result);
      Assert.AreEqual(200,result.StatusCode);

      var phoneBookGot = result.Value as PhoneBook;
      Assert.IsNotNull(phoneBookGot);
      Assert.AreEqual(phoneBookExpected.PhoneBookId,phoneBookGot.PhoneBookId);
      Assert.AreEqual(phoneBookExpected.Name,phoneBookGot.Name);
      Assert.AreEqual(phoneBookExpected.Contacts.Count,phoneBookGot.Contacts.Count);
      Assert.AreEqual(phoneBookExpected.Contacts[0].ContactId,phoneBookGot.Contacts[0].ContactId);
      Assert.AreEqual(phoneBookExpected.Contacts[0].Name,phoneBookGot.Contacts[0].Name);
      Assert.AreEqual(phoneBookExpected.Contacts[0].Number,phoneBookGot.Contacts[0].Number);
    }
  }
}
