﻿using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace EAGL_Tool
{
    // SSH Wiki https://wiki.xentax.com/index.php/EA_SSH_FSH_Image
    public partial class Form1 : Form
    {
        //header
        int entries;
        int entryoffset;
        string filetype;
        int filesize;
        string EAGLVersion;

        //image info
        byte imgformat;
        int width;
        int height;
        byte[] centerx;
        byte[] centery;
        byte[] topx;
        byte[] topy;

        //palette
        int palettetype;
        int paletteblock;
        int colours;

        //metalbin
        int metalbintype;
        string metalbin;
        int metalbinblock;

        //hotspot
        int hotspottype;
        string hotspot;
        int hotspotblock;

        //imagename
        int imagenametype;
        string imgname;
        int imagenameblock;

        //comment;
        int commenttype;
        string comment;
        int commentblock;

        OpenFileDialog ofd = new OpenFileDialog();
        Item obj = new Item();

        public Form1()
        {
            InitializeComponent();
        }
        public class Item
        {
            public string strText;
            public string strValue;
            public override string ToString()
            {
                return this.strText;
            }
        }
        private void parseentries(string name, int entry)
        {
            for (int i = 0; i < entries;)
            {
                listBox1.Items.Add(new ListItem { Name = name, Value = entry });

                i += entries;
                break;


            }

        }
        private void getinfo()
        {
            BinaryReader br = new BinaryReader(File.OpenRead(ofd.FileName));
            int count1 = 2;
            int count2 = 4;
            byte[] data;
            for (int i = 0; i <= ofd.FileName.Length;)
            {
                // FILE HEADER

                br.BaseStream.Position = i;
                // READ FILE TYPE - SHPS,SHPX,ETC..
                data = br.ReadBytes(count2);
                filetype = System.Text.Encoding.UTF8.GetString(data).ToString();
                textBox1.Text = filetype;
                // Read File Size
                filesize = BitConverter.ToInt32(br.ReadBytes(count2), 0);
                textBox2.Text = filesize.ToString();
                // Read File entries
                entries = BitConverter.ToInt32(br.ReadBytes(count2), 0);
                textBox3.Text = entries.ToString();
                // Read GIMEX version
                data = br.ReadBytes(count2);
                EAGLVersion = System.Text.Encoding.UTF8.GetString(data).ToString();
                textBox4.Text = EAGLVersion;
                for (int k = 0; k <= entries;)
                {
                    // Read entry
                    data = br.ReadBytes(4);

                    entryoffset = BitConverter.ToInt32(br.ReadBytes(count2), 0);
                    parseentries(System.Text.Encoding.UTF8.GetString(data).ToString(), entryoffset);
                    k ++;
                }
                i ++;
                break;

            }
            br.Close();

        }
        private void texturedata()
        {
            BinaryReader br = new BinaryReader(File.OpenRead(ofd.FileName));
            //bytes to read
            int count1 = 2;
            int count2 = 3;
            byte[] data;
            //retrieve values of list box selected item
            var value = ((ListItem)listBox1.SelectedItem).Value;
            for (int i = Convert.ToInt32(value); i <= filesize;)
            {
                // img info
                //set binary reader to go the offset of selected item in listbox
                br.BaseStream.Position = i;

                // read img format
                imgformat = br.ReadByte();
                sbyte sb = unchecked((sbyte)imgformat);
                textBox5.Text = sb.ToString();
                // read img block size
                imagenameblock = BitConverter.ToUInt16(br.ReadBytes(3), 0);
                textBox6.Text = imagenameblock.ToString();
                // read img size width and height
                width = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                textBox7.Text = width.ToString();
                height = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                // read img position
                textBox8.Text = height.ToString();
                centerx = br.ReadBytes(2);
                centery = br.ReadBytes(2);
                topx = br.ReadBytes(2);
                topy = br.ReadBytes(2);
                ;

                i++;
                break;

            }
            br.Close();

        }

        //parse to 8
        private void uint8parse(byte[] b1)
        {
            var unsigned = b1;
            var signed = new sbyte[unsigned.Length];
            Buffer.BlockCopy(unsigned, 0, signed, 0, unsigned.Length);
            
            
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                getinfo();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        public class ListItem
        {
            public string Name { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((ListItem)listBox1.SelectedItem).Value;
            texturedata();

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
