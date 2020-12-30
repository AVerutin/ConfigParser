using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class ConnectionStringUnit
    {
        private string ConnectionString;             // СтрокаСоединения
        public string Host { get; set; }             // Имя сервера
        public int Port { get; set; }                // Номер порта
        public string Database { get; set; }         // Имя базыданных
        public string UserName { get; set; }         // Имя пользователя
        public string UserPassword { get; set; }     // Пароль
        public string SslMode { get; set; }          // Режим SSL
        public bool TrustServer { get; set; }        // Доверенный сервер

        public ConnectionStringUnit()
        {
            ConnectionString = default;
        }
        

        public ConnectionStringUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.Connection)
            {
                foreach (KeyValuePair<string, string> param in configurationUnit.Parameters)
                {
                    switch (param.Key.ToUpper())
                    {
                        case "СТРОКАСОЕДИНЕНИЯ":
                            ConnectionString = param.Value;
                            break;
                    }
                }
            }
            
            string[] pars = ConnectionString.Split(";");
            foreach (string par in pars)
            {
                if(!string.IsNullOrEmpty(par))
                {
                    int posEq = par.IndexOf('=');
                    string key = par.Substring(0, posEq);
                    string val = par.Substring(posEq + 1);
                    switch (key.ToUpper())
                    {
                        case "HOST":
                            Host = val;
                            break;
                        case "PORT":
                            try
                            {
                                Port = int.Parse(val);
                            }
                            catch
                            {
                                Port = 0;
                            }
                            break;
                        case "DATABASE":
                            Database = val;
                            break;
                        case "USERNAME":
                            UserName = val;
                            break;
                        case "PASSWORD":
                            UserPassword = val;
                            break;
                        case "SSLMODE":
                            SslMode = val;
                            break;
                        case "TRUST SERVER CERTIFICATE":
                            try
                            {
                                TrustServer = bool.Parse(val);
                            }
                            catch
                            {
                                TrustServer = true;
                            }
                            break;
                    }
                    
                }
            }
        }

        public ConnectionStringUnit(ConnectionStringUnit connectionUnit)
        {
            if(connectionUnit != null)
            {
                Host = connectionUnit.Host;
                Port = connectionUnit.Port;
                Database = connectionUnit.Database;
                UserName = connectionUnit.UserName;
                UserPassword = connectionUnit.UserPassword;
                SslMode = connectionUnit.SslMode;
                TrustServer = connectionUnit.TrustServer;
            }
        }

        public string GetConnectionString() =>
            $"СтрокаСоединения=Host={Host};Port={Port};Database={Database};UserName={UserName};Password={UserPassword};" +
            $"sslmode={SslMode};Trust Server Certificate={TrustServer.ToString()};";
  

        public override string ToString()
        {
            string result = "Подключение\n(\n\t";

            result += GetConnectionString();

            result += "\n)\n";
            return result;
        }
    }
}


