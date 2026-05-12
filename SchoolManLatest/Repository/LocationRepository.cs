using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTap.PostModel;
using TrackTap.Data;

namespace TrackTap.Repository
{
    public class LocationRepository
    {
        public tb_tracktapEntities _Entity = new tb_tracktapEntities();
        public DateTime currentTime = DateTime.UtcNow;
        public Tuple<bool, string, Travel> tracklocation(TrackStudentLocationPostModel model)
        {
            var status = true;
            string msg = "success";
            string busSpecialId = model.busSpecialId;
            var bus = _Entity.tb_Bus.Where(x => x.BusSpecialId == busSpecialId && x.IsActive == true).FirstOrDefault();
            string tripNo = model.tripNo;
            DateTime todayNow = currentTime;
            var tripData = _Entity.tb_Trip.Where(x => x.BusId == bus.BusId && x.TripNo == tripNo && x.IsActive && x.StartTime >= currentTime).FirstOrDefault();
            var travelData = _Entity.tb_Travel.Where(x => x.TripId == tripData.TripId).OrderByDescending(z => z.TravelId).ToList().Select(z=>new Travel(z)).FirstOrDefault();
            return new Tuple<bool, string, Travel>(status, msg, travelData);
        }
    }
}
