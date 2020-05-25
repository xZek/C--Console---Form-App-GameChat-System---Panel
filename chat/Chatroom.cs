// CREATE CHATROOM.CS BY ZEKERİYA- UYSAL
using System;
using System.Collections.Generic;

namespace Do.chat
{
    public class Chatroom
    {
        public string Name { get; set; }
        public Int32 Id { get; set; }
        public Int32 Index { get; set; }
        public Chatroom(Int32 index,String name, Int32 id)
        {
            Index = index;
            Name = name;
            Id = id;
        }
        public Chatroom(String name, Int32 id)
        {
            Name = name;
            Id = id;
            Index = 0;
        }

        public Chatroom()
        {
            
        }

        public override string ToString()
        {
            return  Id + "|" + Name +"|" + Index +"|-1|0|0}";
        }
    }


    public class CustomChatRoom : Chatroom
    {
        public List<User> Users{get;set;}
        public string Administrator { get; set; }
        public CustomChatRoom(String anme, Int32 id) : base (anme,id)
        {
            Users = new List<User>();

        }

        public CustomChatRoom()
        {
           Users = new List<User>();
        }
    }
}
