using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class State:BaseReference
    {
        private tb_State state;
        public State(tb_State obj) { state = obj; }
        public State(long id) { state = _Entities.tb_State.FirstOrDefault(z => z.StateId == id); }
        public long StateId { get { return state.StateId; } }
        public string StateName { get{ return state.State; }}
        public System.Guid StateGuid { get { return state.StateGuid; }}
        public bool IsActive { get { return state.IsActive; }}
    }
}
