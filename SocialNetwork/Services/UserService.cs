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
            SeedDatabase();
            _users = Client.GetCollection<User>("Users");
        }

        private void SeedDatabase()
        {
            AddUser(new User
            {
                Age = 23,
                BlockedList = new List<User>(),
                Circles = new List<Circle>(),
                Friends = new List<User>(),
                Gender = 'm',
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                Name = "Jens Jensen"
            });

            AddUser(new User
            {
                Age = 15,
                BlockedList = new List<User>(),
                Circles = new List<Circle>(),
                Friends = new List<User>(),
                Gender = 'f',
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                Name = "Sine Sunesen"
            });

            AddUser(new User
            {
                Age = 45,
                BlockedList = new List<User>(),
                Circles = new List<Circle>(),
                Friends = new List<User>(),
                Gender = 'm',
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                Name = "Perv Pervsen"
            });

            AddUser(new User
            {
                Age = 34,
                BlockedList = new List<User>(),
                Circles = new List<Circle>(),
                Friends = new List<User>(),
                Gender = 'f',
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                Name = "Line Lystig"
            });
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
            catch(Exception e)
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

        public string GetCircleByName(string name)
        {
            try
            {
                var user = _users.Find(p => p.Name == name).ToList()[0];
                return user.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine("Circle not found");
            }

            return null;
        }

        public string GetUserByName(string name)
        {
            try
            {
                var user = _users.Find(p => p.Name == name).ToList()[0];
                return user.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine("User not found");
            }

            return null;
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
        
    }
}
