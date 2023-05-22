using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_ADO.App.Models
{
    internal class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Resume { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? Price { get; set; }
    }
}
