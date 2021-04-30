using System;
using System.Collections.Generic;
    
namespace PositionTracking.Data
{
    public class Keyword
    {
        
        public int KeywordId { get;private set; }
        public string Value { get; set; }
        public ICollection<KeywordEntry> Entries { get; set; }


        public Keyword()
        {
        }
    }
}
