using Microsoft.EntityFrameworkCore;
using PositionTracking.Data;
using PositionTracking.Engine;
using System;
using System.Linq;
using PositionTracking;
using System.Security.Cryptography;

namespace PositionTracking.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var tst = new EncryptDecryptService();

            var key = "YX6kyXKjxQp3BeYGsCgYoookMPjVSE685xtJy5A8KHVMuso3cq5y4YfHCNMXmtnx";

            Aes aes = Aes.Create();

            KeySizes[] ks = aes.LegalKeySizes;
            foreach (KeySizes item in ks)
            {
                Console.WriteLine("Legal min key size = " + item.MinSize);
                Console.WriteLine("Legal max key size = " + item.MaxSize);
                //Output
                // Legal min key size = 128
                // Legal max key size = 256
            }

            var str = "sandro@position.com";
            Console.WriteLine($"Encrypting : {str}");
            var encryptedString = EncryptDecryptService.EncryptString(key, str);
            Console.WriteLine($"encrypted string = {encryptedString}");

            var decryptedString = EncryptDecryptService.DecryptString(key, encryptedString);
            Console.WriteLine($"decrypted string = {decryptedString}");

            Console.ReadKey();


            //Console.WriteLine("Test:");
            //using (var db = new ApplicationDbContext())
            //{

            //    //Resolver.UpdateRanks(db);
            //}

            //using (var db = new ApplicationDbContext())
            //{
            //    foreach (var item in db.Keywords.Include(k => k.Ratings).Include(k=>k.Project))
            //    {
            //        Console.WriteLine(item.Value + " ranks: ");
            //        foreach (var rating in item.Ratings)
            //        {
            //            Console.WriteLine(" -- " + rating.Rank);
            //        }
            //    }
            //    Console.ReadLine();
            //}

            //var rank = Resolver.GetRank("klime", Languages.lang_hr, Countries.HR,"www.klime.hr", ResolverType.GoogleWeb);

            //Console.WriteLine("Testing Rank : "+rank);
            //Console.ReadLine();
        }
    }
}
