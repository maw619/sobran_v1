using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Status;

namespace employeesV1
{
    public partial class Form1 : Form
    {

        DateTime time;
        DateTime now = DateTime.Now;
        
        DataTable table = new DataTable("My Table");
        
 
        public Form1()
        {
            InitializeComponent();
            //prevent window resizing
            this.FormBorderStyle = FormBorderStyle.FixedSingle; 

            int month = now.Month;
            int day = now.Day;
            string path = $"C:\\Users\\public\\export_({month}_{day}).csv";
            if (!File.Exists(path))
            {
                File.Create(path);
                Application.Restart();
                Environment.Exit(0);
            }
            
        }
  
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
         
        private void Form1_Load(object sender, EventArgs e)
        {

            // Disable the control box (which includes the close button)
            //this.ControlBox = false;
             
            lblDate2.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy").ToString();

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Shift", Type.GetType("System.String"));
            table.Columns.Add("Arrived", Type.GetType("System.String"));
            table.Columns.Add("Time Diff.", Type.GetType("System.String"));
            dataGridView1.DataSource = table;

            int month = now.Month;
            int day = now.Day;
           
            string[] csvLines = System.IO.File.ReadAllLines($"C:\\Users\\public\\export_({month}_{day}).csv");

           
            //populate the combobox
            foreach (string name in getEmployees())
            {
                comboBox1.Items.Add(name);
            }

            //populate the Grid Rows from csv file
            for (int i = 1; i < csvLines.Length; i++)
            {
                table.Rows.Add(csvLines[i].Split(',')[0], csvLines[i].Split(',')[1], csvLines[i].Split(',')[2], csvLines[i].Split(',')[3]); 
            }


            Console.WriteLine(dataGridView1.Rows.Count);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime time;
              
            int yearNow = now.Year;
            int month = now.Month;
            int day = now.Day;
            string shift = "";

            DateTime timeA = new DateTime(yearNow, month, day, 5, 00, 00);
            DateTime timeB = new DateTime(yearNow, month, day, 6, 15, 00);

            if (comboBox1.Text.Contains("Holly"))
            {
                shift = timeA.ToString("h:mm tt", CultureInfo.InvariantCulture);
                time = new DateTime(yearNow, month, day, 5, 00, 00);
            }
            else
            {
                shift = timeB.ToString("h:mm tt", CultureInfo.InvariantCulture);
                time = new DateTime(yearNow, month, day, 6, 15, 00);
            }
  
            TimeSpan diffe = (now - time).Duration();


            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (comboBox1.Text.Equals(dataGridView1.Rows[i].Cells[0].Value.ToString())) 
                {
                    MessageBox.Show($"employee already marked for {month}/{day}/{yearNow}");
                    return;
                } 
            }
            table.Rows.Add(comboBox1.Text, shift, now.ToString("h:mm tt", CultureInfo.InvariantCulture), diffe.ToString("hh':'mm"));
            exportToCsv();
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            exportToCsv();
        }

        public List<string> getEmployees()
        {
            List<string> employees = new List<string>();
            employees.Add("Holly");
            employees.Add("Holly1");
            employees.Add("Holly2");
            employees.Add("Holly3");
            employees.Add("Charlie");
            employees.Add("Charlie1");
            employees.Add("Charlie2");
            employees.Add("Charlie3");
            return employees;

        }

        public void exportToCsv()
        {
            int month = now.Month;
            int day = now.Day;

            if (dataGridView1.Rows.Count > 0)
            {
                 
                string path = $"C:\\Users\\public\\export_({month + "_" + day}).csv";
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
                            int columnCount = dataGridView1.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView1.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value.ToString() + ",";
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

        public void exportToCsvWithUserPrompt()
        {
            int month = now.Month;
            int day = now.Day;

            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = $"C:\\Users\\public\\export_({month + "_" + day}).csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
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
                            int columnCount = dataGridView1.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView1.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Are you sure you want to exit?");
            this.Close();
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            
            int month = now.Month;
            int day = now.Day;
            System.Diagnostics.Process.Start($"C:\\Users\\public\\export_({month}_{day}).csv");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int currentSelectionUser = dataGridView1.CurrentCell.RowIndex;

            DialogResult dialogResult = MessageBox.Show($"Are you sure to you want to delete {dataGridView1.Rows[currentSelectionUser].Cells[0].Value}?", "Confirm", MessageBoxButtons.YesNo);

           
            int index = 0;
            Console.WriteLine(dataGridView1.ColumnCount);
            if (dataGridView1.Rows.Count < 1)
            { 
                MessageBox.Show("There are no more items");
            }
            else
            {
                if (dialogResult == DialogResult.Yes)
                {
                    index = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows.RemoveAt(index);
                    exportToCsv();
                }
                else
                {
                    return;
                }
               
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

   
    }
}
