using System.Collections.Generic;
using System.IO;

namespace Algorithms_CSharp_Course
{
    public class In
    {
        public static IEnumerable<int> ReadInts(string filePath)
        {
            using (TextReader reader = File.OpenText(filePath))
            {
                string lastLine;
                while ((lastLine = reader.ReadLine())!=null)
                {
                    yield return int.Parse(lastLine);
                }
            }
        }
    }
}