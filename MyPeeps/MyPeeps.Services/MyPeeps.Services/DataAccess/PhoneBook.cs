// - Required Assemblies

// - Application Assemblies
using System.Collections.Generic;

namespace MyPeeps.Services.DataAccess
{
  public class PhoneBook
  {
    #region properties

    public int PhoneBookId
    {
      get; set;
    }
    public string Name
    {
      get; set;
    }
    public List<Contact> Contacts
    {
      get; set;
    }

    #endregion

    #region construct

    public PhoneBook()
    {
      PhoneBookId = 0;
      Name = string.Empty;
      Contacts = new List<Contact>();
    }

    public PhoneBook(int phoneBookId, string name, List<Contact> contacts)
    {
      PhoneBookId = phoneBookId;
      Name = name;
      Contacts = contacts;
    }

    #endregion
  }
}
