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
            Console.WriteLine(e.message);
        }
    }
}
