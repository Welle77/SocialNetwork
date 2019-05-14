using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class UserService : BaseService
    {
        private readonly IMongoCollection<User> _users;

        public UserService()
        {
            _users = Client.GetCollection<User>("Users");
        }

        public void AddUser(User userToBeAdded)
        {
            _users.InsertOne(userToBeAdded);
        }

    }
}
