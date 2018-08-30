using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Colors;
using Bond.IO.Safe;
using Bond;
using Bond.Protocols;
using james.Repository;

namespace james.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {

        private readonly IColorsRepository _repository;

        public ColorsController(IColorsRepository repository)
        {
            _repository = repository;
        }

        // GET /colors/1
        [HttpGet("{id:int}")]
        public ActionResult<Color> Get(int id)
        {
            james.Db.Color dbColor = _repository.Get(id);
            if (dbColor == null)
            {
                return NotFound(
                    new NoColor
                    {
                        Message = $"Color '{id}' no trobat"
                    });
            }

            var src = new Color
            {
                Nom = dbColor.Nom,
                Rgb = dbColor.Rgb
            };

            return Ok(src);
        }

        [HttpGet("{nom}")]
        public async Task<IActionResult> Get(string nom)
        {
            james.Db.Color dbColor = await _repository.Get(nom);
            if (dbColor == null)
            {
                return NotFound(
                    new NoColor
                    {
                        Message = $"Color '{nom}' no trobat"
                    });
            }

            var src = new Color
            {
                Nom = dbColor.Nom,
                Rgb = dbColor.Rgb
            };

            return Ok(src);
        }

    }
}
