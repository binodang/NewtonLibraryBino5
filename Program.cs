using NewtonLibraryBino5.Data;

namespace NewtonLibraryBino5
{
    internal class Program
    {
        static void Main(string[] args)
        {
           DataAccess access = new DataAccess();

            //access.Seed();

            //access.Clear();
            access.Menu();


            //access.AddBook();
            //access.RemoveBook();

            //access.CustomerLoanCard(5);
            //access.RemoveCustomerLoanCard(5);

            //access.AddCustomer();
            //access.RemoveCustomer(1);

            //access.AddAuthor();
            //access.RemoveAuthor();

            //access.BookLoanToCustomer();
            //access.ReturnBook();


        }
    }
}