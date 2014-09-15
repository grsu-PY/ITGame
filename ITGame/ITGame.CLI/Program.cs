using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITGame.CLI.Models.Creature;
using ITGame.CLI.Models.Magic;
using ITGame.CLI.Models.Equipment;
namespace ITGame.CLI
{
    class Program
    {
        private static ConsoleColor defaultColor = Console.ForegroundColor;
        static void Main(string[] args)
        {
            Elf legolas = new Elf { Name = "Legolas" };
            legolas.ActionPerformed += ActionPerformed;
            Console.WriteLine("{0}\nCurrent/Max: {1}/{2}", legolas.Name, legolas.HP, legolas.MaxHP);

            legolas.Equip(new Bow { PhysicalAttack = 15 });

            Human gendalf = new Human { Name = "Gendalf" };
            gendalf.ActionPerformed += ActionPerformed;
            Console.WriteLine("{0}\nCurrent/Max: {1}/{2}", gendalf.Name, gendalf.HP, gendalf.MaxHP);

            gendalf.SelectSpell(new AttackSpell { TotalMagicalAttack = 30 });

            gendalf.SetTarget(legolas);

            gendalf.SpellAttack();

            Console.WriteLine("\n\n{0}\nCurrent/Max: {1}/{2}", legolas.Name, legolas.HP, legolas.MaxHP);

            Console.ReadKey();
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
