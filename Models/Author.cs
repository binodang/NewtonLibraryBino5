using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewtonLibraryBino5.Models
{
    internal class Author
    {
        public int Id { get; set; }

        [MaxLength(100)]

        public string Name { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
