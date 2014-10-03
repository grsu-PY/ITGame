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
        {
            #region Game
            //RunGame();
            #endregion

            //SurfaceOnAction();

           // if(args.Length != 0)
                ToCmd(args);

           // EditEntities();

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
            human.Id = Guid.NewGuid();

            var arm = new Armor { ArmorType = ArmorType.Body, Id = Guid.NewGuid(), MagicalDef = 32 };

            EntityRepository.GetInstance<Armor>().Add(arm);


            var dict2 = new Dictionary<string, string>();
            dict2.Add("Agility", "60");
            dict2.Add("Strength", "50");
            dict2.Add("HumanoidRace", "Dwarf");

            var dwarf = EntityRepository.GetInstance<Humanoid>().Create(dict2);
            dwarf.Id = Guid.NewGuid();

            EntityRepository.GetInstance<Humanoid>().Add(human);
            EntityRepository.GetInstance<Humanoid>().Add(dwarf);

            EntityRepository.GetInstance<Humanoid>().SaveChanges();
            EntityRepository.GetInstance<Armor>().SaveChanges();
        }

     
        static void ToCmd(string[] args) {
            
            args = new string[9];
            // args[0] == "create", "read", "update", "delete", "help"
            args[0] = "create";
            // args[1] == parameters for creature
            // creature
            args[1] = "-h";
            args[2] = "_.,12,30";
            // weapon
            args[3] = "-w";
            args[4] = "Sword,_.";
            // spell
            args[5] = "-s";
            args[6] = "AttackSpell,Fire,Wrath,12,_.";
            // armor
            args[7] = "-a";
            args[8] = "Gloves,10,2";
            
            CmdParser parser = new CmdParser(args);

            if (parser.IsHelp) parser.GetHelp();
            else
            {
                List<CmdData> cmdArgs = parser.Parse();
                CmdCommands command = cmdArgs[0].Command;

                switch (command) 
                {
                    case CmdCommands.create:
                        foreach (CmdData cData in cmdArgs) 
                        {
                            Type entityType = Type.GetType(cData.Entity);
                            var entity = EntityRepository.GetInstance(entityType).Create(cData.Properties);
                            entity.Id = new Guid();

                            EntityRepository.GetInstance(entityType).Add(entity);
                            EntityRepository.GetInstance(entityType).SaveChanges();
                        }
                        break;
                    default:
                        break;
                }
                
            }
        }

        /*
         foreach (CmdData data in cmdArgs) 
                {
                    Console.WriteLine(data.Entity);
                    foreach (string key in data.Properties.Keys) 
                    {
                        Console.WriteLine("->" + key + " - " + data.Properties[key]);
                    }
                }
         */

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

        private static void RunGame()
        {
            Humanoid nazgul = new Humanoid() { HumanoidRace = HumanoidRace.Human, Name = "Nazgul" };
            nazgul.ActionPerformed += ActionPerformed;

            nazgul.Equip(new Weapon { PhysicalAttack = 8, WeaponType = WeaponType.Sword });
            nazgul.SelectSpell(selectedDefensiveSpell: new Spell { TotalDuration = 3, MagicalPower = 5 });
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
            attack_spells.Add(new Spell { MagicalPower = 7, ManaCost = 3 });
            attack_spells.Add(new Spell { MagicalPower = 10, ManaCost = 7 });

            ArrayList defensive_spells = new ArrayList();
            defensive_spells.Add(new Spell { MagicalPower = 5, ManaCost = 5 });

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

            foreach (Spell spell in attack_spells)
            {
                Console.WriteLine("{0}: Magical Power:{1}, Mana Cost:{2}.", index, spell.MagicalPower, spell.ManaCost);
                index++;
            }
            int aSpellNum = Int32.Parse(Console.ReadLine()) - 1;

            Console.WriteLine("\nPlease, choose Defensive Spell.\n");
            index = 1;

            foreach (Spell spell in defensive_spells)
            {
                Console.WriteLine("{0}: Magical Power:{1}, Mana Cost:{2}.", index, spell.MagicalPower, spell.ManaCost);
                index++;
            }
            int dSpellNum = Int32.Parse(Console.ReadLine()) - 1;

            gendalf.SelectSpell((Spell)attack_spells[aSpellNum], (Spell)defensive_spells[dSpellNum]);


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

        /// <summary>
        /// Поверхность в действии
        /// </summary>
        public static void SurfaceOnAction()
        {            
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
            
        }

    }
}
