using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Enigma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            _F1_data = this;
            radioButton1.Checked = true;
            _enigma._rotors.Add(_enigma._code_Enigma_I_rotor_I);
            _enigma._rotors.Add(_enigma._code_Enigma_I_rotor_II);
            _enigma._rotors.Add(_enigma._code_Enigma_I_rotor_III);

            for (int i = 0; i < 26; i++)
                _enigma._plugb[(char)(i + 65)] = (char)(i + 65);
        }

        public Data _enigma = new Data();
        public bool _if_plug = true;
        Form_Debug _debug = new Form_Debug();
        public static Form1 _F1_data;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {            
            _enigma._gearI_shift = trackBar1.Value;
            label1.Text = ((char)(65 + trackBar1.Value)).ToString();
            label_I.Text = str_transform(_enigma._gearI_shift,1);           
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {          
            _enigma._gearII_shift = trackBar2.Value;
            label2.Text = ((char)(65 + trackBar2.Value)).ToString();
            label_II.Text = str_transform(_enigma._gearII_shift,2);        
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {            
            _enigma._gearIII_shift = trackBar3.Value;
            label3.Text = ((char)(65 + trackBar3.Value)).ToString();
            label_III.Text = str_transform(_enigma._gearIII_shift,3);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label_I.Text = str_transform(_enigma._gearI_shift,1);
            label_II.Text = str_transform(_enigma._gearII_shift,2);
            label_III.Text = str_transform(_enigma._gearIII_shift,3);

            int j = 0;
            for (int i = 0; i < 26; i++)
            {
                if (i % 9 == 0)
                    j++;

                var tb = new TextBox();

                tb.Location = new System.Drawing.Point(55 * (i % 9) + 30, 30 * j-12 );
                tb.Name = ((char)(i + 65)).ToString() + "_TextBox";
                tb.Size = new System.Drawing.Size(20, 10);
                tb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
                tb.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                tb.MaxLength = 1;
                tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

                tb.KeyPress += toolStripTextBox1_KeyPress;
                tb.TextChanged += plug_check;
                groupBox_plug.Controls.Add(tb);
                
                var lb = new Label();
                lb.Location = new System.Drawing.Point(55 * (i % 9) + 5, 30 * j + 2-12);
                lb.Name = ((char)(i + 65)).ToString() + "_Label";
                lb.Size = new System.Drawing.Size(35, 13);
                lb.Text = ((char)(i + 65)).ToString() + "->";
                lb.Font = new System.Drawing.Font("Consolas", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));

                groupBox_plug.Controls.Add(lb);
            }
        }

        private string str_transform(int _shift, int nomer)
        {
            string str  = new string(_enigma._rotors[nomer-1]);
            string shift = str.Substring(0, _shift);
            str = str.Substring(_shift);
            str += shift;

            for (int i = 0; i < 26; i++)
                str = str.Insert(i * 2 + 1, " ");

            return str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox_input_key.Clear();
            textBox_input_key.Text += label1.Text + label2.Text + label3.Text;
            textBox2.Clear();

            _enigma._msg = "";
            _enigma._msg_plugged = "";
            _enigma._msg_transf_plugged = "";
            _enigma._msg_transformed = "";

            var str = textBox1.Text.Replace(" ", "").Replace(",", "");

            _enigma._msg = str;
            //_enigma._msg_transf_plugged = msg_transformation(_enigma._msg.ToCharArray());
            _enigma.msg_transformation();
            TrackBar_set();

            pictureBox1.Refresh();
            textBox_output_key.Clear();
            textBox_output_key.Text += label1.Text + label2.Text + label3.Text;
            
            string model = "";
            if (radioButton1.Checked)
                model = "Enigma I";
            else
                model = "Germal Railway Enigma (Rocket)";

            string rotors_pos = check_rotors_position();
            textBox2.Text = _enigma._msg_transf_plugged;

            listView1.Items.Add(new ListViewItem(new string[] {model,rotors_pos, _enigma._msg,_enigma._msg_plugged, textBox_input_key.Text,_enigma._msg_transformed,_enigma._msg_transf_plugged, textBox_output_key.Text }));

            if (listView1.Items.Count % 2 == 0)
                listView1.Items[listView1.Items.Count-1].BackColor = Color.LightGray;
        }

        private string check_rotors_position()
        {
            int position= (groupBox3.Text.Length - 6)*100 + (groupBox4.Text.Length - 6) * 10 + (groupBox5.Text.Length - 6);
            return position.ToString();
        }

        private void TrackBar_set()
        {
            trackBar1.Value = _enigma._gearI_shift;
            trackBar2.Value = _enigma._gearII_shift;
            trackBar3.Value = _enigma._gearIII_shift;

            label1.Text = ((char)(65 + trackBar1.Value)).ToString();
            label2.Text = ((char)(65 + trackBar2.Value)).ToString();
            label3.Text = ((char)(65 + trackBar3.Value)).ToString();

            label_I.Text = string.Join(" ",_enigma.rotorI.Values.ToArray());
            label_II.Text = string.Join(" ", _enigma.rotorII.Values.ToArray());
            label_III.Text = string.Join(" ", _enigma.rotorIII.Values.ToArray());

        }

        private void controls_paint(object sender, PaintEventArgs e)
        {
            int x1 = 0, y1 = 0;
            if (sender.GetType().Name == "Label")
            {
                x1 = ((Label)sender).Bounds.X + ((Label)sender).Parent.Location.X;
                y1 = ((Label)sender).Location.Y + ((Label)sender).Parent.Location.Y;
            }
            if (sender.GetType().Name == "GroupBox" && sender != groupBox2)
            {
                x1 = ((GroupBox)sender).Bounds.X;
                y1 = ((GroupBox)sender).Location.Y;
            }

            e.Graphics.DrawLine(Pens.Black, 100 - x1, 0 - y1, 100 - x1, 100 - y1);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int otstup = 12;
            Pen pen_red = new Pen(Color.Red, 2);
            Pen pen_blue = new Pen(Color.Blue, 2);

            e.Graphics.DrawString(label4.Text, new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, 10, 10 + otstup);
            e.Graphics.DrawString(label_I.Text, new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, 10, 22 + otstup);

            e.Graphics.DrawString(label5.Text, new Font("Consolas", 10,FontStyle.Bold), Brushes.Black, 10, 70 + otstup);
            e.Graphics.DrawString(label_II.Text, new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, 10, 82 + otstup);

            e.Graphics.DrawString(label6.Text, new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, 10, 130 + otstup);
            e.Graphics.DrawString(label_III.Text, new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, 10, 142 + otstup);

            e.Graphics.DrawString("Input", new Font("Consolas", 8, FontStyle.Italic | FontStyle.Underline), Brushes.Red, 15 * _enigma._chain[0], -2);
            e.Graphics.DrawString("Output", new Font("Consolas", 8, FontStyle.Italic | FontStyle.Underline), Brushes.Blue, 15 * _enigma._chain[6], -4 + otstup);

            e.Graphics.DrawRectangle(pen_red, 15 * _enigma._chain[0] + 10, 10 + otstup, 14, 28);
            e.Graphics.DrawRectangle(pen_red, 15 * _enigma._chain[1] + 10, 70 + otstup, 14, 28);
            e.Graphics.DrawRectangle(pen_red, 15 * _enigma._chain[2] + 10, 130 + otstup, 14, 28);

            e.Graphics.DrawRectangle(pen_blue, 15 * _enigma._chain[4] + 10, 130 + otstup, 14, 28);
            e.Graphics.DrawRectangle(pen_blue, 15 * _enigma._chain[5] + 10, 70 + otstup, 14, 28);
            e.Graphics.DrawRectangle(pen_blue, 15 * _enigma._chain[6] + 10, 10 + otstup, 14, 28);

            e.Graphics.DrawLine(pen_red, 15 * _enigma._chain[0] + 10 + 7, 10 + 28 + otstup, 15 * _enigma._chain[0] + 10 + 7, 53 + otstup);
            e.Graphics.DrawLine(pen_red, 15 * _enigma._chain[1] + 10 + 7, 70 + otstup, 15 * _enigma._chain[1] + 10 + 7, 53 + otstup);
            e.Graphics.DrawLine(pen_red, 15 * _enigma._chain[0] + 10 + 7, 53 + otstup, 15 * _enigma._chain[1] + 10 + 7, 53 + otstup);

            e.Graphics.DrawLine(pen_red, 15 * _enigma._chain[1] + 10 + 7, 70 + 28 + otstup, 15 * _enigma._chain[1] + 10 + 7, 113 + otstup);
            e.Graphics.DrawLine(pen_red, 15 * _enigma._chain[2] + 10 + 7, 130 + otstup, 15 * _enigma._chain[2] + 10 + 7, 113 + otstup);
            e.Graphics.DrawLine(pen_red, 15 * _enigma._chain[1] + 10 + 7, 113 + otstup, 15 * _enigma._chain[2] + 10 + 7, 113 + otstup);

            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[2] + 10 + 7, 130 + 28 + otstup, 15 * _enigma._chain[2] + 10 + 7, 165 + otstup);
            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[4] + 10 + 7, 130 + 28 + otstup, 15 * _enigma._chain[4] + 10 + 7, 165 + otstup);
            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[4] + 10 + 7, 165 + otstup, 15 * _enigma._chain[2] + 10 + 7, 165 + otstup);

            //e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[5] + 10 + 7, 70 + 28, 15 * _enigma._chain[4] + 10 + 7, 130);
            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[5] + 10 + 7, 70 + 28 + otstup, 15 * _enigma._chain[5] + 10 + 7, 117 + otstup);
            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[4] + 10 + 7, 130 + otstup, 15 * _enigma._chain[4] + 10 + 7, 117 + otstup);
            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[4] + 10 + 7, 117 + otstup, 15 * _enigma._chain[5] + 10 + 7, 117 + otstup);

            //e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[6] + 10 + 7, 10 + 28, 15 * _enigma._chain[5] + 10 + 7, 70);
            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[6] + 10 + 7, 10 + 28 + otstup, 15 * _enigma._chain[6] + 10 + 7, 57 + otstup);
            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[5] + 10 + 7, 70 + otstup, 15 * _enigma._chain[5] + 10 + 7, 57 + otstup);
            e.Graphics.DrawLine(pen_blue, 15 * _enigma._chain[5] + 10 + 7, 57 + otstup, 15 * _enigma._chain[6] + 10 + 7, 57 + otstup);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && toolStripTextBox1.Text.Length == 3)
            {
               var key = toolStripTextBox1.Text.ToCharArray();
               
               _enigma._gearI_shift = (int)key[0] - 65;
               _enigma._gearII_shift = (int)key[1] - 65;
               _enigma._gearIII_shift = (int)key[2] - 65;

               trackBar1.Value = _enigma._gearI_shift;
               trackBar2.Value = _enigma._gearII_shift;
               trackBar3.Value = _enigma._gearIII_shift;

               label1.Text = key[0].ToString();
               label2.Text = key[1].ToString();
               label3.Text = key[2].ToString();

               label_I.Text = str_transform(_enigma._gearI_shift,1);
               label_II.Text = str_transform(_enigma._gearII_shift,2);
               label_III.Text = str_transform(_enigma._gearIII_shift,3);
            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Эмулятор трехроторной шифровальной машины \"Энигма\"\nНаписал Частов Антон\nККС-1-12\n2012 год","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            rotors_reset();
        }

        public void rotors_reset()
        {
            _enigma._gearI_shift = 0;
            _enigma._gearII_shift = 0;
            _enigma._gearIII_shift = 0;

            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 0;

            label1.Text = "A";
            label2.Text = "A";
            label3.Text = "A";

            label_I.Text = str_transform(_enigma._gearI_shift,1);
            label_II.Text = str_transform(_enigma._gearII_shift,2);
            label_III.Text = str_transform(_enigma._gearIII_shift,3);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text.Length == 3)
            {
                var key = toolStripTextBox1.Text.ToCharArray();

                _enigma._gearI_shift = (int)key[0] - 65;
                _enigma._gearII_shift = (int)key[1] - 65;
                _enigma._gearIII_shift = (int)key[2] - 65;

                trackBar1.Value = _enigma._gearI_shift;
                trackBar2.Value = _enigma._gearII_shift;
                trackBar3.Value = _enigma._gearIII_shift;

                label1.Text = key[0].ToString();
                label2.Text = key[1].ToString();
                label3.Text = key[2].ToString();

                label_I.Text = str_transform(_enigma._gearI_shift,1);
                label_II.Text = str_transform(_enigma._gearII_shift,2);
                label_III.Text = str_transform(_enigma._gearIII_shift,3);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void plug_check(object sender, EventArgs e)
        {
            if (_if_plug)  //кривовато
            {
                TextBox tb = (TextBox)sender;

                if (tb.Text.Length == 0)
                {
                    var elementII = tb.Name[0]; // plugb key.
                   
                    var key = _enigma._plugb.FirstOrDefault(p => p.Value == elementII).Key;
                    _enigma._plugb[elementII] = elementII;
                    _enigma._plugb[key] = key;
                }
                else
                {                    
                    var element = tb.Text[0]; // plugb key
                    var elementI = _enigma._plugb[element]; // plugb value 
                    var elementII = tb.Name[0]; // plugb key
                    var elementIII = _enigma._plugb[elementII]; // plugb value 

                    _enigma._plugb[elementI] = elementI;

                    _enigma._plugb[element] = elementII;
                    _enigma._plugb[elementII] = element;

                    if(elementII!=elementIII)
                        _enigma._plugb[elementIII] = elementIII;
                }

                pictureBox2.Refresh();
                textbox_reload();
            }                    
        }

        private void textbox_reload()
        {
            _if_plug = false;

            foreach (var el in _enigma._plugb)
            {
                var tb = el.Key.ToString() + "_TextBox";
                if (el.Key != el.Value)
                    groupBox_plug.Controls.Find(tb, false)[0].Text = el.Value.ToString();
                else
                    groupBox_plug.Controls.Find(tb, false)[0].Text = "";
            }
            _if_plug = true;
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {            
            e.Graphics.DrawString(label4.Text, new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, 10, 10 );
            e.Graphics.DrawString(label4.Text, new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, 10, pictureBox2.Height-25);

            int p = 0;
            List<char> elements = new List<char>();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            foreach (var el in _enigma._plugb)
            {

                if (el.Key != el.Value && elements.IndexOf(el.Value) == -1)
                {
                    elements.Add(el.Key);

                    int code1 = (int)el.Key - 65;
                    int code2 = (int)el.Value - 65;

                    e.Graphics.DrawRectangle(_enigma._pens[p], 15 * code1 + 10, 10, 14, 14);
                    e.Graphics.DrawRectangle(_enigma._pens[p], 15 * code2 + 10, pictureBox2.Height - 25, 14, 14);
                    e.Graphics.DrawLine(_enigma._pens[p], 15 * code1 + 10 + 7, 10 + 14, 15 * code2 + 10+7, pictureBox2.Height - 25);

                    e.Graphics.DrawRectangle(_enigma._pens[p], 15 * code2 + 10, 10, 14, 14);
                    e.Graphics.DrawRectangle(_enigma._pens[p], 15 * code1 + 10, pictureBox2.Height - 25, 14, 14);
                    e.Graphics.DrawLine(_enigma._pens[p], 15 * code2 + 10 + 7, 10 + 14, 15 * code1 + 10+7, pictureBox2.Height - 25);
                    p++;

                }
            }
            
        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            _debug.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                rotors_change();
                rotors_reset();
                reflect_change();
                for (int i = 0; i < 7; i++)
                    _enigma._chain[i] = 1000;
                pictureBox1.Refresh();
            }
        }

        private void rotors_change()
        {
            groupBox3.Text = "Rotor I";
            groupBox4.Text = "Rotor II";
            groupBox5.Text = "Rotor III";
            if (radioButton1.Checked)
            {
                _enigma._rotors.Clear();
                _enigma._rotors.Add(_enigma._code_Enigma_I_rotor_I);
                _enigma._rotors.Add(_enigma._code_Enigma_I_rotor_II);
                _enigma._rotors.Add(_enigma._code_Enigma_I_rotor_III);
            }
            else if (radioButton2.Checked)
            {
                _enigma._rotors.Clear();
                _enigma._rotors.Add(_enigma._code_railway_rotor_I);
                _enigma._rotors.Add(_enigma._code_railway_rotor_II);
                _enigma._rotors.Add(_enigma._code_railway_rotor_III);
            }
        }

        private void reflect_change()
        {
            _enigma._reflection.Clear();

            char[] reflect=null;
            if (radioButton1.Checked)
                 reflect = _enigma._reflect_A; // ??
            else if (radioButton2.Checked)
                reflect = _enigma._reflect_railway;

            for (int i = 0; i < 26; i++)
                _enigma._reflection.Add(_enigma._alphabet[i], reflect[i]);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                rotors_change();
                rotors_reset();
                reflect_change();

                for (int i = 0; i < 7; i++)
                    _enigma._chain[i] = 1000;

                pictureBox1.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rotors_reset();

            var str = groupBox4.Text;
            groupBox4.Text = groupBox3.Text;
            groupBox3.Text = str;
            var rotor_x = _enigma._rotors[1];
            _enigma._rotors[1] = _enigma._rotors[0];
            _enigma._rotors[0] = rotor_x;
            rotors_reset();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rotors_reset();

            var str = groupBox4.Text;
            groupBox4.Text = groupBox5.Text;
            groupBox5.Text = str;
            var rotor_x = _enigma._rotors[1];
            _enigma._rotors[1] = _enigma._rotors[2];
            _enigma._rotors[2] = rotor_x;
            rotors_reset();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 26; i++)
                _enigma._plugb[(char)(i + 65)] = (char)(i + 65);
            textbox_reload();
            pictureBox2.Refresh();
        }

    }
}
 