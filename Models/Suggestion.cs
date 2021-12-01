using System;
using System.Collections.Generic;

namespace game_store_be.Models
{
    public partial class Suggestion
    {
        public string IdSuggestion { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }
    }
}
