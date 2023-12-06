using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewtonLibraryBino5.Models
{
    internal class LoanCard
    {
        public int LoancardID { get; set; }


        public int PIN { get; set; } = new Random().Next(1000, 9999);


        public ICollection<Book>? books { get; set; }
    }
}
