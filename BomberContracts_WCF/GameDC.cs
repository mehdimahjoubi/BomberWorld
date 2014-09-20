using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BomberContracts_WCF
{
    [DataContract]
    public class GameDC
    {
        [DataMember]
        public int GameId
        {
            get;
            set;
        }
        [DataMember]
        public Nullable<int> WinnerPlayerId
        {
            get;
            set;
        }
    }
}
