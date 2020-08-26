// - Required Assemblies
using Microsoft.EntityFrameworkCore;

// - Application Assemblies


namespace MyPeeps.Services.DataAccess
{
  public class MyPeepsDbContext : DbContext
  {
    public MyPeepsDbContext(DbContextOptions<MyPeepsDbContext> options) : base(options)
    {
      ;
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<PhoneBook> PhoneBooks { get; set; }

    // - Seed Data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // - Seed PhoneBooks
      modelBuilder.Entity<PhoneBook>().HasData(new PhoneBook{PhoneBookId = 1, Name = "PhoneBook1"});
      modelBuilder.Entity<PhoneBook>().HasData(new PhoneBook{PhoneBookId = 2, Name = "PhoneBook2"});
      modelBuilder.Entity<PhoneBook>().HasData(new PhoneBook{PhoneBookId = 3, Name = "PhoneBook3"});
      modelBuilder.Entity<PhoneBook>().HasData(new PhoneBook{PhoneBookId = 4, Name = "PhoneBook4"});
      modelBuilder.Entity<PhoneBook>().HasData(new PhoneBook{PhoneBookId = 5, Name = "PhoneBook5"});

      // - Seed Contacts
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 1, Name = "Contact1", Number = "1111111111", PhoneBookId = 1});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 2, Name = "Contact2", Number = "2222222222", PhoneBookId = 1});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 3, Name = "Contact3", Number = "3333333333", PhoneBookId = 1});

      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 4, Name = "Contact4", Number = "4444444444", PhoneBookId = 2});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 5, Name = "Contact5", Number = "5555555555", PhoneBookId = 2});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 6, Name = "Contact6", Number = "6666666666", PhoneBookId = 2});

      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 7, Name = "Contact7", Number = "7777777777", PhoneBookId = 3});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 8, Name = "Contact8", Number = "8888888888", PhoneBookId = 3});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 9, Name = "Contact9", Number = "9999999999", PhoneBookId = 3});

      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 10, Name = "Contact10", Number = "10101010", PhoneBookId = 4});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 11, Name = "Contact11", Number = "111111111", PhoneBookId = 4});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 12, Name = "Contact12", Number = "121212121", PhoneBookId = 4});

      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 13, Name = "Contact13", Number = "131313131", PhoneBookId = 5});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 14, Name = "Contact14", Number = "141414141", PhoneBookId = 5});
      modelBuilder.Entity<Contact>().HasData(new Contact{ContactId = 15, Name = "Contact15", Number = "151515151", PhoneBookId = 5});
    }
  }
}
