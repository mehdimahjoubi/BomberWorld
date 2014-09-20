using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BomberContracts_WCF
{
    [DataContract]
    public class AccountDC
    {
        [DataMember]
        public int AccountId
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
        public string Password
        {
            get;
            set;
        }
        [DataMember]
        public string AccountType
        {
            get;
            set;
        }
    }
}
