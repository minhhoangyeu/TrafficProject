using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Traffic.Data.Interfaces;

namespace Traffic.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TrafficContext _context;

        public UnitOfWork(TrafficContext context)
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

    }
}
