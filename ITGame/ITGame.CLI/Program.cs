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
            Elf legolas = new Elf();
            legolas.HP = legolas.MaxHP;
            Console.WriteLine("Legolas\nCurrent/Max: {0}/{1}", legolas.HP, legolas.MaxHP);

            legolas.Equip(new Bow { PhysicalAttack = 15 });

            Human gendalf = new Human();
            gendalf.HP = gendalf.MaxHP;
            Console.WriteLine("Gendalf\nCurrent/Max: {0}/{1}", legolas.HP, legolas.MaxHP);

            gendalf.SelectSpell(new AttackSpell { TotalMagicalAttack = 30 });

            gendalf.SetTarget(legolas);

            Console.WriteLine("\nGendalf deals {0} damage to Legolas.", 30);
            gendalf.SpellAttack();

            Console.WriteLine("\n\nLegolas\nCurrent/Max: {0}/{1}", legolas.HP, legolas.MaxHP);

            Console.ReadKey();
        }
    }
}
