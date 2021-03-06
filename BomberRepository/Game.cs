//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BomberRepository
{
    public partial class Game
    {
        #region Primitive Properties
    
        public virtual int GameId
        {
            get;
            set;
        }
    
        public virtual Nullable<int> WinnerPlayerId
        {
            get { return _winnerPlayerId; }
            set
            {
                try
                {
                    _settingFK = true;
                    if (_winnerPlayerId != value)
                    {
                        if (Winner != null && Winner.PlayerId != value)
                        {
                            Winner = null;
                        }
                        _winnerPlayerId = value;
                    }
                }
                finally
                {
                    _settingFK = false;
                }
            }
        }
        private Nullable<int> _winnerPlayerId;

        #endregion
        #region Navigation Properties
    
        public virtual Player Winner
        {
            get { return _winner; }
            set
            {
                if (!ReferenceEquals(_winner, value))
                {
                    var previousValue = _winner;
                    _winner = value;
                    FixupWinner(previousValue);
                }
            }
        }
        private Player _winner;
    
        public virtual Conversation Conversation
        {
            get { return _conversation; }
            set
            {
                if (!ReferenceEquals(_conversation, value))
                {
                    var previousValue = _conversation;
                    _conversation = value;
                    FixupConversation(previousValue);
                }
            }
        }
        private Conversation _conversation;

        #endregion
        #region Association Fixup
    
        private bool _settingFK = false;
    
        private void FixupWinner(Player previousValue)
        {
            if (previousValue != null && previousValue.WinnedGames.Contains(this))
            {
                previousValue.WinnedGames.Remove(this);
            }
    
            if (Winner != null)
            {
                if (!Winner.WinnedGames.Contains(this))
                {
                    Winner.WinnedGames.Add(this);
                }
                if (WinnerPlayerId != Winner.PlayerId)
                {
                    WinnerPlayerId = Winner.PlayerId;
                }
            }
            else if (!_settingFK)
            {
                WinnerPlayerId = null;
            }
        }
    
        private void FixupConversation(Conversation previousValue)
        {
            if (previousValue != null && ReferenceEquals(previousValue.CurrentGame, this))
            {
                previousValue.CurrentGame = null;
            }
    
            if (Conversation != null)
            {
                Conversation.CurrentGame = this;
            }
        }

        #endregion
    }
}
