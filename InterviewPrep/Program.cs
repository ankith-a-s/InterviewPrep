using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace InterviewPrep
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating a Thread
            CreateNewThread();
            Thread T1 = new Thread(new ParameterizedThreadStart(Number.PrintNumbers));
            T1.Start(15);
            Console.ReadKey();
        }

        public static void DoTimeConsumingWork() {
            Thread.Sleep(5000);
            Console.WriteLine("Time consuming work ends");
            
        }

        public static void CreateNewThread() {
            Thread workerThread = new Thread(new ThreadStart(DoTimeConsumingWork));
            workerThread.Start();
            Console.WriteLine("New thread is created");
        }
    }

    class Number {
        public static void PrintNumbers(object target) {
            int number = 0;
            if (int.TryParse(target.ToString(), out number))
            {
                for (int i = 1; i <= number; i++)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}
