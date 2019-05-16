using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class Post
    {
        public Post()
        {
            CreationTime = DateTime.Now;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Author")]
        public User Author { get; set; }

        [BsonElement("Content")]
        public string Content { get; set; }

        [BsonElement("ContentType")]
        public string ContentType { get; set; }

        [BsonElement("Circle")]
        public Circle AssociatedCircle { get; set; }

        [BsonElement("Comments")]
        public List<Comment> Comments { get; set; }

        [BsonElement("Time of creation")]
        public DateTime CreationTime { get; set; }
    }
}
