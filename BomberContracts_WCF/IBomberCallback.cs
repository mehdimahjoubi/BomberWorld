using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace BomberContracts_WCF
{
    public interface IBomberCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveFriendsList(IEnumerable<PlayerDC> friends);

        [OperationContract(IsOneWay = true)]
        void RejectConnection();

        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(int senderPlayerID, string msg);

        [OperationContract(IsOneWay = true)]
        void RefuseSubscription();

        [OperationContract(IsOneWay = true)]
        void NotifySubscriptionSuccess();

        [OperationContract(IsOneWay = true)]
        void NotifyPlayerStatusChange(PlayerDC friend);

        [OperationContract(IsOneWay = true)]
        void DisconnectPlayer();

        [OperationContract(IsOneWay = true)]
        void ReceiveSearchFriendsResult(IEnumerable<PlayerDC> players);

        [OperationContract(IsOneWay = true)]
        void NotifyFriendshipRequest(PlayerDC playerDC);

        [OperationContract(IsOneWay = true)]
        void ReceiveProfil(PlayerDC profil, IEnumerable<PlayerDC> friendShipRequests, IEnumerable<ConversationDC> playerConvDCs);

        [OperationContract(IsOneWay = true)]
        void NotifyFriendshipRequestSuccess();

        [OperationContract(IsOneWay = true)]
        void NotifyFriendshipEstablishment(PlayerDC playerDC);

        [OperationContract(IsOneWay = true)]
        void NotifyFriendshipEnding(PlayerDC playerDC);

        [OperationContract(IsOneWay = true)]
        void NotifyFriendProfilChange(PlayerDC playerDC);

        [OperationContract(IsOneWay = true)]
        void InitializeConversation(ConversationDC conversationDC);

        [OperationContract(IsOneWay = true)]
        void ReceiveConversationMessage(ConversationDC c);

        [OperationContract(IsOneWay = true)]
        void DropConversation(ConversationDC conv);

        [OperationContract(IsOneWay = true)]
        void RemovePlayerFromParty(PlayerDC player, ConversationDC conv);



        [OperationContract(IsOneWay = true)]
        void OpenGameWindow(ConversationDC conv);

        [OperationContract(IsOneWay = true)]
        void UpdatePlayerPosition(ConversationDC conv, PlayerDC player);

        [OperationContract(IsOneWay = true)]
        void SpawnBomb(ConversationDC conv, int BombPositionX, int BombPositionY);

        [OperationContract(IsOneWay = true)]
        void KillPlayer(ConversationDC conv, PlayerDC player);
    }
}
