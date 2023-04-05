using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewPrep
{
    // Deadlock happens when 
    // Thread 1 has held lock on Resource 1 but wants to access Resource 2
    // Thread 2 has held lock on Resource 2 but wants to access Resource 1
    class Deadlock
    {
        public Deadlock() {
            Console.WriteLine("Deadlock Class Started");

            Account accountA = new Account(101, 5000);
            Account accountB = new Account(102, 5000);

            AccountManager accountManagerA = new AccountManager(accountA, accountB, 1000);
            Thread T1 = new Thread(accountManagerA.Transfer);
            T1.Start();
            AccountManager accountManagerB = new AccountManager(accountB, accountA, 2000);
            Thread T2 = new Thread(accountManagerB.Transfer);
            T2.Start();
            T1.Join();
            T2.Join();
            Console.WriteLine("Main Completed");
        }
    }

    class Account {
        double _balance; int _id;
        public Account(int id, double balance) {
            this._id = id;
            this._balance = balance;
        }

        public void Withdraw(double amount) {
            this._balance -= amount;
        }

        public void Deposit(double amount) {
            this._balance += amount;
        }
    }

    class AccountManager {
        Account _fromAccount; Account _toAccount; double _amountToTransfer;
        public AccountManager(Account fromAccount, Account toAccount, double amountToTransfer) {
            this._fromAccount = fromAccount;
            this._toAccount = toAccount;
            this._amountToTransfer = amountToTransfer;
        }

        public void Transfer() {
            lock (_fromAccount) {
                Thread.Sleep(1000);
                lock (_toAccount) {
                    _fromAccount.Withdraw(_amountToTransfer);
                    _toAccount.Deposit(_amountToTransfer);
                }
            }
        }
    }
}
