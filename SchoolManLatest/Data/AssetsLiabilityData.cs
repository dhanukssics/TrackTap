using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class AssetsLiabilityData:BaseReference
    {
        private tb_AssetsLiabilityData assetsLiabilityData;
        public AssetsLiabilityData(tb_AssetsLiabilityData obj) { assetsLiabilityData = obj; }
        public AssetsLiabilityData(long id) { assetsLiabilityData = _Entities.tb_AssetsLiabilityData.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return assetsLiabilityData.Id; } }
        public int TypeId { get { return assetsLiabilityData.TypeId; } }
        public DateTime EntryDate { get { return assetsLiabilityData.EntryDate; } }
        public string InviceNumber { get { return assetsLiabilityData.InviceNumber; } }
        public long HeadId { get { return assetsLiabilityData.HeadId; } }
        public decimal Amount { get { return assetsLiabilityData.Amount; } }
        public bool AddStatus { get { return assetsLiabilityData.AddStatus; } }
        public long SchoolId { get { return assetsLiabilityData.SchoolId; } }
        public long UserId { get { return assetsLiabilityData.UserId; } }
        public bool IsActive { get { return assetsLiabilityData.IsActive; } }
        public DateTime TimeStamp { get { return assetsLiabilityData.TimeStamp; } }
        public string Narration { get { return assetsLiabilityData.Narration; } }

    }
}
