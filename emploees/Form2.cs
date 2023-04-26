using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace employeesV1
{
    public partial class Form2 : Form
    {

        DataTable table2 = new DataTable("");
        DateTime time;
        DateTime now = DateTime.Now;
        Form1 form1 = new Form1();
        public Form2()
        {
            InitializeComponent();

        
            DateTime now = DateTime.Now;

            int month = now.Month;
            int day = now.Day;
            string dirPath = @"C:\Users\public\data\callouts";
            string path = $"C:\\Users\\public\\data\\callouts\\callouts_for_({month}_{day}).csv";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!File.Exists(path))
            {
                File.Create(path);
                Application.Restart();
                Environment.Exit(0);
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
               
            int month = now.Month;
            int day = now.Day;


            table2.Columns.Add("Name", Type.GetType("System.String"));
            table2.Columns.Add("Shift", Type.GetType("System.String"));
         
            dataGridView2.DataSource = table2;

         
            string path = $"C:\\Users\\public\\data\\callouts\\callouts_({month}_{day}).csv";
            if (!File.Exists(path))
            {
                File.Create(path);
                Application.Restart();
                Environment.Exit(0);
            }

            string[] csvLines = System.IO.File.ReadAllLines($"C:\\Users\\public\\data\\callouts\\callouts_({month}_{day}).csv");
             
            //populate the combobox
            foreach (string name in form1.getEmployees())
            {
                cmbEmps.Items.Add(name);
            }

            //populate the Grid Rows from csv file
            for (int i = 1; i < csvLines.Length; i++)
            {
                table2.Rows.Add(csvLines[i].Split(',')[0], csvLines[i].Split(',')[1]);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void cmbEmps_SelectedIndexChanged(object sender, EventArgs e)
        {


            table2.Rows.Add(cmbEmps.Text, DateTime.Now.ToString("M/dd/yyyy"));
            exportCsvForm2();
        }

        public void exportCsvForm2()
        {
            int month = now.Month;
            int day = now.Day;

            if (dataGridView2.Rows.Count > -1)
            {

                string path = $"C:\\Users\\public\\data\\callouts\\callouts_({month}_{day}).csv";
                bool fileError = false;

                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (IOException ex)
                    {
                        fileError = true;
                        MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                    }
                }
                if (!fileError)
                {
                    try
                    {
                        int columnCount = dataGridView2.Columns.Count;
                        string columnNames = "";
                        string[] outputCsv = new string[dataGridView2.Rows.Count + 1];
                        for (int i = 0; i < columnCount; i++)
                        {
                            columnNames += dataGridView2.Columns[i].HeaderText.ToString() + ",";
                        }
                        outputCsv[0] += columnNames;

                        for (int i = 1; (i - 1) < dataGridView2.Rows.Count; i++)
                        {
                            for (int j = 0; j < columnCount; j++)
                            {
                                outputCsv[i] += dataGridView2.Rows[i - 1].Cells[j].Value.ToString() + ",";
                            }
                        }

                        File.WriteAllLines(path, outputCsv, Encoding.UTF8);

                        //MessageBox.Show("Data Exported Successfully !!!", "Info");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error :" + ex.Message);
                    }
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
             
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
    }
    }
