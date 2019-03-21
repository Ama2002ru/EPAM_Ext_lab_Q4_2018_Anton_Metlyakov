namespace DAL
{
    public interface IQuizRepository : IBaseRepository<Quiz>
    {
        IdbConnector Db { get; }
    }
}
