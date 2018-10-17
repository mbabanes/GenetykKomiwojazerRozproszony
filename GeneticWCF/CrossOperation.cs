using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticWCF
{
    class CrossOperation
    {
        private static Random rand = new Random();
        private int[] newPerson;
        private int crossPointA;
        private int crossPointB;
        private int momId;
        private int dadId;

        private int chromToReplace;

        private bool[] theSameMomDad;

        private int countOfPoints;
        private int[][] population;

        public CrossOperation(int[][] population, int chromToReplace )
        {
            this.population = population;
            this.chromToReplace = chromToReplace;
            countOfPoints = population[0].Length;
        }


        public void make()
        {
            randCrossPointAAndB();
            randDadAndMom();

            prepareNewPerson();
            prepareTheSameMomDadFlags();

            copyElementsBetweenCrossPointsFromMam();
            findTheSameElementsInNewPersonAndDad();
            findAndCopyRestElementsBetweenCrossPointsFromDad();
            findAndCopyRestElementsFromDad();

            insertNewPersonToPopulation();
        }

        public bool testMake()
        {
            dadId = 3;
            momId = 2;
            crossPointA = 2;
            crossPointB = 4;

            prepareNewPerson();
            prepareTheSameMomDadFlags();
            copyElementsBetweenCrossPointsFromMam();
            findTheSameElementsInNewPersonAndDad();
            findAndCopyRestElementsBetweenCrossPointsFromDad();
            findAndCopyRestElementsFromDad();
            insertNewPersonToPopulation();

            int[] goodResult = { 0, 3, 4, 6, 1, 5, 2 };
            for (int i = 0; i < countOfPoints; i++)
            {
                if (population[chromToReplace][i] != goodResult[i])
                {
                    return false;
                }
            }
            return true;           
        }

        private void randCrossPointAAndB()
        {
            int s = (int)Math.Floor(countOfPoints / 2.0d);
            crossPointA = rand.Next(1, s);
            
            if (crossPointA == 1)
            {
                crossPointB = rand.Next(2, rand.Next(3, s));
            }
            else
            {
                crossPointB = rand.Next((crossPointA + 1), (countOfPoints - 1));
            }
        }

        private void randDadAndMom()
        {
            dadId = rand.Next(1, (population.Length - 1));
            do
            {
                momId = rand.Next(1, (population.Length - 1));
            } while (momId == dadId);          
        }

        

        private void prepareNewPerson()
        {
            this.newPerson = new int[countOfPoints];
            int startPoint = population[0][0];
            this.newPerson[0] = startPoint;
        }


        private void copyElementsBetweenCrossPointsFromMam()
        {
            //Console.WriteLine("Cross Point A:" + crossPointA + " B:" + crossPointB);
            for (int i = crossPointA; i <= crossPointB; i++)
            {
                newPerson[i] = population[momId][i];
            }
        }

        private void prepareTheSameMomDadFlags()
        {
            theSameMomDad = new bool[countOfPoints];
            for (int i = 0; i < theSameMomDad.Length; i++)
            {
                theSameMomDad[i] = false;
            }
        }


        private void findTheSameElementsInNewPersonAndDad()
        {
            for (int i = crossPointA; i <= crossPointB; i++)
            {
                for (int j = crossPointA; j <= crossPointB; j++)
                {
                    if (population[dadId][i] == newPerson[j])
                    {
                        theSameMomDad[i] = true;
                        break;
                    }
                }
            }
        }

        private void findAndCopyRestElementsBetweenCrossPointsFromDad()
        {
            for (int i = crossPointA; i <= crossPointB; i++)
            {
                if (theSameMomDad[i] == false)
                {
                    int value = newPerson[i];
                    bool copiedToNew = false;
                    int indexToReplace = -1;

                    while (copiedToNew == false)
                    {
                        for (int j = 1; j < countOfPoints; j++)
                        {
                            if (value == population[dadId][j])
                            {
                                value = population[momId][j];
                                if (!theSameMomDad[j])
                                {
                                    copiedToNew = true;
                                    indexToReplace = j;
                                }
                                break;
                            }
                        }
                    }

                    newPerson[indexToReplace] = population[dadId][i];
                    theSameMomDad[i] = true;
                    theSameMomDad[indexToReplace] = true;
                }
            }
        }


        private void findAndCopyRestElementsFromDad()
        {
            for (int i = 1; i < countOfPoints; i++)
            {
                if (!theSameMomDad[i])
                {
                    newPerson[i] = population[dadId][i];
                    theSameMomDad[i] = true;
                }
            }
        }


        private void insertNewPersonToPopulation()
        {
            for (int i = 0; i < countOfPoints; i++)
            {
                population[chromToReplace][i] = newPerson[i];
            }
        }
       
    }
}
