using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NewtonLibraryBino5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NewtonLibraryBino5.Data
{
    internal class DataAccess
    {
        public enum BookTitles
        {
            [Description("Alibaba And Aladin")] AAA, [Description("Lord of the rings")] Lotr, [Description("Assassins creed")] AC,
            [Description("Game Of Thrones")] GOT, [Description("Final fantasy")] FF, [Description("Lucifer stripper")] LS, Halo,
            [Description("The stripper PZ")] TSP, [Description("Stripper in da club")] SIDC, [Description("The Road to breast")] TRTB, [Description("Ass univers")] AU, [Description("The Hobbit")] TH,
            [Description("Sex in the kitchen")] SITK, [Description("69 style of ...")] ST, [Description("Live at pornhub")] LAP, [Description("Story of Lisa Ann")] SOLA, [Description("Step mom")] SM,
        }

        public csSeedGenerator rnd = new csSeedGenerator();
        public void Seed() 
        {

            using ( var context = new Context() ) 
            {
                
                for (var i = 0; i <= 10; i++) 
                {
                    Customer customer = new Customer();
                    Author author = new Author();
                    Book book = new Book();
                    LoanCard loanCard = new LoanCard();

                    customer.FirstName = rnd.FirstName;
                    customer.LastName = rnd.LastName;

                    author.Name = rnd.FullName;

                    book.BookTitle = GetEnumDescription(rnd.FromEnum<BookTitles>());

                    context.Books.Add(book);
                    context.Customers.Add(customer);
                    context.Authors.Add(author);
                    context.LoanCards.Add(loanCard);

                }
                //context.SaveChanges(); Den här methoden gör att all i for-loopen spara i Databasen.
                context.SaveChanges();

            }


        }

        public void BookNotLoaned(int bookId) 
        {
            using (var context = new Context()) 
            {
             var book = context.Books.Include(b => b.LoanCards).FirstOrDefault(b => b.LoancardID == bookId);
                if (book != null) 
                {
                    book.LoanCards = null;

                    if (book.LoanCards != null)
                    {
                        book.LoanCards.books.Remove(book);
                        book.BorrowingDate = null;
                        book.DueDate = null;
                        book.Available = false;
                    }
                }
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
