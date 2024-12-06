using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;

class Program
{
    static void Main()
    {
        Generate();

        static void Authenticate()
        {
            // Define the file path where the hash was saved
            string filePath = "password_hash.txt";

            // Check if the file exists before trying to read it
            if (File.Exists(filePath))
            {
                // Read the hash from the file
                string savedHash = File.ReadAllText(filePath, Encoding.UTF8);

                // Output the saved hash
                Console.WriteLine("SHA-256 Hash read from file: \n" + savedHash);
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
            }
        }

        static void Generate()
        {
            int passwordLenght = ReadPasswordLenght();
            string pwdCfg = ReadPwdCfg();

            Dictionary<char, string> charDict = new Dictionary<char, string>();

            charDict.Add('v', "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            charDict.Add('m', "abcdefghijklmnopqrstuvwxyz");
            charDict.Add('p', "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            charDict.Add('c', "0123456789");
            charDict.Add('s', "_#$/-+*@");

            Random random = new Random();

            StringBuilder password = new StringBuilder();

            string allowedChars = "asdfghjkl";

            for (int i = 0; i < pwdCfg.Length; i++)
            {
                char currentChar = pwdCfg[i];

                string set = charDict[currentChar];

                password.Append(set[random.Next(set.Length)]);
            }

            for (int i = 0; i < passwordLenght - pwdCfg.Length; i++)
            {
                password.Append(allowedChars[random.Next(allowedChars.Length)]);
            }

            string shuffledPwd = ShuffleString(password.ToString());
            SaveHash(GenerateSHA256Hash(shuffledPwd));

            Console.WriteLine(shuffledPwd);
            Console.WriteLine(GenerateSHA256Hash(shuffledPwd));
        }

        static string ShuffleString(string input)
        {
            Random random = new Random();
            return new string(input
                .OrderBy(c => random.Next()) // Shuffle using random ordering
                .ToArray());
        }

        static string ReadPwdCfg()
        {
            Console.WriteLine("zadejte typ hesla: ");
            string pwdCfg;
            pwdCfg = Console.ReadLine();
            string allowedChars = "cpsvm";
            while (!(pwdCfg.All(c => allowedChars.Contains(c))))
            {
                Console.WriteLine("Povolene je jen c cislo s special p pismeno m male pismeno v velke pismeno");
                pwdCfg = Console.ReadLine();
            }
            return pwdCfg;
        }

        static int ReadPasswordLenght()
        {
            Console.WriteLine("zadejte delku hesla: ");
            int pwdLenght;
            while (!int.TryParse(Console.ReadLine(), out pwdLenght) || pwdLenght < 6)
            {
                Console.Write("Please enter a valid number (minimum 6): ");
            }
            return pwdLenght;
        }

        static string GenerateSHA256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                // Return the hexadecimal string
                return builder.ToString();
            }
        }

        static void SaveHash(string hash)
        {
            string filePath = "password_hash.txt";

            File.AppendAllText(filePath, hash + "\n", Encoding.UTF8);
        }
    }
}