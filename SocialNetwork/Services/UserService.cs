using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Application;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class UserService : BaseService
    {
        public readonly User CurrentUser;
        private readonly IMongoCollection<User> _users;

        public UserService(string name)
        {
            _users = Client.GetCollection<User>("Users");
            CurrentUser = _users.Find(p => p.Name == name).ToList()[0];
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
