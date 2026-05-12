using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class Trip : BaseReference
    {
        private tb_Trip trip;
        public Trip(tb_Trip obj) { trip = obj; }
        public Trip(long id) { trip = _Entities.tb_Trip.FirstOrDefault(z => z.TripId == id); }
        public long TripId { get { return trip.TripId; } }
        public long DriverId { get { return trip.DriverId; } }
        public long SchoolId { get { return trip.SchoolId; } }
        public string TripNumber { get { return trip.TripNo; } }
        public string LocationStart { get { return trip.FromLocation; } }
        public string LocationEnd { get { return trip.ToLocation; } }
        public System.DateTime StartTime { get { return trip.StartTime; } }
        public System.DateTime? ReachTime { get { return trip.ReachTime; } }
        public System.DateTime TimeStamp { get { return trip.TimeStamp; } }

        public System.Guid TripGuid { get { return trip.TripGuid; } }
        public bool IsActive { get { return trip.IsActive; } }
        public int TravellingStatus { get { return trip.TravellingStatus; } }
        public long BusId { get { return trip.BusId; } }
        public string BusName { get { return trip.tb_Bus.BusName; } }
        public Nullable<int> ShiftStatus { get { return trip.ShiftStatus; } }
        public string DriverName { get { return trip.tb_Driver.DriverName; } }

        public string DriverNumber { get { return trip.tb_Driver.ContactNumber; } }
        public string DriverProfile { get { return trip.tb_Driver.FilePath; } }
        public Driver Driver { get { return new Data.Driver(trip.tb_Driver); } }
        public List<Travel> Travel { get { return trip.tb_Travel.ToList().Select(z => new Travel(z)).ToList(); } }

        public string LastLocation
        {
            get
            {
                if (trip.tb_Travel.ToList().Count > 0)
                {
                    return trip.tb_Travel.ToList().OrderByDescending(z => z.TravelId).FirstOrDefault().Place;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}

