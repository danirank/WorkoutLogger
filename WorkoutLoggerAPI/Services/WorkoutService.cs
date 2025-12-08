
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
    public class WorkoutService
    {
        private readonly MongoContext _context;
        private readonly IMongoCollection<Workout> _collection;


        public WorkoutService(MongoContext context)
        {
            _context = context;
            _collection = context.Workouts;
        }

        //Create
        public async Task<Workout?> AddWorkoutAsync(Workout workout)
        {
            try
            {
                await _collection.InsertOneAsync(workout);
                return workout;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding workout: {ex.Message}");
                return null;
            }
        }

        //Read all 
        public async Task<List<Workout>> GetWorkoutsAsync()
        {
            try
            {
                var workouts = await _collection.AsQueryable<Workout>().ToListAsync();
                return workouts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving workouts: {ex.Message}");
                return new List<Workout>();
            }
        }

        //Read by Id 
        public async Task<Workout?> GetWorkoutByIdAsync(string id)
        {
            try
            {
                var workout = await _collection.Find(w => w.Id == id).FirstOrDefaultAsync();
                return workout;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving workout by id: {ex.Message}");
                return null;
            }
        }

        //Update
        public async Task<bool> UpdateWorkoutAsync(string id, Workout updatedWorkout)
        {
            try
            {
                var result = await _collection.ReplaceOneAsync(w => w.Id == id, updatedWorkout);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating workout: {ex.Message}");
                return false;
            }
        }

        //Delete
        public async Task<bool> DeleteWorkoutAsync(string id)
        {
            try
            {
                var result = await _collection.DeleteOneAsync(w => w.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting workout: {ex.Message}");
                return false;
            }
        }
    }
}
