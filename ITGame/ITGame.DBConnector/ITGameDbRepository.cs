namespace ITGame.DBConnector
{
    public class ITGameDbRepository : DbRepository, IITGameDbRepository
    {
        public HumanoidsDbProjector GetHumanoidsProjector()
        {
            return new HumanoidsDbProjector(Context);
        }
    }
}