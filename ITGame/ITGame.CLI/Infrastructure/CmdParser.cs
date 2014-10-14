using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ITGame.CLI.Extensions;
using ITGame.CLI.Models.Сreature;
using ITGame.CLI.Models.Equipment;
using ITGame.CLI.Models.Magic;

namespace ITGame.CLI.Infrastructure
{
    class CmdParser
    {
        private string[] args;
        private Dictionary<string, string> entityKeys = new Dictionary<string, string>
        {
            {"Humanoid", "-h"},
            {"Weapon", "-w"},
            {"Armor", "-a"},
            {"Spell", "-s"}
        };

        private List<string> patterns = new List<string>
        {
            CmdPattern.Create,
            CmdPattern.Update,
            CmdPattern.Delete,
            CmdPattern.Read,
            CmdPattern.Help
        };

        private CmdCommands command;
        private bool isHelp = false;

        public CmdParser(string[] args)
        {
            if (args.Length != 0)
                this.args = args;
            else 
            {
                Console.WriteLine("Bad parameters. Please, read help.\n");
                Environment.Exit(0);
            }

            if (!CheckLine())
            {
                Console.WriteLine("Bad parameters. Please, read help.\n");
                Environment.Exit(0);
            }

            command = (CmdCommands)Enum.Parse(typeof(CmdCommands), args[0], true);
        }

        public List<CmdData> Parse()
        {
            var retList = new List<CmdData>();
            switch (command)
            {
                case CmdCommands.update:
                case CmdCommands.create:
                    foreach (var entity in entityKeys.Keys)
                    {
                        var index = IsContainsKey(args, entityKeys[entity]);
                        if (index != -1)
                        {
                            var temp = args[index + 1];
                            if (temp.Contains("_."))
                                temp = AdditionParm(args[index + 1], _ecountParam[args[index]]);
                            var props = Regex.Split(temp, "\\s*,");
                            if (command == CmdCommands.update)
                            {
                                Guid guid;
                                if (Guid.TryParse(args[index + 2], out guid))
                                {
                                    retList.Add(new CmdData(command, entity, guid, AdditionTable(props, entity)));
                                }
                                else
                                {
                                    Console.WriteLine("Bad Guid.\n");
                                    Environment.Exit(0);
                                }
                            }
                            else
                                retList.Add(new CmdData(command, entity, AdditionTable(props, entity)));
                        }
                    }
                    break;
                case CmdCommands.delete:
                {
                    foreach (string entity in entityKeys.Keys) 
                    {
                        var keyIndex = IsContainsKey(args, entityKeys[entity]);
                        if (keyIndex != -1) 
                        {
                            Guid guid;
                            if (Guid.TryParse(args[keyIndex + 1], out guid))
                            {
                                string _entity = entityKeys.Single(k => k.Value == entityKeys[entity]).Key;
                                retList.Add(new CmdData(command, _entity, guid, null));
                            }
                            else
                            {
                                Console.WriteLine("Bad Guid.\n");
                                Environment.Exit(0);
                            }
                        }
                    }
                }
                    break;
                case CmdCommands.read:
                {
                    for (int index = 1; index < args[1].Length; index++)
                    {
                        string entity = entityKeys.Single(k => k.Value[1] == args[1][index]).Key;
                        retList.Add(new CmdData(command, entity, null));
                    }
                }
                    break;
            }

            return retList;
        }

        private Dictionary<string, string> AdditionTable(string[] EValues, string key)
        {
            var dtb = new Dictionary<string, string>();
            foreach (string entity in entityKeys.Keys)
            {
                if (entity.Contains(key))
                {
                    var entityProperties = ColumnPropertiesHelper.GetPropertiesNames(entity).ToList();
                    for (var index = 0; index < entityProperties.Count; index++)
                    {
                        if (entityProperties[index].Equals("Weapon") && EValues[index] != "_") 
                        {
                            if (!CheckGuid(EValues[index])) 
                            {
                                Console.WriteLine("Bad guid.\n");
                                Environment.Exit(0);
                            }
                        }
                        if (EValues[index] != "_")
                            dtb.Add(entityProperties[index], EValues[index]);
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
                    if (pattern == patterns[4]) this.isHelp = true;
                    result = true;
                    break;
                }
            }

            return result;
        }

        private bool CheckGuid(string id) 
        {
            Guid guid;
            if (Guid.TryParse(id, out guid)) return true;
            else return false;
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
                    if (fCommand != CmdCommands.help) 
                    {
                        Console.WriteLine("Bad parameters. Please, read help.\n");
                        Environment.Exit(0);
                    }
                    PrintHelp(command);
                }
            }
            else Console.WriteLine("Is not help command. Please, read help.\n");
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
            {"-h", typeof(Humanoid).GetColumnProperties().Count()},
            {"-w", typeof(Weapon).GetColumnProperties().Count()},
            {"-a", typeof(Armor).GetColumnProperties().Count()},
            {"-s", typeof(Spell).GetColumnProperties().Count()}
        };

        private enum helpPart
        {
            Using,
            Parameters,
            Example
        }

        private void PrintPartOfHelp(CmdCommands command, helpPart part) 
        {
            if (part == helpPart.Parameters)
            {
                if (command == CmdCommands.create || command == CmdCommands.update)
                {
                    Console.WriteLine("Available parameters for \"{0}\":\n", command);
                    foreach (string entity in entityKeys.Keys)
                    {
                        Console.WriteLine("\t{0} ( {1} ):", entity, entityKeys[entity]);
                        var entityProperties = ColumnPropertiesHelper.GetPropertiesNames(entity);
                        foreach (var property in entityProperties)
                        {
                            Console.WriteLine("\t\t" + property);
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Available parameters for \"{0}\":\n", command);
                    foreach (var entity in entityKeys.Keys)
                    {
                        Console.WriteLine("\t{0}( {1} )", entity, entityKeys[entity]);
                    }
                }
            }
            else
            {
                var dict = helpText[command] as Dictionary<helpPart, string>;

                Console.WriteLine(dict[part]);
            }
        }

        private void PrintHelp(CmdCommands command) 
        {
            PrintPartOfHelp(command, helpPart.Using);
            PrintPartOfHelp(command, helpPart.Parameters);
            PrintPartOfHelp(command, helpPart.Example);
        }

        private readonly Hashtable helpText = new Hashtable 
        {
            {
                CmdCommands.create, new Dictionary<helpPart, string>{
                    {helpPart.Using, "Using:\n\tcreate <entity> <parameters>\n"},
                    {helpPart.Example, "If parameter is not changed, then use \"_\" instead.\n" +
                                       "If there is a few parameters, then use \"_.\". This operator can be used only one time.\n\n" +
                                       "Examples:\n\tcreate -h Gamer,Elf,_.,10,20,50\n" +
                                                  "\tcreate -w Sword,20,40 -h _,Elf,_.,10,_,_"}}
            },
            {
                CmdCommands.update, new Dictionary<helpPart, string>{
                    {helpPart.Using, "Using:\n\tupdate <entity> <parameters> <guid>\n"},
                    {helpPart.Example, "If parameter is not changed, then use \"_\" instead.\n" +
                                       "If there is a few parameters, then use \"_.\". This operator can be used only one time.\n\n" +
                                       "Examples:\n\tupdate -h Gamer,Elf,_.,10,20,50 0f8fad5b-d9cb-469f-a165-70867728950e\n" +
                                                  "\tupdate -w Sword,20,40 2f8bad23-0034-ba40-a165-adb27728950e -h _,Elf,_.,10,_,_ d9cb0f8fad5b-0f8f-7728469f-a165-7086469f950e"}
                }
            },
            {
                CmdCommands.delete, new Dictionary<helpPart, string>{
                    {helpPart.Using, "Using:\n\tdelete <entity> <guid>\n"},
                    {helpPart.Example, "\nExamples:\n\tdelete -h 0f8fad5b-d9cb-469f-a165-70867728950e" +
                                                            " -w 2f8bad23-0034-ba40-a165-adb27728950e"}
                }
            
            },
            {
                CmdCommands.read, new Dictionary<helpPart, string>{
                    {helpPart.Using, "Using:\n\tread <entity>\n"},
                    {helpPart.Example, "\nExamples:\n\tread -h"+
                                                  "\n\tread -saw"}
                }
            }
        };
    }

    public static class CmdPattern 
    {
        private static readonly string _keys = "[hwas]";
        private static readonly string _letters = "[a-zA-Z]";
        private static readonly string _digits = "[0-9]";
        private static readonly string _lineDown = "_";
        private static readonly string _lineDownDot = @"_\.";
        private static readonly string _guid = @"\w{8}-(\w{4}-){3}\w{12}";
        private static readonly string _len = "{1,4}";

        private static readonly string _create = string.Format(@"^create ((-{0})(?!.*\2) (({1}+|{2}+|{3}|{4}),?)*((({5}+|{6}+|{7}|{8}|({9})),?)(?!{10}))+\s?){11}$",
                                                                    _keys, _letters, _digits, _guid, _lineDown,
                                                                    _letters, _digits, _guid, _lineDown, _lineDownDot,
                                                                    _lineDownDot, _len);
        private static readonly string _update = string.Format(@"^update ((-{0})(?!.*\2) (({1}+|{2}+|{3}|{4}),?)*((({5}+|{6}+|{7}|{8}|({9})),?)(?!{10}))+ ({11})\s?){12}$",
                                                                    _keys, _letters, _digits, _guid, _lineDown,
                                                                    _letters, _digits, _guid, _lineDown, _lineDownDot,
                                                                    _lineDownDot, _guid, _len);
        private static readonly string _delete = string.Format(@"^delete ((-{0})(?!.*\2) ({1})\s?){2}$",
                                                                    _keys, _guid, _len);
        private static readonly string _read = string.Format(@"^read -(({0})(?!.*\2)){1}$",
                                                                    _keys, _len);
        private static readonly string _help = "(?<=(^(create|update|delete|read) ))?help$";

        public static string Create
        {
            get { return CmdPattern._create; }
        }      

        public static string Update
        {
            get { return CmdPattern._update; }
        } 

        public static string Delete
        {
            get { return CmdPattern._delete; }
        } 

        public static string Read
        {
            get { return CmdPattern._read; }
        }

        public static string Help
        {
            get { return CmdPattern._help; }
        } 

    }
}
