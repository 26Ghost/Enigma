using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enigma_cipher_catalogue
{
    public class Data
    {
        public char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public char[] _code_Enigma_I_rotor_I = "EKMFLGDQVZNTOWYHXUSPAIBRCJ".ToCharArray();
        public char[] _code_Enigma_I_rotor_II = "AJDKSIRUXBLHWTMCQGZNPYFVOE".ToCharArray();
        public char[] _code_Enigma_I_rotor_III = "BDFHJLCPRTXVZNYEIWGAKMUSQO".ToCharArray();

        public char[] _variant_I = "EKMFLGDQVZNTOWYHXUSPAIBRCJ".ToCharArray();
        public char[] _variant_II = "AJDKSIRUXBLHWTMCQGZNPYFVOE".ToCharArray();
        public char[] _variant_III = "BDFHJLCPRTXVZNYEIWGAKMUSQO".ToCharArray();

        public char[] _reflect_A = "EJMZALYXVBWFCRQUONTSPIKHGD".ToCharArray();
        public string[] _combinations = new string[26];
        public Dictionary<char, char> _reflection = new Dictionary<char, char>();

        public Dictionary<char, char> rotorI = new Dictionary<char, char>();
        public Dictionary<char, char> rotorII = new Dictionary<char, char>();
        public Dictionary<char, char> rotorIII = new Dictionary<char, char>();

        public List<char[]> _current_variant = new List<char[]>();
        public int[] variations = { 0, 1, 2 };

        public SortedDictionary<char, char> cycle = new SortedDictionary<char, char>();
        public SortedDictionary<char, char> cycle1 = new SortedDictionary<char, char>();
        public SortedDictionary<char, char> cycle2 = new SortedDictionary<char, char>();
            
        public int _gearI_shift = 0;
        public int _gearII_shift = 0;
        public int _gearIII_shift = 0;

        public char symbol = new char();
        public string encrypted_str = "";

        string shift_I = "";
        string code_I = "";

        string shift_II = "";
        string code_II = "";

        string shift_III = "";
        string code_III = "";

        string cycle_str = "";
        char cycle_key = new char();
        char cycle_value = new char();
        string cycle_length = "";

        public string[] arr_cycle_I;
        public string[] arr_cycle_II;
        public string[] arr_cycle_III;

        public string decrypted_key = "";
        public string _rotor_pos = "012";

        public Data()
        {
            for (char symb = 'A'; symb <= 'Z'; symb++)
            {
                _combinations[symb - 65] = symb.ToString() + symb.ToString() + symb.ToString() + symb.ToString() + symb.ToString() + symb.ToString();
                _reflection[symb] = _reflect_A[symb - 65];
            }

            _current_variant.Add(_variant_I);
            _current_variant.Add(_variant_II);
            _current_variant.Add(_variant_III);

        }

        public void gearI_check()
        {
            code_I= (string.Join(string.Empty,_code_Enigma_I_rotor_I));
            shift_I = code_I.Substring(0, _gearI_shift);
            code_I = code_I.Substring(_gearI_shift);
            code_I += shift_I;

            for (int i = 0; i < 26; i++)
                rotorI[_alphabet[i]] = code_I[i];
                
        }
        public void gearII_check()
        {
            code_II = (string.Join(string.Empty, _code_Enigma_I_rotor_II));
            shift_II = code_II.Substring(0, _gearII_shift);
            code_II = code_II.Substring(_gearII_shift);
            code_II += shift_II;

            for (int i = 0; i < 26; i++)
                rotorII[_alphabet[i]] = code_II[i];

        }
        public void gearIII_check()
        {
            code_III = (string.Join(string.Empty, _code_Enigma_I_rotor_III));
            shift_III = code_III.Substring(0, _gearIII_shift);
            code_III = code_III.Substring(_gearIII_shift);
            code_III += shift_III;

            for (int i = 0; i < 26; i++)
                rotorIII[_alphabet[i]] = code_III[i];

        }
        public string cycle_check(SortedDictionary<char, char> cycle)
        {
            cycle_str = "A";
            cycle_key = 'A';
            cycle_value = cycle['A'];
            cycle_length = "";

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
    }
}
