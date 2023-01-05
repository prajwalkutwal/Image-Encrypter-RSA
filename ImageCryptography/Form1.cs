using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageCryptography
{
    public partial class Form1 : Form
    {
        static int  RSA_P = 0;
        static int RSA_Q = 0; 
        static int RSA_E = 0;
        static int d = 0;
        static int n = 0;
        
        static string loadImage = "";
        static string loadcipher = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            disable_all();
        }
        
        public string encrypt(string imageToEncrypt)
        {
            string hex = imageToEncrypt;
            char[] ar = hex.ToCharArray();
            String c = "";
            progressBar1.Maximum = ar.Length;
             for (int i = 0; i < ar.Length; i++)
             {
                Application.DoEvents();
                progressBar1.Value = i;
                if (c == "")
                   c = c + ImageCrypto.RSAalgorithm.BigMod(ar[i], RSA_E, n);
                else
                   c = c + "-" + ImageCrypto.RSAalgorithm.BigMod(ar[i], RSA_E, n);
             }
             return c;
        }

        public string decrypt(String  imageToDecrypt)
        {
                char[] ar = imageToDecrypt.ToCharArray();
                int i = 0, j = 0;
                string c = "", dc = "";
                progressBar2.Maximum = ar.Length;
                try
                {
                    for (; i < ar.Length; i++)
                    {
                        Application.DoEvents();
                        c = "";
                        progressBar2.Value = i;
                        for (j = i; ar[j] != '-'; j++)
                            c = c + ar[j];
                        i = j;

                        int xx = Convert.ToInt16(c);
                        dc = dc + ((char)ImageCrypto.RSAalgorithm.BigMod(xx, d, n)).ToString();
                    }
                }
                catch (Exception ex) { }

            return dc;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (button3.Text == "Set Details")
            {
                if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Enter Valid Detail For RSA", "ERROR");
                }

                else
                {
                    if (ImageCrypto.library.IsPrime(Convert.ToInt16(textBox2.Text)))
                    {
                        RSA_P = Convert.ToInt16(textBox2.Text);
                    }
                    else
                    {
                        textBox2.Text = "";
                        MessageBox.Show("Enter Prime Number");
                        return;
                    }
                    if (ImageCrypto.library.IsPrime(Convert.ToInt16(textBox3.Text)))
                    {
                        RSA_Q = Convert.ToInt16(textBox3.Text);
                    }
                    else
                    {
                        textBox3.Text = "";
                        MessageBox.Show("Enter Prime Number");
                        return;
                    }

                    RSA_E = Convert.ToInt16(textBox4.Text);

                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    button4.Enabled = true;
                    button3.Text = "ReSet Details";
                }
            }
            else
            {
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                button4.Enabled = false;
                button3.Text = "Set Details";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            n=ImageCrypto.RSAalgorithm.n_value(RSA_P, RSA_Q);
            button1.Enabled = false;
            disable_all();
            String en =  encrypt(loadImage);
            File.WriteAllText(textBox5.Text, en);
            MessageBox.Show("Encryption Done");
            button1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button10.Enabled = false;
            disable_all();
            String de = decrypt(loadcipher);
            pictureBox1.Image = ImageCrypto.library.ConvertByteToImage(ImageCrypto.library.DecodeHex(de));
            MessageBox.Show("Decryption Done");
            pictureBox1.Image.Save(textBox6.Text, System.Drawing.Imaging.ImageFormat.Jpeg);
            MessageBox.Show("Picture Saved");
            button10.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.Text == "Set Details")
            {
                if (textBox9.Text == "" || textBox8.Text == "")
                {
                    MessageBox.Show("Enter Valid Detail For RSA", "ERROR");
                }
                else
                {
                    if (Convert.ToInt16(textBox9.Text) > 0)
                    {
                        d = Convert.ToInt16(textBox9.Text);
                    }
                    else
                    {
                        textBox9.Text = "";
                        MessageBox.Show("Enter Valid Number");
                        return;
                    }
                    if (Convert.ToInt16(textBox8.Text) > 0)
                    {
                        n = Convert.ToInt16(textBox8.Text);
                    }
                    else
                    {
                        textBox8.Text = "";
                        MessageBox.Show("Enter Valid Number");
                        return;
                    }

                    textBox9.Enabled = false;
                    textBox8.Enabled = false;
                    button8.Text = "ReSet Details";                    
                    button7.Enabled = true;

                }
            }
            else
            {
                textBox9.Text = "";
                textBox8.Text = "";
                textBox9.Enabled = true;
                textBox8.Enabled = true;
                button8.Text = "Set Details";
                button7.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadImage =  BitConverter.ToString(ImageCrypto.library.ConvertImageToByte(pictureBox1.Image));
            MessageBox.Show("Image Load Successfully");
            groupBox4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog save1 = new SaveFileDialog();
            save1.Filter = "TEXT|*.txt";
            if (save1.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = save1.FileName;
                button5.Enabled = true;
            }
            else
            {
                textBox5.Text = "";
                button5.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open1 = new OpenFileDialog();
            open1.Filter = "JPG|*.JPG";
            if (open1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = open1.FileName;
                pictureBox1.Image = Image.FromFile(textBox1.Text);
                button2.Enabled = true;
                FileInfo fi = new FileInfo(textBox1.Text);

                label9.Text = "File Name: " + fi.Name;
                label10.Text = "Image Resolution: " + pictureBox1.Image.PhysicalDimension.Height + " X " + pictureBox1.Image.PhysicalDimension.Width;

                double imageMB = ((fi.Length / 1024f) / 1024f);                

                label11.Text = "Image Size: " + imageMB.ToString(".##") + "MB";
            }
            else
            {
                textBox1.Text = "";
                label9.Text = "File Name: ";
                label10.Text = "Image Resolution: ";
                label11.Text = "Image Size: ";

                pictureBox1.Image = Properties.Resources.blank;
                button2.Enabled = false;
                
            }
        }

        private void disable_all()
        {            
            button2.Enabled = false;
            groupBox4.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            button9.Enabled = false;
            groupBox5.Enabled = false;
            button7.Enabled = false;
            button6.Enabled = false;
        }

        private void enable_all()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            groupBox4.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog open1 = new OpenFileDialog();
            open1.Filter = "TEXT|*.txt";
            if (open1.ShowDialog() == DialogResult.OK)
            {
                textBox7.Text = open1.FileName;
                button9.Enabled = true;
            }
            else
            {
                textBox7.Text = "";
                button9.Enabled = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {   
            loadcipher = File.ReadAllText(textBox7.Text);
            MessageBox.Show("Load Cipher Successfully");
            groupBox5.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog save1 = new SaveFileDialog();
            save1.Filter = "JPG|*.JPG";
            if (save1.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = save1.FileName;
                button6.Enabled = true;
            }
            else
            {
                textBox6.Text = "";
                button6.Enabled = false;
            }
        }          
    }
}
