using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MyComputer
{
    class Program
    {
        static void Main(string[] args)
        {
            InfoPC();
            Timer();
        }
        static void InfoPC()
        {
            Console.WriteLine("Name user: " + Environment.UserName);
            Console.WriteLine("Name machine: " + Environment.MachineName);
            Console.WriteLine("Version Operation System: " + Environment.OSVersion);

            if (Environment.Is64BitOperatingSystem == true)
            {
                Console.WriteLine("System: 64Bit");
            }
            else
            {
                Console.WriteLine("System: not 64Bit");
            }

            Console.WriteLine("Available processors: " + Environment.ProcessorCount);
            Console.WriteLine("Logical disks: " + String.Join(", ", Environment.GetLogicalDrives()));
            Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------\n");
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            int stolb = 0;
            foreach (DriveInfo d in allDrives)
            {
                Console.SetCursorPosition(stolb, 9);
                Console.WriteLine("Drive {0}", d.Name);
                Console.SetCursorPosition(stolb, 10);
                Console.WriteLine("Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.SetCursorPosition(stolb, 11);
                    Console.WriteLine("Volume label: {0}", d.VolumeLabel);
                    Console.SetCursorPosition(stolb, 12);
                    Console.WriteLine("File system: {0}", d.DriveFormat);
                    Console.SetCursorPosition(stolb, 13);
                    Console.Write("Total available space: ");
                    double gb1 = d.AvailableFreeSpace / 1073741824;
                    Console.WriteLine($"{gb1} Gb");
                    Console.SetCursorPosition(stolb, 14);
                    Console.Write("Total size of drive: ");
                    double gb2 = d.TotalSize / 1073741824;
                    Console.WriteLine($"  {gb2} Gb");
                    double usesize = gb2 - gb1;
                    double procent = usesize / gb2 * 100;
                    Console.SetCursorPosition(stolb, 15);
                    Console.WriteLine($"Total used space:{Math.Round(usesize / gb2 * 100, 0), 8} %");
                    Console.SetCursorPosition(stolb, 16);
                    Console.Write("Diagram [");
                    int count = 20;
                    while(procent > 0)
                    {
                        Console.Write("|");
                        count--;
                        procent -= 5;
                    }
                    while(count > 0)
                    {
                        Console.Write(" ");
                        count--;
                    }
                    Console.WriteLine("]");
                }

                stolb += 40;
            }
            Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------\n");
        }
        static async void Timer()
        {
            int enter = 0;
            while (enter != 1 && enter != 2)
            {
                Console.SetCursorPosition(0, 20);
                Console.WriteLine("New time -> enter 1\nPrevious time -> enter 2");
                Console.SetCursorPosition(0, 22);
                Console.Write(" ");
                Console.SetCursorPosition(0, 22);
                enter = Convert.ToInt32(Console.ReadLine());
                
            }

            if(enter == 1)
            {
                Console.Write("How often to update data in seconds: ");
                int num = 0;
                TimerCallback tm = new TimerCallback(InfoProcessor);
                int time = Convert.ToInt32(Console.ReadLine());

                string namefile = "safetime.txt";
                using (StreamWriter writer = new StreamWriter(namefile, false))
                {
                    await writer.WriteLineAsync(Convert.ToChar(time));
                    writer.Close();
                }
                
                time *= 1000;
                Timer timer = new Timer(tm, num, 0, time);
                Console.ReadLine();
            }
            else
            {
                int num = 0;
                TimerCallback tm = new TimerCallback(InfoProcessor);

                string namefile = "safetime.txt";
                var time = 0;
                using (StreamReader reader = new StreamReader(namefile))
                {
                    time = reader.Read();
                    reader.Close();
                }
                Console.WriteLine($"Timer = {time} seconds\n");
                time *= 1000;
                Timer timer = new Timer(tm, num, 0, time);
                Console.ReadLine();
            }
           
        }
        static void InfoProcessor(object obj)
        {
            var process = Process.GetCurrentProcess();
            Console.SetCursorPosition(0,25);
            Console.WriteLine($"|Id: {process.Id}");
            Console.SetCursorPosition(30, 25);
            Console.WriteLine("|");
            Console.WriteLine($"|Name: {process.ProcessName}");
            Console.SetCursorPosition(30, 26);
            Console.WriteLine("|");
            Console.WriteLine($"|VirtualMemory: {process.VirtualMemorySize64}");
            Console.SetCursorPosition(30, 27);
            Console.WriteLine("|");


        }

    }
    
}

