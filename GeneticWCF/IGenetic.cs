using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GeneticWCF
{
   
    [ServiceContract]
    public interface IGenetic
    {
      
        [OperationContract]
        Result startAlg(int countOfPoints, Point[] points, int startPoint, int lengthOfPopulation, int countOfGenerations);

        [OperationContract]
        Result startAlgWithPopulation(int countOfPoints, Point[] points, int startPoint, int lengthOfPopulation, int countOfGenerations, int[][]population);
    }
}
