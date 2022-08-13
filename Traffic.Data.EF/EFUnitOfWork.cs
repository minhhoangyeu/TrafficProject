using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Traffic.Infrastructure.Interfaces;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Traffic.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly TrafficContext _context;

        public EFUnitOfWork(TrafficContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public DataSet ExecuteSp(string procName, params OracleParameter[] paramters)
        {
                DataSet result = new DataSet();
                var connection = _context.Database.GetDbConnection();

                using (var command = connection.CreateCommand())
                {
                    using (DbDataAdapter adapter = DbProviderFactories.GetFactory(connection).CreateDataAdapter())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = procName;

                        if (paramters != null)
                        {
                            command.Parameters.AddRange(paramters);
                        }

                        adapter.SelectCommand = command;
                        adapter.Fill(result);
                    }
                }

                return result;
            
        }
        public DataSet ExecuteSp(string connectionString, string procName, params OracleParameter[] paramters)
        {
                DataSet result = new DataSet();
                //var connection = _context.Database.GetDbConnection();
                using (var connection = new OracleConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        using (DbDataAdapter adapter = DbProviderFactories.GetFactory(connection).CreateDataAdapter())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = procName;

                            if (paramters != null)
                            {
                                command.Parameters.AddRange(paramters);
                            }

                            adapter.SelectCommand = command;
                            adapter.Fill(result);
                        }
                    }
                }
                return result;
        }
    }
}
