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
            _circleService = new CircleService();
            _postService = new PostService();
        }

        private void SetUpCurrentUser()
        {
            User newUser = new User { Id=ObjectId.GenerateNewId(DateTime.Now).ToString(),BlockedList = new List<User>(),Circles=new List<Circle>(),Friends=new List<User>()};
            Console.WriteLine("What is your name?");
            var inputName = Console.ReadLine();
            newUser.Name = inputName;
            Console.WriteLine("What is your age?");
            var inputAge = Console.ReadLine();

            try
            {
                newUser.Age = int.Parse(inputAge);
            }
            catch (Exception e)
            {
                Console.WriteLine("Age value not valid. Your age is set to 18");
                newUser.Age = 18;
            }
            
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

            Console.WriteLine("\nNavn: " + _userService.CurrentUser.Name + "\nAlder: "+ _userService.CurrentUser.Age+"\n");
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
                        PrintFeed();
                        break;

                    case "Wall":
                        PrintUserWall();
                        break;

                    case "Circles":
                        PrintCircleFeed();
                        break;
                    case "CPost":
                        CreatePost();
                        break;
                    case "Comment":
                        CreateComment();                          

                        break;
                    case "JCircle":
                        JoinCircle();
                        break;
                    case "Info":
                        Console.WriteLine("To see your feed, type 'Feed'");
                        Console.WriteLine("To see your friends wall, type 'Wall'");
                        Console.WriteLine("To create a post, type 'CPost'");
                        Console.WriteLine("To create a comment, type 'CComment'");
                        Console.WriteLine("To join a circle, type 'JCircle'");
                        break;
                    default:
                        Console.WriteLine("Command not found");
                        break;
                }
            } while (true);
        }

        #region Helpers

        private string GetContentType()
        {
            string contentType;
            do
            {
                contentType = Console.ReadLine();
            } while (contentType != "image" || contentType != "text");

            return contentType;
        }

        private void PrintFeed()
        {
            Console.WriteLine("Your public feed looks like this:");
            var feedPosts = _postService.GetFeed(_userService.CurrentUser);
            _postService.PrintPosts(feedPosts);
        }

        private void PrintUserWall()
        {
            Console.WriteLine("Type the name of the users wall you want to see.");
            _userService.PrintUserFriends();

            var nameOfUser = Console.ReadLine();
            var userId = _userService.GetUserIdByName(nameOfUser);

            if (userId != null)
            {
                var wallPosts = _postService.GetPostsByAuthorId(userId);
                _postService.PrintPosts(wallPosts);
            }
            else
            {
                Console.WriteLine("User not found");
            }
        }

        private void PrintCircleFeed()
        {
            Console.WriteLine("Type the name of the circle you want to see the feed for.");
            _userService.PrintUserCircles();

            var nameOfCircle = Console.ReadLine();
            var circle = _userService.GetUserCircleByName(nameOfCircle);

            var circlePosts = _postService.GetPostsByCircleId(circle.Id);
            _postService.PrintPosts(circlePosts);
        }

        private void CreatePost()
        {
            Console.WriteLine("If this is part of a circle, type in the name of the circle. Otherwise just press enter.");
            var circleCPost = Console.ReadLine();
            Circle associatedCircle = null;
            if (circleCPost != "")
            {
                var circle = _userService.GetUserCircleByName(circleCPost);
                associatedCircle = _circleService.GetCircle(circle.Id);
            }

            Console.WriteLine("Choose the type of Content. The supported type is image, text");
            var contentType = GetContentType();

            Console.WriteLine("Type the content of the post");
            var contentCPost = Console.ReadLine();

            var post = new Post
            {
                ContentType = contentType,
                AssociatedCircle = associatedCircle,
                Content = contentCPost,
                Author = _userService.CurrentUser,
                Comments = null
            };

            _postService.AddPost(post);
        }

        private void CreateComment()
        {
            Console.WriteLine("Which post do you want to comment on? Type the #");
            Console.WriteLine("Your public feed looks like this:");

            var feedPosts = _postService.GetFeed(_userService.CurrentUser);
            _postService.PrintPosts(feedPosts);

            var input = Console.ReadLine();
            int postNumber;

            try
            {
                postNumber = int.Parse(input);
            }
            catch (Exception e)
            {
                Console.WriteLine("You did not type a number");
                return;
            }

            if (postNumber < 1 || postNumber > feedPosts.Count)
            {
                Console.WriteLine("The specified post does not exist");
                return;
            }

            Console.WriteLine("Enter your comment.");
            var comment = Console.ReadLine();
            var commentToBeAdded = new Comment()
            {
                Author = _userService.CurrentUser,
                Text = comment
            };

            _postService.PostComment(feedPosts[postNumber-1].Id, commentToBeAdded);
        }

        private void JoinCircle()
        {

        }

        #endregion
    }
}