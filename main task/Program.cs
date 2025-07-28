

public class Account
{
    public string Name { get; set; }
    public double Balance { get; set; }

    public Account(string name = "Unnamed Account", double balance = 0.0)
    {
        Name = name;
        Balance = balance;
    }

    public virtual bool Deposit(double amount)
    {
        if (amount < 0)
            return false;
        Balance += amount;
        return true;
    }

    public virtual bool Withdraw(double amount)
    {
        if (Balance - amount >= 0)
        {
            Balance -= amount;
            return true;
        }
        return false;
    }

    public static double operator +(Account v1, Account v2)
    {
        return v1.Balance + v2.Balance;
    }
}

public class SavingsAccount : Account
{
    public double InterestRate { get; set; }

    public SavingsAccount(string name = "Unnamed Savings", double balance = 0.0, double interestRate = 0.0)
        : base(name, balance)
    {
        InterestRate = interestRate;
    }
}


public class CheckingAccount : Account
{
    private static double fee = 1.5;

    public CheckingAccount(string name = "Unnamed Checking", double balance = 0.0)
        : base(name, balance)
    {
    }

    public override bool Withdraw(double amount)
    {
        return base.Withdraw(amount + fee);
    }
}


public class TrustAccount : Account
{
    public double InterestRate { get; set; }
    private int withdrawalCount = 0;
    private const int MaxWithdrawals = 3;
    private const double MaxWithdrawPercent = 0.2;

    public TrustAccount(string name = "Unnamed Trust", double balance = 0.0, double interestRate = 0.0)
        : base(name, balance)
    {
        InterestRate = interestRate;
    }

    public override bool Deposit(double amount)
    {
        if (amount >= 5000)
            Balance += 50; // Bonus
        return base.Deposit(amount);
    }

    public override bool Withdraw(double amount)
    {
        if (withdrawalCount >= MaxWithdrawals || amount > Balance * MaxWithdrawPercent)
            return false;

        withdrawalCount++;
        return base.Withdraw(amount);
    }
}



public static class AccountUtil
{
    public static void DepositAll(List<Account> accounts, double amount)
    {
        Console.WriteLine("Deposit");
        foreach (var acc in accounts)
        {
            if (acc.Deposit(amount))
                Console.WriteLine($"Deposit {amount} to {acc.Name}");
            else
                Console.WriteLine($"Failed Deposit {amount} to {acc.Name}");
        }
    }

    public static void WithdrawAll(List<Account> accounts, double amount)
    {
        Console.WriteLine(" Withdraw");
        foreach (var acc in accounts)
        {
            if (acc.Withdraw(amount))
                Console.WriteLine($"Withdraw {amount} from {acc.Name}");
            else
                Console.WriteLine($"Failed Withdraw of {amount} from {acc.Name}");
        }
    }
}


class Program
{
    static void Main()
    {
        var accounts = new List<Account>
        {
            new Account("Larry", 1500),
            new Account("Moe", 2000),
        };

        Console.WriteLine("Sum of account balances: " + (accounts[0] + accounts[1]));

        var savAccounts = new List<SavingsAccount>
        {
            new SavingsAccount("Superman", 1000, 3.5),
            new SavingsAccount("Batman", 2000, 4.0)
        };

        var checAccounts = new List<CheckingAccount>
        {
            new CheckingAccount("Larry2", 3000),
            new CheckingAccount("Moe2", 2000)
        };

        var trustAccounts = new List<TrustAccount>
        {
            new TrustAccount("Wonderwoman", 10000, 5.0),
            new TrustAccount("Ironman", 6000, 3.0)
        };

        AccountUtil.DepositAll(savAccounts.Cast<Account>().ToList(), 1000);
        AccountUtil.WithdrawAll(savAccounts.Cast<Account>().ToList(), 500);

        AccountUtil.DepositAll(checAccounts.Cast<Account>().ToList(), 1000);
        AccountUtil.WithdrawAll(checAccounts.Cast<Account>().ToList(), 500);

        AccountUtil.DepositAll(trustAccounts.Cast<Account>().ToList(), 5000);
        AccountUtil.WithdrawAll(trustAccounts.Cast<Account>().ToList(), 1000);
        AccountUtil.WithdrawAll(trustAccounts.Cast<Account>().ToList(), 1000);
        AccountUtil.WithdrawAll(trustAccounts.Cast<Account>().ToList(), 1000);

        Console.WriteLine();
    }
}
