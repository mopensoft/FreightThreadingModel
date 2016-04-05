namespace FreightThreading.Controls
{
    partial class ChartControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartControl));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuChart = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemPageSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStandard = new System.Windows.Forms.ToolStrip();
            this.saveToolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoom = new System.Windows.Forms.ToolStrip();
            this.butZoom = new System.Windows.Forms.ToolStripButton();
            this.butZoomIn = new System.Windows.Forms.ToolStripButton();
            this.butZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.contextMenuChart.SuspendLayout();
            this.toolStripStandard.SuspendLayout();
            this.toolStripZoom.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.chart);
            this.toolStripContainer1.ContentPanel.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(844, 502);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.BackColor = System.Drawing.Color.Transparent;
            this.toolStripContainer1.Size = new System.Drawing.Size(844, 527);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripStandard);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripZoom);
            // 
            // chart
            // 
            this.chart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chart.BackSecondaryColor = System.Drawing.Color.White;
            this.chart.BorderlineColor = System.Drawing.Color.Black;
            this.chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chart.BorderSkin.BackColor = System.Drawing.Color.Black;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisX.LabelStyle.Angle = 90;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "Default";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.ContextMenuStrip = this.contextMenuChart;
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.IsSoftShadows = false;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.BackColor = System.Drawing.Color.LightSteelBlue;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Default";
            legend1.ShadowColor = System.Drawing.Color.Transparent;
            legend1.TextWrapThreshold = 50;
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            this.chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "Default";
            series1.IsVisibleInLegend = false;
            series1.Legend = "Default";
            series1.Name = "Series1";
            series1.ShadowColor = System.Drawing.Color.Transparent;
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(844, 502);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // contextMenuChart
            // 
            this.contextMenuChart.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopy,
            this.toolStripMenuItemSave,
            this.toolStripSeparator1,
            this.toolStripMenuItemPageSetup,
            this.toolStripMenuItemPreview,
            this.toolStripMenuItemPrint});
            this.contextMenuChart.Name = "contextMenuChart";
            this.contextMenuChart.Size = new System.Drawing.Size(134, 120);
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(133, 22);
            this.toolStripMenuItemCopy.Text = "&Copy";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            // 
            // toolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(133, 22);
            this.toolStripMenuItemSave.Text = "&Save";
            this.toolStripMenuItemSave.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(130, 6);
            // 
            // toolStripMenuItemPageSetup
            // 
            this.toolStripMenuItemPageSetup.Name = "toolStripMenuItemPageSetup";
            this.toolStripMenuItemPageSetup.Size = new System.Drawing.Size(133, 22);
            this.toolStripMenuItemPageSetup.Text = "Page Setup";
            this.toolStripMenuItemPageSetup.Click += new System.EventHandler(this.toolStripMenuItemPageSetup_Click);
            // 
            // toolStripMenuItemPreview
            // 
            this.toolStripMenuItemPreview.Name = "toolStripMenuItemPreview";
            this.toolStripMenuItemPreview.Size = new System.Drawing.Size(133, 22);
            this.toolStripMenuItemPreview.Text = "Pre&view";
            this.toolStripMenuItemPreview.Click += new System.EventHandler(this.toolStripMenuItemPreview_Click);
            // 
            // toolStripMenuItemPrint
            // 
            this.toolStripMenuItemPrint.Name = "toolStripMenuItemPrint";
            this.toolStripMenuItemPrint.Size = new System.Drawing.Size(133, 22);
            this.toolStripMenuItemPrint.Text = "&Print";
            this.toolStripMenuItemPrint.Click += new System.EventHandler(this.toolStripMenuItemPrint_Click);
            // 
            // toolStripStandard
            // 
            this.toolStripStandard.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripStandard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripButton1,
            this.printToolStripButton,
            this.copyToolStripButton});
            this.toolStripStandard.Location = new System.Drawing.Point(3, 0);
            this.toolStripStandard.Name = "toolStripStandard";
            this.toolStripStandard.Size = new System.Drawing.Size(81, 25);
            this.toolStripStandard.TabIndex = 4;
            this.toolStripStandard.Text = "toolStripStandard";
            // 
            // saveToolStripButton1
            // 
            this.saveToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton1.Image")));
            this.saveToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton1.Name = "saveToolStripButton1";
            this.saveToolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton1.Text = "&Save Chart";
            this.saveToolStripButton1.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "&Print Chart";
            this.printToolStripButton.Click += new System.EventHandler(this.toolStripMenuItemPrint_Click);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolStripButton.Text = "&Copy Chart";
            this.copyToolStripButton.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            // 
            // toolStripZoom
            // 
            this.toolStripZoom.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripZoom.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.toolStripZoom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.butZoom,
            this.butZoomIn,
            this.butZoomOut});
            this.toolStripZoom.Location = new System.Drawing.Point(84, 0);
            this.toolStripZoom.Name = "toolStripZoom";
            this.toolStripZoom.Size = new System.Drawing.Size(81, 25);
            this.toolStripZoom.TabIndex = 2;
            // 
            // butZoom
            // 
            this.butZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.butZoom.Image = global::FreightThreading.Properties.Resources.Zoom;
            this.butZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butZoom.Name = "butZoom";
            this.butZoom.Size = new System.Drawing.Size(23, 22);
            this.butZoom.Text = "Zoom";
            this.butZoom.Click += new System.EventHandler(this.butZoom_Click);
            // 
            // butZoomIn
            // 
            this.butZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.butZoomIn.Image = global::FreightThreading.Properties.Resources.ZoomIn;
            this.butZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butZoomIn.Name = "butZoomIn";
            this.butZoomIn.Size = new System.Drawing.Size(23, 22);
            this.butZoomIn.Text = "ZoomIn";
            this.butZoomIn.Click += new System.EventHandler(this.butZoomIn_Click);
            // 
            // butZoomOut
            // 
            this.butZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.butZoomOut.Image = global::FreightThreading.Properties.Resources.ZoomOut;
            this.butZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butZoomOut.Name = "butZoomOut";
            this.butZoomOut.Size = new System.Drawing.Size(23, 22);
            this.butZoomOut.Text = "ZoomOut";
            this.butZoomOut.Click += new System.EventHandler(this.butZoomOut_Click);
            // 
            // ChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "ChartControl";
            this.Size = new System.Drawing.Size(844, 527);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.contextMenuChart.ResumeLayout(false);
            this.toolStripStandard.ResumeLayout(false);
            this.toolStripStandard.PerformLayout();
            this.toolStripZoom.ResumeLayout(false);
            this.toolStripZoom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ToolStrip toolStripStandard;
        private System.Windows.Forms.ToolStripButton saveToolStripButton1;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripButton copyToolStripButton;
        private System.Windows.Forms.ToolStrip toolStripZoom;
        private System.Windows.Forms.ToolStripButton butZoom;
        private System.Windows.Forms.ToolStripButton butZoomIn;
        private System.Windows.Forms.ToolStripButton butZoomOut;
        private System.Windows.Forms.ContextMenuStrip contextMenuChart;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPageSetup;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPreview;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPrint;
    }
}
