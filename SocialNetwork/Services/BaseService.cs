using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public abstract class BaseService
    {
        protected readonly IMongoDatabase Client;

        protected BaseService()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("SocialNetwork");
            Client = database;
        }
    }
}
