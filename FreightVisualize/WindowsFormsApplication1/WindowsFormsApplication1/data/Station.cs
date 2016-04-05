using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightThreading.data
{
    public class Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<string> Platforms { get; set; }

        public Station()
        {

        }

        public Station(int id, string name)
        {
            Name = name;
            ID = id;
        }

        public Station(int id, string name, List<string> platforms)
            : this(id, name)
        {
            this.Platforms = platforms;
        }
    }
}
