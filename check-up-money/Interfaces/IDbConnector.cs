namespace check_up_money.Interfaces
{
    public interface IDbConnector
    {
        string ConnectionStringSqlClient { get; }
        string ConnectionStringOdbc { get; }
    }
}