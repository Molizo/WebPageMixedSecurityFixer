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
using System.Net.Sockets;
using System.Media;

namespace HTMLScriptSecurityFixer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            textBoxUsername.Text = Properties.Settings.Default.lastUsername;
            textBoxIP.Text = Properties.Settings.Default.lastIP;
            textBoxPort.Text = Properties.Settings.Default.lastPort;
            textBoxRootPath.Text = Properties.Settings.Default.lastRootPath;
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
            int port = Convert.ToInt32(textBoxPort.Text.ToString());
            try
            {
                FtpClient ftpClient = new FtpClient(textBoxIP.Text, port, textBoxUsername.Text, textBoxPassword.Text);
                ftpClient.Connect();
                ftpClient.SetWorkingDirectory(textBoxRootPath.Text);
                foreach (FtpListItem item in ftpClient.GetListing(textBoxRootPath.Text))
                {
                    if (item.Type == FtpFileSystemObjectType.File)
                    {
                        if (item.FullName.Contains(".htm") || item.FullName.Contains(".php"))
                        {
                            ftpClient.DownloadFile("./file.ext", item.FullName);
                            fixFile("./file.ext");
                            ftpClient.UploadFile("./file.ext", item.FullName);
                            System.Threading.Thread.Sleep(100);
                            File.Delete("./file.ext");
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error connecting to server\nPlease check the connection details", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}