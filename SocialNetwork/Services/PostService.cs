using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    
    public class PostService : BaseService
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService()
        {
            _posts = Client.GetCollection<Post>("Posts");
        }

        public void CreateIndexes()
        {
            //Source https://stackoverflow.com/questions/51248295/mongodb-c-sharp-driver-create-index
            //Answer by: StuiterSlurf

            //Index on authors userid
            var indexKeysAuthor = Builders<Post>.IndexKeys;
            var indexModelAuthor = new CreateIndexModel<Post>(indexKeysAuthor.Ascending(p => p.Author.Id));
            _posts.Indexes.CreateOne(indexModelAuthor);

            //Index on associatedCircles CircleId
            var indexKeysCircle = Builders<Post>.IndexKeys;
            var indexModelCircle = new CreateIndexModel<Post>(indexKeysCircle.Ascending(p => p.AssociatedCircle.Id));
            _posts.Indexes.CreateOne(indexModelCircle);
        }

        public void AddPost(Post postToBeAdded)
        {
            _posts.InsertOne(postToBeAdded);
        }

        public List<Post> GetPostsByAuthorId(string Id)
        {
            return _posts.Find(p => p.Author.Id == Id).ToList();
        }

        public List<Post> GetPostsByCircleId(string Id)
        {
            return _posts.Find(p => p.AssociatedCircle.Id == Id).ToList();
        }
    }
}
