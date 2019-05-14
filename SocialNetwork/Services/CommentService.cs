using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class CommentService : BaseService
    {
        private readonly IMongoCollection<Comment> _comments;

        public CommentService()
        {
            _comments = Client.GetCollection<Comment>("Comments");
        }
    }
}
