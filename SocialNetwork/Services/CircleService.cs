using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class CircleService : BaseService
    {
        private readonly IMongoCollection<Circle> _circles;

        public CircleService()
        {
            Client.DropCollection("Circles");
            SeedDatabase();
            _circles = Client.GetCollection<Circle>("Circles");
        }

        private void SeedDatabase()
        {
            CreateCircle(new Circle
            {
                Id=ObjectId.GenerateNewId(DateTime.Now).ToString(),

            });
        }

        public List<Circle> Get()
        {
            return _circles.Find(circles => true).ToList();
        }

        public Circle GetCircle(string circleid)
        {
            try
            {
                var result = _circles.Find(p => p.Id == circleid).ToList()[0];
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Something bad happened (Circle might not exist)");
            }
            return null;
        }
       
        public void CreateCircle(Circle circle)
        {
            _circles.InsertOne(circle);
        }
        public void RemoveCircle(string id)
        {
            _circles.DeleteOne(circle => circle.Id == id);
        }
    }
}
