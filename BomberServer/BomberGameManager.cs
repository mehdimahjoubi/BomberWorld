using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BomberServer.BomberServiceImp;
using BomberContracts_WCF;

namespace BomberServer
{
    class BomberGameManager
    {
        #region Game events

        /// <summary>
        /// occurs when the host launches the game
        /// </summary>
        public event GameStartEventHandler GameStartEvent;
        public delegate void GameStartEventHandler(ConversationDC conv);

        /// <summary>
        /// occurs when a player replicates his new position on the network
        /// </summary>
        public event PlayerPositionReplicationEventHandler PlayerPositionReplicationEvent;
        public delegate void PlayerPositionReplicationEventHandler(ConversationDC conv, PlayerDC player);

        /// <summary>
        /// occurs when a bomb is spawned
        /// </summary>
        public event BombSpawnReplicationEventHandler BombSpawnEvent;
        public delegate void BombSpawnReplicationEventHandler(ConversationDC conv, int BombPositionX, int BombPositionY);

        /// <summary>
        /// occurs when a player is dead
        /// </summary>
        public event PlayerDeathReplicationEventHandler PlayerDeathEvent;
        public delegate void PlayerDeathReplicationEventHandler(ConversationDC conv, PlayerDC player);

        #endregion

        public ConversationManager Parent { get; set; }

        public BomberGameManager(ConversationManager conv)
        {
            Parent = conv;
            var hostSession = BomberService.Sessions[Parent.Conv.Host.PlayerId];
            GameStartEvent += hostSession.GameStartEventHandler;
            PlayerPositionReplicationEvent += hostSession.PlayerPositionReplicationEventHandler;
            BombSpawnEvent += hostSession.BombSpawnReplicationEventHandler;
            PlayerDeathEvent += hostSession.PlayerDeathReplicationEventHandler;
            foreach (var member in Parent.Conv.Participants)
            {
                BomberService session = BomberService.Sessions[member.PlayerId];
                GameStartEvent += session.GameStartEventHandler;
                PlayerPositionReplicationEvent += session.PlayerPositionReplicationEventHandler;
                BombSpawnEvent += session.BombSpawnReplicationEventHandler;
                PlayerDeathEvent += session.PlayerDeathReplicationEventHandler;
            }
        }

        public void TriggerGameStartEvent()
        {
            GameStartEvent(Parent.Conv);
        }

        public void TriggerPlayerPositionReplicationEvent(PlayerDC p)
        {
            PlayerPositionReplicationEvent(new ConversationDC() { ConversationId = Parent.Conv.ConversationId }, p);
        }

        public void TriggerBombSpawnEvent(int x, int y)
        {
            BombSpawnEvent(new ConversationDC() { ConversationId = Parent.Conv.ConversationId }, x, y);
        }

        public void TriggerPlayerDeathEvent(PlayerDC player)
        {
            PlayerDeathEvent(new ConversationDC() { ConversationId = Parent.Conv.ConversationId }, player);
        }
    }
}
