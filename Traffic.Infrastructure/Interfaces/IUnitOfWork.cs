using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Traffic.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();

        DataSet ExecuteSp(string procName, params OracleParameter[] paramters);
        DataSet ExecuteSp(string connectionString, string procName, params OracleParameter[] paramters);
    }
}
