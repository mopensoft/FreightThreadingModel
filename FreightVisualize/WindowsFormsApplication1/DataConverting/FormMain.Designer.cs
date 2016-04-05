namespace DataConverting
{
    partial class FormMain
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
            this.btnFreightData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFreightData
            // 
            this.btnFreightData.Location = new System.Drawing.Point(23, 13);
            this.btnFreightData.Name = "btnFreightData";
            this.btnFreightData.Size = new System.Drawing.Size(168, 23);
            this.btnFreightData.TabIndex = 0;
            this.btnFreightData.Text = "Convert Freight model data";
            this.btnFreightData.UseVisualStyleBackColor = true;
            this.btnFreightData.Click += new System.EventHandler(this.btnFreightData_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnFreightData);
            this.Name = "FormMain";
            this.Text = "DataConversion";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFreightData;
    }
}

