using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class Clubs:BaseReference
    {
        private tb_Clubs club;
        public Clubs(tb_Clubs obj) { club = obj; }
        public Clubs(long id) { club = _Entities.tb_Clubs.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return club.Id; } }
        public string ClubName { get { return club.ClubName; } }
        public bool IsActive { get { return club.IsActive; } }
        public System.DateTime TimeStamp { get { return club.TimeStamp; } }
    }
}
