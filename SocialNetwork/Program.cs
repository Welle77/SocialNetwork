using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using SocialNetwork.Application;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var app = new SocialNetworkApplication("Jesper Strøm");
            app.Start();
            

        }
    }
}
