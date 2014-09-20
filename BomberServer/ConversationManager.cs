using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BomberContracts_WCF;

namespace BomberServer
{
    class ConversationManager
    {
        public ConversationDC Conv { get; set; }

        /// <summary>
        /// BomberGameManager instance associated to this conversation
        /// </summary>
        public BomberGameManager Game { get; set; }

        public ConversationManager(ConversationDC conv)
        {
            Conv = conv;
        }

        #region conversation events

        public event AudioMessageEventDelegate AudioMessageEvent;
        public delegate void AudioMessageEventDelegate(ConversationDC conv);

        /// <summary>
        /// Occurs whenever a message is sent in the conversation.
        /// </summary>
        public event MessageEventDelegate MessageEvent;
        public delegate void MessageEventDelegate(ConversationDC conv);

        /// <summary>
        /// Occurs whenever a new member joins the conversation.
        /// </summary>
        public event JoinEventDelegate JoinEvent;
        public delegate void JoinEventDelegate(PlayerDC player, ConversationDC conv);

        /// <summary>
        /// Occurs whenever a member leaves the conversation.
        /// </summary>
        public event LeaveEventDelegate LeaveEvent;
        public delegate void LeaveEventDelegate(PlayerDC player, ConversationDC conv);

        /// <summary>
        /// Occurs whenever a member of the conversation is kicked out of it.
        /// </summary>
        public event KickEventDelegate KickEvent;
        public delegate void KickEventDelegate(PlayerDC player, ConversationDC conv);

        /// <summary>
        /// Occurs when the converation ends.
        /// </summary>
        public event EndOfConversationEventDelegate EndOfConvEvent;
        public delegate void EndOfConversationEventDelegate(ConversationDC conv);

        #endregion

        public void TriggerJoinEvent(PlayerDC player)
        {
            JoinEvent(player, Conv);
        }

        public void TriggerLeaveEvent(PlayerDC player)
        {
            LeaveEvent(new PlayerDC { PlayerId = player.PlayerId }, new ConversationDC { ConversationId = Conv.ConversationId });
        }

        public void TriggerMessageEvent(string msg)
        {
            Conv.ConversationLog += msg + " \n";
            Conv.NewMessage = msg;
            MessageEvent(Conv);
        }

        public void TriggerKickEvent(PlayerDC player)
        {
            KickEvent(player, Conv);
        }

        public void TriggerEndOfConvEvent()
        {
            if (EndOfConvEvent != null)
            {
                EndOfConvEvent(new ConversationDC() { ConversationId = Conv.ConversationId });
            }
        }

        public void TriggerAudioMessageEvent(byte[] buffer, int bufferSize, int msgSenderId)
        {
            AudioMessageEvent(new ConversationDC()
            {
                ConversationId = Conv.ConversationId,
                NewAudioMessageBuffer = buffer,
                NewAudioMessageBytesRecorded = bufferSize,
                NewMessageType = 1,
                MessageSenderPlayerId = msgSenderId
            });
        }
    }
}
