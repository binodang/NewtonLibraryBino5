using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NewtonLibraryBino5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


namespace NewtonLibraryBino5.Data
{
    internal class DataAccess
    {
        public enum BookTitles
        {
            [Description("One Piece")] OP, [Description("Lord of the rings")] Lotr, [Description("Assassins creed")] AC,
            [Description("Game Of Thrones")] GOT, [Description("Final fantasy")] FF, [Description("Lucifer stripper")] LS, Halo,
            [Description("Naruto")] NO, [Description("Dragon ball")] DB, [Description("Hunter x Hunter")] HxH, [Description("Solo leveling")] SL, [Description("The Hobbit")] TH,
            [Description("Bleach")] BL, [Description("Death note")] DN, [Description("Hanma baki")] HB, [Description("Slam dunk")] SD, [Description("Rave master")] RM,
        }

        public csSeedGenerator rnd = new csSeedGenerator();
        public void Seed() 
        {

            using ( var context = new Context() ) 
            {
                
                for (var i = 0; i <= 10; i++) 
                {
                    // Skapa nya instanser av Customer, Author, Book och LoanCard.
                    Customer customer = new Customer();
                    Author author = new Author();
                    Book book = new Book();
                    LoanCard loanCard = new LoanCard();

                    // Skapa slumpmässiga namn för kunden.
                    customer.FirstName = rnd.FirstName;
                    customer.LastName = rnd.LastName;

                    // Skapa ett slumpmässigt fullständigt namn för författaren
                    author.Name = rnd.FullName;

                    book.BookTitle = GetEnumDescription(rnd.FromEnum<BookTitles>());

                    // Lägg till de nya objekten i databaskontexten.
                    context.Books.Add(book);
                    context.Customers.Add(customer);
                    context.Authors.Add(author);
                    context.LoanCards.Add(loanCard);

                }
                //context.SaveChanges(); Den här methoden gör att all i for-loopen spara i Databasen.
                context.SaveChanges();

            }


        }



      

        public void CustomerLoanCard(int customerId)
        {
            using (var context = new Context())
            {
                // Hämta kunden med det angivna customerId från databasen.
                var customer = context.Customers.Find(customerId);

                // Kontrollera om kunden inte hittades
                if (customer == null)
                {
                    Console.WriteLine("Cannot find Customer ID! ");
                    return;

                }

                // Skapa ett nytt LoanCard-objekt
                var LoanCard = new LoanCard();

                // Kopplar det nya LoanCard-objektet till kunden genom att lägga det i LoanCards.
                customer.LoanCards = LoanCard;

                context.SaveChanges();
            }


        }
        public void RemoveCustomerLoanCard(int customerloancardId)
        {
            using (var context = new Context())
            {
                // Hämta kunden med lånekorten från databasen
                var removeCustomer = context.Customers.Include(b => b.LoanCards).FirstOrDefault(b => b.CustomerID == customerloancardId);
                if (removeCustomer != null)
                {
                    // Hämta lånekorts-ID från kundens lånekort
                    var CustomerId = removeCustomer.LoanCards.LoancardID;

                    // Ta bort kunden från databasen
                    context.Customers.Remove(removeCustomer);

                    // Hämta boken från databasen baserat på lånekorts-ID
                    var book = context.Books.SingleOrDefault(b => b.LoancardID == CustomerId);

                    if (book != null)
                    {
                        book.Available = false;
                        book.BorrowingDate = null;
                        book.DueDate = null;
                        book.LoanCards = null;
                        book.LoancardID = null;
                    }
                    else 
                    {
                        Console.WriteLine("Books ID cannot found! ");
                    }
                    
                }
                context.SaveChanges();

            }
        }


        public void AddCustomer()
        { 
           using(var context = new Context())
           {
              

                var customer = new Customer
                {
                  
                   FirstName = rnd.FirstName,
                   LastName = rnd.LastName,
                   
                };

                context.Customers.Add(customer);

                context.SaveChanges();

            }


        }
        public void RemoveCustomer(int customerId)
        {
            using (var context = new Context())
            {
                var removeCustomer = context.Customers.Find(customerId);

                if (removeCustomer != null)
                { 
                 context.Customers.Remove(removeCustomer);


                 context.SaveChanges();

                 Console.WriteLine("Removed customer done! ");
                }
            }
            
            
            
        }


        public void AddAuthor()
        {
            using (var context = new Context())
            {
                var author = new Author
                {
                    Name = rnd.FullName 
                };

                context.Authors.Add(author);

                context.SaveChanges();


            }
        }
        public void RemoveAuthor(int authorId)
        {
            using (var context = new Context())
            {
                var removeAuthor = context.Authors.Find(authorId);

                if (removeAuthor != null)
                {
                    context.Authors.Remove(removeAuthor);

                    context.SaveChanges();

                    Console.WriteLine("Removed Author done! ");
                }
            }
        }


        public void AddBook()
        {
            
            using (var context = new Context())
            {
                int i = rnd.Next(0, context.Authors.Count());
                var book = new Book
                {
                    BookTitle = GetEnumDescription(rnd.FromEnum<BookTitles>()),
                    Available = true

                };

                var randomAuthor = context.Authors.Find(i);
       
                if (randomAuthor != null)
                {
                    book.Authors = new List<Author> { randomAuthor };
                }
                else
                {
                    Console.WriteLine("Unable to generate book entry without author information.");

                    return;
                }

                context.Books.Add(book);

                context.SaveChanges();

            }
        }

        public void RemoveBook(int bookId)
        {
            using (var context = new Context())
            {
                var removeBook = context.Books.Include(b => b.LoanCards).FirstOrDefault(b => b.BookID == bookId);
                if (removeBook != null)
                {
                    
                    if (removeBook.LoanCards != null)
                    {
                        removeBook.LoanCards.books.Remove(removeBook);
                    }

                    
                    context.Books.Remove(removeBook);

                    context.SaveChanges();

                    
                }
                
            }
        }

        public void BookLoanToCustomer(int bookId, int customerId)
        {
            using (var context = new Context())
            {
                
                var book = context.Books.Find(bookId);
                var customer = context.Customers.Include(b => b.LoanCards).SingleOrDefault(b => b.CustomerID == customerId);

                if (book != null && customer != null && book.Available && customer.LoanCards != null)
                {
                    
                    book.LoancardID = customer.LoanCards.LoancardID;
                    book.Available = false;
                    book.BorrowingDate = DateTime.Now;
                    book.DueDate = DateTime.Now.AddDays(14); 

                    
                    context.SaveChanges();

                    Console.WriteLine($"Book {bookId} loaned to customer {customerId} successfully.");
                }
                else
                {
                    Console.WriteLine($"Book {bookId} not available or customer with ID {customerId} not found.");
                }
            }
        }

        public void ReturnBook(int bookId)
        {
            using (var context = new Context())
            {
               
               
                var book = context.Books.Include(b => b.LoanCards).SingleOrDefault(b => b.BookID == bookId);

                if (book != null && !book.Available)
                {
                   
                    book.LoanCards = null;
                    book.Available = true;
                    book.BorrowingDate = null;
                    book.DueDate = null;

                    
                    context.SaveChanges();

                    Console.WriteLine($"Book {bookId} returned successfully.");
                }
                else
                {
                    Console.WriteLine($"Book {bookId} not loaned or not found.");
                }
            }
        }

        public void Menu()
        {
            while (true)
            {
                
               
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Remove Book");
                Console.WriteLine("3. Add Customer");
                Console.WriteLine("4. Remove Customer");
                Console.WriteLine("5. Add Author");
                Console.WriteLine("6. Remove Author");
                Console.WriteLine("7. Give Loan Card to Customer");
                Console.WriteLine("8. Delete Loan Card from Customer");
                Console.WriteLine("9. Loan Book to Customer");
                Console.WriteLine("10. Return Book");
                Console.WriteLine("11. Clear Data");
                Console.WriteLine("0. Exit");

                Console.Write("Enter your choice (0-11): ");
                string input = Console.ReadLine();

                switch (input)
                {
                   
                    case "1":
                        AddBook();
                        Console.Write("Add book is done: ");
                        break;
                    case "2":
                        Console.Write("Enter Book ID to remove: ");
                        int bookIdToRemove = int.Parse(Console.ReadLine());
                        RemoveBook(bookIdToRemove);
                        break;
                    case "3":
                        AddCustomer();
                        break;
                    case "4":
                        Console.Write("Enter Customer ID to remove: ");
                        int customerIdToRemove = int.Parse(Console.ReadLine());
                        RemoveCustomer(customerIdToRemove);
                        break;
                    case "5":
                        AddAuthor();
                        break;
                    case "6":
                        Console.Write("Enter Author ID to remove: ");
                        int authorIdToRemove = int.Parse(Console.ReadLine());
                        RemoveAuthor(authorIdToRemove);
                        break;
                    case "7":
                        Console.Write("Enter Customer ID to give a loan card: ");
                        int giveLoanCardCustomerId = int.Parse(Console.ReadLine());
                        CustomerLoanCard(giveLoanCardCustomerId);
                        break;
                    case "8":
                        Console.Write("Enter Customer ID to delete a loan card: ");
                        int deleteLoanCardCustomerId = int.Parse(Console.ReadLine());
                        RemoveCustomerLoanCard(deleteLoanCardCustomerId);
                        break;
                    case "9":
                        Console.Write("Enter Book ID to loan: ");
                        int loanBookId = int.Parse(Console.ReadLine());
                        Console.Write("Enter Customer ID: ");
                        int loanCustomerID = int.Parse(Console.ReadLine());
                        BookLoanToCustomer(loanBookId, loanCustomerID);
                        break;
                    case "10":
                        Console.Write("Enter Book ID to return: ");
                        int returnBookId = int.Parse(Console.ReadLine());
                        ReturnBook(returnBookId);
                        break;
                    case "11":
                        Clear();
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 0 and 11.");
                        break;
                }

               
            }
        }
    


        public void Clear()
        {
            using (var context = new Context())
            {
                var allcustomers = context.Customers.ToList();
                context.Customers.RemoveRange(allcustomers);
                var allBooks = context.Books.ToList();
                context.Books.RemoveRange(allBooks);
                var allAuthors = context.Authors.ToList();
                context.Authors.RemoveRange(allAuthors);
                var allLoanCards = context.LoanCards.ToList();
                context.RemoveRange(allLoanCards);

                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Customers', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Books', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Authors', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('LoanCards', RESEED, 0)");

                context.SaveChanges();
            }
        }


        private string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return value.ToString();
        }
    }
}
