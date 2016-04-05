namespace FreightThreading
{
    partial class FreightViz
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FreightViz));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.contMain = new System.Windows.Forms.SplitContainer();
            this.contTop = new System.Windows.Forms.SplitContainer();
            this.btnDrawChart = new System.Windows.Forms.Button();
            this.txtLinks = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gViewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartTimetable = new FreightThreading.Controls.myChart();
            ((System.ComponentModel.ISupportInitialize)(this.contMain)).BeginInit();
            this.contMain.Panel1.SuspendLayout();
            this.contMain.Panel2.SuspendLayout();
            this.contMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contTop)).BeginInit();
            this.contTop.Panel1.SuspendLayout();
            this.contTop.Panel2.SuspendLayout();
            this.contTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // contMain
            // 
            this.contMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contMain.Location = new System.Drawing.Point(0, 0);
            this.contMain.Name = "contMain";
            this.contMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // contMain.Panel1
            // 
            this.contMain.Panel1.Controls.Add(this.contTop);
            // 
            // contMain.Panel2
            // 
            this.contMain.Panel2.Controls.Add(this.chart);
            this.contMain.Size = new System.Drawing.Size(909, 614);
            this.contMain.SplitterDistance = 243;
            this.contMain.TabIndex = 0;
            // 
            // contTop
            // 
            this.contTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contTop.Location = new System.Drawing.Point(0, 0);
            this.contTop.Name = "contTop";
            // 
            // contTop.Panel1
            // 
            this.contTop.Panel1.Controls.Add(this.btnDrawChart);
            this.contTop.Panel1.Controls.Add(this.chartTimetable);
            this.contTop.Panel1.Controls.Add(this.txtLinks);
            this.contTop.Panel1.Controls.Add(this.label1);
            // 
            // contTop.Panel2
            // 
            this.contTop.Panel2.Controls.Add(this.gViewer);
            this.contTop.Size = new System.Drawing.Size(909, 243);
            this.contTop.SplitterDistance = 432;
            this.contTop.TabIndex = 0;
            // 
            // btnDrawChart
            // 
            this.btnDrawChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDrawChart.Location = new System.Drawing.Point(342, 205);
            this.btnDrawChart.Name = "btnDrawChart";
            this.btnDrawChart.Size = new System.Drawing.Size(75, 23);
            this.btnDrawChart.TabIndex = 3;
            this.btnDrawChart.Text = "Draw Chart";
            this.btnDrawChart.UseVisualStyleBackColor = true;
            this.btnDrawChart.Click += new System.EventHandler(this.btnDrawChart_Click);
            // 
            // txtLinks
            // 
            this.txtLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLinks.Location = new System.Drawing.Point(82, 207);
            this.txtLinks.Name = "txtLinks";
            this.txtLinks.Size = new System.Drawing.Size(236, 20);
            this.txtLinks.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 210);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Link:";
            // 
            // gViewer
            // 
            this.gViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gViewer.ArrowheadLength = 10D;
            this.gViewer.AsyncLayout = false;
            this.gViewer.AutoScroll = true;
            this.gViewer.BackwardEnabled = false;
            this.gViewer.BuildHitTree = true;
            this.gViewer.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.UseSettingsOfTheGraph;
            this.gViewer.EdgeInsertButtonVisible = true;
            this.gViewer.FileName = "";
            this.gViewer.ForwardEnabled = false;
            this.gViewer.Graph = null;
            this.gViewer.InsertingEdge = false;
            this.gViewer.LayoutAlgorithmSettingsButtonVisible = true;
            this.gViewer.LayoutEditingEnabled = true;
            this.gViewer.Location = new System.Drawing.Point(0, 0);
            this.gViewer.LooseOffsetForRouting = 0.25D;
            this.gViewer.MouseHitDistance = 0.05D;
            this.gViewer.Name = "gViewer";
            this.gViewer.NavigationVisible = true;
            this.gViewer.NeedToCalculateLayout = true;
            this.gViewer.OffsetForRelaxingInRouting = 0.6D;
            this.gViewer.PaddingForEdgeRouting = 8D;
            this.gViewer.PanButtonPressed = false;
            this.gViewer.SaveAsImageEnabled = true;
            this.gViewer.SaveAsMsaglEnabled = true;
            this.gViewer.SaveButtonVisible = true;
            this.gViewer.SaveGraphButtonVisible = true;
            this.gViewer.SaveInVectorFormatEnabled = true;
            this.gViewer.Size = new System.Drawing.Size(473, 243);
            this.gViewer.TabIndex = 4;
            this.gViewer.TightOffsetForRouting = 0.125D;
            this.gViewer.ToolBarIsVisible = true;
            this.gViewer.Transform = ((Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation)(resources.GetObject("gViewer.Transform")));
            this.gViewer.UndoRedoButtonsVisible = true;
            this.gViewer.WindowZoomButtonPressed = false;
            this.gViewer.ZoomF = 1D;
            this.gViewer.ZoomFraction = 0.5D;
            this.gViewer.ZoomWhenMouseWheelScroll = true;
            this.gViewer.ZoomWindowThreshold = 0.05D;
            // 
            // chart
            // 
            this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(12, 13);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(885, 342);
            this.chart.TabIndex = 4;
            this.chart.Text = "chart1";
            // 
            // chartTimetable
            // 
            this.chartTimetable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartTimetable.Location = new System.Drawing.Point(12, 12);
            this.chartTimetable.Name = "chartTimetable";
            this.chartTimetable.Size = new System.Drawing.Size(405, 153);
            this.chartTimetable.TabIndex = 0;
            this.chartTimetable.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chartTimetable_MouseClick);
            // 
            // FreightViz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 614);
            this.Controls.Add(this.contMain);
            this.Name = "FreightViz";
            this.Text = "FreightViz";
            this.Load += new System.EventHandler(this.FreightViz_Load);
            this.contMain.Panel1.ResumeLayout(false);
            this.contMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contMain)).EndInit();
            this.contMain.ResumeLayout(false);
            this.contTop.Panel1.ResumeLayout(false);
            this.contTop.Panel1.PerformLayout();
            this.contTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contTop)).EndInit();
            this.contTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer contMain;
        private System.Windows.Forms.SplitContainer contTop;
        private Microsoft.Msagl.GraphViewerGdi.GViewer gViewer;
        private Controls.myChart chartTimetable;
        private System.Windows.Forms.Button btnDrawChart;
        private System.Windows.Forms.TextBox txtLinks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
    }
}