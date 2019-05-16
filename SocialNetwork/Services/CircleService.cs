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
            _circles = Client.GetCollection<Circle>("Circles");
            CreateIndex();
        }

        public void Drop()
        {
            Client.DropCollection("Circles");
        }

        public List<Circle> GetCircles()
        {
            return _circles.Find(circles => true).ToList();
        }

        public Circle GetCircleById(string circleid)
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

        public Circle GetCircleByName(string name)
        {
            try
            {
                var result = _circles.Find(p => p.CircleName == name).ToList()[0];
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

        public void PrintAllCircles()
        {
            List<Circle> circles = GetCircles();

            foreach (var circle in circles)
            {
                Console.WriteLine(circle.CircleName);
            }
        }

        public void CreateIndex()
        {
            //Index on circles name
            var indexKeysName = Builders<Circle>.IndexKeys;
            var indexModelName = new CreateIndexModel<Circle>(indexKeysName.Ascending(c => c.CircleName));
            _circles.Indexes.CreateOne(indexModelName);
        }
    }
}
