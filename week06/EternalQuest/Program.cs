using System;


using System;
using System.IO;
using System.Collections.Generic;


public abstract class Goal
{
    protected string _shortName;
    protected string _description;
    protected int _points;

    public Goal(string name, string description, int points)
    {
        _shortName = name;
        _description = description;
        _points = points;
    }

    public abstract int RecordEvent();
    public abstract bool IsComplete();
    public virtual string GetDetailsString()
    {
        return $"[{(IsComplete() ? "X" : " ")}] {_shortName} ({_description})";
    }
    public abstract string GetStringRepresentation();
}


public class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string desc, int points)
        : base(name, desc, points)
    {
        _isComplete = false;
    }
    public override int RecordEvent()
    {
        _isComplete = true;
        return _points;
    }
    public override bool IsComplete() => _isComplete;

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal|{_shortName}|{_description}|{_points}|{_isComplete}";
    }
}


public class EternalGoal : Goal
{
    public EternalGoal(string name, string desc, int points)
        : base(name, desc, points) { }

    public override int RecordEvent() => _points;
    public override bool IsComplete() => false;

    public override string GetStringRepresentation()
    {
        return $"EternalGoal|{_shortName}|{_description}|{_points}";
    }
}


public class ChecklistGoal : Goal
{
    private int _amountCompleted;
    private int _target;
    private int _bonus;

    public ChecklistGoal(string name, string desc, int points, int target, int bonus)
        : base(name, desc, points)
    {
        _amountCompleted = 0;
        _target = target;
        _bonus = bonus;
    }
    public override int RecordEvent()
    {
        _amountCompleted++;
        if (_amountCompleted == _target)
        {
            return _points + _bonus;
        }
        return _points;
    }
    public override bool IsComplete() => _amountCompleted >= _target;

    public override string GetDetailsString()
    {
        return $"[{(IsComplete() ? "X" : " ")}] {_shortName} ({_description}) -- Completed: {_amountCompleted}/{_target}";
    }
    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal|{_shortName}|{_description}|{_points}|{_amountCompleted}|{_target}|{_bonus}";
    }
}


public class GoalManager
{
    private List<Goal> _goals = new List<Goal>();
    private int _score = 0;

    public void Start()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n1. Create New Goal\n2. List Goals\n3. Record Event\n4. Display Score\n5. Save\n6. Load\n7. Quit");
            Console.Write("Select Choice: ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1": CreateGoal(); break;
                case "2": ListGoalDetails(); break;
                case "3": RecordEvent(); break;
                case "4": DisplayPlayerInfo(); break;
                case "5": SaveGoals(); break;
                case "6": LoadGoals(); break;
                case "7": running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }

    public void DisplayPlayerInfo()
    {
        Console.WriteLine($"\nScore: {_score}");
    }

    public void ListGoalDetails()
    {
        Console.WriteLine("\nYour Goals:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
    }

    public void CreateGoal()
    {
        Console.WriteLine("\nGoal type (1=Simple, 2=Eternal, 3=Checklist): ");
        int t = int.Parse(Console.ReadLine());
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Description: ");
        string desc = Console.ReadLine();
        Console.Write("Points: ");
        int pts = int.Parse(Console.ReadLine());

        if (t == 1) _goals.Add(new SimpleGoal(name, desc, pts));
        else if (t == 2) _goals.Add(new EternalGoal(name, desc, pts));
        else if (t == 3)
        {
            Console.Write("Target times to complete: ");
            int tgt = int.Parse(Console.ReadLine());
            Console.Write("Bonus points: ");
            int bon = int.Parse(Console.ReadLine());
            _goals.Add(new ChecklistGoal(name, desc, pts, tgt, bon));
        }
        else
        {
            Console.WriteLine("Invalid type.");
        }
    }

    public void RecordEvent()
    {
        ListGoalDetails();
        Console.Write("Enter goal number to record event for: ");
        int idx = int.Parse(Console.ReadLine()) - 1;
        if (idx < 0 || idx >= _goals.Count)
        {
            Console.WriteLine("Invalid goal number.");
            return;
        }
        int earned = _goals[idx].RecordEvent();
        _score += earned;
        Console.WriteLine($"Event recorded! Earned {earned} points.");
    }

    public void SaveGoals()
    {
        Console.Write("File name to save: ");
        string fn = Console.ReadLine();
        using (StreamWriter writer = new StreamWriter(fn))
        {
            writer.WriteLine(_score);
            foreach (Goal g in _goals)
                writer.WriteLine(g.GetStringRepresentation());
        }
        Console.WriteLine("Goals saved to file.");
    }

    public void LoadGoals()
    {
        Console.Write("File name to load: ");
        string fn = Console.ReadLine();
        try
        {
            string[] lines = File.ReadAllLines(fn);
            _score = int.Parse(lines[0]);
            _goals.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                if (parts[0] == "SimpleGoal")
                    _goals.Add(new SimpleGoal(parts[1], parts[2], int.Parse(parts[3])));
                else if (parts[0] == "EternalGoal")
                    _goals.Add(new EternalGoal(parts[1], parts[2], int.Parse(parts[3])));
                else if (parts[0] == "ChecklistGoal")
                    _goals.Add(new ChecklistGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[5]), int.Parse(parts[6])));
            }
            Console.WriteLine("Goals loaded from file.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading: {e.Message}");
        }
    }
}


class Program
{
 

    static void Main()
    {
        var manager = new GoalManager();
        manager.Start();
    }
}