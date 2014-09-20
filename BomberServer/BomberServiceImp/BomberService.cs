using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BomberRepository;
using System.Data.Entity;
using System.ServiceModel;
using BomberContracts_WCF;

namespace BomberServer.BomberServiceImp
{
    [ServiceBehavior(
    InstanceContextMode = InstanceContextMode.PerSession,
    ConcurrencyMode = ConcurrencyMode.Single)]
    class BomberService : IBomberService
    {
        public static Dictionary<int, BomberService> Sessions = new Dictionary<int, BomberService>();
        static Dictionary<int, ConversationManager> Conversations = new Dictionary<int, ConversationManager>();

        IBomberCallback Callback;
        Player Player;
        bool ConnexionDenied = false;

        #region IBomberService Members

        public void StartSession(AccountDC a)
        {
            Callback = OperationContext.Current.GetCallbackChannel<IBomberCallback>();
            using (var db = new BomberDbContext())
            {
                var query = from ac in db.Accounts
                            where ac.Login == a.Login
                            && ac.Password == a.Password
                            select ac;
                if (query.Count() == 0)
                {
                    Callback.RejectConnection();
                    ConnexionDenied = true;
                }
                else
                {
                    var p = query.ToList<Account>().ElementAt(0).Player;
                    if (Sessions.Keys.Contains(p.PlayerId))
                    {
                        p.PlayerStatus = "Offline";
                        db.SaveChanges();
                        Sessions[p.PlayerId].Callback.DisconnectPlayer();
                        Callback.DisconnectPlayer();
                        return;
                    }
                    p.PlayerStatus = "Online";
                    db.SaveChanges();
                    Sessions.Add(p.PlayerId, this);
                    this.Player = p;
                    var profil = EntityContractTranslator.PlayerToPlayerDC(p);
                    var friendShipRequests = EntityContractTranslator.PlayersToPlayerDCs(p.PlayersRequestingFriendship);
                    //notifying connected friends:
                    var connectedFriends = from f in p.Friends
                                           where Sessions.Keys.Contains(f.PlayerId)
                                           select f;
                    foreach (var f in connectedFriends)
                    {
                        var session = Sessions[f.PlayerId];
                        session.Callback.NotifyPlayerStatusChange(profil);
                    }
                    //retrieving conversations
                    List<Conversation> convs = new List<Conversation>();
                    foreach (var c in db.Conversations)
                    {
                        if (c.Host.PlayerId == p.PlayerId || (from pl in c.Players select pl.PlayerId).Contains(p.PlayerId))
                            convs.Add(c);
                    }
                    List<ConversationDC> playerConvDCs = new List<ConversationDC>();
                    if (convs.Count > 0)
                    {
                        foreach (var c in convs)
                        {
                            ConversationManager cm = null;
                            if (Conversations.Keys.Contains(c.ConversationId))
                            {
                                cm = Conversations[c.ConversationId];
                            }
                            else
                            {
                                cm = new ConversationManager(new ConversationDC()
                                {
                                    ConversationId = c.ConversationId,
                                    Host = EntityContractTranslator.PlayerToPlayerDC(c.Host),
                                    ConversationLog = c.ConversationLog,
                                    Participants = EntityContractTranslator.PlayersToPlayerDCs(c.Players),
                                    NewMessage = ""
                                });
                                Conversations.Add(cm.Conv.ConversationId, cm);
                            }
                            playerConvDCs.Add(cm.Conv);
                            cm.EndOfConvEvent += EndOfConvEventHandler;
                            cm.JoinEvent += JoinConvEventHandler;
                            cm.KickEvent += KickFromConvEventHandler;
                            cm.LeaveEvent += LeaveConvEventHandler;
                            cm.MessageEvent += ConvMessageEventHandler;
                        }
                    }
                    Callback.ReceiveProfil(profil, friendShipRequests, playerConvDCs);
                }
            }
        }

        public void CreateAccount(AccountDC newAccount, PlayerDC player)
        {
            Callback = OperationContext.Current.GetCallbackChannel<IBomberCallback>();
            using (var db = new BomberDbContext())
            {
                var p = new Player()
                {
                    Pseudonym = player.Pseudonym,
                    PlayerStatus = "Offline",
                    PlayerDescription = player.PlayerDescription
                };
                var a = new Account()
                {
                    Login = newAccount.Login,
                    Password = newAccount.Password,
                    AccountType = "normal",
                    Player = p
                };
                p.Account = a;
                db.Accounts.AddObject(a);
                db.Players.AddObject(p);
                try
                {
                    db.SaveChanges();
                    Callback.NotifySubscriptionSuccess();
                }
                catch
                {
                    Callback.RefuseSubscription();
                }
            }
        }

        public void RequestFriends()
        {
            using (var db = new BomberDbContext())
            {
                var query = (from pl in db.Players
                             where pl.PlayerId == Player.PlayerId
                             select pl).ToList<Player>();
                if (query.Count > 0)
                {
                    var p = query.ElementAt(0);
                    var friends = EntityContractTranslator.PlayersToPlayerDCs(p.Friends);
                    Callback.ReceiveFriendsList(friends);
                }
            }
        }

        public void SearchFriends(PlayerDC refPlayer)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from p in db.Players
                         where p.PlayerId == Player.PlayerId
                         select p).ToList<Player>();
                if (q.Count > 0)
                {
                    var player = q.ElementAt(0);
                    var q1 = (from a in db.Accounts
                              where a.Login.Contains(refPlayer.Login)
                              select a);
                    var players = from a in q1
                                  where a.Player.Pseudonym.Contains(refPlayer.Pseudonym)
                                  select a.Player;
                    var friendsIds = from f in player.Friends
                                     select f.PlayerId;
                    var playersRequestingFriendshipIds = from f in player.PlayersRequestingFriendship
                                                         select f.PlayerId;
                    var potentielNewFriends = from p in players
                                              where !friendsIds.Contains(p.PlayerId)
                                              && !playersRequestingFriendshipIds.Contains(p.PlayerId)
                                              && p.PlayerId != Player.PlayerId
                                              select p;
                    Callback.ReceiveSearchFriendsResult(EntityContractTranslator.PlayersToPlayerDCs(potentielNewFriends));
                }
            }
        }

        public void SendFriendshipRequests(IEnumerable<PlayerDC> players)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from p in db.Players
                         where p.PlayerId == Player.PlayerId
                         select p).ToList<Player>();
                if (q.Count > 0)
                {
                    var thisPlayer = q.ElementAt(0);
                    foreach (var p in players)
                    {
                        var q1 = (from pl in db.Players
                                  where pl.PlayerId == p.PlayerId
                                  select pl).ToList<Player>();
                        if (q1.Count > 0)
                        {
                            var player = q1.ElementAt(0);
                            var q2 = (from r in thisPlayer.FriendshipRequestedPlayers
                                      where r.PlayerId == player.PlayerId
                                      select r).ToList().Count;
                            if (q2 == 0)
                            {
                                thisPlayer.FriendshipRequestedPlayers.Add(player);
                                if (Sessions.Keys.Contains(player.PlayerId))
                                    Sessions[player.PlayerId].Callback.NotifyFriendshipRequest(EntityContractTranslator.PlayerToPlayerDC(thisPlayer));
                            }
                        }
                    }
                    db.SaveChanges();
                    Callback.NotifyFriendshipRequestSuccess();
                }
            }
        }

        public void RefuseFriendshipRequest(PlayerDC playerDC)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from p in db.Players
                         where p.PlayerId == Player.PlayerId
                         select p).ToList<Player>();
                var q1 = (from p in db.Players
                          where p.PlayerId == playerDC.PlayerId
                          select p).ToList<Player>();
                if (q.Count > 0 && q1.Count > 0)
                {
                    var thisPlayer = q.ElementAt(0);
                    var player = q1.ElementAt(0);
                    thisPlayer.PlayersRequestingFriendship.Remove(player);
                    db.SaveChanges();
                }
            }
        }

        public void AcceptFriendshipRequest(PlayerDC playerDC)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from p in db.Players
                         where p.PlayerId == Player.PlayerId
                         select p).ToList();
                var q1 = (from p in db.Players
                          where p.PlayerId == playerDC.PlayerId
                          select p).ToList();
                if (q.Count > 0 && q1.Count > 0)
                {
                    var thisPlayer = q.ElementAt(0);
                    var player = q1.ElementAt(0);
                    thisPlayer.PlayersRequestingFriendship.Remove(player);
                    if (!thisPlayer.Friends.Contains(player))
                        thisPlayer.Friends.Add(player);
                    if (!player.Friends.Contains(thisPlayer))
                        player.Friends.Add(thisPlayer);
                    db.SaveChanges();
                    Callback.NotifyFriendshipEstablishment(playerDC);
                    if (Sessions.Keys.Contains(player.PlayerId))
                        Sessions[player.PlayerId].Callback.NotifyFriendshipEstablishment(EntityContractTranslator.PlayerToPlayerDC(thisPlayer));
                }
            }
        }

        public void RemoveFriend(PlayerDC friend)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from p in db.Players
                         where p.PlayerId == Player.PlayerId
                         select p).ToList();
                var q1 = (from p in db.Players
                          where p.PlayerId == friend.PlayerId
                          select p).ToList();
                if (q.Count > 0 && q1.Count > 0)
                {
                    var thisPlayer = q.ElementAt(0);
                    var friendPlayer = q1.ElementAt(0);
                    thisPlayer.Friends.Remove(friendPlayer);
                    friendPlayer.Friends.Remove(thisPlayer);
                    db.SaveChanges();
                    Callback.NotifyFriendshipEnding(EntityContractTranslator.PlayerToPlayerDC(friendPlayer));
                    if (Sessions.Keys.Contains(friendPlayer.PlayerId))
                        Sessions[friendPlayer.PlayerId].Callback.NotifyFriendshipEnding(EntityContractTranslator.PlayerToPlayerDC(thisPlayer));
                }
            }
        }

        public void TerminateSession()
        {
            if (ConnexionDenied)
                return;
            using (var db = new BomberDbContext())
            {
                var q = from p in db.Players
                        where p.PlayerId == Player.PlayerId
                        select p;
                if (q.Count() > 0)
                {
                    var p = q.ToList().ElementAt(0);
                    p.PlayerStatus = "Offline";
                    db.SaveChanges();
                    var q2 = from f in p.Friends
                             where Sessions.Keys.Contains(f.PlayerId)
                             select f;
                    foreach (var f in q2)
                    {
                        var session = Sessions[f.PlayerId];
                        session.Callback.NotifyPlayerStatusChange(EntityContractTranslator.PlayerToPlayerDC(p));
                    }
                    Sessions.Remove(Player.PlayerId);
                }
            }
            foreach (var cm in Conversations)
            {
                var participantsIds = from c in cm.Value.Conv.Participants
                                      select c.PlayerId;
                if (cm.Value.Conv.Host.PlayerId == Player.PlayerId || participantsIds.Contains(Player.PlayerId))
                {
                    cm.Value.EndOfConvEvent -= EndOfConvEventHandler;
                    cm.Value.JoinEvent -= JoinConvEventHandler;
                    cm.Value.KickEvent -= KickFromConvEventHandler;
                    cm.Value.LeaveEvent -= LeaveConvEventHandler;
                    cm.Value.MessageEvent -= ConvMessageEventHandler;
                }
            }
        }

        public void SendMessage(int targetPlayerID, string msg)
        {
            var targerPlayerSession = Sessions[targetPlayerID];
            targerPlayerSession.Callback.ReceiveMessage(Player.PlayerId, msg);
        }

        public void SaveProfil(PlayerDC profil)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from p in db.Players
                         where p.PlayerId == Player.PlayerId
                         select p).ToList<Player>();
                if (q.Count > 0)
                {
                    var thisPlayer = q.ElementAt(0);
                    thisPlayer.Pseudonym = profil.Pseudonym;
                    thisPlayer.PlayerDescription = profil.PlayerDescription;
                    db.SaveChanges();
                    foreach (var friend in thisPlayer.Friends)
                    {
                        if (Sessions.Keys.Contains(friend.PlayerId))
                        {
                            Sessions[friend.PlayerId].Callback.NotifyFriendProfilChange(EntityContractTranslator.PlayerToPlayerDC(thisPlayer));
                        }
                    }
                }
            }
        }

        public void CreateConversation(ConversationDC newConv)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from p in db.Players
                         where p.PlayerId == newConv.Host.PlayerId
                         select p).ToList();
                var convMembersIds = from i in newConv.Participants
                                     select i.PlayerId;
                var convMembers = (from p in db.Players
                                   where convMembersIds.Contains(p.PlayerId)
                                   select p);
                if (q.Count > 0)
                {
                    var host = q.ElementAt(0);
                    Conversation c = new Conversation()
                    {
                        Host = host,
                        ConversationLog = ""
                    };
                    // discussion : add the host to c.Players or not? let's say no, for now...
                    foreach (var p in convMembers)
                    {
                        if (!c.Players.Contains(p))
                            c.Players.Add(p);
                    }
                    db.Conversations.AddObject(c);
                    db.SaveChanges();
                    var convDC = EntityContractTranslator.ConversationToConversationDC(c);
                    var cm = new ConversationManager(convDC);
                    Conversations.Add(convDC.ConversationId, cm);
                    if (Sessions.Keys.Contains(host.PlayerId))
                    {
                        var s = Sessions[host.PlayerId];
                        cm.EndOfConvEvent += s.EndOfConvEventHandler;
                        cm.JoinEvent += s.JoinConvEventHandler;
                        cm.KickEvent += s.KickFromConvEventHandler;
                        cm.LeaveEvent += s.LeaveConvEventHandler;
                        cm.MessageEvent += s.ConvMessageEventHandler;
                        s.Callback.InitializeConversation(convDC);
                    }
                    foreach (var p in convMembers)
                    {
                        if (Sessions.Keys.Contains(p.PlayerId))
                        {
                            var s = Sessions[p.PlayerId];
                            cm.EndOfConvEvent += s.EndOfConvEventHandler;
                            cm.JoinEvent += s.JoinConvEventHandler;
                            cm.KickEvent += s.KickFromConvEventHandler;
                            cm.LeaveEvent += s.LeaveConvEventHandler;
                            cm.MessageEvent += s.ConvMessageEventHandler;
                            s.Callback.InitializeConversation(convDC);
                        }
                    }
                }
            }
        }

        public void SendConversationMessage(ConversationDC msgContainer)
        {
            if (msgContainer.NewMessageType == 1)
            {
                if (Conversations.Keys.Contains(msgContainer.ConversationId))
                {
                    var cm = Conversations[msgContainer.ConversationId];
                    cm.TriggerAudioMessageEvent(
                        msgContainer.NewAudioMessageBuffer,
                        msgContainer.NewAudioMessageBytesRecorded,
                        msgContainer.MessageSenderPlayerId);
                }
                return;
            }
            //using (var db = new BomberDbContext())
            //{
            //    var q = (from c in db.Conversations
            //             where c.ConversationId == msgContainer.ConversationId
            //             select c).ToList();
            //    if (q.Count > 0)
            //    {
            //        var conv = q.ElementAt(0);
            //        conv.ConversationLog += msgContainer.NewMessage + " \n";
            //        db.SaveChanges();
            //    }
            //}
            if (Conversations.Keys.Contains(msgContainer.ConversationId))
            {
                var cm = Conversations[msgContainer.ConversationId];
                cm.TriggerMessageEvent(msgContainer.NewMessage);
            }
        }

        public void DeleteConversation(ConversationDC Party)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from c in db.Conversations
                         where c.ConversationId == Party.ConversationId
                         select c).ToList();
                if (q.Count > 0)
                {
                    var convToDrop = q.ElementAt(0);
                    convToDrop.Players.Clear();
                    db.Conversations.DeleteObject(convToDrop);
                    db.SaveChanges();
                    if (Conversations.Keys.Contains(Party.ConversationId))
                    {
                        var cm = Conversations[Party.ConversationId];
                        cm.TriggerEndOfConvEvent();
                        Conversations.Remove(Party.ConversationId);
                    }
                }
            }
        }

        public void LeaveConversation(ConversationDC Party)
        {
            using (var db = new BomberDbContext())
            {
                var q = (from c in db.Conversations
                         where c.ConversationId == Party.ConversationId
                         select c).ToList();
                var q1 = (from p in db.Players
                          where p.PlayerId == Player.PlayerId
                          select p).ToList();
                if (q.Count > 0 && q1.Count > 0)
                {
                    var conv = q.ElementAt(0);
                    var thisPlayer = q1.ElementAt(0);
                    conv.Players.Remove(thisPlayer);
                    var remainingPlayers = conv.Players.Count;
                    if (remainingPlayers == 0)
                    {
                        db.Conversations.DeleteObject(conv);
                        if (Conversations.Keys.Contains(Party.ConversationId))
                        {
                            var cm = Conversations[Party.ConversationId];
                            cm.TriggerEndOfConvEvent();
                            Conversations.Remove(Party.ConversationId);
                        }
                    }
                    else if (Conversations.Keys.Contains(Party.ConversationId))
                    {
                        var cm = Conversations[Party.ConversationId];
                        var q2 = (from p in cm.Conv.Participants
                                  where p.PlayerId == Player.PlayerId
                                  select p).ToList();
                        if (q2.Count > 0)
                        {
                            var p = q2.ElementAt(0);
                            cm.Conv.Participants = cm.Conv.Participants.ToList();
                            ((List<PlayerDC>)cm.Conv.Participants).Remove(p);
                            cm.TriggerLeaveEvent(new PlayerDC() { PlayerId = Player.PlayerId });
                            cm.EndOfConvEvent -= EndOfConvEventHandler;
                            cm.JoinEvent -= JoinConvEventHandler;
                            cm.KickEvent -= KickFromConvEventHandler;
                            cm.LeaveEvent -= LeaveConvEventHandler;
                            cm.MessageEvent -= ConvMessageEventHandler;
                        }
                    }
                    db.SaveChanges();
                }
            }
        }

        public void SwitchMainConversation(ConversationDC conversationDC)
        {
            if (Conversations.Keys.Contains(conversationDC.ConversationId))
            {
                var cm = Conversations[conversationDC.ConversationId];
                PlayerDC p = null;
                if (cm.Conv.Host.PlayerId == Player.PlayerId)
                    p = cm.Conv.Host;
                else
                {
                    var q = (from m in cm.Conv.Participants where m.PlayerId == Player.PlayerId select m).ToList();
                    if (q.Count > 0)
                        p = q.ElementAt(0);
                }
                if (p != null)
                {
                    var lastMainRoomId = p.MainRoomId;
                    if (Conversations.Keys.Contains(lastMainRoomId))
                    {
                        var cm1 = Conversations[lastMainRoomId];
                        cm1.AudioMessageEvent -= AudioMessageEventHandler;
                    }
                    cm.AudioMessageEvent += AudioMessageEventHandler;
                    foreach (var cm2 in Conversations)
                    {
                        PlayerDC thisPlayerPossibleOccurence = null;
                        if (cm2.Value.Conv.Host.PlayerId == Player.PlayerId)
                            thisPlayerPossibleOccurence = cm2.Value.Conv.Host;
                        else
                        {
                            var q1 = (from member in cm2.Value.Conv.Participants where member.PlayerId == Player.PlayerId select member).ToList();
                            if (q1.Count > 0)
                                thisPlayerPossibleOccurence = q1.ElementAt(0);
                        }
                        if (thisPlayerPossibleOccurence != null)
                            thisPlayerPossibleOccurence.MainRoomId = conversationDC.ConversationId;
                    }
                }
            }
        }

        #endregion

        private void ConvMessageEventHandler(ConversationDC conv)
        {
            var c = new ConversationDC()
            {
                ConversationId = conv.ConversationId,
                NewMessage = conv.NewMessage
            };
            Callback.ReceiveConversationMessage(c);
        }

        private void LeaveConvEventHandler(PlayerDC player, ConversationDC conv)
        {
            Callback.RemovePlayerFromParty(player, conv);
        }

        private void JoinConvEventHandler(PlayerDC player, ConversationDC conv)
        {
            //TODO
        }

        private void KickFromConvEventHandler(PlayerDC player, ConversationDC conv)
        {
            //TODO
        }

        private void EndOfConvEventHandler(ConversationDC conv)
        {
            Callback.DropConversation(conv);
        }

        private void AudioMessageEventHandler(ConversationDC conv)
        {
            Callback.ReceiveConversationMessage(conv);
        }



        #region BomberGame events' handlers

        public void GameStartEventHandler(ConversationDC conv)
        {
            Callback.OpenGameWindow(conv);
        }

        public void PlayerPositionReplicationEventHandler(ConversationDC conv, PlayerDC player)
        {
            Callback.UpdatePlayerPosition(conv, player);
        }

        public void BombSpawnReplicationEventHandler(ConversationDC conv, int BombPositionX, int BombPositionY)
        {
            Callback.SpawnBomb(conv, BombPositionX, BombPositionY);
        }

        public void PlayerDeathReplicationEventHandler(ConversationDC conv, PlayerDC player)
        {
            Callback.KillPlayer(conv, player);
        }

        #endregion

        #region IBomberService Members


        public void ReplicatePlayerPosition(int convID, string playerLogin, int posX, int posY)
        {
            var playerNewCoord = new PlayerDC()
            {
                Login = playerLogin,
                PlayerPositionX = posX,
                PlayerPositionY = posY
            };
            Conversations[convID].Game.TriggerPlayerPositionReplicationEvent(playerNewCoord);
        }

        public void ReplicateSpawnedBombPosition(int convID, int posX, int posY)
        {
            Conversations[convID].Game.TriggerBombSpawnEvent(posX, posY);
        }

        public void ReplicatePlayerDeath(int convID, string playerLogin)
        {
            Conversations[convID].Game.TriggerPlayerDeathEvent(new PlayerDC() { Login = playerLogin });
        }

        public void StartGame(int convID)
        {
            var convMan = Conversations[convID];
            if (convMan.Conv.Participants.Count() <= 3 && convMan.Conv.Host.MainRoomId == convMan.Conv.ConversationId)
            {
                var q = (from p in convMan.Conv.Participants
                         where p.MainRoomId != convMan.Conv.ConversationId
                         select p).ToList();
                if (q.Count > 0)
                {
                    return; 
                }
                convMan.Game = new BomberGameManager(convMan);
                convMan.Game.TriggerGameStartEvent();
            }
        }

        #endregion
    }
}
