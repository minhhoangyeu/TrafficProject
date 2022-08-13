using Traffic.Data.Entities;
using Traffic.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Traffic.Data.EF
{
    public class DbInitializer
    {
        private readonly TrafficContext _context;

        public DbInitializer(TrafficContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
           
        }

       
      
    }
}