using System;
using System.Data.SqlClient;

namespace Halo.SqlServerConnection
{
    public class BaseConnection
    {
        private string FUserName = String.Empty;
        private string FPassword = String.Empty;
        private string FServerName = String.Empty;
        private string FDatabase = String.Empty;
        private string FAppName = String.Empty;
        private string FHost = System.Environment.MachineName;
        private int FConnectionTimeOut = 60;

        protected void PopulatePrivateFieldsFromConnectionString(string connectionString)
        {
            if (!(String.IsNullOrEmpty(connectionString)))
            {
                SqlConnectionStringBuilder ConnectionString = new SqlConnectionStringBuilder(connectionString);
                FServerName = ConnectionString.DataSource;
                FDatabase = ConnectionString.InitialCatalog;

                if (!(String.IsNullOrEmpty(ConnectionString.ApplicationName)))
                {
                    FAppName = ConnectionString.ApplicationName;
                }
                else
                {
                    FAppName = "Unknown .Net Assembly";
                }

                if (!(String.IsNullOrEmpty(ConnectionString.WorkstationID)))
                {
                    FHost = ConnectionString.WorkstationID;
                }

                FConnectionTimeOut = ConnectionString.ConnectTimeout;

                if (!(ConnectionString.IntegratedSecurity))
                {
                    FUserName = ConnectionString.UserID;
                    FPassword = ConnectionString.Password;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConnection"/> class.
        /// </summary>
        public BaseConnection()
        {
            FAppName = ".Net";
            FHost = System.Environment.MachineName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConnection"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        public BaseConnection(string serverName, string databaseName)
        {
            FServerName = serverName;
            FDatabase = databaseName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConnection"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public BaseConnection(string serverName, string databaseName, string userName, string password)
        {
            FServerName = serverName;
            FDatabase = databaseName;
            FUserName = userName;
            FPassword = password;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConnection"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public BaseConnection(string connectionString)
        {
            PopulatePrivateFieldsFromConnectionString(connectionString);
        }

        public string UserName
        {
            get { return FUserName; }
            set { FUserName = value; }
        }

        public string Password
        {
            get { return FPassword; }
            set { FPassword = value; }
        }

        public string ServerName
        {
            get { return FServerName; }
            set { FServerName = value; }
        }

        public string DatabaseName
        {
            get { return FDatabase; }
            set { FDatabase = value; }
        }
        public string ApplicationName
        {
            get { return FAppName; }
            set { FAppName = value; }
        }
        public string Host
        {
            get { return FHost; }
            set { FHost = value; }
        }
        public int ConnectionTimeOut
        {
            get { return FConnectionTimeOut; }
            set { FConnectionTimeOut = value; }
        }

        public override string ToString()
        {
            SqlConnectionStringBuilder ConnectionString = new SqlConnectionStringBuilder();
            ConnectionString.DataSource = FServerName;
            ConnectionString.InitialCatalog = FDatabase;
            ConnectionString.ApplicationName = FAppName;
            ConnectionString.WorkstationID = FHost;
            ConnectionString.ConnectTimeout = FConnectionTimeOut;
            ConnectionString.IntegratedSecurity = (String.IsNullOrEmpty(FUserName));
            if (!(ConnectionString.IntegratedSecurity))
            {
                ConnectionString.UserID = FUserName;
                ConnectionString.Password = FPassword;
            }
            return ConnectionString.ToString();
        }
    }  /// End of Class
}
