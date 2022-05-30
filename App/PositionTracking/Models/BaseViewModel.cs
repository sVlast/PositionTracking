using PositionTracking.Data;
using System;

namespace PositionTracking.Models
{

    public class BaseViewModel
    {
        public static Languages language;

        public BaseViewModel()
        {
            language = Languages.en;
            Console.WriteLine("BaseViewModel" + language);
        }

        public static void setLanguage(Languages lang)
        {
            language = lang;
        }
    }
}
