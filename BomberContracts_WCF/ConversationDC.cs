using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BomberContracts_WCF
{
    [DataContract]
    public class ConversationDC
    {
        [DataMember]
        public int ConversationId
        {
            get;
            set;
        }
        [DataMember]
        public string ConversationLog
        {
            get;
            set;
        }
        [DataMember]
        public int HostPlayerId
        {
            get;
            set;
        }
        [DataMember]
        public string ParticipantsNames
        {
            get;
            set;
        }
        [DataMember]
        public string NewMessage
        {
            get;
            set;
        }
        [DataMember]
        public PlayerDC Host
        {
            get;
            set;
        }
        [DataMember]
        public IEnumerable<PlayerDC> Participants
        {
            get;
            set;
        }
        /// <summary>
        /// 0 for text msg and 1 for audio msg
        /// </summary>
        [DataMember]
        public int NewMessageType { get; set; }

        [DataMember]
        public byte[] NewAudioMessageBuffer { get; set; }

        [DataMember]
        public int NewAudioMessageBytesRecorded { get; set; }
        [DataMember]
        public int MessageSenderPlayerId { get; set; }
    }
}
