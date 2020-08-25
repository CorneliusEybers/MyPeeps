/* Standards and Conventions:                                         */
/* ==========================                                         */
/* - Global Variable Names in Pascal-Case (first letter Capitalised)  */
/* - Local Variable Names in Camel-Case (first letter LowerCase)      */
/* - All class properties entirely lower case.                        */
/*   This because json deserialize makes the property names lowercase */
/*   when the object received is set to local object.                 */
/* - Quotes standard: Double-Quote(") outter Single-Quote(') inner.   */
/* - Url Basic format: ? indicates QueryString                        */
/*                     & split Parameters                             */
/*                     Parameters maybe named Ex: id=2                */
/*                     Parameters split by = between key and value    */
/**********************************************************************/

// - Global
var CurrentPhoneBooks = [];   // - List of all available Phone Books
var CurrentPhoneBook = {};    // - Details of the Currently selected Phone Book
var UndoDeletePhoneBook = {}; // - When a phonebook is deleted, keep a copy here for undo.
var PageUrl = {};             // - Object contains all URL parts that need.

// - JQuery Events
$(document).ready(PageLoad());
// - I know about dynamic functions and how they work
// - When I can I prefer to use function for the sake of 
//   readability

// - Functions Main
function PageLoad() 
{
  // - Initialise the objects that maintains state...
  CurrentPhoneBook.id = -1;
  CurrentPhoneBook.name = "";
  CurrentPhoneBook.contacts = [];
  PageUrl.url = "";
  PageUrl.queryString = "";
  PageUrl.parameters = [];

  // - Parse the URL and QueryString
  PageUrlBuild();

  // - Load the all Phonebooks Available
  GetPhoneBooks();

  // - Load the current Phonebook
  var phoneBookId = 1;  // ToDo : GetQueryStringParameter("id", 1);
  GetPhoneBook(phoneBookId);

  // - Look Sharp!
  $("#contactname").focus();
}

function PageUrlBuild()
{
  // - Initialise
  var pageUrlAndQry = window.location.href;
  var queryStringIndex = pageUrlAndQry.indexOf("?");

  if (queryStringIndex === -1)
  {
    PageUrl.url = pageUrlAndQry;
  }
  else
  {
    PageUrl.url = pageUrlAndQry.substr(0, queryStringIndex);
    PageUrl.queryString = pageUrlAndQry.substr(queryStringIndex + 1, (pageUrlAndQry.length - queryStringIndex) - 1);
    PageUrl.parameters = PageUrl.queryString.split("&");
  }
}

function GetQueryStringParameter(parameterName, parameterPosition)
{
  var parameterValue = -1;

  if (PageUrl.parameters.length > 0)
  {
    var parameterIndex = 0;
    var parameterFound = false;

    // - Find parameter name
    for (parameterIndex = 0; parameterIndex < PageUrl.parameters.length; parameterIndex++)
    {
      if (PageUrl.parameters[parameterIndex].includes(parameterName))
      {
        parameterFound = true;
        break;
      }
    }

    if (!parameterFound) 
    {
      parameterIndex = -1;
    }

    if (parameterIndex === -1)
    {
      // - No Parameter Name exist, find parameter value by position;
      parameterValue = PageUrl.parameters[parameterPosition];
    }
    else
    {
      var parameterString = PageUrl.parameters[parameterIndex];
      var parameterArray = parameterString.split("=");
      parameterValue = parameterArray[1];
    }
  }

  return parameterValue;
}

// - Functions Data
function GetPhoneBook(phoneBookId)
{
  // - Initialise
  CurrentPhoneBook.id = phoneBookId;
  CurrentPhoneBook.name = "";
  CurrentPhoneBook.contacts = [];

  // - Web Server Call to get current PhoneBook
  $.ajax({
    type: "GET",
    dataType: "json",
    url: "PhoneBook/GetPhoneBook/" + phoneBookId,
    success: function (result)
    {
      CurrentPhoneBook = result;
      ShowContacts();
      ContactsDraw("", "");
    },
    error: function () 
    {
      console.error("Error in function GetPhoneBook()...");
    }
  });
}

function GetPhoneBooks() 
{
  // - Initialise
  CurrentPhoneBooks = [];

  // - Web Server Call to get list of PhoneBooks
  $.ajax(
    {
      type: "GET",
      datatype: "json",
      url: "PhoneBook/GetPhoneBooks",
      success: function (result) 
      {
        CurrentPhoneBooks = result;
        PhoneBooksDraw("");
        PhoneBooksDrawSelection();
      },
      error: function () 
      {
        console.error("Error in function GetPhoneBooks()...");
      }
    });
}

function PostPhoneBook(phoneBook)
{
  // - WebServer Call to save the PhoneBook
  // - Update and Insert handled together
  $.ajax(
    {
      type: "POST",
      dataType: "json",
      url: "PhoneBook/PostPhoneBook/",
      data: phoneBook,
      success: function (result) 
      {
        if (!result) 
        {
          console.error("Changes to the Phone book not saved successfully...");
        };
      },
      error: function ()
      {
        console.error("Error in function PostPhoneBook()");
      }
    });

  // - Refresh the object in the background.
  // - No need to disturb the user...
  GetPhoneBook();
  GetPhoneBooks();

}

function DeletePhoneBook(phoneBookId) 
{
  // - Delete it server side...
  // - ToDo: Implement Undo Delete.
  console.info("phoneBookId: " + phoneBookId);
  $.ajax(
    {
      type: "DELETE",
      dataType: "json",
      url:"PhoneBook/DeletePhoneBook/" + phoneBookId,
      data: phoneBookId,
      success: function(result) 
      {
        UndoDeletePhoneBook = result;
        console.info("Undo Delete Phone placed in local variable for undo...");
        console.info(result);
        console.info(UndoDeletePhoneBook);
      },
      error: function() 
      {
        console.error("Error in function DeletePhoneBook...");
      }
    });
}

// - Functions PhoneBook
function ShowPhoneBooks()
{
  $("#divphonebook").show();
  $("#divmain").hide();
  $("#divcontacts").hide();
  $("#phonebookname").focus();
}

function PhoneBookFind() 
{
  // - Initialise
  ResetMessage();
  var phoneBookToFind = $("#phonebookname").val();

  // - Filter the list
  PhoneBooksDraw(phoneBookToFind);

  // - Look Sharp
  $("#phonebookname").focus();
}

function PhoneBookSave(phoneBookIndex) 
{
  // - Initialise
  ResetMessage();
  var phoneBookNameToSave = $("#phonebookname").val();
  var phoneBook;

  // - Validation
  if (PhoneBookTextIsEmpty(phoneBookNameToSave)) 
  {
    return;
  }

  // - Save the PhoneBook...
  // - New insert, Existing Update
  if (phoneBookIndex == -1) 
  {
    phoneBook = {};
    phoneBook.phoneBookId = -1;
    phoneBook.name = phoneBookNameToSave;
    phoneBook.contacts = [];
    CurrentPhoneBooks.push(phoneBook);
  }
  else 
  {
    phoneBook = CurrentPhoneBooks[phoneBookIndex];
    phoneBook.name = phoneBookNameToSave;
  }

  // -To the Database!!!
  PostPhoneBook(phoneBook);

  // - Cleanup
  $("#phonebookname").val("");
  $("#btnphonebooksave").attr("onclick", "PhoneBookSave(-1)");

  // - Refresh
  PhoneBooksDraw("");

  // - Look Sharp
  $("#phonebookname").focus();
}

function PhoneBookView(phoneBookIndex) 
{
  $("#phonebookname").val(CurrentPhoneBooks[phoneBookIndex].name);
  $("#btnphonebooksave").attr("onclick", "PhoneBookSave(" + phoneBookIndex + ")");

  // - Look Sharp
  $("#phonebookname").focus();
}

function PhoneBookDelete(phoneBookIndex)
{
  // - As a form of Undo, place the deleted into the 
  //   capture area. If user wants to save it again
  //   he/she can just save...
  // - ToDo: Improve to work with object in order to preserve contacts...
  $("#phonebookname").val(CurrentPhoneBooks[phoneBookIndex].name);
  $("#btnphonebooksave").attr("onclick", "PhoneBookSave(-1)");

  // - What if we are deleting the current PhoneBook?
  if (CurrentPhoneBook.phoneBookId == CurrentPhoneBooks[phoneBookIndex].phoneBookId) 
  {
    CurrentPhoneBook.phoneBookId = -1;
    CurrentPhoneBook.name = "";
    CurrentPhoneBook.contacts = [];

    ContactsDraw();
  }

  // - Pass the PhoneBook for a silent delete on the database
  // - No refresh required.
  DeletePhoneBook(CurrentPhoneBooks[phoneBookIndex].phoneBookId);

  // - Remove the PhoneBook from the PhoneBooks
  CurrentPhoneBooks.splice(phoneBookIndex, 1);

  // - Refresh
  PhoneBooksDraw("");
  PhoneBooksDrawSelection();

  // - Look Sharp
  $("#phonebookname").focus();
}

function PhoneBookTextIsEmpty(phoneBookText) 
{
  if (IsEmptyOrSpace(phoneBookText)) 
  {
    ErrorMessage("Please specify the phonebook...");
    return true;
  }

  return false;
}

function PhoneBooksDraw(filter) 
{
  var $ul = $("#phonebooks").empty();

  for (var phoneBookIndex = 0; phoneBookIndex < CurrentPhoneBooks.length; phoneBookIndex++) 
  {
    var currentPhoneBook = CurrentPhoneBooks[phoneBookIndex];

    // - To Filter or not to filter.
    if (!IsEmptyOrSpace(filter))
    {
      if (!currentPhoneBook.name.includes(filter))
      {
        continue;
      }
    }

    var $li = $("<li>").html(currentPhoneBook.name).attr("id", "phonebook_" + phoneBookIndex);
    var $deleteBtn = $("<button onclick='PhoneBookDelete(" + phoneBookIndex + ")'>D</button>").appendTo($li);
    var $viewBtn = $("<button onclick='PhoneBookView(" + phoneBookIndex + ")'>V</button>").appendTo($li);

    $li.appendTo($ul);
  }
}

function PhoneBooksDrawSelection()
{
  var $select = $("#phonebookselection").empty();

  for (var phoneBookIndex = 0; phoneBookIndex < CurrentPhoneBooks.length; phoneBookIndex++) 
  {
    var currentPhoneBook = CurrentPhoneBooks[phoneBookIndex];

    $select.append("<option value='" + currentPhoneBook.phoneBookId + "'>" + currentPhoneBook.name + "</option>");
  }
}

// - Functions Contact
function ShowContacts() 
{
  $("#divmain").show();
  $("#divcontacts").show();
  $("#divphonebook").hide();
  $("#contactname").focus();

  // - Refresh the selectable PhoneBooks
  PhoneBooksDrawSelection();
}

function ContactsShowPerPhoneBook() 
{
  // - Set the PhoneBook selected.
  ContactSetCurrentPhoneBook();

  // - Show the PhoneBook selected.
  ContactsDraw("", "");
}

function ContactFind() 
{
  // - Initialise
  ResetMessage();
  var contactNameToFind = $("#contactname").val();
  var contactNumberToFind = $("#contactnumber").val();

  // - Find the Contact
  ContactsDraw(contactNameToFind, contactNumberToFind);

  // - Look sharp
  $("#contactname").focus();
}

function ContactSave(contactIndex) 
{
  // - Initialise
  ResetMessage();
  var contactNameToSave = $("#contactname").val();
  var contactNumberToSave = $("#contactnumber").val();

  // - Validate
  if (IsEmptyOrSpace(contactNameToSave) || IsEmptyOrSpace(contactNumberToSave))
  {
    ErrorMessage("In order to Save both the name and the number of the contact is required...");
    return;
  }

  // - Save the Contact...
  // - New Insert, Existing Update
  if (contactIndex == -1)
  {
    var contact = {};
    contact.ContactId = -1;
    contact.name = contactNameToSave;
    contact.number = contactNumberToSave;
    CurrentPhoneBook.contacts.push(contact);
  }
  else
  {
    CurrentPhoneBook.contacts[contactIndex].name = contactNameToSave;
    CurrentPhoneBook.contacts[contactIndex].number = contactNumberToSave;
  }

  // - To the Database
  PostPhoneBook(CurrentPhoneBook);

  // - Clean up...
  $("#contactname").val("");
  $("#contactnumber").val("");
  $("#btncontactsave").attr("onclick", "ContactSave(-1)");

  // - Refresh the Grid
  ContactsDraw("", "");

  // - Look Sharp
  $("#contactname").focus();
}

function ContactView(contactIndex)
{
  $("#contactname").val(CurrentPhoneBook.contacts[contactIndex].name);
  $("#contactnumber").val(CurrentPhoneBook.contacts[contactIndex].number);
  $("#btncontactsave").attr("onclick", "ContactSave(" + contactIndex + ")");

  // - Look Sharp
  $("#contactname").focus();
}

function ContactDelete(contactIndex)
{
  // - As a form of Undo, place the deleted into the 
  //   capture area. If user wants to save it again
  //   he/she can just save...
  $("#contactname").val(CurrentPhoneBook.contacts[contactIndex].name);
  $("#contactnumber").val(CurrentPhoneBook.contacts[contactIndex].number);
  $("#btncontactsave").attr("onclick", "ContactSave(-1)");

  // - Remove the Contact from the Contacts
  CurrentPhoneBook.contacts.splice(contactIndex, 1);

  // - Refresh
  ContactsDraw("", "");

  // - Look Sharp
  $("#contactname").focus();
}

function ContactsDraw(filterName, filterNumber)
{
  var $contacts = $("#contacts").empty();

  for (var idxContact = 0; idxContact < CurrentPhoneBook.contacts.length; idxContact++)
  {
    var currentContact = CurrentPhoneBook.contacts[idxContact];

    // - To Filter ot not to filter
    if (!IsEmptyOrSpace(filterName) || !IsEmptyOrSpace(filterNumber)) 
    {
      if (IsEmptyOrSpace(filterName)) 
      {
        if (!currentContact.number.includes(filterNumber))
        {
          continue;
        }
      }
      else if (IsEmptyOrSpace(filterNumber))
      {
        if (!currentContact.name.includes(filterName))
        {
          continue;
        }
      } else
      {
        // - For point of processing to be reached both filters must be specified
        if (!currentContact.name.includes(filterName) || !currentContact.number.includes(filterNumber))
        {
          continue;
        }
      }
    }

    var $li = $("<li>").html(currentContact.name + '(' + currentContact.number + ')').attr("id", "contact_" + idxContact);
    var $deleteBtn = $("<button onclick='ContactDelete(" + idxContact + ")'>D</button>").appendTo($li);
    var $viewBtn = $("<button onclick='ContactView(" + idxContact + ")'>V</button>").appendTo($li);
    $li.appendTo($contacts);
  }
}

function ContactSetCurrentPhoneBook()
{
  // - Initialise
  CurrentPhoneBook.id = 0;
  CurrentPhoneBook.name = "";
  CurrentPhoneBook.contacts = [];

  // - Determine which PhoneBook was selected...
  var phoneBookId = document.getElementById("phonebookselection").options[document.getElementById("phonebookselection").selectedIndex].value;

  // - Update the Global class that manage state...
  for (var idxPhoneBook = 0; idxPhoneBook < CurrentPhoneBooks.length; idxPhoneBook++) 
  {
    var checkPhoneBook = CurrentPhoneBooks[idxPhoneBook];

    if (checkPhoneBook.phoneBookId == phoneBookId) 
    {
      CurrentPhoneBook = checkPhoneBook;
    }
  }
}

// - Functions Tools
function ResetMessage() 
{
  $("#message").text("");
  $("#message").hide();
  $("#message").css({ "color": "white" });
}

function ErrorMessage(message) 
{
  $("#message").text(message);
  $("#message").show();
  $("#message").css({ "color": "red" });
}

function IsEmptyOrSpace(string) 
{
  var result = string === null || string.match(/^ *$/) !== null;

  return result;
}