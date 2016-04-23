using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ShareLibrary
{
    [DataContract]
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string fName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string lName { get; set; }

        /// <summary>
        /// Constructor for Database
        /// </summary>
        /// <param name="ID">ID of the User</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="fName">First Name</param>
        /// <param name="lName">Last Name</param>
        public User(int ID, string username, string password, string fName, string lName)
        {
            this.ID = ID;
            this.username = username;
            this.password = password;
            this.fName = fName;
            this.lName = lName;
        }
        
    }
}
