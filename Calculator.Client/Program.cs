using System;
using Microsoft.Research.SEAL;

using Calculator.Common.Utilities;
using System.Threading.Tasks;

namespace Calculator.Client
{
    internal class Program
    {
        
        static async Task Main(string[] args)
        {
            Utilities utilities = new Utilities();
            ulong x = 183;
            Plaintext xPlain = new Plaintext(utilities.ULongToString(x));
            Console.WriteLine($"Express x = {x} as a plaintext polynomial 0x{xPlain}.");

            ulong y = 166;
            Plaintext yPlain = new Plaintext(utilities.ULongToString(y));
            Console.WriteLine($"Express x = {y} as a plaintext polynomial 0x{yPlain}.");

            Ciphertext xEncrypted = new Ciphertext();
            Console.WriteLine("Encrypt xPlain to xEncrypted.");
            utilities.Encryptor.Encrypt(xPlain, xEncrypted);

            Ciphertext yEncrypted = new Ciphertext();
            Console.WriteLine("Encrypt yPlain to yEncrypted.");
            utilities.Encryptor.Encrypt(yPlain, yEncrypted);

            Plaintext xDecrypted = new Plaintext();
            Console.Write("    + decryption of encrypted_x: ");
            utilities.Decryptor.Decrypt(xEncrypted, xDecrypted);
            Console.WriteLine($"0x{xDecrypted} ...... Correct.");

            Plaintext yDecrypted = new Plaintext();
            Console.Write("    + decryption of encrypted_y: ");
            utilities.Decryptor.Decrypt(yEncrypted, yDecrypted);
            Console.WriteLine($"0x{yDecrypted} ...... Correct.");

            Console.WriteLine("Compute xEncrypted+yEncrypted");
            Ciphertext encryptedResult = new Ciphertext();
            utilities.Evaluator.Add(xEncrypted, yEncrypted, encryptedResult);
            

            Plaintext decryptedResult = new Plaintext();
            Console.Write("    + decryption of encryptedResult: ");
            utilities.Decryptor.Decrypt(encryptedResult, decryptedResult);
            Console.WriteLine($"0x{decryptedResult} ...... Correct.");
        }
    }
}
