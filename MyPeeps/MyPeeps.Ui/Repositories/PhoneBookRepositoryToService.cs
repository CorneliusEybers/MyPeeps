// - Required Assemblies
using System.Collections.Generic;

// - Application Assemblies
using MyPeeps.Ui.Models;

namespace MyPeeps.Ui.Repositories
{
  public class PhoneBookRepositoryToService : IPhoneBookRepository
  {
    #region Methods

    /// <summary>
    /// - Retrieve all the PhoneBooks available.
    /// - Can include all the contacts(sub classes) or not
    /// - Specify to Include ot not to include with parameter.
    /// </summary>
    /// <returns>
    /// - List of all the PhoneBooks.
    /// </returns>
    public List<PhoneBook> ReadPhoneBooks()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// - Retrieve the PhoneBook of the ID.
    /// - Can include all the contacts(sub classes) or not
    /// - Specify Include ot not include with parameter.
    /// </summary>
    /// <param name="phoneBookId">
    /// - The primary Key of the PhoneBook to retrieve
    /// </param>
    /// <returns>
    /// - Phone of the key.
    /// </returns>
    public PhoneBook ReadPhoneBook(int phoneBookId)
    {
      throw new System.NotImplementedException();
    }

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
    public bool CreatePhoneBook(PhoneBook phoneBook)
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// - Update the PhoneBook received in the repository.
    /// </summary>
    /// <param name="phoneBook">
    /// - PhoneBook Object fully loaded to add to repository...
    /// </param>
    /// <returns>
    /// - Primary Key of the PhoneBook added to repository
    /// - -1 on error.
    /// </returns>
    public bool UpdatePhoneBook(PhoneBook phoneBook)
    {
      throw new System.NotImplementedException();
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
      throw new System.NotImplementedException();
    }

    #endregion
  }
}