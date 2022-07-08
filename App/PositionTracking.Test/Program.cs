using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;
using PositionTracking.Engine;

namespace PositionTracking.Test
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var loggerFactory = (ILoggerFactory)new LoggerFactory();
            var logger = loggerFactory.CreateLogger<Program>();

            Console.WriteLine(string.Join('-', args));

            var tst = new EncryptDecryptService();

            var key = "mysmallkey1234551298765134567890";

            var str = "sandro@position.com";
            Console.WriteLine($"Encrypting : {str}");
            var encryptedString = EncryptDecryptService.EncryptString(key, str);
            Console.WriteLine($"encrypted string = {encryptedString}");

            var decryptedString = EncryptDecryptService.DecryptString(key, encryptedString);
            Console.WriteLine($"decrypted string = {decryptedString}");

            string Keyword;
            string Domain;

            try
            {
                Console.WriteLine("Test:");

                if(args.Length == 2)
                {
                    Keyword = args[0];
                    Domain = args[1];
                }
                else
                {
                    Console.WriteLine("Argumenst not provided, using default keyword and domain for testing");
                    Keyword = "klime";
                    Domain = "www.klime.hr";
                }

                var rank = await Resolver.GetRankAsync(Keyword, Languages.hr, Countries.HR, Domain, SearchEngineType.GoogleWeb,null);

                return 1;

            }
            catch (Exception e)
            {
                Console.WriteLine("Testing failed");
                Console.WriteLine("Error: ",e);
                logger.LogError("Error while testing parser!", e) ;
                return -1;
            }
        }
    }
}
