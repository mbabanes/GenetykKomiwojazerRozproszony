using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticWCF
{
    class Adaptation
    {
        private int baseValue;
        private Participation[] participation;
        static Random rand = new Random();

        public Adaptation(Participation[] participation)
        {
            this.participation = participation;
        }


        public Adaptation()
        { }

        public void count()
        {
            sort(0, participation.Length - 1);
            countAdaptation();
        }

        private void countAdaptation()
        {
            countBaseValue();
            int tmpBaseValue = baseValue;
            int sum = 0;
            double adaptation;

            for (int i = 0; i < participation.Length; i++)
            {
                if (sum == baseValue)
                    break;
                if ((baseValue - sum) == 1)
                {
                    participation[i].adaptation = 1;
                    break;
                }

                if ((tmpBaseValue % 2) == 1)
                {
                    adaptation = (tmpBaseValue / 2.0d) + 0.5d;
                }
                else
                {
                    adaptation = tmpBaseValue / 2.0d;
                }

                participation[i].adaptation = (int)adaptation;
                sum += (int)adaptation;
                tmpBaseValue -= (int)adaptation;
            }
        }


        private void countBaseValue()
        {
            baseValue = participation.Length;
            if ((baseValue % 2) == 0)
            {
                baseValue = baseValue / 2;
            }
            else
            {
                double help = (baseValue / 2.0d) + 0.5d;
                baseValue = (int)help;
            }        
        }



        private void sort(int lo, int hi)
        {
            int i = lo;
            int j = hi;
            int half = (int)Math.Floor((lo + hi) / 2.0d);
            int x = participation[half].distance;

            do
            {
                while (participation[i].distance < x) i++;
                while (participation[j].distance > x) j--;

                if (i <= j)
                {
                    Participation tmp = participation[i];
                    participation[i] = participation[j];
                    participation[j] = tmp;
                    i++;
                    j--;
                }
            } while (i <= j);

            if (lo < j) sort(lo, j);
            if (i < hi) sort(i, hi);
        }


        public void testSort()
        {
            testPrepareTestsData();

            sort(0, 9);
            Console.WriteLine();
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine(participation[i].distance);
            }
        }

        private void testPrepareTestsData()
        {
            participation = new Participation[10];

            for (int i = 0; i < 10; i++)
            {
                participation[i] = new Participation();
                participation[i].distance = rand.Next(0, 100);
            }
        }

        public void testCountBaseValue()
        {
            participation = new Participation[10];

            int goodResult = 5;
            countBaseValue();
            int result = baseValue;
            bool test = false;
            if (goodResult == result)
            {
                test = true;
            }
           

            participation = new Participation[7];

            goodResult = 4;
            countBaseValue();
            result = baseValue;
            if (goodResult == result)
                test = true;
            else
                test = false;

            Console.WriteLine("CountingBaseValue:" + test);
        }


        public void testCountAdaptation()
        {
            testPrepareTestsData();
            count();
            for(int i = 0; i < participation.Length; i++)
            {
                Console.WriteLine(participation[i].distance + " : " + participation[i].adaptation);
            }
        }
    }
}
