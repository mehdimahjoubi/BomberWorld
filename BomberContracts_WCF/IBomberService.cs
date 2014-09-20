using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace BomberContracts_WCF
{
    [ServiceContract(Namespace = "MahoubiMehdi/Apps/BomberService",
        CallbackContract = typeof(IBomberCallback),
        SessionMode = SessionMode.Required)]
    public interface IBomberService
    {
        [OperationContract(IsInitiating = true, IsTerminating = false, IsOneWay = true)]
        void StartSession(AccountDC a);

        [OperationContract(IsInitiating = false, IsTerminating = true, IsOneWay = true)]
        void TerminateSession();

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void SendMessage(int targetPlayerID, string msg);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void RequestFriends();

        [OperationContract(IsInitiating = true, IsTerminating = false, IsOneWay = true)]
        void CreateAccount(AccountDC newAccount, PlayerDC player);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void SearchFriends(PlayerDC refPlayer);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void SendFriendshipRequests(IEnumerable<PlayerDC> players);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void RefuseFriendshipRequest(PlayerDC playerDC);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void AcceptFriendshipRequest(PlayerDC playerDC);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void RemoveFriend(PlayerDC friend);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void SaveProfil(PlayerDC profil);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void CreateConversation(ConversationDC newConv);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void SendConversationMessage(ConversationDC msgContainer);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void DeleteConversation(ConversationDC Party);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void LeaveConversation(ConversationDC conversationDC);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void SwitchMainConversation(ConversationDC conversationDC);



        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void ReplicatePlayerPosition(int convID, string playerLogin, int posX, int posY);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void ReplicateSpawnedBombPosition(int convID, int posX, int posY);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void ReplicatePlayerDeath(int convID, string playerLogin);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void StartGame(int convID);
    }
}
