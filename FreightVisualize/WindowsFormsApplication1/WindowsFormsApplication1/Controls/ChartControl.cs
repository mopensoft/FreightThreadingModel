using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FreightThreading.Controls
{
    public partial class ChartControl : UserControl
    {
        public Chart Chart { get { return this.chart; } }

        public ChartControl()
        {
            InitializeComponent();
        }

        private void butZoom_Click(object sender, EventArgs e)
        {
            butZoom.Checked = !butZoom.Checked;
            ZoomSetup(butZoom.Checked, butZoom.Checked);
        }

        private void butZoomIn_Click(object sender, EventArgs e)
        {
            butZoom.Checked = true;
            ZoomInXY();
        }

        private void butZoomOut_Click(object sender, EventArgs e)
        {
            butZoom.Checked = true;
            ZoomOutXY();
        }

        #region Zooming
        private void ZoomSetup(bool X, bool Y)
        {
            foreach (ChartArea area in chart.ChartAreas)
            {
                area.CursorX.IsUserEnabled = X;
                area.CursorX.IsUserSelectionEnabled = X;
                area.AxisX.ScaleView.Zoomable = X;
                area.AxisX.ScrollBar.IsPositionedInside = true;
                area.AxisX.ScaleView.SmallScrollMinSize = (area.AxisX.Maximum - area.AxisX.Minimum) / 1000;
                if (chart.Series.Count > 0 && chart.Series[0].XValueType != ChartValueType.Time)
                    area.AxisX.ScaleView.SmallScrollMinSize = 1;

                area.CursorY.IsUserEnabled = Y;
                area.CursorY.IsUserSelectionEnabled = Y;
                area.AxisY.ScaleView.Zoomable = Y;
                area.AxisY.ScrollBar.IsPositionedInside = true;
                area.AxisY.ScaleView.SmallScrollMinSize = (area.AxisY.Maximum - area.AxisY.Minimum) / 1000;
                if (chart.Series.Count > 0 && chart.Series[0].YValueType != ChartValueType.Time)
                    area.AxisY.ScaleView.SmallScrollMinSize = 1;
            }
        }

        private void ZoomInX()
        {
            //ZoomSetup(true, false);
            foreach (ChartArea area in chart.ChartAreas)
            {
                double min = (double)area.AxisX.ScaleView.ViewMinimum;
                double max = (double)area.AxisX.ScaleView.ViewMaximum;
                double zoomStep = (max - min) / 10; // 25%
                if (zoomStep == 0) 
                    zoomStep = 1;
                if (chart.Series.Count > 0 && chart.Series[0].XValueType != ChartValueType.Time)
                    zoomStep = zoomStep < 2 ? 2 : Math.Truncate(zoomStep);
                
                if (max >= (double)area.AxisX.Minimum + zoomStep
                    && min <= (double)area.AxisX.Maximum - zoomStep)
                {
                    if (max - min > zoomStep * 2)
                        area.AxisX.ScaleView.Zoom(min + zoomStep, max - zoomStep);
                }
                else if (max >= (double)area.AxisX.Minimum + zoomStep)
                {
                    if (max - min > zoomStep)
                        area.AxisX.ScaleView.Zoom(min, max - zoomStep);
                }
                else if (min <= (double)area.AxisX.Maximum - zoomStep)
                {
                    if (max - min > zoomStep)
                        area.AxisX.ScaleView.Zoom(min + zoomStep, max);
                }
            }
        }

        private void ZoomOutX()
        {
            //ZoomSetup(true, false);
            foreach (ChartArea area in chart.ChartAreas)
            {
                double min = (double)area.AxisX.ScaleView.ViewMinimum;
                double max = (double)area.AxisX.ScaleView.ViewMaximum;
                double zoomStep = (max - min) / 10; // 25%
                if (zoomStep == 0)
                    zoomStep = 1;
                if (chart.Series.Count > 0 && chart.Series[0].XValueType != ChartValueType.Time)
                    zoomStep = zoomStep < 2 ? 2 : Math.Truncate(zoomStep);

                if (min >= zoomStep && max <= (double)area.AxisX.Maximum - zoomStep)
                    area.AxisX.ScaleView.Zoom(min - zoomStep, max + zoomStep);
                else if (min >= zoomStep)
                    area.AxisX.ScaleView.Zoom(min - zoomStep, max);
                else if (max <= (double)area.AxisX.Maximum - zoomStep)
                    area.AxisX.ScaleView.Zoom(min, max + zoomStep);
                else
                    area.AxisX.ScaleView.Zoom(area.AxisX.Minimum, area.AxisX.Maximum);
            }
        }

        private void ZoomInY()
        {
            //ZoomSetup(false, true);
            foreach (ChartArea area in chart.ChartAreas)
            {
                double min = (double)area.AxisY.ScaleView.ViewMinimum;
                double max = (double)area.AxisY.ScaleView.ViewMaximum;
                double zoomStep = (double)(max - min) / 10; // 10%
                if (zoomStep == 0)
                    zoomStep = 1;
                if (chart.Series.Count > 0 && chart.Series[0].YValueType != ChartValueType.Time)
                    zoomStep = zoomStep < 2 ? 2 : Math.Truncate(zoomStep);

                if (max >= (double)area.AxisY.Minimum + zoomStep
                    && min <= (double)area.AxisY.Maximum - zoomStep)
                {
                    if (max - min > 2 * zoomStep)
                        area.AxisY.ScaleView.Zoom(min + zoomStep, max - zoomStep);
                }
                else if (max >= (double)area.AxisY.Minimum + zoomStep)
                {
                    if (max - min > zoomStep)
                        area.AxisY.ScaleView.Zoom(min, max - zoomStep);
                }
                else if (min <= (double)area.AxisY.Maximum - zoomStep)
                {
                    if (max - min > zoomStep)
                        area.AxisY.ScaleView.Zoom(min + zoomStep, max);
                }
            }
        }

        private void ZoomOutY()
        {
            //ZoomSetup(false, true);
            foreach (ChartArea area in chart.ChartAreas)
            {
                double min = (double)area.AxisY.ScaleView.ViewMinimum;
                double max = (double)area.AxisY.ScaleView.ViewMaximum;
                double zoomStep = (int)((max - min) / 10); // 10%
                if (zoomStep == 0)
                    zoomStep = 1;
                if (chart.Series.Count > 0 && chart.Series[0].YValueType != ChartValueType.Time)
                    zoomStep = zoomStep < 2 ? 2 : Math.Truncate(zoomStep);

                if (max <= (double)area.AxisY.Maximum - zoomStep &&
                    min >= (double)area.AxisY.Minimum + zoomStep)
                    area.AxisY.ScaleView.Zoom(min - zoomStep, max + zoomStep);
                else if (max <= (double)area.AxisY.Maximum - zoomStep)
                    area.AxisY.ScaleView.Zoom(min, max + zoomStep);
                else if (min >= (double)area.AxisY.Minimum + zoomStep)
                    area.AxisY.ScaleView.Zoom(min - zoomStep, max);
                else
                    area.AxisY.ScaleView.Zoom(area.AxisY.Minimum, area.AxisY.Maximum);
            }
        }

        private void ZoomInXY()
        {
            ZoomSetup(true, true);
            ZoomInX();
            ZoomInY();
        }

        private void ZoomOutXY()
        {
            ZoomSetup(true, true);
            ZoomOutX();
            ZoomOutY();
        }

        internal void UpdateZoomState()
        {
            ZoomSetup(butZoom.Checked, butZoom.Checked);
        }
        #endregion

        #region toolbar buttons handlers
        private void toolStripMenuItemPageSetup_Click(object sender, EventArgs e)
        {
            try
            {
                chart.Printing.PageSetup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Chart Control for .NET Framework", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void toolStripMenuItemPreview_Click(object sender, EventArgs e)
        {
            try
            {
                chart.Printing.PrintPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Chart Control for .NET Framework", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void toolStripMenuItemPrint_Click(object sender, EventArgs e)
        {
            try
            {
                chart.Printing.Print(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Chart Control for .NET Framework", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            // create a memory stream to save the chart image    
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            // save the chart image to the stream    
            chart.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Bmp);

            // create a bitmap using the stream    
            Bitmap bmp = new Bitmap(stream);

            // save the bitmap to the clipboard    
            Clipboard.SetDataObject(bmp);
        }

        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            // Create a new save file dialog
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            // Sets the current file name filter string, which determines 
            // the choices that appear in the "Save as file type" or 
            // "Files of type" box in the dialog box.
            saveFileDialog1.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|EMF-Plus (*.emf)|*.emf|EMF-Dual (*.emf)|*.emf|EMF (*.emf)|*.emf|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            // Set image file format
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                ChartImageFormat format = ChartImageFormat.Bmp;

                if (saveFileDialog1.FileName.EndsWith("bmp"))
                {
                    format = ChartImageFormat.Bmp;
                }
                else if (saveFileDialog1.FileName.EndsWith("jpg"))
                {
                    format = ChartImageFormat.Jpeg;
                }
                else if (saveFileDialog1.FileName.EndsWith("emf") && saveFileDialog1.FilterIndex == 3)
                {
                    format = ChartImageFormat.EmfDual;
                }
                else if (saveFileDialog1.FileName.EndsWith("emf") && saveFileDialog1.FilterIndex == 4)
                {
                    format = ChartImageFormat.EmfPlus;
                }
                else if (saveFileDialog1.FileName.EndsWith("emf"))
                {
                    format = ChartImageFormat.Emf;
                }
                else if (saveFileDialog1.FileName.EndsWith("gif"))
                {
                    format = ChartImageFormat.Gif;
                }
                else if (saveFileDialog1.FileName.EndsWith("png"))
                {
                    format = ChartImageFormat.Png;
                }
                else if (saveFileDialog1.FileName.EndsWith("tif"))
                {
                    format = ChartImageFormat.Tiff;
                }

                // Save image
                chart.SaveImage(saveFileDialog1.FileName, format);
            }
        }
        #endregion

        public void ClearChart(string title)
        {
            Title t = new Title("Default");
            t.TextStyle = TextStyle.Default;
            t.Font = new Font("Trebuchet MS", 15, FontStyle.Bold);
            t.Alignment = ContentAlignment.MiddleCenter;
            t.Text = title;
            this.Chart.Titles.Clear();
            this.Chart.Titles.Add(t);

            this.Chart.ChartAreas.Clear();
            this.Chart.Legends.Clear();
            this.Chart.Series.Clear();
        }

        public void LoadGraph(DataTable data, object[] axeX, ChartValueType xValueType, ChartValueType yValueType, string chartTitle, string xTitle, string yTitle, Color[] colors)
        {
            double miX, maX, miY, maY;

            this.ClearChart(chartTitle);
            if (data == null || data.Columns.Count == 0 || data.Rows.Count == 0)
                return;

            chart.ChartAreas.Add(new ChartArea("Default"));
            maX = chart.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum;
            miX = chart.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum;
            maY = chart.ChartAreas["Default"].AxisY.ScaleView.ViewMaximum;
            miY = chart.ChartAreas["Default"].AxisY.ScaleView.ViewMinimum;

            Legend l = new Legend("Default");
            l.Alignment = StringAlignment.Center;
            l.Docking = Docking.Top;
            l.BackColor = Color.Transparent;
            l.TableStyle = LegendTableStyle.Wide;
            chart.Legends.Clear();
            chart.Legends.Add(l);

            ChartArea area = chart.ChartAreas["Default"];
            area.AxisX.LabelStyle.Angle = 90;
            area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
            // set grid line color
            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // Set X and Y axis crossing
            area.AxisX.Crossing = 0;
            area.AxisY.Crossing = 0;
            area.AxisX.IsMarksNextToAxis = false;
            area.AxisY.IsMarksNextToAxis = false;
            Font font = new Font(area.AxisX.TitleFont.FontFamily, 9, FontStyle.Regular);
            area.AxisX.Title = xTitle;
            area.AxisY.Title = yTitle;
            area.AxisX.TitleFont = font;
            area.AxisY.TitleFont = font;
            area.AxisX.TitleFont = new Font(new Font("Trebuchet MS", 10, FontStyle.Bold), FontStyle.Bold);
            area.AxisY.TitleFont = new Font(new Font("Trebuchet MS", 10, FontStyle.Bold), FontStyle.Bold);
            area.BackColor = Color.Transparent;
            area.ShadowColor = Color.Transparent;
            area.AxisX.ScrollBar.BackColor = Color.LightGray;
            area.AxisX.ScrollBar.ButtonColor = Color.Gray;
            area.AxisY.ScrollBar.BackColor = Color.LightGray;
            area.AxisY.ScrollBar.ButtonColor = Color.Gray;

            chart.Legends["Default"].CustomItems.Clear();

            for (int i = 0; i < data.Columns.Count; i++)
            {
                Random rand = new Random();
                Series newSeries = new Series(data.Columns[i].ColumnName);
                newSeries.BorderWidth = 2;
                newSeries.Color = colors[i];
                newSeries.ShadowOffset = 2;
                newSeries.XValueType = xValueType;
                if (xValueType == ChartValueType.DateTime || xValueType == ChartValueType.Time)
                    area.AxisX.LabelStyle.Format = "hh:mm tt";
                newSeries.YValueType = yValueType;
                if (yValueType == ChartValueType.DateTime || yValueType == ChartValueType.Time)
                    area.AxisY.LabelStyle.Format = "hh:mm tt";
                newSeries["LabelStyle"] = "Auto";
                newSeries.ShadowColor = Color.Transparent;
                newSeries.ShadowOffset = 0;
                newSeries.IsVisibleInLegend = false;
                chart.Series.Add(newSeries);

                LegendItem lItem = new LegendItem();
                lItem.Name = data.Columns[i].ColumnName;
                lItem.Color = colors[i];
                lItem.BorderColor = Color.Transparent;
                lItem.Tag = newSeries;
                chart.Legends["Default"].CustomItems.Add(lItem);
            }

            // load data to each serie
            if (axeX.Length == 1)
            {
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    if (chart.Series[data.Columns[j].ColumnName].XValueType == ChartValueType.Time)
                        for (int i = 0; i < data.Rows.Count; i++)
                            chart.Series[data.Columns[j].ColumnName].Points.AddXY(((DateTime)axeX[i]).ToString("hh:mm tt"), data.Rows[i][j]);
                    else
                        for (int i = 0; i < data.Rows.Count; i++)
                            chart.Series[data.Columns[j].ColumnName].Points.AddXY(axeX[i], data.Rows[i][j]);
                }
            }
            else
            {
                for (int j = 0; j < data.Columns.Count; j++)
                    for (int i = 0; i < data.Rows.Count; i++)
                        chart.Series[data.Columns[j].ColumnName].Points.AddXY(axeX[i], data.Rows[i][j]);
            }
        }

        internal List<ToolStrip> GetToolStrips()
        {
            List<ToolStrip> list = new List<ToolStrip>();
            foreach (Control c in this.toolStripContainer1.TopToolStripPanel.Controls)
                if (c is ToolStrip)
                {
                    if (c.Name == "standardToolStrip")
                        list.Add(c as ToolStrip);
                    else
                        list.Insert(0, c as ToolStrip);
                }

            return list;
        }

        internal void ShowToolStrip(bool hide)
        {
            toolStripStandard.Visible = toolStripZoom.Visible = hide;
        }
    }
}
