﻿using System.Collections.Generic;
using System.Linq;
using TheaterSchedule.DAL.Interfaces;
using Entities.Models;

namespace TheaterSchedule.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly TheaterDatabaseContext db;

        public MessageRepository(TheaterDatabaseContext context)
        {
            this.db = context;
        }

        public void Add(Message message)
        {
            db.Message.Add(message);
        }

        public Message GetMessageById(int id)
        {
            return db.Message.FirstOrDefault(m => m.MessageId == id);
        }

        public List<Message> GetUnrepliedMessages()
        { 
            return db.Message.ToList();
        }
    }
}
