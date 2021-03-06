﻿// - Required Assemblies
using System.Collections.Generic;
using System.Linq;

// - Application Assemblies
using MyPeeps.Ui.Models;

namespace MyPeeps.Ui.Repositories
{
  public class PhoneBookRepositoryMock : IPhoneBookRepository
  {
    #region ClassVariables

    private List<PhoneBook> mc_PhoneBooks;

    private List<Contact> mc_Contacts;

    #endregion

    #region Porperties

    public List<PhoneBook> PhoneBooks
    {
      get { return mc_PhoneBooks; }
      set { mc_PhoneBooks = value; }
    }

    public List<Contact> Contacts
    {
      get { return mc_Contacts; }
      set { mc_Contacts = value; }
    }

    #endregion

    #region Construct

    public PhoneBookRepositoryMock()
    {
      mc_PhoneBooks = new List<PhoneBook>();
      mc_Contacts = new List<Contact>();

      BuildPhoneBookMock(5);
    }

    #endregion

    #region PublicMethods

    /// <summary>
    /// - Create the PhoneBook received in the repository
    /// </summary>
    /// <param name="phoneBook">
    /// - PhoneBook Object fully loaded to add to repository...
    /// </param>
    /// <returns>
    /// - Primary Key of the PhoneBook added to repository
    /// - -1 on error.
    /// </returns>
    public int CreatePhoneBook(PhoneBook phoneBook)
    {
      phoneBook.PhoneBookId = mc_PhoneBooks.Count < 1 ? 1 : mc_PhoneBooks.Max(PhnBok => PhnBok.PhoneBookId) + 1;
      
      mc_PhoneBooks.Add(phoneBook);

      foreach (var contact in phoneBook.Contacts)
      {
        contact.ContactId = mc_Contacts.Count < 1 ? 1 : mc_Contacts.Max(Cnt => Cnt.ContactId) + 1;
        mc_Contacts.Add(contact);
      }

      return phoneBook.PhoneBookId;
    }

    /// <summary>
    /// - Retrieve the PhoneBook of the ID.
    /// - Can include all the contacts(sub classes) or not
    /// - Specify Include ot not include with parameter.
    /// </summary>
    /// <returns>
    /// - Phone of the key.
    /// </returns>
    public List<PhoneBook> ReadPhoneBooks()
    {
      var phoneBooks = mc_PhoneBooks;

      return phoneBooks;
    }

    /// <summary>
    /// - Retrieve all the PhoneBooks available.
    /// - Can include all the contacts(sub classes) or not
    /// - Specify to Include ot not to include with parameter.
    /// </summary>
    /// <param name="phoneBookId">
    /// - The primary Key of the PhoneBook to retrieve
    /// </param>
    /// <returns>
    /// - List of all the PhoneBooks.
    /// </returns>
    public PhoneBook ReadPhoneBook(int phoneBookId)
    {
      var phoneBook = mc_PhoneBooks.FirstOrDefault(PhnBok => PhnBok.PhoneBookId == phoneBookId);

      return phoneBook;
    }

    /// <summary>
    /// - Update the PhoneBook received in the repository.
    /// </summary>
    /// <param name="phoneBook">
    /// - PhoneBook Object fully loaded to add to repository...
    /// </param>
    /// <returns>
    /// - True : Success
    /// - False : Failure
    /// </returns>
    public int UpdatePhoneBook(PhoneBook phoneBook)
    {
      var phoneBookExtant = mc_PhoneBooks.First(PhnBok => PhnBok.PhoneBookId == phoneBook.PhoneBookId);

      phoneBookExtant.Name = phoneBook.Name;

      foreach (var contact in phoneBook.Contacts)
      {
        if (contact.ContactId < 1)
        {
          contact.ContactId = mc_PhoneBooks.Max(PhnBok => PhnBok.PhoneBookId) + 1;
          mc_Contacts.Add(contact);
          phoneBookExtant.Contacts.Add(contact);
        }
        else
        {
          var contactExtant = phoneBookExtant.Contacts.FirstOrDefault(Cnt => Cnt.ContactId == contact.ContactId);

          contactExtant.Name = contact.Name;
          contactExtant.Number = contact.Number;
        }
      }
      
      return phoneBook.PhoneBookId;
    }

    /// <summary>
    /// - Delete the PhoneBook of the ID from the repository
    /// - Retrieve the PhoneBook before it is deleted.
    /// - Pass it bak to the subscriber to offer unto
    ///   delete by re-save again
    /// </summary>
    /// <param name="phoneBookId">
    /// - Primary Key of the PhoneBook to delete from the Repository.
    /// </param>
    /// <returns>
    /// PhoneBook Object fully loaded to the caller to offer undo
    /// functionality.
    /// </returns>
    public PhoneBook DeletePhoneBook(int phoneBookId)
    {
      var phoneBook = mc_PhoneBooks.First(PhnBok => PhnBok.PhoneBookId == phoneBookId);
      
      var phoneBookUndo = new PhoneBook()
                          {
                            PhoneBookId = -1,
                            Name = phoneBook.Name
                          };

      // - Create new instances do not work with deleted references
      foreach (var contact in phoneBook.Contacts)
      {
        var contactUndo = new Contact()
                          {
                            ContactId = -1,
                            Name = contact.Name,
                            Number = contact.Number
                          };

        phoneBookUndo.Contacts.Add(contactUndo);
      }

      // - Remove the Contacts of the PhoneBook (Cascade delete)
      foreach (var contact in phoneBook.Contacts)
      {
        mc_Contacts.Remove(contact);
      }

      mc_PhoneBooks.Remove(phoneBook);

      return phoneBookUndo;
    }

    public Contact DeleteContact(int contactId)
    {
      var contact = mc_Contacts.First(Cnt => Cnt.ContactId == contactId);
      var phoneBook = mc_PhoneBooks.First(PhnBok => PhnBok.PhoneBookId == contact.PhoneBookId);

      var contactUndo = new Contact()
                        {
                          ContactId = -1,
                          PhoneBookId = phoneBook.PhoneBookId,
                          Name = contact.Name,
                          Number = contact.Number
                        };

      mc_Contacts.Remove(contact);
      phoneBook.Contacts.Remove(contact);

      return contactUndo;
    }

    #endregion

    #region PrivateMethods

    private void BuildPhoneBookMock(int numberOfMocks)
    {
      for (var phoneBookId = 1; phoneBookId <= numberOfMocks; phoneBookId++)
      {
        var phoneBook = new PhoneBook();

        phoneBook.PhoneBookId = phoneBookId;
        phoneBook.Name = "PhoneBook" + phoneBookId.ToString("0000");

        for (var contactId = 1; contactId <= numberOfMocks; contactId++)
        {
          var contact = new Contact();

          contact.ContactId = mc_Contacts.Count < 1 ? 1 : mc_Contacts.Max(Cnt => Cnt.ContactId) + 1;
          contact.PhoneBookId = phoneBookId;
          contact.Name = "Contact" + contactId.ToString("00") + "  Book " + phoneBookId.ToString("00");
          contact.Number = "084" + contactId.ToString("000") + contactId.ToString("0000");

          mc_Contacts.Add(contact);
          phoneBook.Contacts.Add(contact);
        }

        mc_PhoneBooks.Add(phoneBook);
      }
      
    }
    #endregion
  }
}