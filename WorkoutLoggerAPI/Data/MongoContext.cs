using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutLogger.Core.Models;

namespace WorkoutLoggerAPI.Data
{
    public class MongoContext
    {
        private IMongoDatabase _db; 
        

        public MongoContext(string database)
        {
            var client = new MongoClient(/*Connectionstring goes here*/);
            _db = client.GetDatabase(database);
        }

        public IMongoCollection<ExcerciseModel> Excercises =>
             _db.GetCollection<ExcerciseModel>("Excercises");

        public IMongoCollection<Workout> Workouts =>
             _db.GetCollection<Workout>("Workouts");

    }
}
