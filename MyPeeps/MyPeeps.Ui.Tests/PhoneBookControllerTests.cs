// - Required Assemblies

using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

      /// <summary>
      /// - This is an instance of the Repository that already have data in it.
      /// - Setup in the UI project for testing purposes.
      /// </summary>
      public IPhoneBookRepository PhoneBookRepositoryMock { get;}

      /// <summary>
      /// - This is an Empty Moq repository using the Moq Framework.
      /// - You have to set it up and then use it...
      /// </summary>
      public Mock<IPhoneBookRepository> PhoneBookRepositoryMoq { get; }

      public PhoneBookController Controller { get; set; }

      #endregion

      #region Construct

      public Resources(bool moqFrameworkRepository)
      {
        // - Can use Mock Repository with data
        // - OR Moq repository that has to be setup and verified.
        PhoneBookRepositoryMock = new PhoneBookRepositoryMock();
        PhoneBookRepositoryMoq = new Mock<IPhoneBookRepository>();

        if (moqFrameworkRepository)
        {
          Controller = new PhoneBookController(PhoneBookRepositoryMoq.Object);
        }
        else
        {
          Controller = new PhoneBookController(PhoneBookRepositoryMock);
        }
      }

      #endregion
    }

    [TestMethod]
    public void GetPhoneBook_Success()
    {
      // - Given
      var resources = new Resources(false);
      var phoneBookId = 4;
      var phoneBookExpected = resources.PhoneBookRepositoryMock.ReadPhoneBook(phoneBookId);

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

    [TestMethod]
    public void GetPhoneBook_NoContacts()
    {
      // - Given
      var resources = new Resources(true);
      int phoneBookId = 4;

      // - Setup the Moq Data that will be returned
      PhoneBook phoneBookNoContacts = new PhoneBook()
                                      {
                                        PhoneBookId = phoneBookId,
                                        Name = "NoContactPhoneBook",
                                        Contacts = new EditableList<Contact>()
                                      };

      // - Setup the Moq Repository
      resources.PhoneBookRepositoryMoq.Setup(Rsp => Rsp.ReadPhoneBook(phoneBookId)).Returns(phoneBookNoContacts);

      // - When
      var result = resources.Controller.GetPhoneBook(phoneBookId) as OkObjectResult;

      // - Then
      Assert.IsNotNull(result);
      Assert.AreEqual(200, result.StatusCode);

      var phoneBookGot = result.Value as PhoneBook;
      Assert.AreEqual(phoneBookNoContacts.PhoneBookId,phoneBookGot.PhoneBookId);
      Assert.AreEqual(phoneBookNoContacts.Name,phoneBookGot.Name);
      Assert.AreEqual(0, phoneBookGot.Contacts.Count);
    }
  }
}
