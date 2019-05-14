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
    }
}
