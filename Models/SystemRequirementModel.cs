using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Models
{
    public class SystemRequirementModel
    {
        public string Id { get; set; }
        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Graphic { get; set; }
        public string Storage { get; set; }
    }
}
