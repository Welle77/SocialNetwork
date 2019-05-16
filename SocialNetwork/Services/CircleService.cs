using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        public List<Circle> Get()
        {
            return _circles.Find(circles => true).ToList();
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
