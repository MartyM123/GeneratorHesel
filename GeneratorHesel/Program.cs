using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class Program
{
    static void Main()
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

        for (int i = 0; i < passwordLenght-pwdCfg.Length; i++)
        {
            password.Append(allowedChars[random.Next(allowedChars.Length)]);
        }

        string shuffledPwd = ShuffleString(password.ToString());

        Console.WriteLine(shuffledPwd);

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
    }
}