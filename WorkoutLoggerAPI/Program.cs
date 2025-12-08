


using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WorkoutLogger.Core.Models;
using WorkoutLoggerAPI.Data;
using WorkoutLoggerAPI.Services;

namespace WorkoutLoggerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Mongo Db
            var mongoDb = new MongoContext("WorkoutLogsDb");
            var excerciseCollection = new ExcerciseService(mongoDb);
            var workoutCollection = new WorkoutService(mongoDb);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            //Excercise Endpoints

            //Add Excercise
            app.MapPost("/addexcercise", async (ExcerciseModel excercise) =>
            {
                var result = await excerciseCollection.AddExcerciseAsync(excercise);

                return Results.Ok(result);
            });

            //Get Excercises
            app.MapGet("/getexcercises", async () =>
            {
                var result = await excerciseCollection.GetExcercisesAsync();

                return result.Count !=0 
                    ? Results.Ok(result) 
                    : Results.NoContent();
            });

            //Get Excercises by Tags
            app.MapPost("/getexcercisesbytags", async (List<string> tags) =>
            {
                var result = await excerciseCollection.GetExcercisesByTags(tags);
                return result.Count != 0
                    ? Results.Ok(result)
                    : Results.Ok("Inga övningar i listan");
            });

            //Get Excercise by Id
            app.MapGet("/getexcercise/{id}", async (string id) =>
            {
                var result = await excerciseCollection.GetExcerciseByIdAsync(id);
                return result is not null 
                       ? Results.Ok(result)
                       : Results.NotFound($"Excercise with id {id} not found");
            });

            //Update Excercise
            app.MapPut("/updateexcercise/{id}", async (string id, ExcerciseModel updatedExcercise) =>
            {
                var result = await excerciseCollection.UpdateExcerciseAsync(id, updatedExcercise);

                return result is not null
                        ? Results.Ok(result)
                        : Results.NotFound($"Excercise with id {id} not found");
            });

            //Delete Excercise
            app.MapDelete("/deleteexcercise/{id}", async (string id) =>
            {
                var result = await excerciseCollection.DeleteExcerciseAsync(id);

                return result 
                        ? Results.Ok(result) 
                        : Results.NotFound($"Excercise with id {id} not found");
            });

            //Seed Data - Only use once to fill db
            app.MapPost("/seedexcercisedata", async () =>
            {
                var result = await excerciseCollection.SeedExcercicesAsync();
                return Results.Ok($"{result} excercises in DB");
            });


            //Workout Endpoints

            //Add Workout
            app.MapPost("/addworkout", async (Workout workout) =>
            {
                var result = await workoutCollection.AddWorkoutAsync(workout);
                return Results.Ok(result);
            }); 

            //Get Workouts
            app.MapGet("/getworkouts", async () =>
            {
                var result = await workoutCollection.GetWorkoutsAsync();
                return Results.Ok(result);
                        
            });

            //Get Workout by Id
            app.MapGet("/getworkout/{id}", async (string id) =>
            {
                var result = await workoutCollection.GetWorkoutByIdAsync(id);
                return result is not null
                        ? Results.Ok(result)
                        : Results.NotFound($"Workout with id {id} not found");
            });

            //Update Workout
            app.MapPut("/updateworkout/{id}", async (string id, Workout updatedWorkout) =>
            {
                var result = await workoutCollection.UpdateWorkoutAsync(id, updatedWorkout);
                return result
                        ? Results.Ok(result)
                        : Results.NotFound($"Workout with id {id} not found");
            });

            //Delete Workout
            app.MapDelete("/deleteworkout/{id}", async (string id) =>
            {
                var result = await workoutCollection.DeleteWorkoutAsync(id);
                return result
                        ? Results.Ok(result)
                        : Results.NotFound($"Workout with id {id} not found");
            });

            app.Run();
        }
    }
}
