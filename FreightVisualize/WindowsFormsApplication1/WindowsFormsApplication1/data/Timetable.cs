using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FreightThreading.data
{
    public class Timetable
    {
        public Network Network { get; set; }
        public List<Station> StationList { get; set; }
        public List<Train> TrainList { get; set; }
        public string Name { get; set; }

        private Timetable()
        {
            StationList = new List<Station>();
            TrainList = new List<Train>();
            Name = "Default";
        }

        /// <summary>
        /// Load timetable from XML
        /// </summary>
        /// <param name="filename"></param>
        public Timetable(string filename) : this() 
        {
            // Read network include links and stations
            // Read timetable
            loadTimetable(filename);
        }

        public bool AddTrain(Train t)
        {
            TrainList.Add(t);
            return true;
        }

        private void loadTimetable(string xmlFile)
        {
            this.Network = new data.Network();
            XElement xmlDoc = XElement.Load(xmlFile);
            var stations = from station in xmlDoc.Element("stations").Descendants("station")
                           select new Station
                           {
                               ID = Convert.ToInt32(station.Attribute("id").Value),
                               Name = station.Attribute("name").Value
                           };
            foreach (Station s in stations)
                Network.AddStation(s);

            var links = from link in xmlDoc.Element("links").Descendants("link")
                        select new Link
                        {
                            ID = Convert.ToInt32(link.Attribute("id").Value),
                            Source = Network.StationDict[Convert.ToInt32(link.Attribute("source").Value)],
                            Target = Network.StationDict[Convert.ToInt32(link.Attribute("target").Value)],
                            TravelTime = Convert.ToInt32(link.Attribute("traveltime").Value)
                        };
            foreach (Link l in links)
                Network.AddLink(l);

            var trains = xmlDoc.Element("trains").Descendants("train").Select(train => new Train
            {
                ID = train.Attribute("id").Value,
                StopList = new List<Stop>(
                    from stop in train.Descendants("stop")
                    select new Stop
                    {
                        Station = Network.StationDict[Convert.ToInt32(stop.Attribute("sid").Value)],
                        Arrival = Utils.StringToDateTime(stop.Attribute("arrival").Value),
                        Departure = Utils.StringToDateTime(stop.Attribute("departure").Value),
                        Platform = stop.Attribute("platform").Value,
                    }
                )
            });

            // Update train details
            try
            {
                foreach (Train t in trains) 
                {
                    t.Timetable = this;
                    this.AddTrain(t);
                    t.UpdateLink();
                    foreach (Stop s in t.StopList)
                        s.Train = t;
                }
            }
            catch (Exception ex)
            {
                
            }

            
        }
    }
}
