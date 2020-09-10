using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PwdGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(GeneratePassword(8, 15));
            Console.ReadKey();
        }

        static string GeneratePassword(int MinLength, int MaxLength)
        {
            string ValidChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ:;<>|=.,_-~!?&%@#$£€°^*§()+[] ";
            string SpecialChars = "!@#$%^&*()";
            
            string pwd = string.Empty;

            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            
            bool NumExist = false;
            bool IsUpper = false;
            bool IsLower = false;
            bool IsSplChr = false;
            bool NoRepeat = true;
            bool NoSeq = true;

            while ((!NumExist || !IsUpper || !IsLower || !IsSplChr) && (NoRepeat || NoSeq) )
            {
                NumExist = false;
                IsUpper = false;
                IsLower = false;
                IsSplChr = false;
                NoRepeat = true;
                NoSeq = true;
                
                pwd = string.Join(string.Empty, Enumerable.Repeat(ValidChars, rnd.Next(MinLength, MaxLength + 1)).Select(s => s[rnd.Next(s.Length)]).ToArray());
                for (int i = 0; i < pwd.Length; i++)
                {
                    // Contains at least 1 lower case letter and 1 upper case letter (all UTF-8), at least 1 number

                    if (!NumExist)
                    {
                        NumExist = (char.IsDigit(pwd[i]) && true);
                    }
                    if (!IsUpper)
                    {
                        IsUpper = (char.IsUpper(pwd[i]) && true);
                    }
                    if (!IsLower)
                    {
                        IsLower = (char.IsLower(pwd[i]) && true);
                    }

                    // A predefined set of special chars must be present
                    if (!IsSplChr)
                    {
                        IsSplChr = (SpecialChars.IndexOf(pwd[i]) >= 0);
                    }


                    // Not more than 2 identical characters in a row (e.g., 111 not allowed)
                    if (i < pwd.Length - 2 && NoRepeat)
                    {
                        NoRepeat = !((pwd[i] == pwd[i + 1]) && (pwd[i] == pwd[i + 2]));
                    }

                    // Not any sequence of the English alphabet / numbers (above 3 letters)
                    if (i < pwd.Length - 2 && NoSeq)
                    {
                        NoSeq = !((pwd[i + 2] - pwd[i + 1]) == (pwd[i + 1] - pwd[i]));
                    }
                    Console.WriteLine(!NumExist +" "+ !IsUpper + " " + !IsLower + " " + !IsSplChr + " " +  NoRepeat +NoSeq);
                }
                
            }
            
            return pwd;

        }
    }
}
