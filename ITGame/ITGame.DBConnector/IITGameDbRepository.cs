namespace ITGame.DBConnector
{
    public interface IITGameDbRepository : IEntityDbRepository
    {
        HumanoidsDbProjector GetHumanoidsProjector();
//        HumanoidsDbProjector GetArmorsProjector();
//        HumanoidsDbProjector GetWeaponsProjector();
//        HumanoidsDbProjector GetSpellsProjector();
//        HumanoidsDbProjector GetCharactersProjector();
    }
}
