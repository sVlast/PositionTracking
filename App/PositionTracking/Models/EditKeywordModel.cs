using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PositionTracking.Data;

namespace PositionTracking.Models
{
    public class EditKeywordModel
    {
        public string Value { get; set; }
        public Languages Language { get; set; }
        public Countries Location { get; set; }
        public Guid Id { get; set; }

    }
}