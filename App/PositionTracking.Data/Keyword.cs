﻿using System;
using System.Collections.Generic;
    
namespace PositionTracking.Data
{
    public class Keyword
    {
        
        public int KeywordId { get;private set; }
        public string Value { get; set; }

        public Languages Language { get; set; }
        public Countries Location { get; set; }
        public ICollection<KeywordRating> Ratings { get; set; }


        public Keyword()
        {
        }
    }
}
