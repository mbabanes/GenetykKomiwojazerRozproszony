using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticWCF
{
    class SelectionOperation
    {
        private int k = 0;
        private int ks = 0;

        private int[][] population;
        private int[][] newPopulation;
        private Participation[] participation;


        public SelectionOperation(int[][] population, Participation[] participation)
        {
            this.population = population;
            this.participation = participation;
            initializeNewPopulation();

        }

        private void initializeNewPopulation()
        {
            newPopulation = new int[population.Length][];

            for (int i = 0; i < population.Length; i++)
            {
                newPopulation[i] = new int[population[0].Length];
            }

        }




        public void make()
        {
            copyBestChromosoms();
            copyRestOfChromosoms();
            putNewPopulation();
        }


        private void copyBestChromosoms()
        {
            for (int i = 0; i < population.Length; i++)
            {
                for (int j = k; j < (k + participation[i].adaptation); j++)
                {
                    population[participation[i].idChrom].CopyTo(newPopulation[j], 0);
                }

                k += participation[i].adaptation;

                if (participation[i].adaptation == 0)
                {
                    ks = i;
                    break;
                }
            }
        }

        private void copyRestOfChromosoms()
        {
            for (int i = k; i < population.Length; i++)
            {
                population[participation[ks].idChrom].CopyTo(newPopulation[i], 0);
                ks++;
            }
        }


        private void putNewPopulation()
        {
            for (int i = 0; i < newPopulation.Length; i++)
            {
                newPopulation[i].CopyTo(population[i], 0);
            }
        }
    }
}
