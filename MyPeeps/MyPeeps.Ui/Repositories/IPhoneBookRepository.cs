
using System.Collections.Generic;
using MyPeeps.Ui.Models;

namespace MyPeeps.Ui.Repositories
{
  public interface IPhoneBookRepository
  {
    /// <summary>
    /// - Retrieve all the PhoneBooks available.
    /// - Can include all the contacts(sub classes) or not
    /// - Specify to Include ot not to include with parameter.
    /// </summary>
    /// <returns>
    /// - List of all the PhoneBooks.
    /// </returns>
    List<PhoneBook> ReadPhoneBooks();

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
    PhoneBook ReadPhoneBook(int phoneBookId);

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
    int CreatePhoneBook(PhoneBook phoneBook);

    /// <summary>
    /// - Update the PhoneBook received in the repository.
    /// </summary>
    /// <param name="phoneBook">
    /// - PhoneBook Object fully loaded to add to repository...
    /// </param>
    /// <returns>
    /// - Primary Key of the PhoneBook updated in repository
    /// - -1 on error.
    /// </returns>
    int UpdatePhoneBook(PhoneBook phoneBook);

    /// <summary>
    /// - Delete the PhoneBook of the ID from the repository
    /// - Retrieve the PhoneBook before it is deleted.
    /// - Pass it back to the subscriber to offer unto
    ///   delete by re-save again
    /// </summary>
    /// <param name="phoneBookId">
    /// - Primary Key of the PhoneBook to delete from the Repository.
    /// </param>
    /// <returns>
    /// PhoneBook Object fully loaded to the caller to offer undo
    /// functionality.
    /// </returns>
    PhoneBook DeletePhoneBook(int phoneBookId);

    /// <summary>
    /// - Delete the Contact specified form its PhoneBook.
    /// - Retrieve the Contact before it is deleted.
    /// - Pass it back to the subscriber to offer unto
    ///   delete by re-save again
    /// </summary>
    /// <param name="contactId">
    /// - Primary Key of Contact Object to be deleted.
    /// </param>
    /// <returns></returns>
    Contact DeleteContact(int contactId);
  }
}