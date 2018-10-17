using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticWCF
{
    class MutationOperation
    {
        private static Random rand = new Random();

        private int mutationPointA;
        private int mutationPointB;
        private int countOfPoints;

        private int[][] population;

        private int chromToMutation;
        private int[] newPerson;

       

        public MutationOperation(int[][] population, int chromToMutation)
        {
            this.population = population;
            this.chromToMutation = chromToMutation;
            this.countOfPoints = population[0].Length;
        }


        public void make()
        {
            randMutationPointAAndB();
            prepareNewPerson();
            makeInversion();
            insertNewPersonToPopulation();
        }


        public bool testMake()
        {
            mutationPointA = 2;
            mutationPointB = 4;

            prepareNewPerson();
            makeInversion();

            int[] goodResult = { 0, 1, 5, 4, 3, 2, 6 };

            for (int i = 0; i < countOfPoints; i++)
            {
                if (newPerson[i] != goodResult[i])
                {
                    return false;
                }
            }

            return true;


        }


        private void randMutationPointAAndB()
        {
            int p = (int)Math.Floor(countOfPoints / 2.0d);

            int mutationPointA = rand.Next(1, p);
            int mutationPointB = rand.Next((p + 1), (countOfPoints - 1));
        }




        private void prepareNewPerson()
        {
            newPerson = new int[countOfPoints];
        }


        private void makeInversion()
        {
            int j = mutationPointA;
            for (int i = mutationPointB; i >= mutationPointA; i--)
            {
                newPerson[j] = population[chromToMutation][i];
                j++;
            }

            for (int i = 0; i < mutationPointA; i++)
            {
                newPerson[i] = population[chromToMutation][i];
            }

            for (int i = mutationPointB + 1; i < countOfPoints; i++)
            {
                newPerson[i] = population[chromToMutation][i];
            }
        }


        private void insertNewPersonToPopulation()
        {
            for (int i = 0; i < countOfPoints; i++)
            {
                population[chromToMutation][i] = newPerson[i];
            }
        }

       


    }
}
