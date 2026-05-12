using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class LibraryStudentBook : BaseReference
    {
        private tb_LibraryBookStudent studentBook;
        public LibraryStudentBook(tb_LibraryBookStudent obj) { studentBook = obj; }
        public LibraryStudentBook(long id) { studentBook = _Entities.tb_LibraryBookStudent.FirstOrDefault(z => z.BookId == id); }
        public long studentBookId { get { return studentBook.StudentBookId; } }
        public long bookId { get { return studentBook.BookId; } }
        public bool status { get { return studentBook.Status; } }
        public bool isActive { get { return studentBook.IsActive; } }
        public DateTime issueDateTime { get { return studentBook.IssueDateTime; } }
        public DateTime? acceptDateTime { get { return studentBook.AcceptDateTime; } }

        public Student Student { get { return new Student(studentBook.StudentId); } }

    }
}
