using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.EntityClient;
using System.Data.Common;
using System.Data.SQLite;

namespace Enigma_cipher_catalogue
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.NonClientAreaEnabled;
        }
        public Data _enigma = new Data();
        //public Data _enigma2 = new Data();

        public List<string[]> for_lv = new List<string[]>();
        DateTime dt = new DateTime();
        public string[] data_b = new string[3];
        Cipher_catEntities data_source;
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            create_db();
            dt = DateTime.Now;
            backgroundWorker1.RunWorkerAsync();
        }
        private void create_db()
        {
            saveFileDialog1.ShowDialog();
            var path = saveFileDialog1.FileName;
            //var name = System.IO.Path.GetFileName(path);

            SQLiteConnection.CreateFile(path);
            SQLiteFactory factory = DbProviderFactories.GetFactory("System.Data.SQLite") as SQLiteFactory;
            SQLiteConnection connection = factory.CreateConnection() as SQLiteConnection;
            connection.ConnectionString = "Data Source=" + path;
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = @"CREATE TABLE [Ciphers_Table] ( [ID] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                                [Cycle] text,
                                                                [Current_pos] text,
                                                                [Start_set] text  );  ";
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();

            connection.Close();
            //создали базу

            System.Data.SQLite.SQLiteConnectionStringBuilder connect = new SQLiteConnectionStringBuilder("Data Source=" + path);
            System.Data.EntityClient.EntityConnectionStringBuilder entity_connect = new System.Data.EntityClient.EntityConnectionStringBuilder();
            entity_connect.Provider = @"System.Data.SQLite";
            entity_connect.ProviderConnectionString = connect.ConnectionString;
            entity_connect.Metadata = @"res://*/DB_Model.csdl|res://*/DB_Model.ssdl|res://*/DB_Model.msl";
            
            data_source = new Cipher_catEntities(entity_connect.ConnectionString);
            MessageBox.Show("База удачно создана и подключена");

            //data_source.Ciphers_Table.AddObject(new Ciphers_Table
            //{
            //    Current_pos = "test",
            //    Cycle = "test",
            //    Start_set = "test"
            //});
            //data_source.SaveChanges();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            //var DataSource = new Enigma_cipher_catalogue.Cipher_catEntities();
            BackgroundWorker worker = sender as BackgroundWorker;
            string crypted = "";
            for (int i = 0; i < 6; i++)
            {
                for (int rI = 0; rI < 26; rI++)
                    for (int rII = 0; rII < 26; rII++)
                        for (int rIII = 0; rIII < 26; rIII++)
                        {
                            for (int msg_count = 0; msg_count < 26; msg_count++)
                            {
                                _enigma._gearI_shift = rI;
                                _enigma._gearII_shift = rII;
                                _enigma._gearIII_shift = rIII;

                                crypted = encrypt(_enigma._combinations[msg_count].ToCharArray(), _enigma);

                                _enigma.cycle[crypted[0]] = crypted[3];
                                _enigma.cycle1[crypted[1]] = crypted[4];
                                _enigma.cycle2[crypted[2]] = crypted[5];
                            }
                            _enigma.decrypted_key = "";


                            _enigma.arr_cycle_I = _enigma.cycle_check(_enigma.cycle).Split(' ');
                            _enigma.arr_cycle_II = _enigma.cycle_check(_enigma.cycle1).Split(' ');
                            _enigma.arr_cycle_III = _enigma.cycle_check(_enigma.cycle2).Split(' ');

                            Array.Sort(_enigma.arr_cycle_I);
                            Array.Sort(_enigma.arr_cycle_II);
                            Array.Sort(_enigma.arr_cycle_III);


                            _enigma.decrypted_key = string.Join(" ", _enigma.arr_cycle_I) + " | " + string.Join(" ", _enigma.arr_cycle_II) + " | " + string.Join(" ", _enigma.arr_cycle_III);
                            data_b[0] = _enigma.decrypted_key;
                            data_b[1] = rI.ToString().PadLeft(2, '0') + " " + rII.ToString().PadLeft(2, '0') + " " + rIII.ToString().PadLeft(2, '0');                           
                            data_b[2] = _enigma._rotor_pos;
                            

                            // System.IO.File.AppendAllLines(@"C:\Users\Admin\Desktop\Enigma_I_code_table_all_666.txt", data_b);                            
                            data_source.Ciphers_Table.AddObject(new Ciphers_Table
                            {
                                Cycle = data_b[0],
                                Current_pos = data_b[1],
                                Start_set = data_b[2]
                            });


                            worker.ReportProgress((i * 1000000 + rI * 10000 + rII * 100 + rIII) * 100 / 5252525);

                            //lv.Items.Add((new ListViewItem(new string[] { _enigma.decrypted_key, _enigma._gearI_shift.ToString() + " " + _enigma._gearII_shift.ToString() + " " + _enigma._gearIII_shift.ToString() })));
                            //for_lv.Add(new string[] { _enigma.decrypted_key, rI.ToString() + " " + rII.ToString() + " " + rIII.ToString() });                                                      
                            //System.IO.File.AppendAllLines(@"C:\Users\Admin\Desktop\Enigma_I_code_table2.txt", new string[] { _enigma.decrypted_key, rI.ToString() + " " + rII.ToString() + " " + rIII.ToString() });

                        }
                data_source.SaveChanges();

                _enigma._rotor_pos = change_rotors(i);
            }
            //DataSource.DeleteDatabase()

        }

        private string change_rotors(int i) //кривовато
        {
            if (i != 2)
            {
                var temp2 = _enigma.variations[0];
                _enigma.variations[0] = _enigma.variations[1];
                _enigma.variations[1] = temp2;

                temp2 = _enigma.variations[2];
                _enigma.variations[2] = _enigma.variations[0];
                _enigma.variations[0] = temp2;

            }
            else
               _enigma.variations = _enigma.variations.Reverse().ToArray();

            _enigma._code_Enigma_I_rotor_I = _enigma._current_variant[_enigma.variations[0]];
            _enigma._code_Enigma_I_rotor_II = _enigma._current_variant[_enigma.variations[1]];
            _enigma._code_Enigma_I_rotor_III = _enigma._current_variant[_enigma.variations[2]];

            return (string.Join(string.Empty, _enigma.variations));

        }

        private string encrypt(char[] msg, Data _enig)
        {
            _enig.encrypted_str = "";

            foreach (var symb in msg)
            {
                if (_enig._gearI_shift++ > 24)
                {
                    _enig._gearI_shift = 0;
                    if (_enig._gearII_shift++ > 24)
                    {
                        _enig._gearII_shift = 0;
                        if (_enig._gearIII_shift++ > 24)
                        {
                            _enig._gearIII_shift = 0;
                        }
                    }
                }
                ////

                _enig.gearI_check();
                _enig.gearII_check();
                _enig.gearIII_check();

                _enig.symbol = _enigma.rotorI[symb];
                _enig.symbol = _enigma.rotorII[_enig.symbol];
                _enig.symbol = _enigma.rotorIII[_enig.symbol];
                _enig.symbol = _enigma._reflection[_enig.symbol];
                _enig.symbol = _enigma.rotorIII.FirstOrDefault(p => p.Value == _enig.symbol).Key;
                _enig.symbol = _enigma.rotorII.FirstOrDefault(p => p.Value == _enig.symbol).Key;
                _enig.symbol = _enigma.rotorI.FirstOrDefault(p => p.Value == _enig.symbol).Key;

                _enig.encrypted_str += _enig.symbol;

            }
            return _enig.encrypted_str;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            toolStripStatusLabel1.Text = e.ProgressPercentage.ToString() + " % processed";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var time_passed = DateTime.Now - dt;
            MessageBox.Show("Все данные успешно подсчитаны (" + (6 * 26 * 26 * 26 * 26).ToString() + " итераций)  и записаны в базуданный за " + time_passed.TotalSeconds.ToString() + " секунд");
            backgroundWorker1.Dispose();
            dataGridView1.DataSource = data_source.Ciphers_Table;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Генератор таблиц для взолома \"Энигмы\" по методу Раевского \nНаписал Частов Антон\nККС-1-12\n2012 год", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    
        }


    }
}
