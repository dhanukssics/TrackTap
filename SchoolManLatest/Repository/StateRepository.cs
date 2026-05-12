using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTap.Data;

namespace TrackTap.Repository
{
    public class StateRepository
    {
        public tb_tracktapEntities _Entity = new tb_tracktapEntities();

        public Tuple<bool, string, List<State>> AllStateList()
        {
            var status = false;
            var msg = "failed";
            //var stateData = _Entity.tb_State.Where(x => x.IsActive == true).ToList().Select(x => new State(x)).ToList();
            var stateData = StaticData.fetchState();
            if(stateData.Count>0)
            {
                status = true;
                msg = "Success";
                return new Tuple<bool, string, List<State>>(status, msg, stateData);
            }
            else
            {
                status = false;
                msg = "failed";
                return new Tuple<bool, string, List<State>>(status, msg, stateData);
            }
        }
    }
}
