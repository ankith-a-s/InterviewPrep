using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace InterviewPrep
{
    public delegate void SumOfNumbersCallback(int sumOfNumbers);

    class Program
    {
        static void Main(string[] args)
        {
            // Creating a Thread
            CreateNewThread();
            Console.WriteLine("------------------------------------");

            // Parameterized Thread
            // - Type safety is not provided

            // This can also be written as 
            // Thread workerThread = new Thread(Number.PrintNumbers);
            Thread T1 = new Thread(new ParameterizedThreadStart(Number.PrintNumbers));
            T1.Start(15);
            Console.WriteLine("------------------------------------");

            // Pass parameters to thread in type safe manner
            NumberTypeSafe numberTypeSafe = new NumberTypeSafe(16);
            Thread T2 = new Thread(numberTypeSafe.PrintNumbers);
            T2.Start();
            Console.WriteLine("------------------------------------");

            // Retrieving data from thread using callback method
            SumOfNumbersCallback sumOfNumbersCallback = new SumOfNumbersCallback(PrintSum);
            NumberCallback numberCallback = new NumberCallback(5, sumOfNumbersCallback);
            Thread T3 = new Thread(numberCallback.PrintSumOfNumbers);
            T3.Start();
            Console.WriteLine("------------------------------------");

            // Thread.Join and Thread.IsAlive functions
            Console.WriteLine("Thread T4 started");
            Thread T4 = new Thread(NumberWithoutParam.PrintNumbers);
            T4.Start();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Thread T4 completed");
            Console.WriteLine("is Thread T4 Alive" + T4.IsAlive);
            T4.Join();
            Thread T5 = new Thread(NumberWithoutParam.PrintNumbers);
            T5.Start();
            Console.WriteLine("Thread T5 completed");
            Console.WriteLine("------------------------------------");

            // Protecting shared resources from concurrent access
            // we've 2 options
            // 1. Interlocked.increment(ref value)
            // 2. Locking mechanism 
            Thread T6 = new Thread(TestThreadingShared.AddOneMillion);
            Thread T7 = new Thread(TestThreadingShared.AddOneMillion);
            Thread T8 = new Thread(TestThreadingShared.AddOneMillion);
            T6.Start();
            T7.Start();
            T8.Start();
            T6.Join();
            T7.Join();
            T8.Join();
            Console.WriteLine("Final Total" + TestThreadingShared.total);
            Console.WriteLine("------------------------------------");

            // It's even possible to lock the thread using Monitor.Lock 
            Thread T9 = new Thread(TestThreadingMonitor.AddOneMillion);
            T9.Start();
            Console.WriteLine("Final Total" + TestThreadingMonitor.total);
            Console.WriteLine("------------------------------------");

            // Deadlock
            Deadlock deadlock = new Deadlock();

            Console.ReadKey();
        }

        public static void PrintSum(int sumOfNumbers) {
            Console.WriteLine("Sum of numbers = " + sumOfNumbers);
        }

        public static void DoTimeConsumingWork() {
            Thread.Sleep(5000);
            Console.WriteLine("Time consuming work ends");
            
        }

        public static void CreateNewThread() {
            // This can also be written as 
            // Thread workerThread = new Thread(DoTimeConsumingWork);
            Thread workerThread = new Thread(new ThreadStart(DoTimeConsumingWork));
            workerThread.Start();
            Console.WriteLine("New thread is created");
        }
    }

    class TestThreadingMonitor
    {

        public static int total = 0;
        static object _lock = new object();

        public static void AddOneMillion()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Monitor.Enter(_lock);
                try {
                    total += 1;
                }
                finally {
                    Monitor.Exit(_lock);
                }
            }
        }
    }

    class TestThreadingShared {

        public static int total = 0;
        static object _lock = new object();

        public static void AddOneMillion()
        {
            for (int i = 0; i < 1000000; i++) {
                lock (_lock) {
                    total += 1;
                }
            }
        }
    }

    class NumberWithoutParam
    {
        public static void PrintNumbers()
        {
            Thread.Sleep(3000);
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


    class NumberTypeSafe
    {
        private int _target;
        public NumberTypeSafe(int target) {
            this._target = target;
        }
        public void PrintNumbers()
        {
            for (int i = 1; i <= this._target; i++)
            {
                Console.WriteLine(i);
            }
        }
    }

    class NumberCallback
    {
        private int _target;
        SumOfNumbersCallback _callback;
        public NumberCallback(int target, SumOfNumbersCallback callback)
        {
            this._target = target;
            this._callback = callback;
        }
        public void PrintSumOfNumbers()
        {
            int sum = 0;
            for (int i = 1; i <= this._target; i++)
            {
                sum += i;
            }
            this._callback(sum);
        }
    }
}
