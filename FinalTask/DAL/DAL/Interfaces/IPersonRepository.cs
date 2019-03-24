namespace DAL
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        IdbConnector Db { get; set; }
    }
}
