using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    
    public class PostService : BaseService
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService()
        {
            Client.DropCollection("Posts");
            _posts = Client.GetCollection<Post>("Posts");
            CreateIndexes();
        }

        public void CreateIndexes()
        {
            //Source https://stackoverflow.com/questions/51248295/mongodb-c-sharp-driver-create-index
            //Answer by: StuiterSlurf

            //Index on authors userid
            var indexKeysAuthor = Builders<Post>.IndexKeys;
            var indexModelAuthor = new CreateIndexModel<Post>(indexKeysAuthor.Ascending(p => p.Author.Id));
            _posts.Indexes.CreateOne(indexModelAuthor);

            //Index on associatedCircles CircleId
            var indexKeysCircle = Builders<Post>.IndexKeys;
            var indexModelCircle = new CreateIndexModel<Post>(indexKeysCircle.Ascending(p => p.AssociatedCircle.Id));
            _posts.Indexes.CreateOne(indexModelCircle);
        }

        public void AddPost(Post postToBeAdded)
        {
            _posts.InsertOne(postToBeAdded);
        }

        public void PostComment(string postId, Comment commentToBeAdded)
        {
            //Takes the first element because the should only ever be one (PostId is unique)
            try
            {
                PostComment((_posts.Find(p => p.Id == postId).ToList())[0], commentToBeAdded);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happened (Post might not exist)");
            }
        }

        public void PostComment(Post post, Comment commentToBeAdded)
        {
            //No concurrency safety (Should probably use some sort of Optimistic Concurrency framework instead)
            try
            {
                postComment(post, commentToBeAdded);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happened (Post might not exist)");
            }
        }

        private void postComment(Post post, Comment commentToBeAdded)
        {
            post.Comments.Add(commentToBeAdded);
            var result = _posts.ReplaceOne(p => p.Id == post.Id, post);
        }

        public List<Post> GetPostsByAuthorId(string Id)
        {
            return _posts.Find(p => p.Author.Id == Id).ToList();
        }

        public List<Post> GetPostsByCircleId(string Id)
        {
            return _posts.Find(p => p.AssociatedCircle.Id == Id).ToList();
        }

        public void PrintPosts(List<Post> posts)
        {
            int i = 0;
            foreach (var post in posts)
            {
                Console.Write("Post #" + ++i+" ");
                if(post.ContentType == "image")
                    PrintPicturePost(post);
                else if(post.ContentType == "text")
                    PrintTextPost(post);
                else
                {
                    Console.WriteLine("This post has no Type");
                }

                Console.WriteLine("");
            }
        }

        public void PrintTextPost(Post post)
        {
            if(post.AssociatedCircle!=null)
                Console.WriteLine("Posted in the circle " + post.AssociatedCircle.CircleName+":");
            Console.WriteLine("Posted by: " + post.Author.Name + " at " + post.CreationTime.ToString());
            Console.WriteLine( "The textpost contains " + post.Content);
            Console.WriteLine("Comments:");
            foreach (var comment in post.Comments)
            {
                Console.WriteLine("Author "+ comment.Author + " wrote: \t" + comment.Text);
            }
        }

        public void PrintPicturePost(Post post)
        {
            if(post.AssociatedCircle != null)
                Console.WriteLine("Posted in the circle " + post.AssociatedCircle.CircleName+":");
            Console.WriteLine("Posted by: " + post.Author.Name + " at " + post.CreationTime.ToString());
            Console.WriteLine("****************************************\n\n\n");
            Console.WriteLine("***\t" + post.Content + "\t***");
            Console.WriteLine("\n\n\n****************************************");
            Console.WriteLine("Comments:");
            foreach (var comment in post.Comments)
            {
                Console.WriteLine("Author " + comment.Author + " wrote: \t" + comment.Text);
            }
        }

        public List<Post> GetFeed(User user)
        {
            List<Post> FriendPostList = new List<Post>();
            List<Post> CirclePostList = new List<Post>();
            List<Post> ResultingList = new List<Post>();

            foreach (var currentUserFriend in user.Friends)
            {
                FriendPostList.AddRange(GetPostsByAuthorId(currentUserFriend.Id));
            }

            foreach (var currentUserCircle in user.Circles)
            {
                CirclePostList.AddRange(GetPostsByCircleId(currentUserCircle.Id));
            }

            ResultingList.AddRange(GetPostsByAuthorId(user.Id));



            foreach (var feedPost in FriendPostList)
            {
                bool isBlocked = false;

                foreach (var person in feedPost.Author.BlockedList)
                {
                    if (person.Id == user.Id)
                    {
                        isBlocked = true;
                    }
                }

                if(!isBlocked)
                    ResultingList.Add(feedPost);      
            }

            foreach (var circlePost in CirclePostList)
            {
                bool isBlocked = false;

                foreach (var person in circlePost.Author.BlockedList)
                {
                    if (person.Id == user.Id)
                    {
                        isBlocked = true;
                    }
                }

                if (!isBlocked)
                {
                    foreach (var post in ResultingList)
                    {
                        if (post.Id == circlePost.Id) continue;
                        ResultingList.Add(circlePost);
                    }
                }
                    
            }

            ResultingList.Sort(delegate(Post post, Post post1) { return post1.CreationTime.CompareTo(post.CreationTime); });

            return ResultingList;
        }
    }
}
