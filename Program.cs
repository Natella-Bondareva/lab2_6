using System;
using System.Collections.Generic;
using static lab2_6.Worker;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace lab2_6
{
    internal class Program
    {

        public static bool Check(string[] line)
        {
            int currentYear = DateTime.Now.Year;
            bool b = true;
            if (line.Length < 5)
            {
                b = false;
            }
            else if (line[1].Length > 2 || line[2].Length > 2)
            {
                b = false;
            }
            else if (int.Parse(line[4]) > currentYear || int.Parse(line[4]) < 0)
            {
                b = false;
            }
            return b;
        }
        public static List<Workers> Input()
        {
            Console.WriteLine("Введіть кількість працівників:");
            int n = int.Parse(Console.ReadLine());
            List<Workers> workers = new List<Workers>();
            for (int i = 0;n>i; i++)
            {
                Console.WriteLine("Введіть прізвище та ініціали працівника, посаду і рік початку роботи (все через пропуск)");
                string[] line = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (Check(line))
                {
                    Workers worker= new Workers(line);
                    workers.Add(worker);
                }
            }
            if (workers.Count < n) 
            {
                Console.WriteLine($"Додано не всіх учнів. Успішно додана кількість: {workers.Count}");
            }
            return workers;
        }
        public static void WritingToTxt(List<Workers> workers)
        {
            string path = @"D:\nvs\лабси\lab2_6\data.txt";
            using(StreamWriter sw = new StreamWriter(path))
            {
                foreach (Workers worker in workers)
                {
                    sw.WriteLine(worker.fullName + " " + worker.position + " " + worker.year);
                }
            }
        }
        public static void WritingToXml(List<Workers> workers)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Workers>));
            string path = @"D:\nvs\лабси\lab2_6\data.xml";
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, workers);
            }
        }
        //public static void WritingToJson(List<Workers> workers)
        //{
        //    string path = @"D:\nvs\лабси\lab2_6\data.json";
        //    File.WriteAllText(path, string.Empty);
        //    foreach (Workers worker in workers)
        //    {
        //        File.AppendAllText(path, JsonConvert.SerializeObject(worker));
        //    }
        //}
        public static void WritingToJson(List<Workers> workers)
        {
            string path = @"D:\nvs\лабси\lab2_6\data.json";

            File.WriteAllText(path,string.Empty);
            File.AppendAllText(path,JsonConvert.SerializeObject(workers));

        }
        public static void Choice(ref List<Workers> workers)
        {
            int n;
            do
            {
                Console.WriteLine("1 - зчитати інформацію з файла data.txt\n2 - щоб зчитати інформацію з data.xml" +
                    "\n3 - зчитати інформацію з файла data.json\nНатисніть 0 для виходу");
                n = int.Parse(Console.ReadLine());
                switch (n)
                {
                    case 1:
                        ReadFromTxt(ref workers);
                        Task(workers);
                        break;
                    case 2:
                        ReadFromXml(ref workers);
                        Task(workers);
                        break;
                    case 3:
                        ReadFromJson(ref workers);
                        Task(workers);
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Спробуйте ще раз =)");
                        break;
                }
            }
            while (n != 0);
        }
        public static void ReadFromTxt(ref List<Workers> workers)
        {
            string path = @"D:\nvs\лабси\lab2_6\data.txt";
            try
            {
                FileStream fs = File.Open(path, FileMode.Open);
                using (var reader = new StreamReader(fs))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine() ?? string.Empty;
                        string[] str = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (Check(str))
                        {
                            Workers worker = new Workers(str);
                            workers.Add(worker);
                        }
                    }
                }
            }
            catch(System.IO.FileNotFoundException)
            {
                Console.WriteLine("Файл не знайдено.");
            }
        }
        //public static void ReadFromJson(ref List<Workers> workers)
        //{
        //    string path = @"D:\nvs\лабси\lab2_6\data.json";
        //    JsonTextReader read = new JsonTextReader(new StreamReader(path));

        //    while (true)
        //    {
        //        if (!read.Read())
        //        {
        //            break;
        //        }
        //        JsonSerializer serializer = new JsonSerializer();
        //        Workers worker = serializer.Deserialize<Workers>(read);
        //        workers.Add(worker);
        //    }
        //}
        public static void ReadFromJson(ref List<Workers> workers)
        {
            try
            {
                string path = @"D:\nvs\лабси\lab2_6\data.json";

                JsonTextReader read = new JsonTextReader(new StreamReader(path));
                JsonSerializer serializer = new JsonSerializer();
                workers = serializer.Deserialize<List<Workers>>(read);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Файл не знайдено.");
            }
            catch (System.InvalidOperationException)
            {
                Console.WriteLine("Файл пустий.");
            }
        }
        public static void ReadFromXml(ref List<Workers> workers)
        {
            try
            {
                string path = @"D:\nvs\лабси\lab2_6\data.xml";
                XmlSerializer serializer = new XmlSerializer(typeof(List<Workers>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    workers = (List<Workers>)serializer.Deserialize(fs);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Файл не знайдено.");
            }
            catch (System.InvalidOperationException)
            {
                Console.WriteLine("Файл пустий.");
            }
        }
        public static void Task(List<Workers> workers)
        {
            Console.WriteLine("Введіть число, щоб знайти робітників чий стаж перевищує це значення:");
            int n = int.Parse(Console.ReadLine());
            int currentYear = DateTime.Now.Year;
            bool b = true;
            foreach (Workers worker in workers)
            {
                if (currentYear - worker.year >= n)
                {
                    Console.WriteLine($"{worker.fullName}");
                    b = false;
                }
            }
            if (b)
            {
                Console.WriteLine("Таких робітників, на жаль, не знайдено");
            }
        }
        static void Main(string[] args)
        { 

            List<Workers> workers = new List<Workers>();
            int n;
            do
            {
                Console.WriteLine("1 - щоб записати працівників у файли\n2 - щоб зчитати інформацію з файлів\n Натисніть 0 для виходу"); ;
                n = int.Parse(Console.ReadLine());
                switch (n)
                {
                    case 1:
                        workers = Input();
                        workers.Sort();
                        WritingToTxt(workers);
                        WritingToXml(workers);
                        WritingToJson(workers);
                        break;
                    case 2:                       
                        Choice(ref workers);
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Спробуйте ще раз =)");
                        break;
                }
            }
            while (n != 0);
        }
    }
}