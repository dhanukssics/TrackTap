using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class LibraryBook : BaseReference
    {
        private tb_LibraryBook book;
        public LibraryBook(tb_LibraryBook obj) { book = obj; }
        public LibraryBook(long id) { book = _Entities.tb_LibraryBook.FirstOrDefault(z => z.BookId == id); }
        public long bookId { get { return book.BookId; } }
        public long serialNo { get { return book.SerialNumber; } }
        public long categoryId { get { return book.CategoryId; } }
        public string title { get { return book.Title; } }
        public string author { get { return book.Author; } }
        public int status { get { return book.Status; } }
        public bool IsActive { get { return book.IsActive; } }
        public Nullable<decimal> RandomNumber { get { return book.RandomNumber; } }
        public BookCategory Category { get { return new BookCategory(book.tb_BookCategory); } }


        public List<LibraryStudentBook> GetBookStudentList()
        {
            return book.tb_LibraryBookStudent.Where(z => z.IsActive).ToList().Select(x => new LibraryStudentBook(x)).ToList();
        }
    }
}
