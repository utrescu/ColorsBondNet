using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace james.Db
{
    [Table("color")]
    public class Color
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Rgb { get; set; }

    }
}