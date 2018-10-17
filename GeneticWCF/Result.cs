using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GeneticWCF
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public int[][] population;

        [DataMember]
        public int[] ways;

        [DataMember]
        public long timeOfCounting;


        public Result() { }
    }
}
