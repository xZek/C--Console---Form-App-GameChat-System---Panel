// CREATING  USER.CS BY ZEKERIYA UYSAL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Do.chat
{
    public class User
    {
        public Socket _handler;
        private readonly byte[] _buffer = new byte[2048];
        public uint UserId { get; set; }
        public string Username { get; set; }
        public string FactionId { get; set; }
        public string SessionId { get; set; }
        public string Clan { get; set; }
        public int Count = 0;
        public DateTime Check = new DateTime(2013, 3, 1, 7, 0, 0);
        readonly String[] _nonAllowedWords = { "bitch","fuck","amk", "ananı","sikerim","sıkerım","ibne","piç","dafuq","youtube.com","http://","www.","puto","pendejo","baboso","puta" };
        public List<Int32> ChatsJoined { get; set; }

        public bool IsAdministrator { get; set; }
      
              public void Disconnect()
        {
            try
            {
                _handler.Shutdown(SocketShutdown.Both);
                _handler.Close();
                _handler = null;
                Program.AddLog(Username, "", "Disconnected!");
                Program.chatUsers.Remove(UserId);

            }
            catch (Exception)
            {
            }
              
              }
        public User(Socket handler)
        {
         
            _handler = handler;
            UserId =0;
            Username = "";
            SessionId = "";
            Clan = "";
            IsAdministrator = false;
            ChatsJoined = new List<int>();
            try
            {

                _handler.BeginReceive(_buffer, 0, _buffer.Length, 0, ReadCallBack, this);
            }
            catch(Exception)
            { }
        }

        private void ReadCallBack(IAsyncResult result)
        {
            try
            {
                var bytesread = _handler.EndReceive(result);
                if (bytesread <= 0)
                {
                    Disconnect();
                    return;
                }
                var content = Encoding.UTF8.GetString(_buffer, 0, bytesread);

                if (content == "")
                {
                    return;
                }
                if (content.StartsWith("<policy-file-request/>"))
                {
                    Send("<?xml version=\"1.0\"?><cross-domain-policy xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://www.adobe.com/xml/schemas/PolicyFileSocket.xsd\"><allow-access-from domain=\"*\" to-ports=\"*\" secure=\"false\" /><site-control permitted-cross-domain-policies=\"master-only\" /></cross-domain-policy>");
                    GC.Collect();    
                }
                else if (content.StartsWith("bu"))
                {
                    var packet = content.Replace("@", "%").Split('%');
                    Clan = packet[7];
                    Username = packet[2];
                   // IsAdministrator = Program.IsUserAdministrator(Username);
                    SessionId = packet[4];
                    if(SessionId.Length>100)
                    {
                        Disconnect();
                        Console.WriteLine("User Not Found!");
                        return;
                    }
                    UserId = Convert.ToUInt32(packet[3]);
                    var session = Convert.ToUInt64(SessionId);
                    var userRow = Program.checkSession(UserId, session);
                    var banned = Program.IsUserBanned(UserId);
                    if (userRow == null)
                    {
                        Disconnect();
                        Console.WriteLine("User Not Found!");
                        return;
                    }

                    Username = userRow["Name"].ToString().Replace("|","I").Replace(" ", "").Replace(Environment.NewLine,"") ;
                    if (Username == "" || Username == " ") {
                        Disconnect();
                        Console.WriteLine("User disconnected. " + Username);
                    }
                    IsAdministrator =  userRow["rank"].ToString() == "21";
                    FactionId = userRow["factionId"].ToString();
                    Clan = Clan != "noclan" ? userRow["tagName"].ToString() : "";
                    if (UserId == 12011)
                        Username = "ADM_" + Username;
                    Program.AddLog(Username, "", "Connected!");
                    
                    if (Program.chatUsers.ContainsKey(UserId))
                    {
                        Program.chatUsers[UserId]._handler.Shutdown(SocketShutdown.Both);
                        Program.chatUsers[UserId]._handler.Close();
                        Program.chatUsers[UserId]._handler = null;
                        Program.chatUsers.Remove(UserId);
                    }

                    Program.chatUsers.Add(UserId, this);
                    var chatName = "Company";
                    if (FactionId == "1")
                        chatName = "MMO";
                    if (FactionId == "2")
                        chatName ="EIC";
                    if (FactionId == "3")
                        chatName = "VRU";
                    var servers = "997|Global|0|-1|0|0}250|Türkçe|1|-1|0|0}255|Español|2|-1|0|0}256|English|2|-1|0|0}25" + FactionId + "|" + chatName + "|1|-1|0|0";
                    Send("by%" + servers + "#");
                    Send("bv%" + UserId + "#");

                    if (banned)
                    {
                        Console.WriteLine("User banned disconnected: " + userRow["Name"]);
                        Send("fk%" + 997 + "@You're banned!#");
                        Send("fk%" + 997 + "@You're banned!#");
                        Send("fk%" + 997 + "@If you want get unban please create a ticket in forum!#");
                        Send("at%#");
                        Disconnect();
                        return;
                    }

                    ChatsJoined.Add(Program.Chatrooms.FirstOrDefault().Value.Id);
                  if(!IsAdministrator)
                      Inactivity();

                }
                else if (content.StartsWith("a"))
                {
                    var cooldown = (DateTime.Now - Check);
                    
                    Count = 1;
                    var packet = content.Replace("@","%").Split('%');
                    var roomId = packet[1];
                    var message = packet[2];
                    if (Username == " " || Username == "") return;
                    if (message == " " || message == "") return;
                 if (cooldown.TotalSeconds < 1 && !IsAdministrator)
                 {
                     Send("fk%" + roomId + "@Don't spam!#");
                     Program.AddLog(Username, message, "Spamblocked!");

                     return;
                 }

                 if (message.ToString().Length > 180 && !IsAdministrator)
                 {
                     Send("fk%" + roomId + "@Max length 180 characters!#");
                     return;
                 }

                 Check = DateTime.Now;
                
                 if (_nonAllowedWords.Any(word => message.ToLower().Contains(word) && !IsAdministrator))
                 {
                     Send("as%#");
                     Disconnect();
                     return;
                 }
                    var cmd = message.Split(' ')[0];
                    if (message == " " || message == "") return;
                    Console.WriteLine(Username + ": " + message + " in room: " + roomId);

                    if (message.StartsWith("/users"))
                    {
                        if (IsAdministrator)
                        {
                            var users = Program.chatUsers.Values.Aggregate(String.Empty, (current, user) => current + user.Username + ", ");

                            users = users.Remove(users.Length - 2);
                            Send("fk%" + roomId + "@" + "Users online " + Program.chatUsers.Count + ": " + users + "#");
                        }
                        else
                        {
                            foreach (var usar in Program.chatUsers)
                            {
                                usar.Value.Send("fk%997@" + Username + " tried to use /users and isn't admin.#");
                            }
                        }
                    }
                    else if (cmd == "/system")
                    {
                        if (IsAdministrator)
                        {
                            var msg = message.Remove(0, 8);
                            foreach (var pair in Program.chatUsers)
                            {
                             
                                    pair.Value.Send("fk%997@" + msg + "#");
                                
                            }
                        }
                                           
                    }
                    else if (message.StartsWith("/reconnect"))
                    {

                        _handler.Shutdown(SocketShutdown.Both);
                        _handler.Close();
                        _handler = null;
                        Program.chatUsers.Remove(UserId);
                    }
                    else switch (cmd)
                    {
                        case "/kick":
                            if (IsAdministrator)
                            {
                                var user = message.Remove(0,6);
                                foreach (var pair in Program.chatUsers)
                                {
                                    if (String.Equals(pair.Value.Username, user, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        pair.Value.Send("as%#");
                                        pair.Value._handler.Shutdown(SocketShutdown.Both);
                                        pair.Value._handler.Close();
                                        pair.Value._handler = null;
                                        Program.AddLog(Username, user, "Kicked client");
                                        Program.chatUsers.Remove(pair.Key);
                                        foreach (var usar in Program.chatUsers)
                                        {
                                           
                                                usar.Value.Send("fk%997@" + user + "  kicked.#");
                                        
                                        }
                                        return;
                                    }
                                }
                            }
                            break;
                        case "/ban":
                            if (IsAdministrator)
                            {
                                var user = message.Remove(0, 5) ;
                                foreach (var pair in Program.chatUsers)
                                {
                                    if (pair.Value.Username.ToLower() == user.ToLower())
                                    {
                                        pair.Value.Send("at%#");
                                        pair.Value._handler.Shutdown(SocketShutdown.Both);
                                        pair.Value._handler.Close();
                                        pair.Value._handler = null;
                                        Program.chatUsers.Remove(pair.Key);
                                        Program.AddLog(Username, user, "Banned client");
                                        Program.BanUser(pair.Value.UserId, pair.Value.Username);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                            
                            }
                            break;
                        case "/unban":
                            if (IsAdministrator)
                            {
                                var user = message.Split(' ')[1];
                                Program.AddLog(Username, user, "Unbanned client");

                                Program.UnbanUser(user);
                            }
                            else
                            {
                            
                            }
                            break;
                        case "/banip":
                            if (IsAdministrator)
                            {
                                var user = message.Split(' ')[1];
                                foreach (var pair in Program.chatUsers)
                                {
                                    if (pair.Value.Username.ToLower() == user.ToLower())
                                    {
                                        pair.Value.Send("at%#");
                                        var ip = pair.Value._handler.RemoteEndPoint.ToString().Split(':')[0];
                                        Program.BanIp(ip);
                                        pair.Value._handler.Shutdown(SocketShutdown.Both);
                                        pair.Value._handler.Close();
                                        pair.Value._handler = null;
                                        Program.chatUsers.Remove(pair.Key);
                                        Program.AddLog(Username, ip, "Banned ip");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                            
                            }
                            break;
                        case "/unbanip":
                            if (IsAdministrator)
                            {
                                var user = message.Split(' ')[1];

                                Program.UnbanIp(user);
                                Program.AddLog(Username, user, "Unbanned ip");
                            }
                            else
                            {
                            
                            }
                            break;
                        case "/w":
                            foreach (var pair in Program.chatUsers)
                            {
                            
                                var user = message.Split(' ')[1];

                                if (user.ToLower() == Username.ToLower())
                                {

                                    Send("fk%" + roomId + "@" + "Error, you can't whisper yourself." + "#");
                                    return;
                                }
                                if (String.Equals(pair.Value.Username.ToLower(), user.ToLower(), StringComparison.CurrentCultureIgnoreCase) && pair.Value.ChatsJoined.Contains(Convert.ToInt32(roomId)))
                                {
                                    message = message.Remove(0, user.Length + 4);
                                    Program.AddLog(Username, user, "Whisper: " + message);
                                    pair.Value.Send("cv%" + Username + "@" + message + "#");
                                    Send("cw%" + user + "@" + message + "#");
                                    return;
                                }
                            }
                            break;
                        case "/create":
                        {
                            var chat = new CustomChatRoom
                            {
                                Name = message.Split(' ')[1],
                                Id = new Random().Next(700, 999),
                                Administrator = Username
                            };
                            Send("cj%" + chat.Id + "@" + chat.Name + "@" +
                                 UserId + "@" + Username + "#");
                            Program.AddLog(Username, "", "Created room: " + chat.Name);
  
                            Program.PrivateChats.Add(chat);
                        }
                            break;
                        case "/invite":
                        {
                            var user = message.Split(' ')[1];
                            var roomname = "";
                            var userfound = false;
                            User userf = null;
                            foreach (var usern in Program.chatUsers.Where(usern => usern.Value.Username.ToLower() == user.ToLower()))
                            {
                                userfound = true;
                                userf = usern.Value;
                            }
                        
                            if (!userfound)
                            {
                                Send("cp#");
                                return;
                            }

                            foreach (var rm in Program.PrivateChats.Where(rm => Convert.ToInt32(roomId) == rm.Id))
                            {
                                roomname = rm.Name;
                            }
                            if (userf == null) return;
                            Send("cr%" + user + "#");
                            userf.Send("cj%" + roomId + "@" + roomname + "@" + UserId + "@" + Username + "#");
                            Program.AddLog(Username, user, "Invited to room");
                        }
                            break;
                        case "/close":
                        {
                            var userfound = false;
                            var chat = new CustomChatRoom();
                            foreach (var usern in Program.PrivateChats.Where(usern => usern.Id == Convert.ToInt32(roomId)))
                            {
                                userfound = true;
                                chat = usern;
                            }
                            if (!userfound)
                            {
                                Send("fk%" + roomId + "@" + "Error, you can't close this chat!" + "#");
                                return;
                            }
                            if (chat.Administrator.ToLower() != Username.ToLower())
                            {
                                Send("fk%" + roomId + "@" + "Error, you can't close this chat!" + "#");
                                return;
                            }
                            foreach (var u in chat.Users)
                            {
                                u.Send("ck%" + chat.Id + "#");
                            }
                            Program.AddLog(Username, "", "closed room: " + chat.Name);
                            Program.PrivateChats.Remove(chat);             
                        }
                            break;
                        case "/leave":
                        {
                            var userfound = false;
                            var chat = new CustomChatRoom();
                            foreach (var usern in Program.PrivateChats)
                            {
                                if (usern.Id == Convert.ToInt32(roomId))
                                {
                                    userfound = true;
                                    chat = usern;
                                }
                            }
                            if (!userfound)
                            {
                                Send("fk%" + roomId + "@" + "Error, you can't leave this chat!" + "#");
                                return;
                            }
                            chat.Users.Remove(this);
                            Program.AddLog(Username, "", "left room: " + chat.Name);
                            var close = false;
                            foreach (var u in chat.Users)
                            {
                                if (Username.ToLower() == chat.Administrator.ToLower())
                                {
                                    u.Send("ck%" + chat.Id + "#");
                                    close = true;
                                }
                                else
                                {
                                    u.Send("eb%" + UserId + "@" + Username + "@" + chat.Id + "#");
                                }
                            }
                            Send("ck%" + chat.Id + "#");
                            if (close || chat.Users.Count == 0)
                            {
                                Program.PrivateChats.Remove(chat);
                            }
                        }
                            break;
                        default:
                            Program.AddLog(Username, roomId, message);
                            foreach (var pair in Program.chatUsers)
                            { 
                                if (pair.Value.ChatsJoined.Contains(Convert.ToInt32(roomId)))
                                {
                                    if (IsAdministrator)
                                    {
                                        if (Clan == "")
                                        {
                                            pair.Value.Send("j%" + roomId + "@" + Username + "@" + message + "#");
                                        }
                                        else
                                        {
                                            pair.Value.Send("j%" + roomId + "@" + Username + "@" + message + "@" + Clan + "#");
                                        }
                                    }
                                    else
                                    {
                                        if (Clan == "")
                                        {
                                            pair.Value.Send("a%" + roomId + "@" + Username + "@" + message + "#");
                                        }
                                        else
                                        {
                                            pair.Value.Send("a%" + roomId + "@" + Username + "@" + message + "@" + Clan + "#"); 
                                        }
                                    }
                               
                                }                                        
                            }
                            break;
                    }

                }
                else if (content.StartsWith("bz"))
                {

                    var oldchat = Convert.ToInt32(content.Split('%')[1]);
                    var newchat = Convert.ToInt32(content.Split('%')[2].Split('@')[0]);
                    foreach (var chat in Program.PrivateChats.Where(chat => chat.Id == newchat && oldchat == newchat))
                    {
                        chat.Users.Add(this);
                        foreach (var user in chat.Users.Where(user => Username != user.Username))
                        {
                            user.Send("ea%" + Username + "%" + newchat + "#");
                        }
                    }
                    if (!ChatsJoined.Contains(newchat))
                    {
                        ChatsJoined.Add(newchat);
                    }
                }
              
              
            }
            catch (SocketException e)
            {
                if (e.NativeErrorCode.Equals(10035))
                    Out.WriteLine("Socket Exception Event: " + e.Message, "Socket.Exception", ConsoleColor.DarkRed);
                else
                {
                    Disconnect();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (_handler != null && _handler.Connected)
                {
                    try
                    {
                        _handler.BeginReceive(_buffer, 0, _buffer.Length, 0,
                             ReadCallBack, this);
                    }
                    catch
                    {
                        
                    }
                   
                }
            }
        }
        public void TESTS(){

            var users = Program.chatUsers.Values.Aggregate(String.Empty, (current, user) => current + user.Username + ", ");

            users = users.Remove(users.Length - 2);
         //   Send("fk%" + 997 + "@" + "Users online " + Program.chatUsers.Count + ": " + users + "#");
            Console.WriteLine("WORK");
        }
        private async void Inactivity()
        {
            while (Program.chatUsers.ContainsKey(UserId))
            {
                await Task.Delay(6000000);
                if (Count == 0)
                {
                    Disconnect();
                    Program.AddLog(Username, "", "disconnected due inactivity!");
                    return;
                }
                Count = 0;
            }
        }

        public void Send(string str)
        {
            try
            {
                if (_handler == null || !_handler.Connected) return;
                var byteData = Encoding.UTF8.GetBytes(str + (char) 0x00);


                _handler.BeginSend(byteData, 0, byteData.Length, 0,
                    SendCallBack, _handler);
            }
            catch
            {
            }
        }

        private static void SendCallBack(IAsyncResult result)
        {
            try
            {
                var handler = (Socket) result.AsyncState;
                handler.EndSend(result);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }
}
