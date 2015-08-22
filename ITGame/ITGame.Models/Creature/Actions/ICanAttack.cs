namespace ITGame.Models.Creature.Actions
{
    public interface ICanAttack : IAction
    {
        void WeaponAttack();
        void SpellAttack();
    }
}
