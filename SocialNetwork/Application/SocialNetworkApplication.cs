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
            _userService = new UserService(name);
            _postService = new PostService();
            _circleService = new CircleService();
        }

        public void Start()
        {
            Console.WriteLine("Navn: " + _userService.CurrentUser.Name + "\nAlder: " + _userService.CurrentUser.Age + "\n");
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
                            _postService.PrintPosts(list);
                        }

                        Console.WriteLine("Your circle feed looks like this");
                        foreach (var currentUserCircle in _userService.CurrentUser.Circles)
                        {
                            var list = _postService.GetPostsByCircleId(currentUserCircle.Id);
                            _postService.PrintPosts(list);
                        }
                        break;

                    case "Wall":
                        Console.WriteLine("Type the name of the users wall you want to see.");
                        foreach (var currentUserFriend in _userService.CurrentUser.Friends)
                        {
                            Console.WriteLine(currentUserFriend.Name);
                        }

                        var nameOfWall = Console.ReadLine();
                        var userId = _userService.GetUserByName(nameOfWall);

                        var wallPosts = _postService.GetPostsByAuthorId(userId);
                        _postService.PrintPosts(wallPosts);
                        break;

                    case "Circles":
                        Console.WriteLine("Type the name of the circle you want to see the feed for.");
                        foreach (var currentUserCircle in _userService.CurrentUser.Circles)
                        {
                            Console.WriteLine(currentUserCircle.CircleName);
                        }

                        var nameOfCircle = Console.ReadLine();
                        var circleId = _userService.GetCircleByName(nameOfCircle);

                        var circlePosts = _postService.GetPostsByCircleId(circleId);
                        _postService.PrintPosts(circlePosts);


                        break;
                    case "CPost":
                        Console.WriteLine("If this is part of a circle, type in the name of the circle. Otherwise just press enter.");
                        var circleCPost = Console.ReadLine();
                        Circle associatedCircle = null;
                        if (circleCPost != "")
                        {
                            var circleByName = _userService.GetCircleByName(circleCPost);
                            
                        }
                        Console.WriteLine("Choose the type of Content. The supported type is image, text");
                        var contentType = ContentType();
                        Console.WriteLine("Type the content of the post");


                        var post = new Post(){ContentType = contentType, AssociatedCircle =};
                        _postService.AddPost(post);
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

        private string ContentType()
        {
            string contentType;
            do
            {
                contentType = Console.ReadLine();
            } while (contentType != "image" || contentType != "text");

            return contentType;
        }
    }
}