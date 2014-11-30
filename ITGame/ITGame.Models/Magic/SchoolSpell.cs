
namespace ITGame.Models.Magic
{
    [System.Flags]
    public enum SchoolSpell : byte
    {
        None = (byte)0,
        Shadow = (byte)1,
        Arcane = (byte)2,
        Fire = (byte)4
    }
}
