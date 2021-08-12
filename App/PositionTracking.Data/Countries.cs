using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PositionTracking.Data
{
    public enum Countries
    {
        /// <summary>United States</summary>
        [Display(Name = "United States")]
        US,
        /// <summary>United Kingdom</summary>
        [Display(Name = "United Kingdom")]
        GB,
        /// <summary>Croatia</summary>
        [Display(Name = "Croatia")]
        HR,
        /// <summary>Bosnia & Herzegovina</summary>
        [Display(Name = "Bosnia & Herzegovina")]
        BA,
        /// <summary>Serbia</summary>
        [Display(Name = "Serbia")]
        RS,
        /// <summary>Germany</summary>
        [Display(Name = "Germany")]
        DE,
        /// <summary>Slovenia</summary>
        [Display(Name = "Slovenia")]
        SI,

    }
}
