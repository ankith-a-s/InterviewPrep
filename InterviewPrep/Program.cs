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

            // Parameterized Thread
            // - Type safety is not provided

            // This can also be written as 
            // Thread workerThread = new Thread(Number.PrintNumbers);
            Thread T1 = new Thread(new ParameterizedThreadStart(Number.PrintNumbers));
            T1.Start(15);

            // Pass parameters to thread in type safe manner
            NumberTypeSafe numberTypeSafe = new NumberTypeSafe(16);
            Thread T2 = new Thread(numberTypeSafe.PrintNumbers);
            T2.Start();

            // Retrieving data from thread using callback method
            SumOfNumbersCallback sumOfNumbersCallback = new SumOfNumbersCallback(PrintSum);
            NumberCallback numberCallback = new NumberCallback(5, sumOfNumbersCallback);
            Thread T3 = new Thread(numberCallback.PrintSumOfNumbers);
            T3.Start();

            // Thread.Join and Thread.IsAlive functions


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
