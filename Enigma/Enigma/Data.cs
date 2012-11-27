using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Enigma
{
    public class Data
    {
        public char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public char[] _code_Enigma_I_rotor_I = "EKMFLGDQVZNTOWYHXUSPAIBRCJ".ToCharArray();
        public char[] _code_Enigma_I_rotor_II = "AJDKSIRUXBLHWTMCQGZNPYFVOE".ToCharArray();
        public char[] _code_Enigma_I_rotor_III = "BDFHJLCPRTXVZNYEIWGAKMUSQO".ToCharArray();

        public char[] _code_railway_rotor_I = "JGDQOXUSCAMIFRVTPNEWKBLZYH".ToCharArray();
        public char[] _code_railway_rotor_II = "NTZPSFBOKMWRCJDIVLAEYUXHGQ".ToCharArray();
        public char[] _code_railway_rotor_III = "JVIUBHTCDYAKEQZPOSGXNRMWFL".ToCharArray();
        
        public char[] _reflect_railway = "QYHOGNECVPUZTFDJAXWMKISRBL".ToCharArray();
        public char[] _reflect_A = "EJMZALYXVBWFCRQUONTSPIKHGD".ToCharArray();
        public char[] _reflect_B = "YRUHQSLDPXNGOKMIEBFZCWVJAT".ToCharArray();
        public char[] _reflect_C = "FVPJIAOYEDRZXWGCTKUQSBNMHL".ToCharArray();

        public List<char[]> _rotors = new List<char[]>();

        //<временное решение>
        public string _variant_I = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
        public string _variant_II = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
        public string _variant_III = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
        public List<char[]> _EnigI_rotors = new List<char[]>();
        //</временное решение>

        //public char[] _code = {'C', 'K', 'N', 'M', 'T', 'D', 'V', 'Z', 'A', 'I', 'W', 'G', 'S','Y','B','P', 'J', 'U', 'O', 'H', 'X', 'F', 'L', 'E', 'R', 'Q' };
        //public char[] _reflect = {'N', 'Z', 'L', 'W', 'V', 'O', 'H', 'G', 'R', 'M', 'U', 'C', 'J', 'A', 'F', 'Q', 'P', 'I', 'Y', 'X','K', 'E', 'D', 'T', 'S', 'B' };
        //public char[] _msg;

        public string _msg;
        public string _msg_plugged;
        public string _msg_transformed;
        public string _msg_transf_plugged;

        public int _gearI_shift = 0;
        public int _gearII_shift = 0;
        public int _gearIII_shift = 0;
        public int[] _chain  = { 1000, 1000, 1000, 1000, 1000, 1000, 1000 };
        public List<char> _plug = new List<char>();
        public Dictionary<char, char> _plugb = new Dictionary<char, char>();

       // public Pen[] _pens = { Pens.Black, Pens.Red, Pens.Blue, Pens.Tomato, Pens.Green, Pens.Gold, Pens.Khaki, Pens.LightSeaGreen, Pens.Magenta, Pens.LimeGreen, Pens.SkyBlue, Pens.White, Pens.Violet };
        public Pen[] _pens = { new Pen(Color.Red, 2), new Pen(Color.Blue, 2), new Pen(Color.Green, 2), new Pen(Color.Magenta, 2), new Pen(Color.Lime, 2), new Pen(Color.Orange, 2), new Pen(Color.Violet, 2), new Pen(Color.SkyBlue, 2), new Pen(Color.SeaShell, 2), new Pen(Color.Plum, 2), new Pen(Color.Khaki, 2), new Pen(Color.Olive, 2), new Pen(Color.Moccasin, 2) };
       
        
        public Dictionary<char, char> _reflection = new Dictionary<char, char>();
        public Dictionary<char, char> rotorI = new Dictionary<char,char>();
        public Dictionary<char, char> rotorII = new Dictionary<char,char>();
        public Dictionary<char, char> rotorIII = new Dictionary<char,char>();


        public Data()
        {
            _EnigI_rotors.Add(_variant_I.ToCharArray());
            _EnigI_rotors.Add(_variant_II.ToCharArray());
            _EnigI_rotors.Add(_variant_III.ToCharArray());
        }

        public void msg_transformation()
        {
            char symbol;
            _msg_transf_plugged = "";
            _msg_plugged = "";
            _msg_transformed = "";
            
            foreach (var symb in _msg)
            {
                symbol = _plugb[symb];
                _msg_plugged += symbol;

                if (_gearI_shift++ > 24)
                {
                    _gearI_shift = 0;
                    if (_gearII_shift++ > 24)
                    {
                        _gearII_shift = 0;
                        if (_gearIII_shift++ > 24)
                        {
                            _gearIII_shift = 0;
                            //MessageBox.Show("Роторы достигли максимальной позиции и были сброшены", "Achtung", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                //TrackBar_set();

                gears_shift_check(rotorI,_gearI_shift,1);
                gears_shift_check(rotorII, _gearII_shift, 2);
                gears_shift_check(rotorIII, _gearIII_shift, 3);
                

                _chain[0] = (int)symbol - 65;
                symbol = rotorI[symbol];

                _chain[1] = (int)symbol - 65;
                symbol = rotorII[symbol];
                
                _chain[2] = (int)symbol - 65;
                symbol = rotorIII[symbol];

                _chain[3] = (int)symbol - 65;
                symbol = _reflection[symbol];

                _chain[4] = rotorIII.FirstOrDefault(p => p.Value == symbol).Key - 65;
                symbol = rotorIII.FirstOrDefault(p => p.Value == symbol).Key;
                _chain[5] = rotorII.FirstOrDefault(p => p.Value == symbol).Key - 65;
                symbol = rotorII.FirstOrDefault(p => p.Value == symbol).Key;
                _chain[6] = rotorI.FirstOrDefault(p => p.Value == symbol).Key - 65;
                symbol = rotorI.FirstOrDefault(p => p.Value == symbol).Key;

                _msg_transformed += symbol;
                symbol = _plugb[symbol];
               
                _msg_transf_plugged += symbol.ToString();

                //rotorI.Clear();
                //rotorII.Clear();
                //rotorIII.Clear();

            }
            //_msg_transf_plugged = str;
        }

        private void gears_shift_check(Dictionary<char,char> rotor,int rot_shift,int num)
        {
            rotor.Clear();

            string str = new string(_rotors[num - 1]);
            string shift = str.Substring(0, rot_shift);
            str = str.Substring(rot_shift);
            str += shift;

            for (int i = 0; i < 26; i++)
                rotor.Add(_alphabet[i], str[i]);
        }
    }
    
}
