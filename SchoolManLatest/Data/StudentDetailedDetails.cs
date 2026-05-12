using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
   public class StudentDetailedDetails:BaseReference
    {
        private tb_StudentDetailedDetails details;
        public StudentDetailedDetails(tb_StudentDetailedDetails obj) { details = obj; }
        public StudentDetailedDetails(long id) { details = _Entities.tb_StudentDetailedDetails.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return details.Id; } }
        public long StudentId { get { return details.StudentId; } }
        public bool IsActive { get { return details.IsActive; } }
        public System.DateTime TimeStamp { get { return details.TimeStamp; } }
        public string GuardianName { get { return details.GuardianName; } }
        public string RelationShip { get { return details.RelationShip; } }
        public string Occupation { get { return details.Occupation; } }
        public string Guardian_Address { get { return details.Guardian_Address; } }
        public string Contact_LandLine { get { return details.Contact_LandLine; } }
        public string Contact_OfficeLine { get { return details.Contact_OfficeLine; } }
        public string Previous_SchoolName { get { return details.Previous_SchoolName; } }
        public string Previous_Standard { get { return details.Previous_Standard; } }
        public Nullable<System.DateTime> Previous_DateOfAdmission { get { return details.Previous_DateOfAdmission; } }
        public Nullable<System.DateTime> Previous_DateOfLeaving { get { return details.Previous_DateOfLeaving; } }
        public string TC_Filepath { get { return details.TC_Filepath; } }
        public string TC_Number { get { return details.TC_Number; } }
        public string TC_Date { get { return details.TC_Date; } }
        public string PlaceOFBirth { get { return details.PlaceOFBirth; } }
        public string DOBCertificate_FilePath { get { return details.DOBCertificate_FilePath; } }
        public Nullable<long> ReligionId { get { return details.ReligionId; } }
        public Nullable<long> CategoryId { get { return details.CategoryId; } }
        public string Caste { get { return details.Caste; } }
        public Nullable<int> NationalityId { get { return details.NationalityId; } }
        public string MotherTongue { get { return details.MotherTongue; } }
        public string PermanentBodyMark1 { get { return details.PermanentBodyMark1; } }
        public string PermanentBodyMark2 { get { return details.PermanentBodyMark2; } }
        public string BoardingPoint { get { return details.BoardingPoint; } }
        public string KnownLanguage1 { get { return details.KnownLanguage1; } }
        public string KnownLanguage2 { get { return details.KnownLanguage2; } }
        public string Taluk { get { return details.Taluk; } }
        public string Revenue_District { get { return details.Revenue_District; } }
        public Nullable<int> PanchayatiRajSystemId { get { return details.PanchayatiRajSystemId; } }
        public string DistrictPanchayath { get { return details.DistrictPanchayath; } }
        public string BlockPanchayath { get { return details.BlockPanchayath; } }
        public Nullable<int> InstructionMediumId { get { return details.InstructionMediumId; } }
        public Nullable<int> FirstLanguagePaper1 { get { return details.FirstLanguagePaper1; } }
        public Nullable<int> FirstLanguagePaper2 { get { return details.FirstLanguagePaper2; } }
        public Nullable<int> ThirdLanguage { get { return details.ThirdLanguage; } }
        public Nullable<bool> IsVaccinated { get { return details.IsVaccinated; } }
        public Nullable<System.DateTime> VaccinatedDate { get { return details.VaccinatedDate; } }
        public Nullable<int> LearingDisabilityId { get { return details.LearingDisabilityId; } }
        public Nullable<int> EconomicalStatus { get { return details.EconomicalStatus; } }
    }
}
