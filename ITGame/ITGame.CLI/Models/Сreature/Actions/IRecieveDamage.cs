using ITGame.CLI.Models.Magic;

namespace ITGame.CLI.Models.Сreature.Actions
{
    public interface IRecieveDamage : IAction
    {
        void RecieveDamage(Damage damage, SchoolSpell spellType = SchoolSpell.None);
    }
}
