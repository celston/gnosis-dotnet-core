using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Gnosis.Testing
{
    public static class Utility
    {
        #region Public Methods

        public static string GetRandomWord()
        {
            return words.Value[random.Value.Next(0, words.Value.Count)];
        }

        public static string GetRandomPhrase()
        {
            return GetRandomPhrase(5);
        }

        public static string GetRandomPhrase(int numWords)
        {
            StringBuilder sb = new StringBuilder();

            for (var i = 0; i < numWords; i++)
            {
                if (i != 0)
                {
                    sb.Append(" ");
                }
                sb.Append(GetRandomWord());
            }

            return sb.ToString();
        }

        #endregion

        #region Private Lazy Fields

        static Lazy<List<string>> words = new Lazy<List<string>>(() =>
        {
            List<string> result = new List<string>();
            
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("Gnosis.Testing.dictionary.txt");
            StreamReader sr = new StreamReader(s);
            while (!sr.EndOfStream)
            {
                result.Add(sr.ReadLine());
            }

            return result;
        });

        static Lazy<Random> random = new Lazy<Random>(() =>
        {
            return new Random();
        });

        #endregion

        
    }
}
