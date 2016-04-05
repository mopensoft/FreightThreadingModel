using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightThreading.data
{
    public class Link
    {
        public int ID { get; set; }
        public Station Source { get; set; }
        public Station Target { get; set; }
        public int TravelTime { get; set; }

        public Link()
        {

        }

        public Link(int id, Station source, Station target, int tralvelTime) 
        {
            this.ID = id;
            this.Source = source;
            this.Target = target;
            this.TravelTime = tralvelTime;
        }
    }
}
