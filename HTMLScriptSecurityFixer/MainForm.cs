using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FluentFTP;

namespace HTMLScriptSecurityFixer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonFolderSelect_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            fixFolder(folderBrowserDialog.SelectedPath);
        }

        private void fixFolder(string path)
        {
            string[] filesHtml = System.IO.Directory.GetFiles(path, "*.html", System.IO.SearchOption.AllDirectories);
            foreach (string file in filesHtml)
                fixFile(file);
            string[] filesPhp = System.IO.Directory.GetFiles(path, "*.php", System.IO.SearchOption.AllDirectories);
            foreach (string file in filesPhp)
                fixFile(file);
            string[] filesHtm = System.IO.Directory.GetFiles(path, "*.htm", System.IO.SearchOption.AllDirectories);
            foreach (string file in filesHtm)
                fixFile(file);
        }

        private void fixFile(string path)
        {
            string fileContents = System.IO.File.ReadAllText(path);
            string fixedContents = fileContents.Replace("http://", "https://");
            System.IO.File.WriteAllText(path, fixedContents);
            Console.WriteLine("Fixed file " + path);
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            fixFile(openFileDialog.FileName);
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            FtpClient ftpClient = new FtpClient()
        }
    }
}