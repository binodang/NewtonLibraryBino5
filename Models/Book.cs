using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewtonLibraryBino5.Models
{
    internal class Book
    {
        public int BookID { get; set; }
        public string? BookTitle { get; set; }
        public Guid ISBN { get; set; } = new Guid();
        public bool Available { get; set; }
        public DateTime BorrowingDate { get; set; }
        public DateTime DueDate { get; set; }
        public int PublicationYear { get; set; } = new Random().Next(1900, 2023);
        public int Rating { get; set; } = new Random().Next(1, 5);


        public ICollection<Author>? Authors { get; set; }

        public int? LoancardID { get; set; }

        public LoanCard? LoanCards { get; set; }
    }
}
