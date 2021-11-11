namespace Products.Api.Models
{
    public interface IAppSettings
    {
        string ConnectionString { get; }
    }

    public class AppSettings : IAppSettings
    {
        public string ConnectionString { get; private set; }

        public AppSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}