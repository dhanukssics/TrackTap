using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SPBalanceSheet:BaseReference
    {
        private sp_BalanceSheet_Result balanceSheet;
        public SPBalanceSheet(sp_BalanceSheet_Result obj) { balanceSheet = obj; }
        public int TypeId { get { return balanceSheet.TypeId; } }
        public string Head { get { return balanceSheet.Head; } }
        public string Assets { get { return balanceSheet.Assets; } }
        public Nullable<decimal> AssetsAmount { get { return balanceSheet.AssetsAmount; } }
        public string Liability { get { return balanceSheet.Liability; } }
        public Nullable<decimal> LiabilityAmount { get { return balanceSheet.LiabilityAmount; } }
    }
}
