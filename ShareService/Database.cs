using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using ShareLibrary;


namespace ShareService
{
    public class Database
    {
        //Define Variables
        private  readonly string ip = "52.29.133.101";
        private  readonly string port = "3306";
        private  readonly string dbName = "Share";
        private  readonly string dbUsername = "Ryan";
        private  readonly string dbPassword ="Pa$$w0rd";
        protected MySqlConnection connection;
        private static Database db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="dbName"></param>
        /// <param name="dbUsername"></param>
        /// <param name="dbPassword"></param>
        public Database()
        {
            this.ip = ip;
            this.port = port;
            this.dbName = dbName;
            this.dbUsername = dbUsername;
            this.dbPassword = dbPassword;
            this.Connect();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Connect()
        {
            string connect = String.Format("SERVER={0};PORT={1};DATABASE={2};UID={3};PASSWORD={4}", this.ip, this.port, this.dbName, this.dbUsername, this.dbPassword);
            this.connection = new MySqlConnection(connect);
            this.connection.Open();
        }

        /// <summary>
        /// Run the query.
        /// </summary>
        /// <param name="query">String with the query</param>
        /// <returns>Results</returns>
        protected MySqlDataReader runQuery(string query)
        {
            Console.WriteLine("Query: " + query);
            MySqlCommand cmd = new MySqlCommand(query, this.connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        protected void runQueryNonReturn(string query)
        {
            Console.WriteLine("Query: " + query);
            MySqlCommand cmd = new MySqlCommand(query, this.connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Login(string ip,string username,string password)
        {
            string query = String.Format("SELECT * FROM User WHERE username='{0}' AND pwd='{1}'",username,password);
            User u = null;
            using (MySqlDataReader reader = this.runQuery(query))
            {
                while (reader.Read())
                {
                    u = this.createUserObject(reader);
                }
            }
            this.LoginAudit(ip, u);
            return u;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Register(string ip,string fName,string lName,string username,string password)
        {
            string query = String.Format("INSERT INTO User VALUES(default,'{0}','{1}','{2}','{3}')",fName,lName,username,password);
            this.runQueryNonReturn(query);
            return this.Login(ip,username,password);
        }

        public ShareFile FileDetails(User user,int fileID)
        {
            string query = string.Format("SELECT * FROM File WHERE ID = {0} AND UserID = {1}", fileID, user.ID);
            using (MySqlDataReader reader = this.runQuery(query))
            {
                while (reader.Read())
                {
                    return this.createFileUserFilesObject(reader);
                }
            }
            return null;
        }

        public void AddFileToDB(ShareFile file,User user,string ip)
        {
            string query = String.Format("INSERT INTO File VALUES(default,{0},'{1}',{2},{3},{4})",user.ID,file.fileName, file.t, file.n,file.ToReturn);
            this.runQueryNonReturn(query);
            this.FileUploadAudit(file, user, ip);
        }

        public void FileUploadAudit(ShareFile file,User user,string ip)
        {
            string query = String.Format("INSERT INTO Audit VALUES(default,'FileUpload','FileUploaded',{0},'{1}','{2}')", user.ID, ip, this.DateToUnix(DateTime.Now));
            this.runQueryNonReturn(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="user"></param>
        public void LoginAudit(string ip,User user)
        {
            string query;
            if (user != null)
            {
                 query = String.Format("INSERT INTO Audit VALUES(default,UserLogin,LoggedIn,{0},{1},{2})", user.ID, ip, this.DateToUnix(DateTime.Now));
            }else
            {
                query = String.Format("INSERT INTO Audit VALUES(default,UserLogin,FailedLogin,-1,{0},{1})", ip, this.DateToUnix(DateTime.Now));
            }
        }

        public List<ShareFile> getUserFiles(User user, string ip)
        {
            List<ShareFile> files = new List<ShareFile>();
            string query = String.Format("SELECT * FROM File WHERE UserID = {0}", user.ID);
            using (MySqlDataReader reader = this.runQuery(query))
            {
                while(reader.Read())
                {
                    files.Add(this.createFileUserFilesObject(reader));
                }
            }
            this.getUserFileAudit(user, ip);
            return files;
        }

        public void getUserFileAudit(User user, string ip)
        {
            string query= String.Format("INSERT INTO Audit VALUES(default,Get User Files,Get User Files,{0},{1},{2})", user.ID,ip, this.DateToUnix(DateTime.Now));
            this.runQueryNonReturn(query);
        }

        /// <summary>
        /// 
        /// </summary>
        protected User createUserObject(MySqlDataReader reader)
        {
            return new User(int.Parse(reader["ID"].ToString()),reader["username"].ToString(),reader["pwd"].ToString(),reader["fName"].ToString(),reader["lName"].ToString());
        }

        protected ShareFile createFileUserFilesObject(MySqlDataReader reader)
        {
            return new ShareFile(int.Parse(reader["ID"].ToString()), reader["fileName"].ToString());
        }

        public static Database getDB()
        {
            if (db == null)
            {
                db = new Database();
            }
            return db;
        }

        public double DateToUnix(DateTime date)
        {

            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double  span = (date- start).TotalSeconds;
            return span;
        }


    }
}