using System.Configuration;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly OracleDbProvider _dbProvider = new OracleDbProvider();
        protected string _spOwner;

        public BaseRepository()
        {
            _spOwner = ConfigurationManager.AppSettings["ProcedureOwner"];
        }

        protected string Sp(string name)
        {
            return string.IsNullOrEmpty(_spOwner) ? name : $"{_spOwner}.{name}";
        }
    }
}
