using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PLinqExample
{
    class Program
    {
        static List<AlphaToNumericToMorseMapping> _mappings;
        static void Main(string[] args)
        {
            PopulateMappings();
            string stringToTranslate = "Welcome to a simple PLinq Example".ToUpper();
            //string stringToTranslate = "Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Ut enim ad minim veniam quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur Excepteur sint occaecat cupidatat non proident sunt in culpa qui officia deserunt mollit anim id est laborum";

            TranslateToMorseWithForEachLoop(stringToTranslate);

            Console.ReadLine();

            TranslateToMorseWithForLoop(stringToTranslate);

            Console.ReadLine();


            TranslateToMorseWithOrderedPLinq(stringToTranslate);

            Console.ReadLine();

            TranslateToMorseWithPLinq(stringToTranslate);

            Console.ReadLine();
        }

        private static Task TranslateToMorseWithForEachLoop(string word)
        {            
            List<string> morse = new List<string>();
            Console.WriteLine($"Starting translation with loop to morse: #{DateTime.Now.ToString("HH:mm:ss:fff")}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (char c in word)
            {
                morse.Add(TranslateFromCharToMorse(c));
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Finishing translation with loop to morse: #{DateTime.Now.ToString("HH:mm:ss:fff")}");
            DisplayOutcomes(stopwatch.ElapsedTicks, morse);
            return (Task.CompletedTask);

        }

        private static Task TranslateToMorseWithForLoop(string word)
        {
            List<string> morse = new List<string>();
            Console.WriteLine($"Starting translation with for loop to morse: #{DateTime.Now.ToString("HH:mm:ss:fff")}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for(int i=0; i < word.Length; i++)
            {
                morse.Add(TranslateFromCharToMorse(word[i]));
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Finishing translation with for loop to morse: #{DateTime.Now.ToString("HH:mm:ss:fff")}");
            DisplayOutcomes(stopwatch.ElapsedTicks, morse);
            return (Task.CompletedTask);

        }


        private static Task TranslateToMorseWithOrderedPLinq(string word)
        {
            Console.WriteLine($"Starting translation with orderd pliq to morse: #{DateTime.Now.ToString("HH:mm:ss:fff")}");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var morseValues = from w in word.AsParallel().AsOrdered()
                              select TranslateFromCharToMorse(w);
            
            stopwatch.Stop();
            Console.WriteLine($"Finishing translation with plinq to morse: #{DateTime.Now.ToString("HH:mm:ss:fff")}");            
            DisplayOutcomes(stopwatch.ElapsedTicks, morseValues.ToList());
            return (Task.CompletedTask);
        }

        private static Task TranslateToMorseWithPLinq(string word)
        {
            Console.WriteLine($"Starting translation with pliq to morse: #{DateTime.Now.ToString("HH:mm:ss:fff")}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var morseValues = from w in word.AsParallel()
                              select TranslateFromCharToMorse(w);
            
            stopwatch.Stop(); 
            Console.WriteLine($"Finishing translation with plinq to morse: #{DateTime.Now.ToString("HH:mm:ss:fff")}");            
            DisplayOutcomes(stopwatch.ElapsedTicks, morseValues.ToList());
            return (Task.CompletedTask);
        }

        private static void DisplayMorseCodeProduced(List<string> morseValues)
        {
            Console.WriteLine($"{string.Join(',', morseValues)}");
        }

        private static void DisplayDurationAndThreadCount(long duration, int threadCount)
        {
            Console.WriteLine($"Took: {duration}, over {threadCount} threads");
        }

        private static void TranslateMorseArrayToString(List<string> morseArray)
        {
            List<char> charArray = new List<char>();
            foreach(string morse in morseArray)
            {
                charArray.Add(TranslateFromMorseToChar(morse));
            }
            Console.WriteLine($"Retranslated: {string.Join("", charArray)}");
        }

        private static char TranslateFromMorseToChar(string morse)
        {
            return _mappings.First(m => m.MorseValue == morse).AlphaValue;
        }

        private static string TranslateFromCharToMorse(char c)
        {
            Thread.Sleep(50);
            return _mappings.First(m => m.AlphaValue == c).MorseValue;
        }

        private static void DisplayOutcomes(long elapsed, List<string> morse)
        {
            DisplayMorseCodeProduced(morse);
            DisplayDurationAndThreadCount(elapsed, Process.GetCurrentProcess().Threads.Count);
            TranslateMorseArrayToString(morse);
        }

        private static void PopulateMappings()
        {
            _mappings = new List<AlphaToNumericToMorseMapping>();
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = ' ', MorseValue = " ", NumericValue = 0 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'A', MorseValue = ".-", NumericValue = 1 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'B', MorseValue = "-...", NumericValue = 2 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'C', MorseValue = "-.-.", NumericValue = 3 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'D', MorseValue = "-..", NumericValue = 4 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'E', MorseValue = ".", NumericValue = 5 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'F', MorseValue = "..-.", NumericValue = 6 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'G', MorseValue = "--.", NumericValue = 7 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'H', MorseValue = "....", NumericValue = 8 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'I', MorseValue = "..", NumericValue = 9 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'J', MorseValue = ".---", NumericValue = 10 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'K', MorseValue = "-.-", NumericValue = 11 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'L', MorseValue = ".-..", NumericValue = 12 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'M', MorseValue = "--", NumericValue = 13 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'N', MorseValue = "-.", NumericValue = 14 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'O', MorseValue = "---", NumericValue = 15 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'P', MorseValue = ".--.", NumericValue = 16 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'Q', MorseValue = "--.-", NumericValue = 17 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'R', MorseValue = ".-.", NumericValue = 18 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'S', MorseValue = "...", NumericValue = 19 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'T', MorseValue = "-", NumericValue = 20 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'U', MorseValue = "..-", NumericValue = 21 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'V', MorseValue = "...-", NumericValue = 22 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'W', MorseValue = ".--", NumericValue = 23 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'X', MorseValue = "-..-", NumericValue = 24 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'Y', MorseValue = "-.--", NumericValue = 25 });
            _mappings.Add(new AlphaToNumericToMorseMapping() { AlphaValue = 'Z', MorseValue = "--..", NumericValue = 26 });
        }

    }
}
