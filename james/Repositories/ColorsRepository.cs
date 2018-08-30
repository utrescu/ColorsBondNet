using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using james.Db;

namespace james.Repository
{

    public class ColorsRepository : IColorsRepository
    {
        private readonly ColorsContext _context;

        public ColorsRepository(ColorsContext context)
        {
            _context = context;
        }

        public async Task<Color> Get(string nom)
        {
            return await _context.Colors
                    .Where(b => b.Nom.Equals(nom))
                    .FirstOrDefaultAsync();
        }

        public Color Get(int id) => _context.Colors.Find(id);

    }
}
