using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GameProject
{
    public static class HighScores
    {
        public static int Difficulty { get; set; }
        private static string _fileName = Directory.GetCurrentDirectory() + "\\Data.txt";

        public static double[] HighScoreArr { get; set; }

        public static void ReadFile()
        {
            HighScoreArr = new double[3];
            if (File.Exists(_fileName))
            {
                int x = 0;
                using (StreamReader sr = new StreamReader(_fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        HighScoreArr[x] = double.Parse(sr.ReadLine());
                        x++;
                    }
                }
            }
            else
            {
                File.Create(_fileName).Close();
                using (StreamWriter sw = new StreamWriter(_fileName))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        sw.WriteLine(0);
                    }

                }
            }
        }
        public static void WriteFile(double score)
        {
            HighScoreArr[Difficulty] = score;
            File.Delete(_fileName);
            File.Create(_fileName).Close();
            using (StreamWriter sw = new StreamWriter(_fileName))
            {
                for(int i = 0; i < 3; i++)
                {
                    sw.WriteLine(HighScoreArr[i].ToString());
                }
                
            }
        }
    }
}
