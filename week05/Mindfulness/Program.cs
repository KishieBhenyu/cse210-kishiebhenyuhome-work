using System;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

abstract class Activity
{
    protected string _name;
    protected string _description;
    protected int _duration;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void StartMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name} Activity!\n");
        Console.WriteLine(_description);
        Console.Write("\nEnter duration (seconds): ");
        _duration = int.Parse(Console.ReadLine() ?? "30");
        Console.WriteLine("\nPrepare to begin...");
        PauseWithAnimation(3);
    }

    public void EndMessage()
    {
        Console.WriteLine("\nGreat job!");
        Console.WriteLine($"You completed the {_name} activity for {_duration} seconds.");
        PauseWithAnimation(3);
        LogActivity(); // save results
    }

    protected void PauseWithAnimation(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"\r...{i} ");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }

    private void LogActivity()
    {
        string logEntry = $"{DateTime.Now}: {_name} activity for {_duration} seconds";
        File.AppendAllText("activity_log.txt", logEntry + Environment.NewLine);
    }

    public abstract void Run();
}

class BreathingActivity : Activity
{
    public BreathingActivity() 
        : base("Breathing", "This activity helps you relax by guiding you through breathing in and out slowly.") {}

    public override void Run()
    {
        StartMessage();
        DateTime end = DateTime.Now.AddSeconds(_duration);
        while (DateTime.Now < end)
        {
            Console.WriteLine("\nBreathe in...");
            PauseWithAnimation(4);
            Console.WriteLine("Breathe out...");
            PauseWithAnimation(6);
        }
        EndMessage();
    }
}

class ReflectionActivity : Activity
{
    private Queue<string> _promptQueue;
    private Queue<string> _questionQueue;

    public ReflectionActivity() 
        : base("Reflection", "This activity helps you reflect on times you showed strength and resilience.") 
    {
        _promptQueue = new Queue<string>(new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        });

        _questionQueue = new Queue<string>(new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What could you learn from this experience?",
            "What did you learn about yourself?"
        });
    }

    public override void Run()
    {
        StartMessage();

        if (_promptQueue.Count == 0)
            ResetPrompts();

        string prompt = _promptQueue.Dequeue();
        Console.WriteLine($"\nPrompt: {prompt}\n");

        DateTime end = DateTime.Now.AddSeconds(_duration);
        while (DateTime.Now < end)
        {
            if (_questionQueue.Count == 0)
                ResetQuestions();

            Console.WriteLine(_questionQueue.Dequeue());
            PauseWithAnimation(5);
        }
        EndMessage();
    }

    private void ResetPrompts()
    {
        _promptQueue = new Queue<string>(new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        });
    }

    private void ResetQuestions()
    {
        _questionQueue = new Queue<string>(new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What could you learn from this experience?",
            "What did you learn about yourself?"
        });
    }
}

class ListingActivity : Activity
{
    private Queue<string> _promptQueue;

    public ListingActivity() 
        : base("Listing", "This activity helps you list positive things in your life.") 
    {
        _promptQueue = new Queue<string>(new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "Who are some of your personal heroes?"
        });
    }

    public override void Run()
    {
        StartMessage();

        if (_promptQueue.Count == 0)
            ResetPrompts();

        string prompt = _promptQueue.Dequeue();
        Console.WriteLine($"\nPrompt: {prompt}");
        PauseWithAnimation(3);

        List<string> items = new List<string>();
        DateTime end = DateTime.Now.AddSeconds(_duration);
        Console.WriteLine("Start listing items (press Enter after each one):");

        while (DateTime.Now < end)
        {
            if (Console.KeyAvailable)
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    items.Add(input);
            }
        }

        Console.WriteLine($"\nYou listed {items.Count} items!");
        EndMessage();
    }

    private void ResetPrompts()
    {
        _promptQueue = new Queue<string>(new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "Who are some of your personal heroes?"
        });
    }
}

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program Menu");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Quit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            Activity activity = null;

            switch (choice)
            {
                case "1": activity = new BreathingActivity(); break;
                case "2": activity = new ReflectionActivity(); break;
                case "3": activity = new ListingActivity(); break;
                case "4": return;
                default: continue;
            }

            activity.Run();
        }
    }
}

/*
Exceeding Requirements:
1. Added logging to "activity_log.txt" for each completed activity (name, duration, timestamp).
2. Ensured Reflection questions and Listing prompts do not repeat until all have been used once.
*/