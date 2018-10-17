using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;

namespace GeneticWCF
{
   
    public class GeneticService : IGenetic
    {
        public Result startAlg(int countOfPoints, Point[] points, int startPoint, int lengthOfPopulation, int countOfGenerations)
        {
            Genetic genetic = new Genetic(lengthOfPopulation, countOfGenerations, countOfPoints, points, startPoint);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            genetic.start();
            stopwatch.Stop();

            Result result = new Result();

            result.ways = genetic.getBestWay();
            result.population = genetic.getPopulation();
            result.timeOfCounting = stopwatch.ElapsedMilliseconds;

            return result;
        }

        public Result startAlgWithPopulation(int countOfPoints, Point[] points, int startPoint, int lengthOfPopulation, int countOfGenerations, int[][] population)
        {
            Genetic genetic = new Genetic(lengthOfPopulation, countOfGenerations, countOfPoints, points, startPoint, population);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            genetic.start();
            stopwatch.Stop();

            Result result = new Result();

            result.ways = genetic.getBestWay();
            result.population = genetic.getPopulation();
            result.timeOfCounting = stopwatch.ElapsedMilliseconds;

            return result;
        }            
    }
}
