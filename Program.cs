// CREATING PROGRAM.CS BY ZEKERIYA - UYSAL 
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MySQLManager.Database;
using System.Runtime.InteropServices;
using Do.chat;
using System.Windows.Forms;
namespace Do
{
    class Program
    {
     
        public static Dictionary<Int32, Chatroom> Chatrooms { get; set; }

        public static List<CustomChatRoom> PrivateChats { get; set; }

        public static Dictionary<uint, User> chatUsers;
        public string versionfile = "0.1.0.0";
        public static DateTime Time = DateTime.Now;
        public static Dictionary<string, string> Setting;
        public static DatabaseManager chatmanager;
        public static DatabaseManager manager;
        private static DateTime unixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        private static ConsoleEventDelegate handler;
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        [STAThread]
        static void Main(string[] args)
        {
            handler = ConsoleEventCallback;
            SetConsoleCtrlHandler(handler, true);
            Task mytask = Task.Run(() =>
            {
                TEXT form = new TEXT();
                form.ShowDialog();
            });
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
            Chatrooms = new Dictionary<int, Chatroom>();
            PrivateChats = new List<CustomChatRoom>();


            Console.Title = "Z3K | CHAT SYSTEM";
            Console.SetWindowSize(70, 30);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
           
            chatUsers = new Dictionary<uint,User>();
          
           Server();
            Out.WriteLine("-------------------------------");
            Out.WriteLine("");
            Out.WriteLine("Starting Darkorbit Emulator by ~ Z3K");
            Out.WriteLine("");
            Out.WriteLine("-------------------------------");
          
            Console.ReadLine();
        }   
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null) Console.WriteLine(exception.StackTrace);
            Console.ReadKey();
            Environment.Exit(-1);
        }
        public static bool ConsoleEventCallback(int eventType)
        {
            if (eventType != 2) return false;
            try
            {
                foreach (var user in chatUsers)
                {
                    user.Value.Disconnect();
                }
            }catch(Exception)
            { }
            return false;
        }

        public static int GetUnixTimestamp
        {
            get { return (int)(DateTime.UtcNow - unixDateTime).TotalSeconds; }
        }

        static async void Server()
        {
            if (!File.Exists("app.ini"))
            {
                Out.WriteLine("Error #1 loading app.ini", "Boot", ConsoleColor.Red);
                Console.ReadKey();
                Environment.Exit(0);
            }
           
            Setting = await ReadSettings(File.ReadAllText("app.ini"));
            if (!Setting.ContainsKey("server") || !Setting.ContainsKey("mport") || !Setting.ContainsKey("username"))
            {
                Out.WriteLine("Error loading app.ini", "Boot", ConsoleColor.Red);
                Console.ReadKey();
                Environment.Exit(0);
            }Console.Beep();
            AddRooms();
            Out.WriteLine("");
            Out.WriteLine("-----------------------------------------------------");
            Out.WriteLine("Starting Private Server Chat Emulator by ~ Z3K");
            Out.WriteLine("------------------------------------------------------");
            Console.WriteLine();
            CountUser();
            AsynchronousSocketListener.StartListening();
          
        } 
        private static void AddRooms()
        {
            try
            {
                chatmanager = new DatabaseManager(30, 10, DatabaseType.MySQL);
                chatmanager.setServerDetails(Setting["server"], uint.Parse(Setting["mport"]), Setting["username"], "", "chatserver");
                chatmanager.init();

            }
            catch (Exception e)
            {
                Out.WriteLine("Error: " + e.Message, "MySQL", ConsoleColor.Red);
                Console.ReadKey();
                Environment.Exit(0);
            }
            try
            {
                manager = new DatabaseManager(30, 10, DatabaseType.MySQL);
                manager.setServerDetails(Setting["server"], uint.Parse(Setting["mport"]), Setting["username"], "", "aurora");
                manager.init();
            }
            catch (Exception e)
            {
                Out.WriteLine("Error: " + e.Message, "MySQL", ConsoleColor.Red);
                Console.ReadKey();
                Environment.Exit(0);
            }

            using (var dbClient = chatmanager.getQueryreactor())
            {
                var data = (DataTable)dbClient.query("SELECT * FROM chatrooms");
                foreach (DataRow row in data.Rows)
                {
                    var chat = new Chatroom
                    {
                        Index = Convert.ToInt32(row[0]),
                        Name = row[1].ToString(),
                        Id = Convert.ToInt32(row[2])
                    };
                    Chatrooms.Add(chat.Id, chat);
                }
                var mmo = new Chatroom
                {
                    Index = Convert.ToInt32(1),
                    Name = "MMO",
                    Id = Convert.ToInt32(22)
                }; var vru = new Chatroom
                {
                    Index = Convert.ToInt32(1),
                    Name = "VRU",
                    Id = Convert.ToInt32(253)
                }; var eic = new Chatroom
                {
                    Index = Convert.ToInt32(1),
                    Name = "EIC",
                    Id = Convert.ToInt32(252)
                };

            }
            Out.WriteLine("" + Chatrooms.Count + " Chat rooms are loaded.");

        }

        private static async void CountUser()
        {
            await Task.Delay(1000);

            TimeSpan a;
            long mem;
            while (true)
            {
                await Task.Delay(1000);
                a = DateTime.Now - Time;
                int users = chatUsers.Count;

                mem = Process.GetCurrentProcess().WorkingSet64;
                Console.Title = users + " game clients connected! - " + BytesToString(mem) + " - Running for " + a.Hours + ":" + a.Minutes + ":" + a.Seconds;
            }
        }
        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num) + suf[place];
        }
        public static bool IsUserBanned(uint username)
        {
            try
            {
                var sql = "SELECT * FROM bannedusers WHERE id = @username;";

                using (var dbClient = chatmanager.getQueryreactor())
                {
                   var ds = (DataTable)dbClient.query(sql, (new[] { "username" }), (new[] { username.ToString() }));
                    return ds.Rows.Count != 0 ? true : false;
                }
            }
            catch { return false; }
        }

        public static bool IsIpBanned(String ip)
        {
            try
            {
                var sql = "SELECT * FROM `bannedips` WHERE `ip` LIKE @userIP;";

                using (var dbClient = chatmanager.getQueryreactor())
                {
                    var ds = (DataTable)dbClient.query(sql, (new[] { "userIP" }), (new[] { ip.ToString() }));
                    return ds.Rows.Count != 0;
                }
            }
            catch { return false; }
        }

        public static void BanUser(uint username, string name)
        {
            try
            {
                var sql = "INSERT INTO bannedusers (id, Username) VALUES (@username, @name)";

                using (var dbClient = chatmanager.getQueryreactor())
                {
                    dbClient.query(sql, (new[] { "username", "name" }), (new[] { username.ToString(), name.ToString() }));
                }
            }
            catch { }
        }

        public static void UnbanUser(String username)
        {
            try
            {
                var sql = " DELETE FROM `bannedusers` WHERE `username` LIKE @username OR `id` = @username;";

                using (var dbClient = chatmanager.getQueryreactor())
                {
                  dbClient.query(sql, (new[] { "username" }), (new[] { username.ToString() }));
                }
            }
            catch { }
        }

        public static void BanIp(String ip)
        {
            try
            {
                var sql = "INSERT INTO `bannedips` (`Ip`) VALUES (@userIP);";

                using (var dbClient = chatmanager.getQueryreactor())
                {
                    dbClient.query(sql, (new[] { "userIP" }), (new[] { ip.ToString() }));
                }
            }
            catch { }
        }

        public static void UnbanIp(String ip)
        {
            try
            {
                var sql = " DELETE FROM `bannedips` WHERE `Ip` LIKE @userIP;";

                using (var dbClient = chatmanager.getQueryreactor())
                {
                    dbClient.query(sql, (new[] { "userIP" }), (new[] { ip.ToString() }));
                }
            }
            catch { }
        }

        public static bool IsUserAdministrator(String username)
        {
            try
            {
                var sql = "SELECT * FROM `administrators` WHERE `username` LIKE @username;";

                using (var dbClient = chatmanager.getQueryreactor())
                {
                    var ds = (DataTable)dbClient.query(sql, (new[] { "username" }), (new[] { username.ToString() }));
                    return ds.Rows.Count != 0;
                }
            }
            catch { return false; }
        }

        public static void AddLog(string from, string to, string message, int isBan = 0)
        {
            if (isBan == 1)
            {
                var sql = "INSERT INTO `log` (`Timestamp`,`From`,`To`,`Message`) VALUES (@date,@from,@to,@message);";

                using (var dbClient = chatmanager.getQueryreactor())
                {
                    dbClient.query(sql, (new[] { "date","from","to","message" }), (new[] { DateTime.Now.ToString(),from.ToString(),to.ToString(),"banned" }));
                }
            }
           File.AppendAllText("chatLog.txt", "[" + DateTime.Now + "] - [" + from + "] >> " + to + ":: '" + message + "'" + Environment.NewLine);
        }
        public static string ToEnum(bool isTrue)
        {
            return (isTrue) ? "1" : "0";
        }

        public static bool ToBool(string isTrue)
        {
            return (isTrue == "1");
        }

        public static DataRow checkSession(uint userId, ulong sessionId)
        {
     
                if (sessionId < 1 || userId < 1000)
                {
                    return null;
                }
                using (var dbClient = manager.getQueryreactor())
                {
                    var data = (DataTable)dbClient.query("SELECT users.Name,server_1_players.rank,server_1_players.factionId,server_1_players.clanID FROM server_1_players, users WHERE server_1_players.tokenId = @sessionId AND server_1_players.playerID = @ID AND users.ID = server_1_players.userID", (new[] { "sessionId", "ID" }), (new[] { sessionId.ToString(), userId.ToString() }));
                    if (data.Rows.Count != 1) return null;
                    var row = data.Rows[0];
                    if (row["clanId"].ToString() != "0")
                    {
                        data = (DataTable)dbClient.query("SELECT users.Name,clan.tagName,server_1_players.factionId,server_1_players.rank,server_1_players.clanId FROM server_1_players, server_1_clan, users WHERE server_1_players.tokenId = @sessionId AND server_1_players.playerID = @ID AND users.ID = server_1_players.userID AND clan.clanID = server_1_players.clanID", (new[] { "sessionId", "ID" }), (new[] { sessionId.ToString(), userId.ToString() }));
                    }
                    return data.Rows[0];
                }
        }

        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static int getDistance(double x, double z, double y, double w)
        {
            return Convert.ToInt32(Math.Sqrt(Math.Pow(x - z, 2.0D) + Math.Pow(y - w, 2.0D)));
        }
      

        #region Azure.Boot Functions
 
        static async Task<Dictionary<string, string>> ReadSettings(string settingsIni)
        {
            var Settings = new Dictionary<string, string>();

            try
            {
                var strReader = new StringReader(settingsIni);
                string line = null;

                while ((line = await strReader.ReadLineAsync()) != null)
                {
                    if (line.Length < 1 || line.StartsWith("#") || line.StartsWith("["))
                    {
                        continue;
                    }

                    var delimiterIndex = line.IndexOf("=", StringComparison.Ordinal);

                    if (delimiterIndex != -1)
                    {
                        var key = line.Substring(0, delimiterIndex);
                        var val = line.Substring(delimiterIndex + 1);

                        try
                        {
                            Settings.Add(key, val);
                        }
                        catch
                        {
                        }
                    }
                }

                strReader.Close();
            }
            catch (Exception e)
            {
                Out.WriteLine("Error #2 loading app.ini: " + e.Message, "Azure.Boot", ConsoleColor.Red);
                Console.ReadKey();
                Environment.Exit(0);
            }

            return Settings;
        }
        #endregion
    }

    static class Out
    {

        public static void WriteLine(string format, string header = "", ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + DateTime.Now + " -");
            if (header != "")
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(header);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] ");

            }

            Console.Write(" - ");
            Console.ForegroundColor = color;
            Console.WriteLine(format);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
