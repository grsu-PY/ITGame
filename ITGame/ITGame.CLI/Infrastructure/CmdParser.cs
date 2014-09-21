using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ITGame.CLI.Infrastructure
{
    class CmdParser
    {
        private string[] args;
        private Hashtable keys = new Hashtable();

        public CmdParser(string[] args) 
        {
            this.args = args;
            this.keys.Add("creature", "-c");
            this.keys.Add("weapon", "-w");
            this.keys.Add("armor", "-a");
            this.keys.Add("spell", "-s");
        }

        public Hashtable Parse() 
        {
            Hashtable result = new Hashtable();
            result.Add("command", args[0]);
            result.Add("entity", args[1]);
            Regex rgx = new Regex("\\s*,");
            foreach (string key in keys.Keys) 
            {
                int index = Contains(args, (string)keys[key]);
                if (index != -1)
                {
                    string[] temp = rgx.Split(args[index + 1]);
                    result.Add(key, temp);
                }
            }

            return result;
        }

        private int Contains(string[] arr, string pat) 
        {
            int result = -1;

            for (int index = 0; index < arr.Length; index++) 
            {
                if (arr[index] == pat) 
                {
                    result = index;
                    break;
                }
            }

            return result;
        }
    }
}
