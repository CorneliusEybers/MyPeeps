// - Required Assemblies
using System.Collections.Generic;

// - Application Assemblies



namespace MyPeeps.Ui.Models
{
  public class PhoneBook
  {
    #region properties

    public int PhoneBookId { get; set; }
    public string  Name { get; set; }
    public List<Contact> Contacts { get; set; }

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
      Contacts  = contacts;
    }

    #endregion
  }
}
