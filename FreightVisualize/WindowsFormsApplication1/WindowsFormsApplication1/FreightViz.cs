using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

// Graph library namespaces
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Layout.MDS;
using gColor = Microsoft.Msagl.Drawing.Color;
using gLabel = Microsoft.Msagl.Drawing.Label;
using gMouseButtons = System.Windows.Forms.MouseButtons;
using gNode = Microsoft.Msagl.Core.Layout.Node;
using gPoint = Microsoft.Msagl.Core.Geometry.Point;

using FontStyle = System.Drawing.FontStyle;
using Color = System.Drawing.Color;

using FreightThreading.data;

namespace FreightThreading
{
    public partial class FreightViz : Form
    {
        public string timetableFile;
        static DateTime ReferenceDate = new DateTime(2016, 1, 1);
        private Timetable timetable;

        public FreightViz()
        {
            InitializeComponent();
        }

        private void FreightViz_Load(object sender, EventArgs e)
        {
            try
            {
                timetable = new Timetable(timetableFile);
                createGraph2(timetable);
                List<Train> l = new List<Train>() { timetable.TrainList[0] };
                List<Station> sList = new List<Station> { timetable.Network.StationDict[8], timetable.Network.StationDict[10] };
                List<Link> lList = new List<Link> { timetable.Network.LinkDict[1], timetable.Network.LinkDict[6], timetable.Network.LinkDict[10], timetable.Network.LinkDict[16] };
                //drawChart(timetable.TrainList, sList);
                //drawChart(timetable.TrainList, lList);
                drawChart2(timetable.TrainList, lList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void createGraph2(Timetable timetable)
        {
            Graph graph = new Graph();
            foreach (Station s in timetable.Network.StationDict.Values)
                graph.AddNode(s.ID.ToString());
            foreach (Link l in timetable.Network.LinkDict.Values)
                graph.AddEdge(l.Source.ID.ToString(), l.ID.ToString(), l.Target.ID.ToString()).Attr.Id = l.ID.ToString();
            graph.Attr.LayerDirection = LayerDirection.LR;
            gViewer.Graph = graph;
            foreach (Edge e in graph.Edges)
            {
                e.Label.IsVisible = true;
            }
        }

        /// <summary>
        /// Draw chart to show the traffic on each links. It's 2D so 
        /// - if multiple links need to be in a thread to show train running.
        /// - one link is easy to show
        /// Trains need to pass at least a link to be shown
        /// </summary>
        /// <param name="trains"></param>
        /// <param name="links">Links need to be in order</param>
        private void drawChart(List<Train> trains, List<Link> links)
        {
            MessageBox.Show("start drawing");   
            if (trains.Count == 0 || links.Count == 0)
                return;

            DateTime startTime = new DateTime(2016, 1, 1);

            Title title = new Title("Timetable - Sample");
            title.TextStyle = TextStyle.Default;
            title.Font = new Font("Trebuchet MS", 15, FontStyle.Bold);
            title.Alignment = ContentAlignment.MiddleCenter;
            chartTimetable.Chart.Titles.Clear();
            chartTimetable.Chart.Titles.Add(title);
            //create a legend
            chartTimetable.Chart.Legends.Clear();
            Legend legend = new Legend("Default");
            legend.BackColor = Color.Transparent;
            legend.Alignment = StringAlignment.Center;
            legend.Docking = Docking.Top;
            legend.TableStyle = LegendTableStyle.Wide;
            Legend hideLegend = new Legend("Hide");
            hideLegend.BackColor = Color.Transparent;

            //create chart area
            chartTimetable.Chart.ChartAreas.Clear();
            chartTimetable.Chart.Series.Clear();
            ChartArea area = new ChartArea("Default");
            area.BackColor = Color.Transparent;
            area.AxisX.LabelStyle.Angle = 90;
            area.AxisX.IsLabelAutoFit = false;
            area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
            area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            area.AxisX.Crossing = 0;
            area.AxisY.Crossing = 0;
            area.AxisX.IsMarksNextToAxis = false;
            area.AxisY.IsMarksNextToAxis = false;
            chartTimetable.Chart.ChartAreas.Add(area);
            area.AxisX.ScrollBar.BackColor = Color.LightGray;
            area.AxisX.ScrollBar.ButtonColor = Color.Gray;
            area.AxisY.ScrollBar.BackColor = Color.LightGray;
            area.AxisY.ScrollBar.ButtonColor = Color.Gray;
            area.AxisY.LabelStyle.Format = "hh:mm tt";

            // set zooming interval for time value
            chartTimetable.Chart.ChartAreas["Default"].CursorY.Interval = 0;

            DateTime upperY = DateTime.MinValue;
            DateTime lowerY = DateTime.MaxValue;

            area.AxisY.Minimum = ReferenceDate.AddHours(3.5).ToOADate() - ReferenceDate.ToOADate();
            area.AxisY.Maximum = ReferenceDate.AddHours(26).AddMinutes(-1).ToOADate() - ReferenceDate.ToOADate();

            foreach (Train run in trains)
            {
                Series seri = new Series(run.ID.ToString());
                seri.ChartType = SeriesChartType.Line;
                seri.Color = Color.Blue;// Color.FromArgb(ran.Next(255), ran.Next(255), ran.Next(255));
                seri.YValueType = ChartValueType.DateTime;
                // add data point
                int i = 0;
                bool lastLinkAdded = false;
                for (int l = 0; l < links.Count; l++)
                {
                    Link link = links[l];
                    if (run.HasLink(link)) // train contain link
                    {
                        Stop stop;
                        DateTime stopTime;
                        // Add link source node
                        if (lastLinkAdded == false)
                        {
                            stop = run.FindStop(link.Source);
                            stopTime = stopToTime(stop);
                            if (stopTime < lowerY) lowerY = stopTime;
                            if (stopTime > upperY) upperY = stopTime;
                            seri.Points.AddXY(stop.Station.Name, stopTime.ToOADate() - ReferenceDate.ToOADate());
                            seri.Points[l].MarkerColor = Color.Red;
                            seri.Points[l].MarkerStyle = MarkerStyle.Circle;
                            //if (i > 0 && run.FindStop(_stations[i - 1]) == null)
                            //    seri.Points[i].Color = Color.Transparent;
                            seri.Points[l].SetCustomProperty("Series", run.ID.ToString());
                            seri.Points[l].SetCustomProperty("Station", stop.Station.Name);
                            seri.Points[l].ToolTip = "Train: " + stop.Train.ID.ToString();// + " \n" + stop.GetDescription();
                        }

                        // Add link target node
                        stop = run.FindStop(link.Target);
                        stopTime = stopToTime(stop);
                        if (stopTime < lowerY) lowerY = stopTime;
                        if (stopTime > upperY) upperY = stopTime;
                        seri.Points.AddXY(stop.Station.Name, stopTime.ToOADate() - ReferenceDate.ToOADate());
                        seri.Points[l+1].MarkerColor = Color.Red;
                        seri.Points[l+1].MarkerStyle = MarkerStyle.Circle;
                        //if (i > 0 && run.FindStop(_stations[i - 1]) == null)
                        //    seri.Points[i].Color = Color.Transparent;
                        seri.Points[l+1].SetCustomProperty("Series", run.ID.ToString());
                        seri.Points[l+1].SetCustomProperty("Station", stop.Station.Name);
                        seri.Points[l+1].ToolTip = "Train: " + stop.Train.ID.ToString();// + " \n" + stop.GetDescription();

                        lastLinkAdded = true;
                    }
                    else
                    {
                        if (lastLinkAdded == false)
                        {
                            seri.Points.AddXY(link.Source.Name, run.StopList[0].Departure.ToOADate());
                            seri.Points[i].Color = Color.Transparent;
                        }
                        lastLinkAdded = false;
                    }
                }
                seri.ChartArea = "Default";
                chartTimetable.Chart.Series.Add(seri);
            }
            MessageBox.Show("end drawing");
        }

        private void drawChart2(List<Train> trains, List<Link> links)
        {
            if (trains.Count == 0 || links.Count == 0)
                return;

            DateTime startTime = new DateTime(2016, 1, 1);

            Title title = new Title("Timetable - Sample");
            title.TextStyle = TextStyle.Default;
            title.Font = new Font("Trebuchet MS", 15, FontStyle.Bold);
            title.Alignment = ContentAlignment.MiddleCenter;
            chart.Titles.Clear();
            chart.Titles.Add(title);
            //create a legend
            chart.Legends.Clear();
            Legend legend = new Legend("Default");
            legend.BackColor = Color.Transparent;
            legend.Alignment = StringAlignment.Center;
            legend.Docking = Docking.Top;
            legend.TableStyle = LegendTableStyle.Wide;
            Legend hideLegend = new Legend("Hide");
            hideLegend.BackColor = Color.Transparent;

            //create chart area
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            ChartArea area = new ChartArea("Default");
            area.BackColor = Color.Transparent;
            area.AxisX.LabelStyle.Angle = 90;
            area.AxisX.IsLabelAutoFit = false;
            area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
            area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            area.AxisX.Crossing = 0;
            area.AxisY.Crossing = 0;
            area.AxisX.IsMarksNextToAxis = false;
            area.AxisY.IsMarksNextToAxis = false;
            chart.ChartAreas.Add(area);
            area.AxisX.ScrollBar.BackColor = Color.LightGray;
            area.AxisX.ScrollBar.ButtonColor = Color.Gray;
            area.AxisY.ScrollBar.BackColor = Color.LightGray;
            area.AxisY.ScrollBar.ButtonColor = Color.Gray;
            area.AxisY.LabelStyle.Format = "hh:mm tt";

            // set zooming interval for time value
            chart.ChartAreas["Default"].CursorY.Interval = 0;

            DateTime upperY = DateTime.MinValue;
            DateTime lowerY = DateTime.MaxValue;

            area.AxisY.Minimum = ReferenceDate.AddHours(3.5).ToOADate() - ReferenceDate.ToOADate();
            area.AxisY.Maximum = ReferenceDate.AddHours(26).AddMinutes(-1).ToOADate() - ReferenceDate.ToOADate();

            foreach (Train run in trains)
            {
                Series seri = new Series(run.ID.ToString());
                seri.ChartType = SeriesChartType.Line;
                seri.Color = Color.Blue;// Color.FromArgb(ran.Next(255), ran.Next(255), ran.Next(255));
                seri.YValueType = ChartValueType.DateTime;
                // add data point
                int i = 0;
                bool lastLinkAdded = false;
                for (int l = 0; l < links.Count; l++)
                {
                    Link link = links[l];
                    if (run.HasLink(link)) // train contain link
                    {
                        Stop stop;
                        DateTime stopTime;
                        // Add link source node
                        if (lastLinkAdded == false)
                        {
                            stop = run.FindStop(link.Source);
                            stopTime = stopToTime(stop);
                            if (stopTime < lowerY) lowerY = stopTime;
                            if (stopTime > upperY) upperY = stopTime;
                            seri.Points.AddXY(stop.Station.Name, stopTime.ToOADate() - ReferenceDate.ToOADate());
                            seri.Points[l].MarkerColor = Color.Red;
                            seri.Points[l].MarkerStyle = MarkerStyle.Circle;
                            //if (i > 0 && run.FindStop(_stations[i - 1]) == null)
                            //    seri.Points[i].Color = Color.Transparent;
                            seri.Points[l].SetCustomProperty("Series", run.ID.ToString());
                            seri.Points[l].SetCustomProperty("Station", stop.Station.Name);
                            seri.Points[l].ToolTip = "Train: " + stop.Train.ID.ToString();// + " \n" + stop.GetDescription();
                        }

                        // Add link target node
                        stop = run.FindStop(link.Target);
                        stopTime = stopToTime(stop);
                        if (stopTime < lowerY) lowerY = stopTime;
                        if (stopTime > upperY) upperY = stopTime;
                        seri.Points.AddXY(stop.Station.Name, stopTime.ToOADate() - ReferenceDate.ToOADate());
                        seri.Points[l + 1].MarkerColor = Color.Red;
                        seri.Points[l + 1].MarkerStyle = MarkerStyle.Circle;
                        //if (i > 0 && run.FindStop(_stations[i - 1]) == null)
                        //    seri.Points[i].Color = Color.Transparent;
                        seri.Points[l + 1].SetCustomProperty("Series", run.ID.ToString());
                        seri.Points[l + 1].SetCustomProperty("Station", stop.Station.Name);
                        seri.Points[l + 1].ToolTip = "Train: " + stop.Train.ID.ToString();// + " \n" + stop.GetDescription();

                        lastLinkAdded = true;
                    }
                    else
                    {
                        if (lastLinkAdded == false)
                        {
                            seri.Points.AddXY(link.Source.Name, run.StopList[0].Departure.ToOADate());
                            seri.Points[i].Color = Color.Transparent;
                        }
                        lastLinkAdded = false;
                    }
                }
                seri.ChartArea = "Default";
                chart.Series.Add(seri);
            }
        }

        private void drawChart(List<Train> _runs, List<Station> _stations)
        {
            if (_runs.Count == 0 || _stations.Count == 0)
                return;

            DateTime startTime = new DateTime(2016,1,1);
            
            Dictionary<string,int> TrainOrder = new Dictionary<string,int>();
            for (int i = 0; i < _runs.Count; i++)
                TrainOrder.Add(_runs[i].ID.ToString(), i);

            Title title = new Title("Timetable - Sample");
            title.TextStyle = TextStyle.Default;
            title.Font = new Font("Trebuchet MS", 15, FontStyle.Bold);
            title.Alignment = ContentAlignment.MiddleCenter;
            chartTimetable.Chart.Titles.Clear();
            chartTimetable.Chart.Titles.Add(title);
            //create a legend
            chartTimetable.Chart.Legends.Clear();
            Legend legend = new Legend("Default");
            legend.BackColor = Color.Transparent;
            legend.Alignment = StringAlignment.Center;
            legend.Docking = Docking.Top;
            legend.TableStyle = LegendTableStyle.Wide;
            Legend hideLegend = new Legend("Hide");
            hideLegend.BackColor = Color.Transparent;

            //create chart area
            chartTimetable.Chart.ChartAreas.Clear();
            ChartArea area = new ChartArea("Default");
            area.BackColor = Color.Transparent;
            area.AxisX.LabelStyle.Angle = 90;
            area.AxisX.IsLabelAutoFit = false;
            area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
            area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            area.AxisX.Crossing = 0;
            area.AxisY.Crossing = 0;
            area.AxisX.IsMarksNextToAxis = false;
            area.AxisY.IsMarksNextToAxis = false;
            chartTimetable.Chart.ChartAreas.Add(area);
            area.AxisX.ScrollBar.BackColor = Color.LightGray;
            area.AxisX.ScrollBar.ButtonColor = Color.Gray;
            area.AxisY.ScrollBar.BackColor = Color.LightGray;
            area.AxisY.ScrollBar.ButtonColor = Color.Gray;
            area.AxisY.LabelStyle.Format = "hh:mm tt";

            // set zooming interval for time value
            chartTimetable.Chart.ChartAreas["Default"].CursorY.Interval = 0;

            DateTime upperY = DateTime.MinValue;
            DateTime lowerY = DateTime.MaxValue;

            foreach (Train run in _runs)
            {
                Random ran = new Random();
                Series seri = new Series(run.ID.ToString());
                seri.ChartType = SeriesChartType.Line;
                seri.Color = Color.Blue;// Color.FromArgb(ran.Next(255), ran.Next(255), ran.Next(255));
                seri.YValueType = ChartValueType.DateTime;
                // add data point
                int i = 0;
                for (int s = 0; s < _stations.Count; s++)
                {
                    Station stat = _stations[s];
                    Stop stop = run.FindStop(stat);
                    if (stop == null)
                    {
                        //if (TLPSettings.INTERFACE_SHOW_STATION_NAME_XAXIS)
                        {
                            //seri.Points.AddXY(stat.Name, run.StopList[0].Departure.ToOADate());
                            //seri.Points[i].Color = Color.Transparent;
                        }
                        //else
                        //{
                        //    seri.Points.AddXY(stat.ID, run.DepartureTime.ToOADate());
                        //    seri.Points[i].Color = Color.Transparent;
                        //}
                    }
                    else
                    {
                        DateTime stopTime = stopToTime(stop);
                        if (stopTime < lowerY) lowerY = stopTime;
                        if (stopTime > upperY) upperY = stopTime;
                        //if (TLPSettings.INTERFACE_SHOW_STATION_NAME_XAXIS)
                            seri.Points.AddXY(stop.Station.Name, stopTime.ToOADate() - ReferenceDate.ToOADate());//stop.DepartureTime.ToOADate());
                        //else
                        //    seri.Points.AddXY(stop.Station.ID, stopTime.ToOADate() - ReferenceDate.ToOADate());//stop.DepartureTime.ToOADate());
                        seri.Points[i].MarkerColor = Color.Red;
                        //if (stop.IsStop)
                            seri.Points[i].MarkerStyle = MarkerStyle.Circle;
                        //if (i > 0 && run.FindStop(_stations[i - 1]) == null)
                        //    seri.Points[i].Color = Color.Transparent;
                        seri.Points[i].SetCustomProperty("Series", run.ID.ToString());
                        seri.Points[i].SetCustomProperty("Station", stop.Station.Name);
                        seri.Points[i].ToolTip = "Train: " + stop.Train.ID.ToString();// + " \n" + stop.GetDescription();
                        i++;
                    }
                }
                seri.ChartArea = "Default";
                chartTimetable.Chart.Series.Add(seri);
            }
            area.AxisY.Minimum= ReferenceDate.AddHours(3.5).ToOADate() - ReferenceDate.ToOADate(); //Utility.DateTimeUltility.GetUpperCap(upperY).ToOADate() - Constants.ReferenceDate.ToOADate();
            area.AxisY.Maximum = ReferenceDate.AddHours(24).AddMinutes(-1).ToOADate() - ReferenceDate.ToOADate(); //Utility.DateTimeUltility.GetLowerCap(lowerY).ToOADate() - Constants.ReferenceDate.ToOADate();
        }

        private DateTime stopToTime(Stop s)
        {
            //if (s.Index == s.TrainRun.StopList.Count - 1)
            //    return s.ArrivalTime;
            //if (s.Index == 0)
            //    return s.DepartureTime;
            //if (toolStripButArrival.Checked)
            //    return s.Arrival;

            return s.Departure;
        }

        private void createGraph()
        {
            // Create graph
            Graph graph = new Graph();
            graph.AddNode("O1");
            graph.AddNode("O2");
            graph.AddNode("O3");
            graph.AddNode("O4");

            graph.AddNode("I1");
            graph.AddNode("I2");
            graph.AddNode("I3");
            graph.AddNode("I4");
            graph.AddNode("I5");
            graph.AddNode("I6");
            graph.AddNode("I7");

            graph.AddNode("D1");
            graph.AddNode("D2");
            graph.AddNode("D3");
            graph.AddNode("D4");

            (graph.AddEdge("O1", "1", "I1")).Attr.Id = "1";
            graph.AddEdge("O2", "2", "I1").Attr.Id = "2";
            graph.AddEdge("O3", "3", "I2").Attr.Id = "3";
            graph.AddEdge("O4", "4", "I2").Attr.Id = "4";

            graph.AddEdge("I1", "5", "I3").Attr.Id = "5";
            graph.AddEdge("I1", "6", "I4").Attr.Id = "6";
            graph.AddEdge("I2", "7", "I4").Attr.Id = "7";
            graph.AddEdge("I2", "8", "I5").Attr.Id = "8";

            graph.AddEdge("I3", "9", "I6").Attr.Id = "9";
            graph.AddEdge("I4", "10", "I6").Attr.Id = "10";
            graph.AddEdge("I4", "11", "I7").Attr.Id = "11";
            graph.AddEdge("I5", "12", "I7").Attr.Id = "12";

            graph.AddEdge("I6", "13", "D1").Attr.Id = "13";
            graph.AddEdge("I6", "14", "D2").Attr.Id = "14";
            graph.AddEdge("I7", "15", "D3").Attr.Id = "15";
            graph.AddEdge("I7", "16", "D4").Attr.Id = "16";

            graph.Attr.LayerDirection = LayerDirection.LR;

            gViewer.Graph = graph;
            string edges = "";
            foreach (Edge e in graph.Edges)
            {
                e.Label.IsVisible = true;
                edges += e.Attr.Id + " ";
            }
        }

        private void resetGrahpColor()
        {
            foreach (Node n in gViewer.Graph.Nodes)
            {
                n.Attr.Color = gColor.Black;
            }
            foreach (Edge e in gViewer.Graph.Edges)
            {
                e.Attr.Color = gColor.Black;
                e.Attr.LineWidth = 1;
            }
            gViewer.Refresh();
        }

        private void btnDrawChart_Click(object sender, EventArgs e)
        {
            resetGrahpColor();
            List<Link> lList = new List<Link>();
            try
            {
                string[] strArray = txtLinks.Text.Split(' ');
                foreach (string s in strArray)
                {
                    Link l = timetable.Network.LinkDict[int.Parse(s)];
                    if (l != null)
                    {
                        gViewer.Graph.FindNode(l.Source.ID.ToString()).Attr.Color = gColor.Blue;
                        gViewer.Graph.FindNode(l.Target.ID.ToString()).Attr.Color= gColor.Blue;
                        gViewer.Graph.EdgeById(l.ID.ToString()).Attr.Color = gColor.Blue;
                        gViewer.Graph.EdgeById(l.ID.ToString()).Attr.LineWidth = 3;
                        gViewer.Refresh();
                        lList.Add(l);
                    }
                }
                drawChart2(timetable.TrainList, lList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chartTimetable_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("clicked");
        }
    }
}
