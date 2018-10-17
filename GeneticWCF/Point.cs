using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GeneticWCF
{
    [DataContract]
    public class Point 
    {
        [DataMember]
        public int x;

        [DataMember]
        public int y;

        public Point() { }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
