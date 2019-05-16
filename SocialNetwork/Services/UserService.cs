using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork.Application;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class UserService : BaseService
    {
        public User CurrentUser;
        private readonly IMongoCollection<User> _users;

        public UserService()
        {
            Client.DropCollection("Users");
            _users = Client.GetCollection<User>("Users");
        }

        public void AddUser(User userToBeAdded)
        {
            _users.InsertOne(userToBeAdded);
        }

        public void AddFriend(string objectId)
        {
            try
            {
                var result = _users.Find(p => p.Id == objectId).ToList()[0];
                CurrentUser.Friends.Add(result);
                _users.ReplaceOne(p => p.Id == CurrentUser.Id, CurrentUser);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happened (User might not exist)");
            }
        }

        public void RemoveFriend(string objectId)
        {
            try
            {
                var result = _users.Find(p => p.Id == objectId).ToList()[0];
                var userFound = CurrentUser.Friends.Remove(result);
                _users.ReplaceOne(p => p.Id == CurrentUser.Id, CurrentUser);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happened (User might not exist)");
            }
        }


        public void BlockUser(string objectId)
        {
            try
            {
                var result = _users.Find(p => p.Id == objectId).ToList()[0];
                CurrentUser.BlockedList.Add(result);
                _users.ReplaceOne(p => p.Id == CurrentUser.Id, CurrentUser);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happened (User might not exist)");
            }
        }

        public void UnblockUser(string objectId)
        {
            try
            {
                var result = _users.Find(p => p.Id == objectId).ToList()[0];
                var userFound = CurrentUser.BlockedList.Remove(result);
                _users.ReplaceOne(p => p.Id == CurrentUser.Id, CurrentUser);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happened (User might not exist)");
            }
        }

        public void SignUpForCircle(Circle circle)
        {
            CurrentUser.Circles.Add(circle);
            _users.ReplaceOne(p => p.Id == CurrentUser.Id, CurrentUser);
        }

        public void RemoveFromCircle(Circle circle)
        {
            CurrentUser.Circles.Remove(circle);
            _users.ReplaceOne(p => p.Id == CurrentUser.Id, CurrentUser);
        }

        public Circle GetUserCircleByName(string name)
        {
            var circle = CurrentUser.Circles.Find(p => p.CircleName == name);
            return circle;
        }
        
        public string GetUserIdByName(string name)
        {
            try
            {
                var user = _users.Find(p => p.Name == name).ToList()[0];
                return user.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("User not found");
            }

            return null;
        }

        public void PrintUserCircles()
        {
            foreach (var circle in CurrentUser.Circles)
            {
                Console.WriteLine(circle.CircleName);
            }
        }

        public void PrintUserFriends()
        {
            foreach (var friend in CurrentUser.Friends)
            {               
                Console.WriteLine(friend.Name);
            }
        }

        public User GetUser(string objectId)
        {
            try
            {
                var result = _users.Find(p => p.Id == objectId).ToList()[0];
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happened (User might not exist)");
            }

            return null;
        }

        public List<User> GetAllOtherUsers()
        {
            return _users.Find(p => p.Id != CurrentUser.Id).ToList();
        }

        public List<User> GetAllUsers()
        {
            return _users.Find(p=>true).ToList();
        }

        public bool IsThisUserBlocked(string objectId)
        {
            var result = _users.Find(p => p.Id == objectId).ToList()[0];
            if (result.BlockedList.Contains(CurrentUser))
            {
                return true;
            }

            return false;
        }

        public void CreateIndex()
        {
            //Index on users name
            var indexKeysName = Builders<User>.IndexKeys;
            var indexModelName = new CreateIndexModel<User>(indexKeysName.Ascending(u => u.Name));
            _users.Indexes.CreateOne(indexModelName);
        }     
    }
}
