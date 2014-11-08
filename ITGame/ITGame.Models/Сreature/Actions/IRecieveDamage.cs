using ITGame.Models.Magic;

namespace ITGame.Models.Сreature.Actions
{
    public interface IRecieveDamage : IAction
    {
        void RecieveDamage(Damage damage, SchoolSpell spellType = SchoolSpell.None);
    }
}
