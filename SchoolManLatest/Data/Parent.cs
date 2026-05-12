using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class Parent:BaseReference
    {
        private tb_Parent parent;
        public Parent(tb_Parent obj) { parent = obj; }
        public Parent(long id) { parent = _Entities.tb_Parent.FirstOrDefault(z => z.ParentId == id); }
        public long ParentId { get { return parent.ParentId; } }
        public string ParentName { get { return parent.ParentName; } }
        public string Address { get { return parent.Address; } }
        public string City { get { return parent.City; } }
        public string Email { get { return parent.Email; } }
        public string ContactNumber { get { return parent.ContactNumber; } }
        public string Password { get { return parent.Password; } }
        public System.DateTime TimeStamp { get { return parent.TimeStamp; } }
        public System.Guid ParentGuid { get { return parent.ParentGuid; } }
        public bool IsActive { get { return parent.IsActive; } }
        public string State { get { return parent.State; } }
        public string FilePath { get { return parent.FilePath; } }
        public string MotherName { get { return parent.MotherName; } }
        public List<Student> GetChildren()
        {
            var data = _Entities.tb_Student.Where(x => x.ParentId == ParentId && x.IsActive).ToList().Select(x => new Student(x)).ToList();
            return data;
        }
    }
}
