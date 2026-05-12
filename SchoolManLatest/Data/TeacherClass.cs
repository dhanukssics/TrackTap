using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class TeacherClass :BaseReference
    {
          private tb_TeacherClass  teacherClass;
          public TeacherClass(tb_TeacherClass obj) { teacherClass = obj; }
          public TeacherClass(long id) { teacherClass = _Entities.tb_TeacherClass.FirstOrDefault(z => z.TeacherClassId == id); }
          public long TeacherClassId { get { return teacherClass.TeacherClassId; } }
          public long TeacherId { get { return teacherClass.TeacherId; } }
          public long ClassId { get { return teacherClass.ClassId; } }
          public long DivisionId { get { return teacherClass.DivisionId; } }
          public string ClassName { get { return teacherClass.tb_Class.Class; } }
          public string DivisionName { get { return teacherClass.tb_Division.Division; } }

    }
}
