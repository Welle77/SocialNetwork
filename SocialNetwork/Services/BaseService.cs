using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace SocialNetwork.Services
{
    public abstract class BaseService
    {
        private readonly IMongoDatabase _client;

        public BaseService()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("BookstoreDb");
            _client = database;
        }
    }
}
