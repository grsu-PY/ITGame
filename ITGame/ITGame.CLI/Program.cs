using ITGame.DBConnector;
using ITGame.Infrastructure.Data;
using ITGame.Infrastructure.Extensions;
using ITGame.Infrastructure.Parser;
using ITGame.Models.Environment;
using ITGame.Models.Equipment;
using ITGame.Models.Сreature;
using ITGame.Models.Сreature.Actions;
using ITGame.UIElements;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITGame.CLI
{
    class Program
    {
        private static IEntityRepository _repository;
        private static IEntityRepository _dbRepository;
        private static ConsoleColor defaultColor = Console.ForegroundColor;
        static void Main(string[] args)
        {
            WorkWithDb();

            _repository = new EntityRepository();
            _dbRepository = new DBRepository();
            // SurfaceOnAction();

            // if(args.Length != 0)
            //ToCmd(args);

            // if(args.Length != 0)
            //ToCmdAsync(args).Wait();
            // EditEntities();

            //ThreadingExample().Wait();

            Console.ReadKey();
        }

        private static void WorkWithDb()
        {
            ////////////////////////////////////////////////////////////////////////////////////
            ///Все типы имеют полное имя, чтоб было видно из какого пространства имен они взяты.
            ///DBConnector.ITGameDBModels.Armor для работы с базой
            ///ITGame.Models.Equipment.Armor для работы в приложении
            ///И так для всех типов, которые должны хранится в базе. Или почти для всех.. хз ;)
            ////////////////////////////////////////////////////////////////////////////////////
            var armor = new DBConnector.ITGameDBModels.Armor
            {
                ArmorType = ITGame.Models.Equipment.ArmorType.Body,
                Equipped = true,
                Id = Guid.NewGuid(),
                MagicalDef = 23,
                PhysicalDef = 43,
                Weight = 100,
                Name = "Good Name"
            };

            Guid charId;
            bool wasNull = false;
            var characterDB = _dbRepository.GetInstance<DBConnector.ITGameDBModels.Character>().GetAll().FirstOrDefault();
            if (characterDB == null)
            {
                wasNull = true;
                var _charId = Guid.NewGuid();
                characterDB = new DBConnector.ITGameDBModels.Character
                {
                    Id = _charId,
                    Name = "aliaksei  " + _charId.ToString().Substring(0, 10),//из-за ограничения в 40 char в базе для стринги, обрезаю строку
                    Password = "password",
                    Role = 1
                };
            }
            charId = characterDB.Id;

            var humanoid = new DBConnector.ITGameDBModels.Humanoid
            {
                Id = Guid.NewGuid(),
                CharacterId = charId,
                Name = "der4mInc  " + charId.ToString().Substring(0, 10),
                Level = 1,
                Strength = 10,
                Wisdom = 20,
                Constitution = 10,
                HumanoidRaceType = HumanoidRaceType.Human,
                Agility = 10
            };
            humanoid.Armors.Add(armor);
            characterDB.Humanoids.Add(humanoid);
            armor.Humanoids.Add(humanoid);
            if (wasNull)
            {
                _dbRepository.GetInstance<DBConnector.ITGameDBModels.Character>().Add(characterDB);
            }
            else
            {
                _dbRepository.GetInstance<DBConnector.ITGameDBModels.Character>().Update(characterDB);
            }
            _dbRepository.GetInstance<DBConnector.ITGameDBModels.Character>().SaveChanges();

            #region Add Fixed Races
            /* //Run only one time
            var Races = new List<DBConnector.ITGameDBModels.HumanoidRace>
            {
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.Human,
                    Name="Human"
                },
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.Elf,
                    Name="Elf"
                },
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.Dwarf,
                    Name="Dwarf"
                },
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.Orc,
                    Name="Orc"
                },
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.None,
                    Name="None"
                },
            };

            DBRepository.GetInstance<DBConnector.ITGameDBModels.HumanoidRace>().DbSet.AddRange(Races);
            DBRepository.GetInstance<DBConnector.ITGameDBModels.HumanoidRace>().SaveChanges();
            */
            #endregion
        }

        private static async Task ThreadingExample()
        {
            var dict1 = new Dictionary<string, string>();
            dict1.Add("Agility", "101");
            dict1.Add("HumanoidRace", "Human");

            var repo = _repository.GetInstance<Humanoid>();
            var repo2 = _repository.GetInstance<Armor>();
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

            var human = _repository.GetInstance<Humanoid>().Create(dict1);
            human.Id = Guid.NewGuid();

            var arm = new Armor { ArmorType = ArmorType.Body, Id = Guid.NewGuid(), MagicalDef = 32 };

            _repository.GetInstance<Armor>().Add(arm);


            var dict2 = new Dictionary<string, string>();
            dict2.Add("Agility", "60");
            dict2.Add("Strength", "50");
            dict2.Add("HumanoidRace", "Dwarf");

            var dwarf = _repository.GetInstance<Humanoid>().Create(dict2);
            dwarf.Id = Guid.NewGuid();

            _repository.GetInstance<Humanoid>().Add(human);
            _repository.GetInstance<Humanoid>().Add(dwarf);

            _repository.GetInstance<Humanoid>().SaveChanges();
            _repository.GetInstance<Armor>().SaveChanges();
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
                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);
                            var newEntity = _repository.GetInstance(entityType).Create(cData.Properties);
                            newEntity.Id = Guid.NewGuid();

                            try
                            {
                                _repository.GetInstance(entityType).Add(newEntity);
                                _repository.GetInstance(entityType).SaveChanges();

                                Console.WriteLine("{0} created\n", cData.EntityType);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        foreach (CmdData cData in cmdArgs)
                        {
                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);
                            var newEntity = _repository.GetInstance(entityType).Create(cData.Properties);
                            newEntity.Id = Guid.NewGuid();

                            try
                            {
                                _repository.GetInstance(entityType).Add(newEntity);
                                _repository.GetInstance(entityType).SaveChanges();

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
                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);
                            var updatedEntity = _repository.GetInstance(entityType).Create(cData.Properties);
                            updatedEntity.Id = cData.EntityGuid;
                            try
                            {
                                _repository.GetInstance(entityType).Update(updatedEntity);
                                _repository.GetInstance(entityType).SaveChanges();

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
                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);

                            _repository.GetInstance(entityType).Delete(cData.EntityGuid);
                            _repository.GetInstance(entityType).SaveChanges();

                            Console.WriteLine("Entity of type {0} with id {1} have just successfully been deleted", cData.EntityType, cData.EntityGuid);
                        }
                        break;
                    case CmdCommands.read:
                        foreach (var cData in cmdArgs)
                        {
                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);
                            var entities = _repository.GetInstance(entityType).GetAll();

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

                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);
                            var newEntity = await _repository.GetInstance(entityType).CreateAsync(cData.Properties);
                            newEntity.Id = Guid.NewGuid();

                            try
                            {
                                await _repository.GetInstance(entityType).AddAsync(newEntity);
                                await _repository.GetInstance(entityType).SaveChangesAsync();

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

                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);
                            var updatedEntity = await _repository.GetInstance(entityType).CreateAsync(cData.Properties);
                            updatedEntity.Id = cData.EntityGuid;

                            try
                            {
                                await _repository.GetInstance(entityType).UpdateAsync(updatedEntity);
                                await _repository.GetInstance(entityType).SaveChangesAsync();

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

                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);

                            await _repository.GetInstance(entityType).DeleteAsync(cData.EntityGuid);
                            await _repository.GetInstance(entityType).SaveChangesAsync();

                            porgressBar.StopProgress();
                            Console.WriteLine("Entity of type {0} with id {1} have just successfully been deleted", cData.EntityType, cData.EntityGuid);
                        }
                        break;
                    case CmdCommands.read:
                        foreach (CmdData cData in cmdArgs)
                        {
                            porgressBar.StartProgress();

                            var entityType = TypeExtension.GetTypeFromModelsAssembly(cData.EntityType);
                            var entities = await _repository.GetInstance(entityType).GetAllAsync();

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
                new Humanoid{Name="Legolas",HumanoidRace=HumanoidRaceType.Elf},
                new Humanoid{Name="Ahaha",HumanoidRace=HumanoidRaceType.Dwarf}
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
