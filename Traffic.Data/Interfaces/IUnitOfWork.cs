using System;
using System.Data;
using System.Threading.Tasks;

namespace Traffic.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();

        //DataSet ExecuteSp(string procName, params OracleParameter[] paramters);
        //DataSet ExecuteSp(string connectionString, string procName, params OracleParameter[] paramters);
    }
}
