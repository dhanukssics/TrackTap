using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class BusReportModel
    {

        public long SchoolId { get; set; }
        public string BusName { get; set; }
        public int TripNumber { get; set; }

        public BusModel BusModel { get; set; }
        public TripModel TripModel { get; set; }
        public StudentsModel StudentsModel { get; set; }
        public DriverModel DriverModel { get; set; }
        public StudentClassModel StudentClassModel { get; set; }
        public DivitionModel DivitionModel { get; set; }

        public List<BusReportModel> BusReportModel_Lists { get; set; }
        public List<BusModel> BusNames_Lists { get; set; }
        public List<BusModel> Trip_Lists { get; set; }
        public List<StudentsModel> StudentsModel_Lists { get; set; }

    }

    public class DriverModel
    {
        public long DriverId { get; set; }
        public long SchoolId { get; set; }
        public string DriverSpecialId { get; set; }
        public string DriverName { get; set; }
        public string LicenseNumber { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public System.Guid DriverGuid { get; set; }
        public bool IsActive { get; set; }
        public string FilePath { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class BusModel
    {
        public long BusId { get; set; }
        public string BusSpecialId { get; set; }
        public int TripNumber { get; set; }
        public string LocationStart { get; set; }
        public string LocationEnd { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public System.Guid BusGuid { get; set; }
        public bool IsActive { get; set; }
        public string BusType { get; set; }
        public string BusName { get; set; }
        public long SchoolId { get; set; }
    }

    public class TripModel
    {
        public long TripId { get; set; }
        public long DriverId { get; set; }
        public long SchoolId { get; set; }
        public string TripNo { get; set; }
        public System.DateTime TripDate { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime ReachTime { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public System.Guid TripGuid { get; set; }
        public bool IsActive { get; set; }
        public int TravellingStatus { get; set; }
        public long BusId { get; set; }
        public Nullable<int> ShiftStatus { get; set; }
    }

    public class StudentsModel
    {
        public long StudentId { get; set; }
        public long SchoolId { get; set; }
        public string StudentSpecialId { get; set; }
        public string StundentName { get; set; }
        public string ParentName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ContactNumber { get; set; }
        public string ClasssNumber { get; set; }
        public long ClassId { get; set; }
        public long DivisionId { get; set; }
        public long BusId { get; set; }
        public string TripNo { get; set; }
        public string FilePath { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.Guid> StudentGuid { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string ParentEmail { get; set; }
        public string PostalCode { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Aadhaar { get; set; }
        public string BioNumber { get; set; }
        public Nullable<bool> IsSamrtPhoneUser { get; set; }

        public string Class { get; set; }
        public string Division { get; set; }

    }

    public class StudentClassModel
    {
        public long ClassId { get; set; }
        public long SchoolId { get; set; }
        public string Class { get; set; }
        public System.DateTime Timestamp { get; set; }
        public System.Guid ClassGuild { get; set; }
        public bool IsActive { get; set; }
        public int ClassOrder { get; set; }
        public bool PublishStatus { get; set; }
        public long AcademicYearId { get; set; }
    }

    public class DivitionModel
    {
        public long DivisionId { get; set; }
        public long ClassId { get; set; }
        public string Division { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public System.Guid DivisionGuid { get; set; }
        public bool IsActive { get; set; }
    }
}