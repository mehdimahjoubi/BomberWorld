using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BomberRepository;
using BomberContracts_WCF;

namespace BomberServer
{
    public static class EntityContractTranslator
    {
        public static PlayerDC PlayerToPlayerDC(Player p)
        {
            PlayerDC playerDC = new PlayerDC()
            {
                PlayerDescription = p.PlayerDescription,
                PlayerId = p.PlayerId,
                PlayerStatus = p.PlayerStatus,
                Pseudonym = p.Pseudonym
            };
            using (var db = new BomberDbContext())
            { 
                var q = (from pl in db.Players
                         where pl.PlayerId == playerDC.PlayerId
                         select pl).ToList<Player>();
                if (q.Count > 0)
                {
                    var player = q.ElementAt(0);
                    playerDC.Login = player.Account.Login;
                }
            }
            return playerDC;
        }

        public static IEnumerable<PlayerDC> PlayersToPlayerDCs(IEnumerable<Player> players)
        {
            var playerDCs = new List<PlayerDC>();
            foreach (var p in players)
            {
                playerDCs.Add(PlayerToPlayerDC(p));
            }
            return playerDCs;
        }

        public static ConversationDC ConversationToConversationDC(Conversation conv)
        {
            ConversationDC convDC = null;
            using (var db = new BomberDbContext())
            {
                var q = (from co in db.Conversations
                          where co.ConversationId == conv.ConversationId
                          select co).ToList();
                if (q.Count > 0)
                {
                    var c = q.ElementAt(0);
                    convDC = new ConversationDC()
                    {
                        ConversationId = c.ConversationId,
                        ConversationLog = c.ConversationLog,
                        Host = PlayerToPlayerDC(c.Host),
                        Participants = PlayersToPlayerDCs(c.Players)
                    };
                }
            }
            return convDC;
        }
    }
}
