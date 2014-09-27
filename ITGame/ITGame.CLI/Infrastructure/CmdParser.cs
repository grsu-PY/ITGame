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
        private Dictionary<string, string> keys = new Dictionary<string, string>() 
        {
            {"Creature", "-c"},
            {"Weapon", "-w"},
            {"Armor", "-a"},
            {"Spell", "-s"}
        };
        private List<string> patterns = new List<string>() 
        {
            @"^(create|update) [Hh]umanoid ((-[cwas])(?!.*\3) (([a-zA-Z]+|[0-9]+|_),?)*((([a-zA-Z]+|[0-9]+|_|(_\.)),?)(?!_\.))+\s?)+$",
            @"^delete [Hh]umanoid$",
            @"(?<=(^(create|update|delete|read) ))?help$"
        };

        private string splitPattern = "\\s*,";
        private bool isHelp = false;

        public CmdParser(string[] args) 
        {
            this.args = args;

            if (!CheckLine()) 
            {
                Console.WriteLine("Bad parameters.\n");
                Environment.Exit(0);
            }
        }

        public Hashtable Parse() 
        {
            Hashtable result = new Hashtable();
            result.Add("Command", args[0]);
            result.Add("Entity", args[1]);

            foreach (string key in keys.Keys) 
            {
                int index = IsContainsKey(args, keys[key]);
                if (index != -1)
                {
                    if (args[index + 1].Contains(','))
                    {
                        string temp = args[index + 1];
                        if (temp.Contains("_."))
                            temp = AdditionParm(args[index + 1], ecountParam[args[index]]);
                        string[] tempArr = Regex.Split(temp, splitPattern);
                        result.Add(key, tempArr);
                    }
                    else result.Add(key, args[index + 1]);
                }
            }

            result = AdditionTable(result);
            return result;
        }

        private Hashtable AdditionTable(Hashtable table)
        {
            Hashtable rtb = new Hashtable();
            foreach (string key in table.Keys) 
            {
                foreach (string ckey in entityInfo.Keys)
                {
                    if (ckey.Contains(key))
                    {
                        Dictionary<string, string> dtemp = new Dictionary<string, string>();
                        string[] EValues = (string[])table[key];
                        string[] EKeys = (string[])entityInfo[ckey];
                        for (int index = 0; index < EKeys.Length; index++)
                        {
                            if(EValues[index] != "_")
                                dtemp.Add(EKeys[index], EValues[index]);
                        }

                        rtb[key] = dtemp;
                        break;
                    }
                }
            }
            rtb.Add("Command", table["Command"]);
            rtb.Add("Entity", table["Entity"]);

            return rtb;
        }

        private string AdditionParm(string line, int cparm)
        {
            string rline = line;
            int index = rline.IndexOf("_.");
            rline = rline.Replace("_.", "");
            rline = rline.Insert(index, "_");
            while (GetComa(rline) != cparm - 1) rline = rline.Insert(index, "_,");

            return rline;
        }

        private int GetComa(string line)
        {
            int rnum = 0;
            foreach (char symb in line)
            {
                if (symb.Equals(',')) rnum++;
            }

            return rnum;
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
                if (args[0] == "help") 
                {
                    Console.WriteLine("Options:");
                    foreach (string key in commandInfo.Keys) 
                    {
                        Console.WriteLine("\t" + key + " - " + commandInfo[key]);
                    }
                }
                else if (args[1] == "help") 
                {
                    if (args[0] == "create" || args[0] == "update")
                    {
                        Console.WriteLine(string.Format("Using:\n\t{0} <entity> <parameters>\n", args[0]));
                        Console.WriteLine(string.Format("Available parameters for \"{0}\":\n", args[0]));
                        foreach (string key in entityInfo.Keys) 
                        {
                            Console.WriteLine("\t" + key + ":");
                            string[] arr = (string[])entityInfo[key];
                            foreach (string p in arr) 
                            {
                                Console.WriteLine("\t\t" + p);
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine("If parameter is not changed, then use \"_\" instead.\n"+
                                          "If there is a few parameters, then use \"_.\". This operator can be used only one time.\n\n"+
                                          "Examples:\n\t"+args[0]+" humanoid -c Gamer,Elf,_.,10,20,50\n"+
                                          "\t"+args[0]+" humanoid -w Sword,20,40 -c _,Elf,_.,10,_,_");

                    }
                    else if (args[0] == "delete") 
                    {
                        Console.WriteLine(string.Format("Using:\n\t{0} <entity>\n\nExamples:\n\t{0} Humanoid", args[0]));
                    }
                    else if (args[0] == "read") 
                    {
                        Console.WriteLine(string.Format("Using:\n\t{0}", args[0]));
                    }
                }
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

        private Dictionary<string, string> commandInfo = new Dictionary<string, string>()
        {
            {"create", "Create a new entity"},
            {"update", "Update entity"},
            {"delete", "Remove entity"},
            {"read", "View the existing entity"},
            {"help", "Get help"}
        };
        private Dictionary<string, int> ecountParam = new Dictionary<string, int>()
        {
            {"-c", 8},
            {"-w", 3},
            {"-a", 3},
            {"-s", 3}
        };
        private Hashtable entityInfo = new Hashtable() 
        {
            {"Creature( -c <parameters> )", new string[]
                {
                    "Name",
                    "Race",
                    "Strength",
                    "Agility",
                    "Wisdom",
                    "Constitution",
                    "MaxHP",
                    "MaxMP"
                }},
            {"Weapon( -w <parameters> )", new string[]
                {
                    "WeaponType",
                    "MagicalAttack",
                    "PhysicalAttack"
                }},
            {"Armor( -a <parameters> )", new string[]
                {
                    "ArmorType",
                    "MagicalDef",
                    "PhysicalDef"
                }},
            {"Spell( -s <parameters> )", new string[]
                {
                    "SpellName",
                    "MagicalPower",
                    "ManaCost"
                }}
        };
    }
}
