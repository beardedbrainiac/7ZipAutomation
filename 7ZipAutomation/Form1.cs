using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using _7ZipAutomation.Extensions;
using _7ZipAutomation.Models;

namespace _7ZipAutomation
{
    public partial class Form1 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bindingSource1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void DoAction(DataTable table, List<ArchiveModel> archives)
        {
            for (var i = 0; i < archives.Count; i++)
            {
                table.Rows[i]["Status"] = "Ongoing";
                
                Application.DoEvents();

                ProcessStartInfo p = new ProcessStartInfo();
                p.FileName = @"C:\Program Files\7-Zip\7z.exe";
                p.Arguments = $"a -t7z \"{archives[i].ArchivePath}\" \"{archives[i].FolderPath}\" -mx=9";
                p.WindowStyle = ProcessWindowStyle.Hidden;

                Application.DoEvents();

                Process process = Process.Start(p);
                process.WaitForExit();

                Application.DoEvents();

                archives[i].Archived = true;
                table.Rows[i]["Archived"] = archives[i].Archived;

                if (checkBox1.Checked)
                {
                    DirectoryInfo di = new DirectoryInfo(archives[i].FolderPath);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.Delete(archives[i].FolderPath);

                    archives[i].Deleted = true;
                    table.Rows[i]["Deleted"] = archives[i].Deleted;
                }

                table.Rows[i]["Status"] = "Completed";
            }

            textBox1.Clear();
            textBox2.Clear();

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            checkBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            checkBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;

            var archives = Directory.EnumerateDirectories(textBox1.Text)
                .Select((x, index) => new ArchiveModel()
                {
                    Id = index + 1,
                    FolderPath = x,
                    ArchivePath = $"{textBox2.Text}\\{x.Split('\\').AsEnumerable().LastOrDefault()}"
                })
                .ToList();

            var table = new DataTable("Archives");
            table = archives.ToDataTable();
            bindingSource1.DataSource = table;
            dataGridView1.DataSource = bindingSource1;

            Task.Factory.StartNew(() => DoAction(table, archives));
        }
        
        private void textBox1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
        
        private void textBox2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog2.ShowDialog();
            textBox2.Text = folderBrowserDialog2.SelectedPath;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool enable = !string.IsNullOrWhiteSpace((textBox1.Text)) && !string.IsNullOrWhiteSpace((textBox2.Text));
            button1.Enabled = enable;
            button2.Enabled = enable;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool enable = !string.IsNullOrWhiteSpace((textBox1.Text)) && !string.IsNullOrWhiteSpace((textBox2.Text));
            button1.Enabled = enable;
            button2.Enabled = enable;
        }
    }
}
