using System;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.Bson;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Application
{
    public class SocialNetworkApplication
    {
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly CircleService _circleService;

        public SocialNetworkApplication()
        {
            _userService=new UserService();
            _postService=new PostService();
            _circleService = new CircleService();
        }

        private void SetUpCurrentUser()
        {
            User newUser = new User { Id=ObjectId.GenerateNewId(DateTime.Now).ToString()};
            Console.WriteLine("What is your name?");
            var inputName = Console.ReadLine();
            newUser.Name = inputName;
            Console.WriteLine("What is your age?");
            var inputAge = Console.ReadLine();
            newUser.Age = int.Parse(inputAge);
            Console.WriteLine("Type f for female, m for male (Yes there are only two genders)");
            var inputGender = Console.ReadKey();

            if(inputGender.KeyChar=='m' || inputGender.KeyChar=='f')
                newUser.Gender = inputGender.KeyChar;
            else
            {
                newUser.Gender = 'u';
            }

            _userService.AddUser(newUser);
            _userService.CurrentUser = newUser;
        }


        public void Start()
        {
            SetUpCurrentUser();

            Console.WriteLine("Navn: " + _userService.CurrentUser.Name + "\nAlder: "+ _userService.CurrentUser.Age+"\n");
            Console.WriteLine("To see your feed, type 'Feed'");
            Console.WriteLine("To see your friends wall, type 'Wall'");
            Console.WriteLine("To create a post, type 'CPost'");
            Console.WriteLine("To create a comment, type 'CComment'");
            Console.WriteLine("To see your options again, type 'Info'");
            LongAssSwitchStatement();
        }

        private void LongAssSwitchStatement()
        {
            do
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "Feed":
                        //Query
                        break;
                    case "Wall":
                        //Query
                        break;
                    case "Circles":
                        //Query
                        break;
                    case "CPost":
                        //Query
                    break;
                    case "Comment":
                        //Query
                    break;
                    case "Info":
                        Console.WriteLine("To see your feed, type 'Feed'");
                        Console.WriteLine("To see your friends wall, type 'Wall'");
                        Console.WriteLine("To create a post, type 'CPost'");
                        Console.WriteLine("To create a comment, type 'CComment'");
                        break;
                    default:
                        Console.WriteLine("Command not found");
                        break;
                }
            } while (true);
        }
    }
}