using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using ITGame.CLI.Models.Creature;
using ITGame.CLI.Models.Magic;
using ITGame.CLI.Models.Equipment;
using ITGame.CLI.Models.Creature.Actions;
using ITGame.CLI.Models.Environment;
using ITGame.CLI.Infrastructure;
namespace ITGame.CLI
{
    class Program
    {
        private static ConsoleColor defaultColor = Console.ForegroundColor;
        static void Main(string[] args)
        {/*
            var _surfaceRules = new Dictionary<SurfaceType, SurfaceRule>();
            _surfaceRules.Add(SurfaceType.Ground, new SurfaceRule { HP = 50 });

            IEnumerable<Creature> creatures = new List<Creature>
            {
                new Humanoid{Name="Legolas",HumanoidRace=HumanoidRace.Elf},
                new Humanoid{Name="Ahaha",HumanoidRace=HumanoidRace.Dwarf}
            };
            foreach (var cr in creatures)
            {
                cr.ActionPerformed += ActionPerformed;
                Console.WriteLine(cr.Name + " - " + cr.Wisdom);
            }
            //добавление новых, или изменение старых правил
            Surface.ConfigureRules(_surfaceRules);
            //существа подписываются на изменения поверхности
            Surface.RegisterInfluenceFor(creatures);
            //после смены поверхности, параметры у существ меняются
            Surface.CurrentSurfaceType = SurfaceType.Water;
            foreach (var cr in creatures)
            {
                Console.WriteLine(cr.Name + " - " + cr.Wisdom);
            }
            */
            #region Game
            //RunGame();
            #endregion

            ToCmd(args);

            EditEntities();

            Console.ReadKey();
        }

        private static void EditEntities()
        {
            // при инициализации происходит загрузка данных из файлов (таблиц)
            // см. App.config там хранится инфа о расположении базы данных(файлов с таблицами)
            // файлы с таблицами называются по имени сущности

            var dict1 = new Dictionary<string, string>();
            dict1.Add("Agility", "101");
            dict1.Add("HumanoidRace", "Human");
            
            var human = EntityRepository.GetInstance<Humanoid>().Create(dict1);
            var arm = new Armor { ArmorType = ArmorType.Body, Id = Guid.NewGuid(), MagicalDef = 32 };

            EntityRepository.GetInstance<Armor>().Add(arm);


            var dict2 = new Dictionary<string, string>();
            dict2.Add("Agility", "60");
            dict2.Add("Strength", "50");
            dict2.Add("HumanoidRace", "Dwarf");

            var dwarf = EntityRepository.GetInstance<Humanoid>().Create(dict2);

            EntityRepository.GetInstance<Humanoid>().Add(human);
            EntityRepository.GetInstance<Humanoid>().Add(dwarf);

            EntityRepository.GetInstance<Humanoid>().SaveChanges();
            EntityRepository.GetInstance<Armor>().SaveChanges();
        }

        private static void RunGame()
        {
            Humanoid nazgul = new Humanoid() { HumanoidRace = HumanoidRace.Human, Name = "Nazgul" };
            nazgul.ActionPerformed += ActionPerformed;

            nazgul.Equip(new Weapon { PhysicalAttack = 8, WeaponType = WeaponType.Sword });
            nazgul.SelectSpell(selectedDefensiveSpell: new DefensiveSpell { TotalDuration = 3, MagicalPower = 5 });
            // nazgul.SpellDefense();

            Console.WriteLine("Nazgul\nCurrentHP/MaxHP: {0}/{1}\n" +
                                      "Physical Defense: {2}\n" +
                                      "Magical Defense: {3}\n" +
                                      "Physical Attack Bonus: {4}",
                                      nazgul.HP, nazgul.MaxHP,
                                      nazgul.PhysicalDefence,
                                      nazgul.MagicalDefence,
                                      nazgul.PhysicalAttack);

            Humanoid gendalf = new Humanoid() { HumanoidRace = HumanoidRace.Human, Name = "Gendalf" };
            gendalf.MP = 5; // checked event
            gendalf.ActionPerformed += ActionPerformed;
            // gendalf.SelectSpell(new AttackSpell { MagicalPower = 20, ManaCost = 3 });
            //  gendalf.Equip(new Staff { PhysicalAttack = 3, MagicalAttack = 10 });

            Console.WriteLine("\nGendalf\nCurrentHP/MaxHP: {0}/{1}\n" +
                                         "CurrentMP/MaxMP: {2}/{3}\n" +
                                         "Physical Defense: {4}\n" +
                                         "Magical Attack Bonus: {5}\n" +
                                         "Physical Attack Bonus: {6}",
                                          gendalf.HP, gendalf.MaxHP,
                                          gendalf.MP, gendalf.MaxMP,
                                          gendalf.PhysicalDefence,
                                          gendalf.MagicalAttack, gendalf.PhysicalAttack);



            gendalf.SetTarget(nazgul);
            nazgul.SetTarget(gendalf);

            ArrayList weapons = new ArrayList();
            weapons.Add(new Weapon { PhysicalAttack = 6, WeaponType = WeaponType.Sword });
            weapons.Add(new Weapon { PhysicalAttack = 2, MagicalAttack = 3, WeaponType = WeaponType.Staff });

            ArrayList attack_spells = new ArrayList();
            attack_spells.Add(new AttackSpell { MagicalPower = 7, ManaCost = 3 });
            attack_spells.Add(new AttackSpell { MagicalPower = 10, ManaCost = 7 });

            ArrayList defensive_spells = new ArrayList();
            defensive_spells.Add(new DefensiveSpell { MagicalPower = 5, ManaCost = 5 });

            Console.WriteLine("\n\nHello, Gendalf.\nYou should choose spells and weapons.\n\n");
            Console.WriteLine("\nPlease, choose Weapon.\n");
            int index = 1;

            foreach (Weapon weapon in weapons)
            {
                Console.WriteLine("{0}: Physical Damage:{1}, Magical Damage:{2}.", index, weapon.PhysicalAttack, weapon.MagicalAttack);
                index++;
            }
            int num = Int32.Parse(Console.ReadLine()) - 1;
            gendalf.Equip((Weapon)weapons[num]);

            Console.WriteLine("\nPlease, choose Attack Spell.\n");
            index = 1;

            foreach (AttackSpell spell in attack_spells)
            {
                Console.WriteLine("{0}: Magical Power:{1}, Mana Cost:{2}.", index, spell.MagicalPower, spell.ManaCost);
                index++;
            }
            int aSpellNum = Int32.Parse(Console.ReadLine()) - 1;

            Console.WriteLine("\nPlease, choose Defensive Spell.\n");
            index = 1;

            foreach (DefensiveSpell spell in defensive_spells)
            {
                Console.WriteLine("{0}: Magical Power:{1}, Mana Cost:{2}.", index, spell.MagicalPower, spell.ManaCost);
                index++;
            }
            int dSpellNum = Int32.Parse(Console.ReadLine()) - 1;

            gendalf.SelectSpell((AttackSpell)attack_spells[aSpellNum], (DefensiveSpell)defensive_spells[dSpellNum]);


            int round = 1;
            int nznum = 0;
            string text = "";
            while (gendalf.HP != 0 || nazgul.HP != 0)
            {
                Console.WriteLine("\n\nRound {0}: Gendalf, You go!.\n", round);
                Console.WriteLine("1 - Weapon Attack, 2 - Spell Attack, 3 - Spell Defense");
                int attack = Int32.Parse(Console.ReadLine());
                switch (attack)
                {
                    case 1:
                        gendalf.WeaponAttack();
                        text = gendalf.Weapon.MagicalAttack != 0 ?
                            String.Format("\nGendalf deals ({0}+{1})+({2}+{3}) physical and magical damage.\n", gendalf.Weapon.PhysicalAttack, gendalf.PhysicalAttack, gendalf.Weapon.MagicalAttack, gendalf.MagicalAttack) :
                            String.Format("\nGendalf deals {0}+{1} physical damage.\n", gendalf.Weapon.PhysicalAttack, gendalf.PhysicalAttack);
                        break;
                    case 2:
                        gendalf.SpellAttack();
                        text = String.Format("\nGendalf deals {0}+{1} magical damage.\n", gendalf.AttackSpell.MagicalPower, gendalf.MagicalAttack);
                        break;
                    case 3:
                        gendalf.SpellDefense();
                        text = String.Format("\nGendalf will absorb {0}+{1} magical damage for {2} rounds.\n", gendalf.AttackSpell.MagicalPower, gendalf.MagicalAttack, gendalf.DefensiveSpell.TotalDuration);
                        break;
                    default:
                        Console.WriteLine("Are you sleeping?");
                        break;
                }
                Console.WriteLine(text);
                text = "";
                Console.WriteLine("\nNazgul\nCurrentHP/MaxHP: {0}/{1}\n" +
                                            "CurrentMP/MaxMP: {2}/{3}",
                                           nazgul.HP, nazgul.MaxHP, nazgul.MP, nazgul.MaxMP);


                Console.WriteLine("\n\nRound {0}: Nazgul, You go!.\n", round);
                if (nznum % 3 == 0)
                {
                    nazgul.SpellDefense();
                    text = text = String.Format("Nazgul will absorb {0}+{1} magical damage.\n", nazgul.DefensiveSpell.MagicalPower, nazgul.MagicalAttack);
                    Console.WriteLine(text);
                }
                else
                {
                    nazgul.WeaponAttack();
                    text = String.Format("\nNazgul deals {0}+{1} physical damage.\n", nazgul.Weapon.PhysicalAttack, nazgul.PhysicalAttack);
                    Console.WriteLine(text);
                }
                nznum++;
                Console.WriteLine("\nGendalf\nCurrentHP/MaxHP: {0}/{1}\n" +
                                           "CurrentMP/MaxMP: {2}/{3}",
                                           gendalf.HP, gendalf.MaxHP, gendalf.MP, gendalf.MaxMP);

                round++;

            }
        }

        static void ToCmd(string[] args) {

            Humanoid humanoid = null;
            
            //
            // Здесь будет реализовано чтение базы и соответственно инициализация параметрами.
            //

            args = new string[3];
            // args[0] == "create", "read", "update", "delete", "help"
            args[0] = "create";
            // args[1] == same creature
            args[1] = "humanoid";
            // args[2] == parameters for creature
            // creature
            args[2] = "help"; // args[2] = "-c";
            /*
             args[3] = "Human,Legolas,30,10";
             // weapon
             args[4] = "-w";
             args[5] = "10,12";
             // spell
             args[6] = "-s";
             args[7] = "11";
             */

            // В результате разбиения, получаем таблицу, в которой, 
            // к каждому набору параметров для объекта, обращаемся по ключу.
            // tab["command"] - string
            // tab["creature"] - string[] и т.д.
            CmdParser parser = new CmdParser(args);

            if (parser.IsHelp) parser.GetHelp();
            else
            {
                // В этом блоке будем инициализировать все объекты
                Hashtable tab = parser.Parse();

                #region printTab
                foreach (string key in tab.Keys)
                {
                    Console.WriteLine(key + "\n");
                    if (tab[key].GetType().IsArray)
                    {
                        string[] arr = (string[])tab[key];
                        for (int index = 0; index < arr.Length; index++)
                        {
                            Console.WriteLine("-" + arr[index]);
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("-" + tab[key] + "\n");
                    }
                }
                #endregion
            }
        }

        static void ActionPerformed(object sender, ActionPerformedEventArgs e)
        {

            ChangeConsoleColor(e.actionType);
            Console.WriteLine(e.message);
            ResetConsoleColor();
        }

        static void ChangeConsoleColor(ActionType type)
        {
            switch (type)
            {
                case ActionType.Fight:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ActionType.Take:
                    break;
                case ActionType.Voice:
                    break;
                case ActionType.Equip:
                    break;
                case ActionType.Info:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ActionType.SystemInfo:
                    break;
                default:
                    Console.ForegroundColor = defaultColor;
                    break;
            }
        }
        static void ResetConsoleColor()
        {
            Console.ForegroundColor = defaultColor;
        }
    }
}
