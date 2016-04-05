using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace FreightThreading
{
    public partial class MainGUI : Form
    {
        public MainGUI()
        {
            InitializeComponent();
            txtArgument.Text = @"-a -I D:\Temp\FreightThreading";
            txtArgument.Text += @" -d D:\Temp\FreightThreading\test1.dzn";
            txtArgument.Text += @" -d D:\Temp\FreightThreading\ft16.dzn";
            txtArgument.Text +=@" D:\Temp\FreightThreading\model.mzn";
            //txtArgument.Text = @"-a -I D:\Temp\workshop_04 D:\Temp\workshop_04\groupphoto.mzn";
        }

        string line;
        StringBuilder output = new StringBuilder();

        private void button1_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "mzn-gecode.bat";
            process.StartInfo.Arguments = txtArgument.Text;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //process.OutputDataReceived += (sendingProcess, dataLine) =>
            //{
            //    line = dataLine.Data;
            //    output.AppendLine(line);
            //    writeOutput2TextBox();
            //};
            //process.ErrorDataReceived += (sendingProcess, dataLine) =>
            //{
            //    line = dataLine.Data;
            //    output.AppendLine(line);
            //    writeOutput2TextBox();
            //};

            //process.Start();
            //process.BeginOutputReadLine();

            textBox1.Text = "";
            process.Start();

            //// Synchronously read the standard output of the spawned process. 
            StreamReader reader = process.StandardOutput;
            StreamReader error = process.StandardError;
            long count = 0;
            while ((line = reader.ReadLine()) != null)
            {
                // Write the redirected output to this application's window.
                if (line.Contains("--------"))
                    line +=" Sol - " + (++count).ToString();
                writeOutput2TextBox();
                System.Threading.Thread.Sleep(50);
            }
            while ((line = error.ReadLine()) != null)
            {
                // Write the redirected output to this application's window.
                writeOutput2TextBox();
                System.Threading.Thread.Sleep(50);
            }

            process.WaitForExit();
            process.Close();
            //textBox1.Text = output.ToString();
            //Console.WriteLine("\n\nPress any key to exit.");
            //Console.ReadLine();
        }

        private void writeOutput2TextBox()
        {
            if (textBox1.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(writeOutput2TextBox));
                return;
            }

            textBox1.Text += line + "\r\n";
            textBox1.Refresh();
        }

        private void btnViewGraph_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {   
                FreightViz form = new FreightViz();
                form.timetableFile = openDlg.FileName;
                form.ShowDialog();
            }
            
        }
    }
}
