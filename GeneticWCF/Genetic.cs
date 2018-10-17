using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticWCF
{
    class Genetic
    {
        private int countOfGenerations;
        private int lengthOfPopulation;
        private static Random rand = new Random();

        private int[][] population;
        private Point[] points;
       
        private int countOfPoints;
        private int[][] distances;

        private float crossProbably = 0.2f;
        private float mutationProbably = 0.1f;

        private Participation[] participation;

        private enum Operation { Mutation, Cross, NoOperation };

        private int startPoint;


        public Genetic()
        {

        }

        public Genetic(int lengthOfPopulation, int countOfGenerations, int countOfPoints, Point[] points, int startPoint)
        {
            this.lengthOfPopulation = lengthOfPopulation;
            this.countOfGenerations = countOfGenerations;
            this.countOfPoints = countOfPoints;
            this.points = points;
            this.startPoint = startPoint;

            initializePopulationConnectionsAndParticipation();
        }

        public Genetic(int lengthOfPopulation, int countOfGenerations, int countOfPoints, Point[] points, int startPoint, int[][]population)
        {
            this.lengthOfPopulation = lengthOfPopulation;
            this.countOfGenerations = countOfGenerations;
            this.countOfPoints = countOfPoints;
            this.points = points;
            this.startPoint = startPoint;
            this.population = population;

            initializeConnectionsAndParticipation();
        }

        private void initializePopulationConnectionsAndParticipation()
        {
            population = new int[lengthOfPopulation][];

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new int[countOfPoints];
            }

            distances = new int[countOfPoints][];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = new int[countOfPoints];
            }

            participation = new Participation[lengthOfPopulation];
        }


        private void initializeConnectionsAndParticipation()
        {
            distances = new int[countOfPoints][];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = new int[countOfPoints];
            }

            participation = new Participation[lengthOfPopulation];
        }

        private void countDistancesBetweenPoints()
        {
            for (int i = 0; i < distances.Length; i++)
            {
                for (int j = 0; j < distances.Length; j++)
                {
                    if (i == j)
                    {
                        distances[i][j] = 0;
                    }
                    else
                    {
                        int x = (int)Math.Pow((points[j].x - points[i].x), 2);
                        int y = (int)Math.Pow((points[j].y - points[i].y), 2);

                        distances[i][j] = (int)Math.Round(Math.Sqrt(x + y));
                    }
                }
            }
        }


        public void start()
        {
            go();
        } 

        private void go()
        {
            countDistancesBetweenPoints();
            randFirstPopulation();
           
            for (int i = 0; i < countOfGenerations; i++)
            {                
                countParticipation();               
                selection();
                operations();
            }         
        }

        private void randFirstPopulation()
        {
            bool[] usedPoints = new bool[countOfPoints];          

            for (int i = 0; i < population.Length; i++)
            {
                for (int q = 0; q < usedPoints.Length; q++)
                {
                    usedPoints[q] = false;
                }
                usedPoints[startPoint] = true;

                for (int j = 1; j < population[0].Length; j++)
                {
                    population[i][0] = startPoint;
                    while (true)
                    {
                        population[i][j] = rand.Next(0, population[0].Length);

                        if (usedPoints[population[i][j]] == false)
                        {
                            usedPoints[population[i][j]] = true;
                            break;
                        }
                    }
                }
            }
        }


        private void countParticipation()
        {
            countDistanceOfEachChromAndSaveToParticipation();
            Adaptation adaptation = new Adaptation(participation);
            adaptation.count();
        }

        private void countDistanceOfEachChromAndSaveToParticipation()
        {
            int distance;
            for (int i = 0; i < lengthOfPopulation; i++)
            {
                distance = 0;
                for (int j = 0; j < (countOfPoints - 1); j++)
                {
                    int a = population[i][j];
                    int b = population[i][j + 1];
                    
                    distance += distances[a][b];
                }

                participation[i] = new Participation();
                participation[i].idChrom = i;
                participation[i].distance = distance;
            }
        }


        private void selection()
        {
            SelectionOperation selectionOperation = new SelectionOperation(population, participation);
            selectionOperation.make();
        }

        private void operations()
        {
            for (int i = 0; i < population.Length; i++)
            {
                switch ( randOperation() )
                {
                    case Operation.Mutation:
                        {
                            int chromToMutation = i;
                            MutationOperation mutation = new MutationOperation(population, chromToMutation);
                            mutation.make();
                            break;
                        }
                    case Operation.Cross:
                        {
                            int chromToReplace = i;
                            CrossOperation cross = new CrossOperation(population, chromToReplace);
                            cross.make();
                            break;
                        }
                }
            }
        }

        private Operation randOperation()
        {
            float value = (float) rand.NextDouble();
           

            if ( mutationProbably < crossProbably )
            {
                if ( (value >= 0) && (value <= mutationProbably) )
                {
                    return Operation.Mutation;
                }

                if ( (value > mutationProbably) && (value <= (crossProbably + mutationProbably) ) )
                {
                    return Operation.Cross;
                }
            }
            else
            {
                if ( (value >= 0) && (value <= crossProbably) )
                {
                    return Operation.Cross;
                }

                if ( (value > crossProbably) && (value <= (crossProbably + mutationProbably)) )
                {
                    return Operation.Mutation;
                }
            }

            return Operation.NoOperation;
        }


        public int[][] getPopulation()
        {
            return population;
        }

        public int[] getBestWay()
        {
            int[] bestWays = new int[population.Length];

            for (int i = 0; i < participation.Length; i++)
            {
                bestWays[i] = participation[i].distance;
                //Console.WriteLine(participation[i].distance);
            }

            return bestWays;
        }
    }
}
