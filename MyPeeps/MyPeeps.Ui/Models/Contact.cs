// - Required Assemblies

// - Application Assemblies

namespace MyPeeps.Ui.Models
{
  public class Contact
  {
    #region Properties

    public int ContactId { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public int PhoneBookId { get; set; }

    #endregion

    #region Construct

    public Contact()
    {
      ContactId = 0;
      Name = string.Empty;
      Number = string.Empty;
      PhoneBookId = -1;
    }

    public Contact(int contactId, string name, string number, int phoneBookId)
    {
      ContactId   = 0;
      Name        = string.Empty;
      Number      = string.Empty;
      PhoneBookId = -1;
    }

    #endregion
  }
}
