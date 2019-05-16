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

        public SocialNetworkApplication(string name)
        {
            _userService=new UserService(name);
            _postService=new PostService();
            _circleService = new CircleService();
        }

        public void Start()
        {
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
                        Console.WriteLine("Your public feed looks like this:");
                        foreach (var currentUserFriend in _userService.CurrentUser.Friends)
                        {
                            var list = _postService.GetPostsByAuthorId(currentUserFriend.Id);         
                            _postService.PrintPost(list);
                        }

                        Console.WriteLine("Your circle feed looks like this");
                        foreach (var currentUserCircle in _userService.CurrentUser.Circles)
                        {
                            var list = _postService.GetPostsByCircleId(currentUserCircle.Id);
                            _postService.PrintPost(list);
                        }
                        
                        break;
                    case "Wall":
                        Console.WriteLine("Type the name of the users wall you want to see:");
                        foreach (var currentUserFriend in _userService.CurrentUser.Friends)
                        {
                            Console.WriteLine(currentUserFriend.Name);
                        }
                        var name = Console.ReadLine();


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