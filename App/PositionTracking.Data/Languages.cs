 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PositionTracking.Data
{
    public enum Languages
    {
        /// <summary>English</summary>
        [Display(Name = "English")]
        en,
        /// <summary>Croatian</summary>
        [Display(Name = "Croatian")]
        hr,
        /// <summary>Bosnian</summary>
        [Display(Name = "Bosnian")]
        bs,
        /// <summary>Serbian</summary>
        [Display(Name = "Serbian")]
        sr,
        /// <summary>German</summary>
        [Display(Name = "German")]
        de,
        /// <summary>Slovenian</summary>
        [Display(Name = "Slovenian")]
        sl,
        
    }
}
