using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("BlockedList")]
        public List<User> BlockedList { get; set; }

        [BsonElement("Circles")]
        public List<Circle> Circles { get; set; }

        [BsonElement("Friends")]
        public List<User> Friends { get; set; }


        //Singleton pattern for user
        private static readonly User Instance = new User();
        private User() { }

        public static User GetUser()
        {
            return Instance;
        }
    }
}
