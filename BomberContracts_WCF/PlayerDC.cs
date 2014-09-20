using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BomberContracts_WCF
{
    [DataContract]
    public class PlayerDC
    {
        [DataMember]
        public int PlayerId
        {
            get;
            set;
        }
        [DataMember]
        public string Pseudonym
        {
            get;
            set;
        }
        [DataMember]
        public string PlayerStatus
        {
            get;
            set;
        }
        [DataMember]
        public string PlayerDescription
        {
            get;
            set;
        }
        [DataMember]
        public int NumberOfVictories
        {
            get;
            set;
        }
        [DataMember]
        public string MessagesLog
        {
            get;
            set;
        }
        [DataMember]
        public string NextMessage
        {
            get;
            set;
        }
        [DataMember]
        public string Login
        {
            get;
            set;
        }
        [DataMember]
        public bool HasNotSeenMsg
        {
            get;
            set;
        }
        [DataMember]
        public bool IsConnected
        {
            get;
            set;
        }
        [DataMember]
        public int MainRoomId
        {
            get;
            set;
        }

        [DataMember]
        public int PlayerPositionX
        {
            get;
            set;
        }
        [DataMember]
        public int PlayerPositionY
        {
            get;
            set;
        }
    }
}
