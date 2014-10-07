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
        private Dictionary<string, string> keys = new Dictionary<string, string>
        {
            {"Humanoid", "-h"},
            {"Weapon", "-w"},
            {"Armor", "-a"},
            {"Spell", "-s"}
        };
        private List<string> patterns = new List<string>
        {
            @"^create ((-[hwas])(?!.*\3) (([a-zA-Z]+|[0-9]+|_),?)*((([a-zA-Z]+|[0-9]+|_|(_\.)),?)(?!_\.))+\s?)+$",
            @"^update ((-[hwas])(?!.*\3) (([a-zA-Z]+|[0-9]+|_),?)*((([a-zA-Z]+|[0-9]+|_|(_\.)),?)(?!_\.))+ (\w{8}-(\w{4}-){3}\w{12})\s?)+$",
            @"^delete (Humanoid|Weapon|Armor|Spell) (\w{8}-(\w{4}-){3}\w{12})$",
            @"(?<=(^(create|update|delete|read) ))?help$",
            @"^read (Humanoid|Weapon|Armor|Spell)$"
        };

        private CmdCommands command;
        private string splitPattern = "\\s*,";
        private bool isHelp = false;

        public CmdParser(string[] args)
        {
            this.args = args;

            command = (CmdCommands)Enum.Parse(typeof(CmdCommands), args[0], true);

            if (!CheckLine())
            {
                Console.WriteLine("Bad parameters.\n");
                Environment.Exit(0);
            }
        }

        public List<CmdData> Parse()
        {
            var result = new Hashtable();
            var retList = new List<CmdData>();
            switch (command)
            {
                case CmdCommands.update:
                case CmdCommands.create:
                    foreach (var key in keys.Keys)
                    {
                        var index = IsContainsKey(args, keys[key]);
                        if (index != -1)
                        {
                            var temp = args[index + 1];
                            if (temp.Contains("_."))
                                temp = AdditionParm(args[index + 1], _ecountParam[args[index]]);
                            var props = Regex.Split(temp, splitPattern);
                            if (command == CmdCommands.update)
                            {
                                Guid guid;
                                if (Guid.TryParse(args[index + 2], out guid))
                                {
                                    retList.Add(new CmdData(command, key, guid, AdditionTable(props, key)));
                                }
                                else
                                {
                                    Console.WriteLine("Bad Guid\n");
                                    Environment.Exit(0);
                                }
                            }
                            else
                                retList.Add(new CmdData(command, key, AdditionTable(props, key)));
                        }


                    }
                    break;
                case CmdCommands.delete:
                {
                    Guid guid;
                    if (Guid.TryParse(args[2], out guid))
                        retList.Add(new CmdData(command, args[1], guid, null));
                    else
                    {
                        Console.WriteLine("Bad Guid\n");
                        Environment.Exit(0);
                    }
                }
                    break;
                case CmdCommands.read:
                    retList.Add(new CmdData(command, args[1], null));
                    break;
            }

            return retList;
        }

        private Dictionary<string, string> AdditionTable(string[] EValues, string key)
        {
            var dtb = new Dictionary<string, string>();
            foreach (string ckey in _entityInfo.Keys)
            {
                if (ckey.Contains(key))
                {
                    var EKeys = (string[])_entityInfo[ckey];
                    for (var index = 0; index < EKeys.Length; index++)
                    {
                        if (EValues[index] != "_")
                            dtb.Add(EKeys[index], EValues[index]);
                    }
                    break;
                }
            }

            return dtb;
        }

        private string AdditionParm(string line, int cparm)
        {
            var rline = line;
            var index = rline.IndexOf("_.");
            rline = rline.Replace("_.", "");
            rline = rline.Insert(index, "_");
            while (GetComa(rline) != cparm - 1) rline = rline.Insert(index, "_,");

            return rline;
        }

        private int GetComa(string line)
        {
            return line.Count(symb => symb.Equals(','));
        }

        private int IsContainsKey(string[] arr, string pat)
        {
            var result = -1;

            for (var index = 0; index < arr.Length; index++)
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
            var builder = new StringBuilder();
            foreach (var value in array)
            {
                builder.Append(value + " ");
            }
            builder = builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        private bool CheckLine()
        {
            var result = false;

            var tempLine = ConvertArrayToString(args);

            foreach (var pattern in patterns)
            {
                if (Regex.IsMatch(tempLine, pattern))
                {
                    if (pattern == patterns[3]) this.isHelp = true;
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
                if (command == CmdCommands.help)
                {
                    Console.WriteLine("Options:");
                    foreach (var key in _commandInfo.Keys)
                    {
                        Console.WriteLine("\t" + key + " - " + _commandInfo[key]);
                    }
                }
                else
                {
                    var fCommand = (CmdCommands)Enum.Parse(typeof(CmdCommands), args[1]);
                    if (fCommand != CmdCommands.help) return;
                    switch (command)
                    {
                        case CmdCommands.update:
                        case CmdCommands.create:
                            if (command == CmdCommands.update)
                                Console.WriteLine("Using:\n\t{0} <entity> <parameters> <guid>\n", command);
                            else
                                Console.WriteLine("Using:\n\t{0} <entity> <parameters>\n", command);
                            Console.WriteLine("Available parameters for \"{0}\":\n", command);
                            foreach (string key in _entityInfo.Keys)
                            {
                                Console.WriteLine("\t" + key + ":");
                                var arr = (string[])_entityInfo[key];
                                foreach (var p in arr)
                                {
                                    Console.WriteLine("\t\t" + p);
                                }
                                Console.WriteLine();
                            }
                            if (command == CmdCommands.update)
                                Console.WriteLine("If parameter is not changed, then use \"_\" instead.\n" +
                                                  "If there is a few parameters, then use \"_.\". This operator can be used only one time.\n\n" +
                                                  "Examples:\n\t" + command + " -h Gamer,Elf,_.,10,20,50 0f8fad5b-d9cb-469f-a165-70867728950e\n" +
                                                  "\t" + command + " -w Sword,20,40 2f8bad23-0034-ba40-a165-adb27728950e -c _,Elf,_.,10,_,_ d9cb0f8fad5b-0f8f-7728469f-a165-7086469f950e");
                            else
                                Console.WriteLine("If parameter is not changed, then use \"_\" instead.\n" +
                                                  "If there is a few parameters, then use \"_.\". This operator can be used only one time.\n\n" +
                                                  "Examples:\n\t" + command + " -h Gamer,Elf,_.,10,20,50\n" +
                                                  "\t" + command + " -w Sword,20,40 -c _,Elf,_.,10,_,_");
                            break;
                        case CmdCommands.delete:
                            Console.WriteLine("Using:\n\t" + command + " <entity> <guid>\n");
                            Console.WriteLine("Available parameters for \"{0}\":\n", command);
                            foreach (var key in keys.Keys)
                            {
                                Console.WriteLine("\t" + key);
                            }
                            Console.WriteLine("\nExamples:\n\t" + command + " Humanoid 0f8fad5b-d9cb-469f-a165-70867728950e");
                            break;
                        case CmdCommands.read:
                            Console.WriteLine("Using:\n\t{0} <entity>\n", command);
                            Console.WriteLine("Available parameters for \"{0}\":\n", command);
                            foreach (var key in keys.Keys)
                            {
                                Console.WriteLine("\t" + key);
                            }
                            Console.WriteLine("\nExamples:\n\t" + command + " Humanoid");
                            break;
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

        private readonly Dictionary<string, string> _commandInfo = new Dictionary<string, string>
        {
            {CmdCommands.create.ToString(), "Create a new entity"},
            {CmdCommands.update.ToString(), "Update entity"},
            {CmdCommands.delete.ToString(), "Remove entity"},
            {CmdCommands.read.ToString(), "View the existing entity"},
            {CmdCommands.help.ToString(), "Get help"}
        };
        private readonly Dictionary<string, int> _ecountParam = new Dictionary<string, int>
        {
            {"-h", 8},
            {"-w", 4},
            {"-a", 4},
            {"-s", 6}
        };
        private readonly Hashtable _entityInfo = new Hashtable
        {
            {"Humanoid( -h <parameters> )", new[]
                {
                    "Name",
                    "HumanoidRace",
                    "Strength",
                    "Agility",
                    "Wisdom",
                    "Constitution",
                    "MaxHP",
                    "MaxMP"
                }},
            {"Weapon( -w <parameters> )", new[]
                {
                    "Name",
                    "WeaponType",
                    "MagicalAttack",
                    "PhysicalAttack"
                }},
            {"Armor( -a <parameters> )", new[]
                {
                    "Name",
                    "ArmorType",
                    "MagicalDef",
                    "PhysicalDef"
                }},
            {"Spell( -s <parameters> )", new[]
                {
                    "Name",
                    "SpellType",
                    "SchoolSpell",
                    "MagicalPower",
                    "ManaCost",
                    "TotalDuration"
                }}
        };
    }
}
