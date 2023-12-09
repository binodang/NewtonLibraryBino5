using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewtonLibraryBino5.Models
{
    internal class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        public string FirstName { get; set; }
        [MaxLength(70)]

        public string LastName { get; set; }
        [MaxLength(70)]

        public LoanCard? LoanCards { get; set; }
    }
}
