using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using System.IO;
using System.Data.SQLite;

namespace Enigma
{
    [Serializable]
    public partial class Form_Debug : Form
    {

        public Form_Debug()
        {
            InitializeComponent();
            Debug_enig._rotors.Add(Debug_enig._code_Enigma_I_rotor_I);
            Debug_enig._rotors.Add(Debug_enig._code_Enigma_I_rotor_II);
            Debug_enig._rotors.Add(Debug_enig._code_Enigma_I_rotor_III);

            for (int i = 0; i < 26; i++)
            {
                Debug_enig._plugb[(char)(i + 65)] = (char)(i + 65);
                Debug_enig._reflection.Add(Debug_enig._alphabet[i], Debug_enig._reflect_A[i]);
            }

            int j=0;
            foreach (ListViewItem el in listView2.Items)
                el.SubItems.Add((++j).ToString());             
        }
        DB_ModelContainer data_source;
        Data Debug_enig = new Data();

     /*   private void button1_Click(object sender, EventArgs e)
        {
            var rg = new Regex(@"\b[A-z](\w*)[A-z]\b", RegexOptions.None);

            var text_str = textBox1.Text.ToUpper().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var text = textBox1.Text.ToUpper().Replace(" ", string.Empty);
            var output_text = "";

            foreach (var el in text_str)
            {
                foreach (Match m in rg.Matches(el))
                {
                    //Console.WriteLine(m.Groups[0].Value);
                    output_text += m.Groups[0].Value;
                }
                output_text += "\n";
            }
            textBox1.Text = output_text;
            text_str = output_text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //var rotors_pos = Debug_enig._rotors[0][0].ToString() + Debug_enig._rotors[1][0].ToString() + Debug_enig._rotors[2][0].ToString();             
        }*/

        private void decrypt(string real_cycle)
        {
            var smth = data_source.Ciphers_Table.Where(p => p.Cycle == real_cycle);
            int i=0;
            int count = smth.Count();

            foreach (var el in smth)
            {
                Debug_enig._rotors[0] = Debug_enig._EnigI_rotors[(int)el.Start_set[0] - 48];
                Debug_enig._rotors[1] = Debug_enig._EnigI_rotors[(int)el.Start_set[1] - 48];
                Debug_enig._rotors[2] = Debug_enig._EnigI_rotors[(int)el.Start_set[2] - 48];

                var pos1 = el.Current_pos.Substring(0, 2);
                var pos2 = el.Current_pos.Substring(3, 2);
                var pos3 = el.Current_pos.Substring(6, 2);

                Debug_enig._gearI_shift = int.Parse(pos1);
                Debug_enig._gearII_shift = int.Parse(pos2);
                Debug_enig._gearIII_shift = int.Parse(pos3);


                Debug_enig._msg = textBox4.Text;
                Debug_enig.msg_transformation();
                var decrypted_key = Debug_enig._msg_transformed;
                //var decrypted_key = Form1._F1_data.msg_transformation(textBox4.Text.ToCharArray());

                if (decrypted_key.Substring(0, 3) == decrypted_key.Substring(3, 3))
                {
                    var msg = textBox4.Text.Substring(6);
                    Debug_enig._gearI_shift =(int)decrypted_key[0] - 65;
                    Debug_enig._gearII_shift = (int)decrypted_key[1] - 65;
                    Debug_enig._gearIII_shift = (int)decrypted_key[2] - 65;

                    Debug_enig._msg = msg;
                    Debug_enig.msg_transformation();
                    var decrypted_msg = Debug_enig._msg_transformed; // no plugboard!
                    //var decrypted_msg = Form1._F1_data.msg_transformation(msg.ToCharArray());
                    var to_lv = new string[] { el.Cycle, decrypted_key.Substring(0, 3), el.Current_pos, el.Start_set, decrypted_msg };
                    //if (MessageBox.Show("Сообщение: " + decrypted_msg + "\n" + "Ключ(оператор): " + decrypted_key.Substring(0, 3) + "\n" + "Ключ(дневной): " + el.Current_pos + "\n" + "Положение роторов: " + el.Start_set, "BOMBE", MessageBoxButtons.YesNo, MessageBoxIcon.None) == System.Windows.Forms.DialogResult.Yes)                       
                    //{ 
                    //    backgroundWorker1.CancelAsync(); 
                    //    break; 
                    //}
                    listView1.Invoke(new MethodInvoker(delegate { 
                        listView1.Items.Add(new ListViewItem(to_lv)); }));
                }
                backgroundWorker1.ReportProgress(++i*100/count, i.ToString()+" / "+count.ToString());
            }
            Debug_enig._rotors[0] = Debug_enig._EnigI_rotors[0];
            Debug_enig._rotors[1] = Debug_enig._EnigI_rotors[1];
            Debug_enig._rotors[2] = Debug_enig._EnigI_rotors[2];
            GC.Collect();
        }
        
        private string encode_messages(string rand_day_key, string rotors_set)
        {
            Debug_enig._rotors[0] = Debug_enig._EnigI_rotors[(int)rotors_set[0] - 48];
            Debug_enig._rotors[1] = Debug_enig._EnigI_rotors[(int)rotors_set[1] - 48];
            Debug_enig._rotors[2] = Debug_enig._EnigI_rotors[(int)rotors_set[2] - 48];
            //richTextBox3.Text = "";
            SortedDictionary<char, char> cycle = new SortedDictionary<char, char>();
            SortedDictionary<char, char> cycle1 = new SortedDictionary<char, char>();
            SortedDictionary<char, char> cycle2 = new SortedDictionary<char, char>();
            for (int i = 0; i < 26; i++)
            {
                var our_rand_key = ((char)(i + 65)).ToString();
                our_rand_key += our_rand_key;
                our_rand_key += our_rand_key;
                our_rand_key += our_rand_key;
                our_rand_key = our_rand_key.Substring(0, 6);


                Debug_enig._gearI_shift = (int)rand_day_key[0] - 65;
                Debug_enig._gearII_shift = (int)rand_day_key[1] - 65;
                Debug_enig._gearIII_shift = (int)rand_day_key[2] - 65;

                //var our_encrypted_key = Form1._F1_data.msg_transformation(our_rand_key.ToCharArray());

                Debug_enig._msg = our_rand_key;
                Debug_enig.msg_transformation();
                var our_encrypted_key = Debug_enig._msg_transformed; // no plugboard!
                
                //MessageBox.Show(our_encrypted_key);
                cycle[our_encrypted_key[0]] = our_encrypted_key[3];
                cycle1[our_encrypted_key[1]] = our_encrypted_key[4];
                cycle2[our_encrypted_key[2]] = our_encrypted_key[5];

                Debug_enig._gearI_shift = (int)our_rand_key[0] - 65;
                Debug_enig._gearII_shift = (int)our_rand_key[1] - 65;
                Debug_enig._gearIII_shift = (int)our_rand_key[2] - 65;
                
                //Debug_enig._msg = message[i];
                Debug_enig._msg = listView2.Items[i].SubItems[0].Text; 
                Debug_enig.msg_transformation();
                var crypted_msg = Debug_enig._msg_transformed; // no plugboard!
                
                //var crypted_msg = Form1._F1_data.msg_transformation(message[i].ToCharArray());
                //textBox4.Text = our_encrypted_key + Form1._F1_data.msg_transformation(textBox3.Text.ToCharArray());
                listView3.Items.Add(new ListViewItem(new string[] { our_encrypted_key + crypted_msg,(i+1).ToString() }));
            }

            #region _debug_test

            //Debug_enig._rotors[0] = Debug_enig._EnigI_rotors[0];
            //Debug_enig._rotors[1] = Debug_enig._EnigI_rotors[1];
            //Debug_enig._rotors[2] = Debug_enig._EnigI_rotors[2];

            Random rand = new Random();
            var rand_test_key = ((char)rand.Next(65, 65 + 26)).ToString() + ((char)rand.Next(65, 65 + 26)).ToString() + ((char)rand.Next(65, 65 + 26)).ToString();
            rand_test_key += rand_test_key;

            Debug_enig._gearI_shift = (int)rand_day_key[0] - 65;
            Debug_enig._gearII_shift = (int)rand_day_key[1] - 65;
            Debug_enig._gearIII_shift = (int)rand_day_key[2] - 65;
            //var rand_test_key_crypted = Form1._F1_data.msg_transformation(rand_test_key.ToCharArray());

            Debug_enig._msg = rand_test_key;
            Debug_enig.msg_transformation();
            var rand_test_key_crypted = Debug_enig._msg_transformed; // no plugboard!


            Debug_enig._gearI_shift = (int)rand_test_key[0] - 65;
            Debug_enig._gearII_shift = (int)rand_test_key[1] - 65;
            Debug_enig._gearIII_shift = (int)rand_test_key[2] - 65;

            Debug_enig._msg = textBox3.Text;
            Debug_enig.msg_transformation();
            textBox4.Text = rand_test_key_crypted + Debug_enig._msg_transformed; //no plugboard!
            #endregion


            var real_cycle = cycle_check(cycle).Split(' ');
            var real_cycle1 = cycle_check(cycle1).Split(' ');
            var real_cycle2 = cycle_check(cycle2).Split(' ');

            Array.Sort(real_cycle);
            Array.Sort(real_cycle1);
            Array.Sort(real_cycle2);

            var full_real_cycle = string.Join(" ", real_cycle) + " | " + string.Join(" ", real_cycle1) + " | " + string.Join(" ", real_cycle2);
            MessageBox.Show(full_real_cycle,"Cycle");
            Debug_enig._rotors[0] = Debug_enig._EnigI_rotors[0];
            Debug_enig._rotors[1] = Debug_enig._EnigI_rotors[1];
            Debug_enig._rotors[2] = Debug_enig._EnigI_rotors[2];
            return full_real_cycle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            var path = openFileDialog1.FileName;

            System.Data.SQLite.SQLiteConnectionStringBuilder connect = new SQLiteConnectionStringBuilder("Data Source=" + path);
            System.Data.EntityClient.EntityConnectionStringBuilder entity_connect = new System.Data.EntityClient.EntityConnectionStringBuilder();
            entity_connect.Provider = @"System.Data.SQLite";
            entity_connect.ProviderConnectionString = connect.ConnectionString;
            entity_connect.Metadata = @"res://*/DB_Model.csdl|res://*/DB_Model.ssdl|res://*/DB_Model.msl";

            data_source = new DB_ModelContainer(entity_connect.ConnectionString);
            
            dataGridView1.DataSource = data_source.Ciphers_Table;
            MessageBox.Show("База удачно подключена");
        }

        public string cycle_check(SortedDictionary<char, char> cycle)
        {
            var cycle_str = "A";
            var cycle_key = 'A';
            var cycle_value = cycle['A'];
            var cycle_length = "";

            while (cycle.Count != 1)
            {
                if (cycle_str.IndexOf(cycle_value) != -1)
                {
                    cycle.Remove(cycle_key);
                    cycle_key = cycle.ElementAt(0).Key;
                    cycle_value = cycle[cycle_key];
                    cycle_str += " " + cycle_key.ToString();
                }
                else
                {
                    cycle_str += cycle_value.ToString();
                    cycle.Remove(cycle_key);
                    cycle_key = cycle_value;
                    cycle_value = cycle[cycle_key];
                }
            }

            foreach (var el in cycle_str.Split(' '))
                cycle_length += el.Length + " ";

            return cycle_length;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            decrypt((string)e.Argument);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            toolStripLabel1.Text = (string)e.UserState;   
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            toolStripLabel1.Text = " ";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            var path = openFileDialog1.FileName;

            System.Data.SQLite.SQLiteConnectionStringBuilder connect = new SQLiteConnectionStringBuilder("Data Source=" + path);
            System.Data.EntityClient.EntityConnectionStringBuilder entity_connect = new System.Data.EntityClient.EntityConnectionStringBuilder();
            entity_connect.Provider = @"System.Data.SQLite";
            entity_connect.ProviderConnectionString = connect.ConnectionString;
            entity_connect.Metadata = @"res://*/DB_Model.csdl|res://*/DB_Model.ssdl|res://*/DB_Model.msl";

            data_source = new DB_ModelContainer(entity_connect.ConnectionString);

            dataGridView1.DataSource = data_source.Ciphers_Table;
            MessageBox.Show("База удачно подключена");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (data_source != null)
            {
                Random rnd = new Random();

                string rand_day_key = ((char)(65 + rnd.Next(0, 26))).ToString() + ((char)(65 + rnd.Next(0, 26))).ToString() + ((char)(65 + rnd.Next(0, 26))).ToString();

                int[] array = { 0, 1, 2 };
                array = array.OrderBy(n => Guid.NewGuid()).ToArray();
                var rand_day_rotor_set = string.Join(string.Empty, array.Select(x => x.ToString()).ToArray());
                MessageBox.Show(rand_day_key + " " + rand_day_rotor_set);

                string real_cycle = "";
                if (listView2.Items.Count >= 26)
                {
                    real_cycle = encode_messages(rand_day_key, rand_day_rotor_set);
                    backgroundWorker1.RunWorkerAsync(real_cycle);
                    //decrypt(real_cycle);
                }
                else
                    MessageBox.Show("Недостаточно сообщений для составления цепочек (циклов)");
            }
            else
                MessageBox.Show("База данных не подключена!");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox3.PasswordChar = '*';
            else
                textBox3.PasswordChar = '\0';
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Пока что возможно расшифровать только модель \"Enigma I\" и без коммутационной панели");
        }

    }
}
