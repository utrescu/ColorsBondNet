using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using james.Db;

namespace james.Repository
{

    public interface IColorsRepository
    {
        Color Get(int id);
        Task<Color> Get(string nom);
    }

}