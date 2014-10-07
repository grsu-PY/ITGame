namespace ITGame.CLI.Models.Сreature.Actions
{
    public interface ICanAttack : IAction
    {
        void WeaponAttack();
        void SpellAttack();
    }
}
