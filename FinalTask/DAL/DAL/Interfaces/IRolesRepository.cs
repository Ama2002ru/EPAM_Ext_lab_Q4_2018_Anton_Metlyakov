namespace DAL
{
    public interface IRolesRepository : IBaseRepository<Role>
    {
        IdbConnector Db { get; set; }
    }
}
