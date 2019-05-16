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


        public void Start()
        {
            //SeedDatabase();
            SetUpCurrentUser();

            Console.WriteLine("\nNavn: " + _userService.CurrentUser.Name + "\nAlder: "+ _userService.CurrentUser.Age+"\n");
            Console.WriteLine("To see your feed, type 'Feed'");
            Console.WriteLine("To see your friends wall, type 'Wall'");
            Console.WriteLine("To create a post, type 'CPost'");
            Console.WriteLine("To create a comment, type 'Comment'");
            Console.WriteLine("To join a circle, type JCircle");
            Console.WriteLine("To add a new friend, type AFriend");
            Console.WriteLine("To block a user, type BUser");
            Console.WriteLine("To reset database, type Reset");
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
                    case "Reset":
                        _postService.Drop();
                        _userService.Drop();
                        _circleService.Drop();

                        SeedDatabase();
                        SetUpCurrentUser();
                        break;
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
                    case "AFriend":
                        AddFriend();
                        break;
                    case "BUser":
                        BlockUser();
                        break;
                    case "Info":
                        Console.WriteLine("To see your feed, type 'Feed'");
                        Console.WriteLine("To see your friends wall, type 'Wall'");
                        Console.WriteLine("To create a post, type 'CPost'");
                        Console.WriteLine("To create a comment, type 'Comment'");
                        Console.WriteLine("To join a circle, type 'JCircle'");
                        Console.WriteLine("To add a new friend, type AFriend");
                        Console.WriteLine("To block a user, type BUser");
                        Console.WriteLine("To reset database, type Reset");
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
            } while (contentType != "image" && contentType != "text");

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

            var list = _userService.GetAllUsers();
            foreach (var user in list)
            {
                Console.WriteLine(user.Name);
            }

            var nameOfUser = Console.ReadLine();
            var userId = _userService.GetUserIdByName(nameOfUser);

            if (userId != null)
            {
                if (!_userService.IsThisUserBlocked(userId))
                {
                    var wallPosts = _postService.GetPostsByAuthorId(userId);
                    _postService.PrintPosts(wallPosts);
                }
                else
                {
                    Console.WriteLine("User has blocked you");
                }
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
            _userService.PrintUserCircles();
            var circleCPost = Console.ReadLine();
            Circle associatedCircle = null;

            if (circleCPost != "")
            {
                var circle = _userService.GetUserCircleByName(circleCPost);
                associatedCircle = _circleService.GetCircleById(circle.Id);
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
                Comments = new List<Comment>(),
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
            Console.WriteLine("Type the name of the circle you want to join:");
            var listOfCircles = _circleService.GetCircles();
            _circleService.PrintCircles(listOfCircles);

            var input = Console.ReadLine();
            var circle= _circleService.GetCircleByName(input);

            if (circle == null)
            {
                Console.WriteLine("Circle does not exist");
                return;
            }

            _userService.SignUpForCircle(circle);
        }

        private void AddFriend()
        {
            Console.WriteLine("Type the name of the new friend you want to add");
            var users = _userService.GetAllOtherUsers();
            foreach (var user in users)
            {
                Console.WriteLine(user.Name);
            }

            var input = Console.ReadLine();
            string userId = _userService.GetUserIdByName(input);

            if (userId == null)
            {
                Console.WriteLine("User does not exist");
                return;
            }

            _userService.AddFriend(userId); 
        }

        private void SeedDatabase()
        {
            User Jens = new User
            {
                Age = 23,
                BlockedList = new List<User>(),
                Circles = new List<Circle>(),
                Friends = new List<User>(),
                Gender = 'm',
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                Name = "Jens Jensen"
            };
            User Sine = new User
            {
                Age = 15,
                BlockedList = new List<User>(),
                Circles = new List<Circle>(),
                Friends = new List<User>(),
                Gender = 'f',
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                Name = "Sine Sunesen"
            };
            User Perv = new User
            {
                Age = 45,
                BlockedList = new List<User>(),
                Circles = new List<Circle>(),
                Friends = new List<User>(),
                Gender = 'm',
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                Name = "Perv Pervsen"
            };
            User Line = new User
            {
                Age = 34,
                BlockedList = new List<User>(),
                Circles = new List<Circle>(),
                Friends = new List<User>(),
                Gender = 'f',
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                Name = "Line Lystig"
            };

            //Line has blocked Perv
            Line.BlockedList.Add(Perv);

            _userService.AddUser(Jens);
            _userService.AddUser(Sine);
            _userService.AddUser(Perv);
            _userService.AddUser(Line);

            Circle OldShit = new Circle
            {
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                CircleName = "Buy and sell old shit",
                Members = new List<User>(),
            };
            Circle People = new Circle
            {
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                CircleName = "Meet new people",
                Members = new List<User>(),
            };
            Circle Important = new Circle
            {
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString(),
                CircleName = "Discuss important things",
                Members = new List<User>(),
            };

            _circleService.CreateCircle(OldShit);
            _circleService.CreateCircle(People);
            _circleService.CreateCircle(Important);

            Post PostPs = new Post
            {
                AssociatedCircle = OldShit,
                Author = Jens,
                Comments = new List<Comment>(),
                Content = "Playstation 3 for sale!",
                ContentType = "text",
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString()
            };
            Post PostSine = new Post
            {
                AssociatedCircle = People,
                Author = Line,
                Comments = new List<Comment>(),
                Content = "I'd like to meet new people plz",
                ContentType = "text",
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString()
            };
            Post PostImportant1 = new Post
            {
                AssociatedCircle = Important,
                Author = Line,
                Comments = new List<Comment>(),
                Content = "The world is dying",
                ContentType = "text",
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString()
            };
            Post PostImportant2 = new Post
            {
                AssociatedCircle = Important,
                Author = Perv,
                Comments = new List<Comment>(),
                Content = "Trump is a bastard",
                ContentType = "text",
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString()
            };
            Post PostImageOldShit = new Post
            {
                AssociatedCircle = OldShit,
                Author = Jens,
                Comments = new List<Comment>(),
                Content = "imageServer.com/img.png",
                ContentType = "image",
                Id = ObjectId.GenerateNewId(DateTime.Now).ToString()
            };

            _postService.AddPost(PostImageOldShit);
            _postService.AddPost(PostImportant2);
            _postService.AddPost(PostImportant1);
            _postService.AddPost(PostPs);
            _postService.AddPost(PostSine);
        }

        private void SetUpCurrentUser()
        {
            bool confirmed = false;
            do
            {
                Console.WriteLine("Type the name of the user you want to log in as");
                var list = _userService.GetAllUsers();
                foreach (var user in list)
                {
                    Console.WriteLine(user.Name);
                }

                var input = Console.ReadLine();
                var userId = _userService.GetUserIdByName(input);
                if (userId != null)
                {
                    _userService.CurrentUser = _userService.GetUser(userId);
                    confirmed = true;
                }
            } while (confirmed == false);

        }

        private void BlockUser()
        {
            Console.WriteLine("Type the name of the user, you want to block");
            var list = _userService.GetAllOtherUsers();
            foreach (var user in list)
            {
                Console.WriteLine(user.Name);
            }

            var input = Console.ReadLine();

            var userId = _userService.GetUserIdByName(input);

            if (userId != null)
            {
                _userService.BlockUser(userId);
            }
        }

        #endregion
    }
}