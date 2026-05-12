using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
   public static class StaticData
    {
       public static tb_tracktapEntities _StaticEntities = new tb_tracktapEntities();
       public static List<State> fetchState()
       {
          return  _StaticEntities.tb_State.Where(x => x.IsActive == true).ToList().Select(x => new State(x)).ToList();
       }

    }
}
