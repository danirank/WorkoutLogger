using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutLogger.Core.Constants;
using WorkoutLogger.Core.Models;
using WorkoutLoggerAPI.Data;


namespace WorkoutLoggerAPI.Services
{
    public class ExcerciseService
    {

        private readonly MongoContext _context;
        private readonly IMongoCollection<ExcerciseModel> _collection;

        public ExcerciseService(MongoContext context)
        {
            _context = context;
            _collection = context.Excercises;
        }
        //Create
        public async Task<ExcerciseModel?> AddExcerciseAsync(ExcerciseModel excercise)
        {
            try
            {
                await _collection.InsertOneAsync(excercise);
                return excercise;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding excercise: {ex.Message}");
                return null;
            }
        }

        //Read all
        public async Task<List<ExcerciseModel>> GetExcercisesAsync()
        {
            try
            {
                var excercises = await _collection.AsQueryable<ExcerciseModel>().ToListAsync();
                return excercises;

            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred while retrieving excercises: {ex.Message}");
                return new List<ExcerciseModel>();

            }
        }
        
        //Read by id
        public async Task<ExcerciseModel?> GetExcerciseByIdAsync(string id)
        {
            try
            {
                var excercise = await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
                return excercise;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving excercise by id: {ex.Message}");

                return null;
            }
        }

        //Read by tags
        public async Task<List<ExcerciseModel>> GetExcercisesByTags(List<string> tags)
        {
            try
            {
                var filter = Builders<ExcerciseModel>.Filter.AnyIn(e => e.Tags, tags);
                var excercises = await _collection.Find(filter).ToListAsync();
                return excercises;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving excercises by tags: {ex.Message}");
                return new List<ExcerciseModel>();
            }
        }

        //Update
        public async Task<ExcerciseModel?> UpdateExcerciseAsync(string id, ExcerciseModel updatedExcercise)
        {
            try
            {
                var result = await _collection.ReplaceOneAsync(e => e.Id == id, updatedExcercise);
                return result.ModifiedCount == 1  ? updatedExcercise : null;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating excercise: {ex.Message}");
                return null;
            }
        }
        
        
        //Delete
        public async Task<bool> DeleteExcerciseAsync(string id)
        {
            try
            {
                var result = await _collection.DeleteOneAsync(e => e.Id == id);
                return result.DeletedCount > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting excercise: {ex.Message}");
                return false;
            }

        }

        //Seed Data
        //Use one time to seed excercises to MongoDb
        public async Task<int> SeedExcercicesAsync()
        {
            try
            {
                List<ExcerciseModel> SeedExercises = new()
            {
            // ---- Chest / UpperBody ----
            new ExcerciseModel { Name = "Bänkpress", Tags = new(){ Tags.Chest, Tags.UpperBody } },
            new ExcerciseModel { Name = "Armhävningar", Tags = new(){ Tags.Chest, Tags.UpperBody } },

            // ---- Back / UpperBody ----
            new ExcerciseModel { Name = "Chins", Tags = new(){ Tags.Back, Tags.UpperBody } },
            new ExcerciseModel { Name = "Skivstångsrodd", Tags = new(){ Tags.Back, Tags.UpperBody } },

            // ---- Shoulders / UpperBody ----
            new ExcerciseModel { Name = "Militärpress", Tags = new(){ Tags.Shoulders, Tags.UpperBody } },
            new ExcerciseModel { Name = "Hantellyft åt sidan", Tags = new(){ Tags.Shoulders, Tags.UpperBody } },

            // ---- Biceps / UpperBody ----
            new ExcerciseModel { Name = "Hantelcurl", Tags = new(){ Tags.Biceps, Tags.UpperBody } },
            new ExcerciseModel { Name = "Skivstångscurl", Tags = new(){ Tags.Biceps, Tags.UpperBody } },

            // ---- Triceps / UpperBody ----
            new ExcerciseModel { Name = "Dips", Tags = new(){ Tags.Triceps, Tags.UpperBody } },
            new ExcerciseModel { Name = "Triceps Pushdown", Tags = new(){ Tags.Triceps, Tags.UpperBody } },

            // ---- Core / Abs ----
            new ExcerciseModel { Name = "Plankan", Tags = new(){ Tags.Core, Tags.Abs } },
            new ExcerciseModel { Name = "Hängande Benlyft", Tags = new(){ Tags.Abs, Tags.Core } },

            // ---- Lower Body ----
            new ExcerciseModel { Name = "Knäböj", Tags = new(){ Tags.Quads, Tags.Glutes, Tags.LowerBody } },
            new ExcerciseModel { Name = "Marklyft", Tags = new(){ Tags.Hamstrings, Tags.Back, Tags.LowerBody } },
            new ExcerciseModel { Name = "Benpress", Tags = new(){ Tags.Quads, Tags.LowerBody } },
            new ExcerciseModel { Name = "Utfall", Tags = new(){ Tags.Glutes, Tags.Quads, Tags.LowerBody } },
            new ExcerciseModel { Name = "Vadpress", Tags = new(){ Tags.Calves, Tags.LowerBody } },

            // ---- Explosive / Olympic Lifts ----
            new ExcerciseModel { Name = "Snatch", Tags = new(){ Tags.OlympicLifts, Tags.Explosive, Tags.FullBody } },
            new ExcerciseModel { Name = "Clean & Jerk", Tags = new(){ Tags.OlympicLifts, Tags.Explosive, Tags.FullBody } },
            new ExcerciseModel { Name = "Power Clean", Tags = new(){ Tags.OlympicLifts, Tags.Explosive, Tags.FullBody } },

            // ---- Gymnastics ----
            new ExcerciseModel { Name = "Toes to Bar", Tags = new(){ Tags.Gymnastics, Tags.Core, Tags.UpperBody } },
            new ExcerciseModel { Name = "Ring Muscle-Up", Tags = new(){ Tags.Gymnastics, Tags.FullBody } },
            new ExcerciseModel { Name = "Handstand Push-Up", Tags = new(){ Tags.Gymnastics, Tags.Shoulders, Tags.UpperBody } },

            // ---- Conditioning ----
            new ExcerciseModel { Name = "Burpees", Tags = new(){ Tags.Conditioning, Tags.FullBody } },
            new ExcerciseModel { Name = "Roddmaskin", Tags = new(){ Tags.Conditioning, Tags.FullBody } },
            new ExcerciseModel { Name = "Assault Bike", Tags = new(){ Tags.Conditioning, Tags.FullBody } }
            };
                await _collection.InsertManyAsync(SeedExercises);
                var count = await _collection.CountDocumentsAsync(_ => true);
                return (int)count;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding excercises: {ex.Message}");
                return 0;
            }

        }
    }
}
