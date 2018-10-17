using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ClientZGUI.GeneticService;
using System.Diagnostics;

namespace Client
{
    class Host
    {
        public string fullAddressOfHost;
        public int counter = 0;

        private GeneticClient geneticClient;
        public Result result;


        private Point[] points;
        private int countOfPoints;
        private int startPoint;

        private int countOfGenerations;

        public string ipOfHost;
        public bool finished = false;
        public long singleTimeOfWork = 0;
        public long allTimeOfWork = 0;
        
        public bool readed = false;
       
        public int lengthOfPopulation;

        public Host() { }

        public Host(string ipOfHost, Point[] points, int startPoint, int lengthOfPopulation, int countOfGenerations)
        {
            this.ipOfHost = ipOfHost;
            this.fullAddressOfHost = "http://" + ipOfHost + ":8888/GeneticService/GeneticService";


            this.points = points;
            this.countOfPoints = points.Length;
            this.startPoint = startPoint;
            this.lengthOfPopulation = lengthOfPopulation;
            this.countOfGenerations = countOfGenerations;
        }



        public void start()
        {
            readed = false;
            finished = false;
            Thread genetic = new Thread(doGenetic);
            genetic.Start();
            counter++;
        }

        public void startWithPopulation()
        {
            readed = false;
            finished = false;
            Thread genetic = new Thread(doGeneticWithPopulation);
            genetic.Start();
            counter++;
        }

        private void doGenetic()
        {
            Stopwatch stopwatch = new Stopwatch();

            openConnection();

            stopwatch.Start();
            goAlgoritm();
            stopwatch.Stop();

            closeConnection();

            singleTimeOfWork = stopwatch.ElapsedMilliseconds;
            allTimeOfWork += singleTimeOfWork;
            finished = true;

            //showResultInConsole();
        }


        private void doGeneticWithPopulation()
        {
            Stopwatch stopwatch = new Stopwatch();

            openConnection();

            stopwatch.Start();
            goAlgoritmWithPopulation();
            stopwatch.Stop();

            closeConnection();


            singleTimeOfWork = stopwatch.ElapsedMilliseconds;
            allTimeOfWork += singleTimeOfWork;
            finished = true;
        }

        private void openConnection()
        {
            geneticClient = new GeneticClient("WSHttpBinding_IGenetic", fullAddressOfHost);
        }

        private void goAlgoritm()
        {
            this.result = geneticClient.startAlg(countOfPoints, points, startPoint, lengthOfPopulation, countOfGenerations);
        }

        private void goAlgoritmWithPopulation()
        {
            if (lengthOfPopulation == result.population.Length)
            {
                this.result = geneticClient.startAlgWithPopulation(countOfPoints, points, startPoint, lengthOfPopulation, countOfGenerations, result.population);
            }
            else
            {
                int[][] pop = new int[this.lengthOfPopulation][];

                for (int i = 0; i < lengthOfPopulation; i++)
                {
                    pop[i] = result.population[i];
                }

                this.result = geneticClient.startAlgWithPopulation(countOfPoints, points, startPoint, lengthOfPopulation, countOfGenerations, pop);
            }
        }

        private void closeConnection()
        {
            geneticClient.Close();
        }

     
    }
}
