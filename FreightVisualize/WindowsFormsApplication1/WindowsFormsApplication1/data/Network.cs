using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightThreading.data
{
    public class Network
    {
        public Dictionary<int, Station> StationDict { get; set; }
        public Dictionary<int, Link> LinkDict { get; set; }

        public Network()
        {
            StationDict = new Dictionary<int, Station>();
            LinkDict = new Dictionary<int, Link>();
        }

        public bool AddStation(Station s)
        {
            if (StationDict.ContainsKey(s.ID) == false)
            {
                StationDict.Add(s.ID, s);
                return true;
            }
            return false;
        }

        public bool AddLink(Link l)
        {
            if (LinkDict.ContainsKey(l.ID) == false)
            {
                LinkDict.Add(l.ID, l);
                return true;
            }
            return false;
        }

        public Link FindLink(string source, string target)
        {
            return LinkDict.Values.Single(l => l.Source.Name == source && l.Target.Name == target);
        }

        public Link FindLink(Station source, Station target)
        {
            return FindLink(source.Name, target.Name);
        }
    }
}
