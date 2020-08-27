// - Required Assemblies
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;

// - Application Assemblies
using MyPeeps.Ui.Models;
using Newtonsoft.Json;

namespace MyPeeps.Ui.Repositories
{
  public class PhoneBookRepositoryToService : IPhoneBookRepository, IDisposable
  {
    #region ClassVariables

    private IConfiguration mc_Configuration;
    private HttpClient mc_PhoneBookHttpClient;

    #endregion

    #region ConstructDestruct

    public PhoneBookRepositoryToService(IConfiguration configuration)
    {
      mc_Configuration = configuration;
      mc_PhoneBookHttpClient = new HttpClient();
      mc_PhoneBookHttpClient.DefaultRequestHeaders.Accept.Clear();
      mc_PhoneBookHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    ~PhoneBookRepositoryToService()
    {
      try
      {
        this.Dispose();
      }
      catch (Exception)
      {
        // - Error on Dispose? do nothing
        ;
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    private void Dispose(Boolean disposing)
    {
      if (disposing)
      {
        // - Dispose stuff
        mc_PhoneBookHttpClient.Dispose();
        mc_PhoneBookHttpClient = null;
      }
    }

    #endregion

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
      var response = mc_PhoneBookHttpClient.GetAsync("http://localhost:63149/api/phonebook/GetPhoneBooks");

      string phoneBooksJson = response.Result.Content.ReadAsStringAsync().Result;
      var phoneBooks = JsonConvert.DeserializeObject<List<PhoneBook>>(phoneBooksJson);

      return phoneBooks;
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
      var response = mc_PhoneBookHttpClient.GetAsync("http://localhost:63149/api/phonebook/GetPhoneBook/" + phoneBookId.ToString());

      // - Get the load off!!!
      string phoneBookJson = response.Result.Content.ReadAsStringAsync().Result;
      var phoneBook = JsonConvert.DeserializeObject<PhoneBook>(phoneBookJson);

      return phoneBook;
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
    public int CreatePhoneBook(PhoneBook phoneBook)
    {
      // - Get the load on!!!
      string phoneBookJson = JsonConvert.SerializeObject(phoneBook);
      var phoneBookStringContent = new StringContent(phoneBookJson, Encoding.UTF8, "application/json");

      var response = mc_PhoneBookHttpClient.PostAsync("http://localhost:63149/api/PhoneBook/PostPhoneBook", phoneBookStringContent);

      // - Get the load off!!!
      string phoneBookIdJson = response.Result.Content.ReadAsStringAsync().Result;
      var phoneBookId = JsonConvert.DeserializeObject<int>(phoneBookIdJson);

      return phoneBookId;
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
    public int UpdatePhoneBook(PhoneBook phoneBook)
    {
      // - Get the load on!!!
      string phoneBookJson          = JsonConvert.SerializeObject(phoneBook);
      var    phoneBookStringContent = new StringContent(phoneBookJson, Encoding.UTF8, "application/json");

      var response = mc_PhoneBookHttpClient.PutAsync("http://localhost:63149/api/phonebook/PutPhoneBook", phoneBookStringContent);

      // - Get the load off!!!
      string phoneBookIdJson = response.Result.Content.ReadAsStringAsync().Result;
      var phoneBookId = JsonConvert.DeserializeObject<int>(phoneBookIdJson);

      return phoneBookId;
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
      var response = mc_PhoneBookHttpClient.DeleteAsync("http://localhost:63149/api/PhoneBook/DeletePhoneBook/" + phoneBookId.ToString());

      // - Get the load off!!!
      string phoneBookJson = response.Result.Content.ReadAsStringAsync().Result;
      var phoneBook = JsonConvert.DeserializeObject<PhoneBook>(phoneBookJson);

      return phoneBook;
    }

    public Contact DeleteContact(int contactId)
    {
      var response = mc_PhoneBookHttpClient.DeleteAsync("http://localhost:63149/api/PhoneBook/DeleteContact/" + contactId.ToString());

      // - Get the load off!!!
      string contactJson = response.Result.Content.ReadAsStringAsync().Result;
      var contact = JsonConvert.DeserializeObject<Contact>(contactJson);

      return contact;
    }

    #endregion
  }
}