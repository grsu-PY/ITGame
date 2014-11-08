
namespace ITGame.Models
{
    public class Damage
    {
        private int physicalDamage;
        private int magicalDamage;
        public int PhysicalDamage
        {
            get { return physicalDamage; }
            set
            {
                physicalDamage = value > 0 ? value : 0;
            }
        }

        public int MagicalDamage
        {
            get { return magicalDamage; }
            set
            {
                magicalDamage = value > 0 ? value : 0;
            }
        }
    }
}
