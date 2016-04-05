using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightThreading.data
{
    public class Train
    {
        public string ID { get; set; }
        public List<Stop> StopList { get; set; }
        public List<Link> LinkList { get; set; }
        public Timetable Timetable { get; set; }

        public Train()
        {
            StopList = new List<Stop>();
            LinkList = new List<Link>();
        }

        public bool AddStop(Stop s)
        {
            StopList.Add(s);
            return true;
        }

        public Stop FindStop(Station stat)
        {
            return StopList.Find(i => i.Station.ID == stat.ID);
        }

        public void UpdateLink()
        {
            LinkList.Clear();
            for (int i = 0; i < StopList.Count - 1; i++)
            {
                Link l = this.Timetable.Network.FindLink(StopList[i].Station, StopList[i + 1].Station);
                if (l == null)
                    throw new Exception(string.Format("Link from {0} to {1} cannot found", StopList[i].Station.Name, StopList[i + 1].Station.Name));
                LinkList.Add(l);
            }
        }

        public Link FindLink(Station source, Station target)
        {
            return LinkList.Find(l => l.Source.Name == source.Name && l.Target.Name == target.Name);
        }

        public bool HasLink(Link l)
        {
            return LinkList.Contains(l);
        }
    }

    public class Stop
    {
        public Station Station { get; set; }
        public DateTime Arrival{ get; set; }
        public DateTime Departure { get; set; }
        public string Platform { get; set; }
        public Train Train { get; set; }

        public Stop()
        {

        }

        public Stop(Station s, DateTime arr, DateTime dept, string platform)
        {
            this.Station = s;
            this.Arrival = arr;
            this.Departure = dept;
            this.Platform = platform;
        }
    }
}
