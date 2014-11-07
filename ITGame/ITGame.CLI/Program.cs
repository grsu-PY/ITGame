using System;
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
            // SurfaceOnAction();

            // if(args.Length != 0)
                  //ToCmd(args);

            // if(args.Length != 0)
            ToCmdAsync(args).Wait();

            // EditEntities();

            //ThreadingExample().Wait();

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
            args[2] = "Anton,_.";
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
            //args[0] = "create";
            //// args[1] == parameters for creature
            //// creature
            //args[1] = "-h"; // args[1] = "-h";
            //args[2] = "_.";
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
