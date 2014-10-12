﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ITGame.CLI.Models.Magic;
using ITGame.CLI.Models.Equipment;
using ITGame.CLI.Models.Environment;
using ITGame.CLI.Infrastructure;
using ITGame.CLI.Extensions;
using ITGame.CLI.Models.Сreature;
using ITGame.CLI.Models.Сreature.Actions;

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

            // SurfaceOnAction();

            // if(args.Length != 0)
            //      ToCmd(args);

            // if(args.Length != 0)
            //     ToCmdAsync(args).Wait();

            // EditEntities();

            // ThreadingExample().Wait();

            // Console.ReadKey();
        }

        private static async Task ThreadingExample()
        {            
            var dict1 = new Dictionary<string, string>();
            dict1.Add("Agility", "101");
            dict1.Add("HumanoidRace", "Human");
            
            var repo = EntityRepository.GetInstance<Humanoid>();
            var repo2 = EntityRepository.GetInstance<Armor>();
            var bar = new ProgressBar();

            bar.StartProgress();
            var createTask = repo.CreateAsync(dict1);
            var getAllTask = repo2.GetAllAsync();

            var entity = await createTask;
            var all = await getAllTask;
            bar.StopProgress();

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
             
        static void ToCmd(string[] args)
        {
            #region Закомменченые комманды
            args = new string[3];
            // args[0] == "create", "read", "update", "delete", "help"
            args[0] = "create";
            // args[1] == parameters for creature
            // creature
            args[1] = "-h"; // args[1] = "-h";
            args[2] = "Anton,Elf,11,_.";
            //args[3] = "356f811d-c876-4bc8-8d90-fa2c69cd1a25";
            /*args[2] = "_.,20";
            args[3] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            // weapon
            args[4] = "-w";
            args[5] = "Sword,_.";
            args[6] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            // spell
            args[7] = "-s";
            args[8] = "AttackSpell,Fire,Wrath,12,_.";
            args[9] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            // armor
            args[10] = "-a";
            args[11] = "Gloves,10,2";
            args[12] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            */
            #endregion

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
                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);
                            var newEntity = EntityRepository.GetInstance(entityType).Create(cData.Properties);
                            newEntity.Id = Guid.NewGuid();

                            try
                            {
                                EntityRepository.GetInstance(entityType).Add(newEntity);
                                EntityRepository.GetInstance(entityType).SaveChanges();

                                Console.WriteLine("{0} created\n", cData.EntityType);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        foreach (CmdData cData in cmdArgs)
                        {
                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);
                            var newEntity = EntityRepository.GetInstance(entityType).Create(cData.Properties);
                            newEntity.Id = Guid.NewGuid();

                            try
                            {
                                EntityRepository.GetInstance(entityType).Add(newEntity);
                                EntityRepository.GetInstance(entityType).SaveChanges();

                                Console.WriteLine("{0} created\n", cData.EntityType);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        break;
                    case CmdCommands.update:
                        foreach (CmdData cData in cmdArgs)
                        {
                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);
                            var updatedEntity = EntityRepository.GetInstance(entityType).Create(cData.Properties);
                            updatedEntity.Id = cData.EntityGuid;
                            try
                            {
                                EntityRepository.GetInstance(entityType).Update(updatedEntity);
                                EntityRepository.GetInstance(entityType).SaveChanges();

                                Console.WriteLine("Entity of type {0} with id {1} have just successfully been updated", cData.EntityType, cData.EntityGuid);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        break;
                    case CmdCommands.delete:
                        foreach (CmdData cData in cmdArgs)
                        {
                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);

                            EntityRepository.GetInstance(entityType).Delete(cData.EntityGuid);
                            EntityRepository.GetInstance(entityType).SaveChanges();

                            Console.WriteLine("Entity of type {0} with id {1} have just successfully been deleted", cData.EntityType, cData.EntityGuid);
                        }
                        break;
                    case CmdCommands.read:
                        foreach (var cData in cmdArgs)
                        {
                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);
                            var entities = EntityRepository.GetInstance(entityType).GetAll();

                            foreach (var entity in entities)
                            {
                                Console.WriteLine(entity.ToString());
                            }
                        }
                        break;
                    default:
                        break;
                }
                
            }
        }
        static async Task ToCmdAsync(string[] args)
        {
            #region Закомменченые комманды
            //args = new string[3];
            //// args[0] == "create", "read", "update", "delete", "help"
            //args[0] = "asd";
            //// args[1] == parameters for creature
            //// creature
            //args[1] = "asd"; // args[1] = "-h";
            //args[2] = "Anton,Elf,11,_.";
            ////args[3] = "356f811d-c876-4bc8-8d90-fa2c69cd1a25";
            ///*args[2] = "_.,20";
            //args[3] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            //// weapon
            //args[4] = "-w";
            //args[5] = "Sword,_.";
            //args[6] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            //// spell
            //args[7] = "-s";
            //args[8] = "AttackSpell,Fire,Wrath,12,_.";
            //args[9] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            //// armor
            //args[10] = "-a";
            //args[11] = "Gloves,10,2";
            //args[12] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            
            #endregion

            CmdParser parser = new CmdParser(args);
            var porgressBar = new ProgressBar();

            if (parser.IsHelp) parser.GetHelp();
            else
            {
                List<CmdData> cmdArgs = parser.Parse();
                CmdCommands command = cmdArgs[0].Command;

                Console.WriteLine();

                switch (command) 
                {
                    case CmdCommands.create:
                        foreach (CmdData cData in cmdArgs)
                        {
                            porgressBar.StartProgress();

                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);
                            var newEntity = await EntityRepository.GetInstance(entityType).CreateAsync(cData.Properties);
                            newEntity.Id = Guid.NewGuid();

                            try
                            {
                                await EntityRepository.GetInstance(entityType).AddAsync(newEntity);
                                await EntityRepository.GetInstance(entityType).SaveChangesAsync();

                                porgressBar.StopProgress();
                                Console.WriteLine("{0} created\n", cData.EntityType);
                            }
                            catch (Exception e)
                            {
                                porgressBar.StopProgress();
                                Console.WriteLine(e.Message);
                            }
                        }
                        break;
                    case CmdCommands.update:
                        foreach (CmdData cData in cmdArgs)
                        {
                            porgressBar.StartProgress();

                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);
                            var updatedEntity = await EntityRepository.GetInstance(entityType).CreateAsync(cData.Properties);
                            updatedEntity.Id = cData.EntityGuid;

                            try
                            {
                                await EntityRepository.GetInstance(entityType).UpdateAsync(updatedEntity);
                                await EntityRepository.GetInstance(entityType).SaveChangesAsync();

                                porgressBar.StopProgress();
                                Console.WriteLine("Entity of type {0} with id {1} have just successfully been updated", cData.EntityType, cData.EntityGuid);
                            }
                            catch (Exception e)
                            {
                                porgressBar.StopProgress();
                                Console.WriteLine(e.Message);
                            }
                        }
                        break;
                    case CmdCommands.delete:
                        foreach (CmdData cData in cmdArgs)
                        {
                            porgressBar.StartProgress();

                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);

                            await EntityRepository.GetInstance(entityType).DeleteAsync(cData.EntityGuid);
                            await EntityRepository.GetInstance(entityType).SaveChangesAsync();

                            porgressBar.StopProgress();
                            Console.WriteLine("Entity of type {0} with id {1} have just successfully been deleted", cData.EntityType, cData.EntityGuid);
                        }
                        break;
                    case CmdCommands.read:
                        foreach (CmdData cData in cmdArgs)
                        {
                            porgressBar.StartProgress();

                            var entityType = TypeExtension.GetTypeFromCurrentAssembly(cData.EntityType);
                            var entities = await EntityRepository.GetInstance(entityType).GetAllAsync();

                            porgressBar.StopProgress();

                            foreach (var entity in entities)
                            {
                                Console.WriteLine(entity.ToString());
                            }
                        }
                        break;
                    default:
                        break;
                }

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
            var surfaceRules = new Dictionary<SurfaceType, SurfaceRule>();
            surfaceRules.Add(SurfaceType.Ground, new SurfaceRule { HP = 50 });

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
            Surface.ConfigureRules(surfaceRules);
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
