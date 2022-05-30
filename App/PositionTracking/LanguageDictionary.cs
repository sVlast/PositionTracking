using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using PositionTracking.Data;
using Microsoft.Extensions.Configuration;

namespace PositionTracking
{
    public class LanguageDictionary
    {
        private readonly string _dictionaryPath;
        private Dictionary<Languages, int> _languageIndexes;

        private Dictionary<string, string[]> _translations;

        


        public LanguageDictionary(IConfiguration c)
        {
            var settings = c.GetSection("Settings");

            _dictionaryPath = settings.GetValue<string>("DictionaryPath");

            Load();

        }


        private void LoadHeader(string[] values)
        {
            for (var i=0; i<values.Length; i++)
            {
                var x = Enum.Parse<Languages>(values[i]);
                _languageIndexes.Add(x, i);

            }
           

        }

        private void Load()
        {
            _languageIndexes = new Dictionary<Languages, int>();
            _translations = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

            using (var reader = new StreamReader(_dictionaryPath))
            {
                var firstLine = true;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line == null)
                        continue;

                    var values = line.Split(";");
                    if (firstLine)
                    {
                        LoadHeader(values);
                        firstLine = false;
                    }
                    else
                    {
                        _translations.Add(values[0], values);
                    }
                }
                
                

            }
            

        }
        public string Translate(string value, Languages language)
        { 
            if (language == Languages.en)
                return value;

            var index = _languageIndexes[language];
            return _translations[value][index];

        }

    }
}
