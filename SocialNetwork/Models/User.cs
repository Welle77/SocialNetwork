using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [BsonElement("Gender")]
        public char Gender { get; set; }

        [BsonElement("Age")]
        public int Age { get; set; }

        [BsonElement("BlockedList")]
        public List<User> BlockedList { get; set; }

        [BsonElement("Circles")]
        public List<Circle> Circles { get; set; }

        [BsonElement("Friends")]
        public List<User> Friends { get; set; }
    }
}
