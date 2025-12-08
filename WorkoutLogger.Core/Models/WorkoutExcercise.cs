using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutLogger.Core.Models
{
    //Model for an excercise within a workout
    public class WorkoutExcercise
    {
        public string ExcerciseId { get; set; }

        public string ExcerciseName { get; set; }

        public int Set { get; set; }

        public int Reps { get; set; }

        

        public double Weight { get; set; }

        public double ExcerciseVolume  => Set * Reps * Weight;

        public string Comment { get; set;}
    }
}
