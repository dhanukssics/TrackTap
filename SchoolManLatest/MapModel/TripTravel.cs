using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.ClassLibrary.MapModel
{
    public class TripTravel
    {
        public long TravelId { get; set; }
        public long TripId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Place { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string TimeStampString { get; set; }
        public System.Guid TravelGuid { get; set; }
    }
}
