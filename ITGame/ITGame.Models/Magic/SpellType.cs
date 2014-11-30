
namespace ITGame.Models.Magic
{
    [System.Flags]
    public enum SpellType : byte
    {
        None = (byte)0,
        AttackSpell = (byte)1,
        DefensiveSpell = (byte)2
    }
}
