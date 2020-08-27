// - Required Assemblies
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// - Application Assemblies
using MyPeeps.Services.Controllers;
using MyPeeps.Services.DataAccess;

namespace MyPeeps.Services.Tests
{
  public class Resources
  {
    #region ClassVariables

    private IConfigurationRoot mc_Configuration;
    private PhoneBookController mc_PhoneBookController;
    private MyPeepsDbContext mc_MyPeepsDbContext;

    #endregion

    #region Properties

    public PhoneBookController Controller
    {
      get
      {
        return mc_PhoneBookController;
      }
    }

    public MyPeepsDbContext DbContext
    {
      get
      {
        return mc_MyPeepsDbContext;
      }
    }

    #endregion

    #region Constructor

    public Resources()
    {
      mc_Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
      mc_MyPeepsDbContext = CreateDbContext();
      mc_PhoneBookController = new PhoneBookController(mc_MyPeepsDbContext);
    }

    #endregion

    #region Methods

    private MyPeepsDbContext CreateDbContext()
    {
      var contextBuilder = new DbContextOptionsBuilder<MyPeepsDbContext>().UseSqlServer(mc_Configuration.GetConnectionString("MyPeepsConnection"));

      return (MyPeepsDbContext)Activator.CreateInstance(typeof(MyPeepsDbContext), contextBuilder.Options);
    }

    #endregion
  }

  [TestClass]
  public class PhoneBookControllerTests
  {
    #region TestMethods

    [TestMethod]
    public void TestMethodPutPhoneBook_Success()
    {
      // - Setup
      var resources = new Resources();

      // - Given
      var phoneBook = CreateTestPhoneBook();

      // - When
      var result = resources.Controller.PutPhoneBook(phoneBook);

      // - Then

    }

    #endregion

    #region TestSetupMethods

    private PhoneBook CreateTestPhoneBook()
    {
      var phoneBook = new PhoneBook()
      {
        PhoneBookId = 3,
        Name        = "PhoneBook3",
        Contacts = new List<Contact>()
                 {
                   new Contact()
                   {
                     ContactId   = 7,
                     Name        = "Contact7 Test",
                     Number      = "77",
                     PhoneBookId = 3
                   },
                   new Contact()
                   {
                     ContactId   = 8,
                     Name        = "Contact8 Test",
                     Number      = "887",
                     PhoneBookId = 3
                   },
                   new Contact()
                   {
                     ContactId   = 9,
                     Name        = "Contact9 Test",
                     Number      = "99",
                     PhoneBookId = 3
                   },
                   new Contact()
                   {
                     ContactId   = -23,
                     Name        = "Contact20 New",
                     Number      = "29",
                     PhoneBookId = 3
                   }
                 }
      };

      return phoneBook;
    }

    #endregion
  }
}