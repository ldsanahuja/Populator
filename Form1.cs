using System;
using System.Windows.Forms;
using System.IO;


namespace Populator
{
    public partial class Form1 : Form
    {
        private string currentDir;
        private string currentFile;
        private string currentFileNoPath;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Get current working directory
            currentDir = Directory.GetCurrentDirectory();
            textBox2.Text = currentDir;
            //set label3 text
            SetLabel3();
        }

        private void SetLabel3()
        {
            //the current dir
            label3.Text = Directory.GetDirectories(currentDir).Length.ToString() + " directorios en la carpeta";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //bye-bye
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dialog box. Any file can be selected
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = currentDir;
                ofd.Filter = "Todos los archivos (*.*) | *.*";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //set textbox with path
                    textBox1.Text = ofd.FileName;
                    //save path and file
                    currentFile = ofd.FileName;
                    //save filename without path
                    currentFileNoPath = ofd.SafeFileName;
                }
                else
                    label1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //dialog box, directories
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    //set both as currentdir and label3
                    label2.Text = fbd.SelectedPath;
                    currentDir = fbd.SelectedPath;
                    SetLabel3();
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DoReplication();
        }

        private void DoReplication()
        {
            //Sanity checks
            if (!Directory.Exists(currentDir))
            {
                MessageBox.Show("La carpeta selectionada no existe o es incorrecta", "Error en destino", MessageBoxButtons.OK);
                return;
            }
            if (!File.Exists(currentFile))
            {
                MessageBox.Show("El archivo selectionado no existe", "Error en origen", MessageBoxButtons.OK);
                return;
            }
            if (Directory.GetDirectories(currentDir).Length <= 0)
            {
                MessageBox.Show("No hay subdirectorios en la carpeta seleccionada", "Error de copia", MessageBoxButtons.OK);
                return;
            }
            //its ok, lets do it. Add a stopwatch just for fun
            int counter = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            foreach(string d in Directory.GetDirectories(currentDir))
            {
                File.Copy(currentFile, d + "\\" + currentFileNoPath); //d has no ending slashes!
                counter++;
            }
            sw.Stop();
            //enough simple op to not have any trycatch
            //TODO: If errors in this step, add a trycatch
            MessageBox.Show("Copiado el archivo en " + counter + " carpetas en " + sw.ElapsedMilliseconds + "ms.", "Listo!", MessageBoxButtons.OK);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //About dialog
            MessageBox.Show("Selecciona un archivo a copiar y un destino, se copiará el archivo en cada subcarpeta.\n \n Version 0.1\n Lluis Sanahuja 2026\n https://github.com/ldsanahuja", "Acerca de...");
        }
    }
}
