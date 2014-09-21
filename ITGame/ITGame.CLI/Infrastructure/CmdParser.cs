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
        private Dictionary<string, string> keys = new Dictionary<string, string>();
        private List<string> patterns = new List<string>() 
        {
            @"^(create|update) [Hh]umanoid (-[cwas] (\w+,?)+\s?)+$",
            @"^delete [Hh]umanoid$",
            @"(?<=(^(((create|update|delete)( [Hh]umanoid)? )|read )))help$"  // itgame update help / itgame create humanoid help
        };
        //private string mainRPattern = @"^read ([Hh]umanoid|all)$";
        //private string mainHPattern = @"";
        private string splitPattern = "\\s*,";
        private bool isHelp = false;

        public CmdParser(string[] args) 
        {
            this.args = args;

            this.keys.Add("creature", "-c");
            this.keys.Add("weapon", "-w");
            this.keys.Add("armor", "-a");
            this.keys.Add("spell", "-s");

            if (!CheckLine()) 
            {
                Console.WriteLine("Bad parameters.\n");
                Environment.Exit(0);
            }
        }

        public Hashtable Parse() 
        {
            Hashtable result = new Hashtable();
            result.Add("command", args[0]);
            result.Add("entity", args[1]);

            foreach (string key in keys.Keys) 
            {
                int index = IsContainsKey(args, keys[key]);
                if (index != -1)
                {
                    if (args[index + 1].Contains(','))
                    {
                        string[] temp = Regex.Split(args[index + 1], splitPattern);
                        result.Add(key, temp);
                    }
                    else result.Add(key, args[index + 1]);
                }
            }

            return result;
        }

        private int IsContainsKey(string[] arr, string pat) 
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

        private string ConvertArrayToString(string[] array) 
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array) 
            {
                builder.Append(value+" ");
            }
            builder = builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        private bool CheckLine() 
        {
            bool result = false;

            string tempLine = ConvertArrayToString(args);

            foreach (string pattern in patterns)
            {
                if (Regex.IsMatch(tempLine, pattern))
                {
                    if (pattern == patterns[2]) this.isHelp = true;
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void GetHelp() 
        {
            if (isHelp) 
            {
                Console.WriteLine("Describe\n");
            }
            else Console.WriteLine("Is not help command.\n");
        }

        public bool IsHelp 
        {
            get
            {
                return isHelp;
            }
        }
    }
}
