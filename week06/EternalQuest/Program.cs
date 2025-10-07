using System;
using System.Collections.Generic;
using System.IO;

public abstract class Goal
{
    protected string _shortName;
    protected string _description;
    protected int _points;

    public Goal(string name, string desc, int points)
    {
        _shortName = name;
        _description = desc;
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
        if (!_isComplete)
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
            return _points + _bonus;
        return _points;
    }
    public override bool IsComplete() => _amountCompleted >= _target;
    public override string GetDetailsString()
    {
        return $"[{(IsComplete() ? "X" : " ")}] {_shortName} ({_description}) -- Completed {_amountCompleted}/{_target}";
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
            Console.WriteLine("1. Create Goal\n2. List Goals\n3. Record Event\n4. Display Score\n5. Save\n6. Load\n7. Quit");
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
            }
        }
    }
    public void CreateGoal()
    {
        Console.WriteLine("Goal type (1 = Simple, 2 = Eternal, 3 = Checklist): ");
        int t = int.Parse(Console.ReadLine());
        Console.Write("Name: "); string name = Console.ReadLine();
        Console.Write("Description: "); string desc = Console.ReadLine();
        Console.Write("Points: "); int pts = int.Parse(Console.ReadLine());
        if (t == 1) _goals.Add(new SimpleGoal(name, desc, pts));
        else if (t == 2) _goals.Add(new EternalGoal(name, desc, pts));
        else if (t == 3)
        {
            Console.Write("Target: "); int tgt = int.Parse(Console.ReadLine());
            Console.Write("Bonus: "); int bon = int.Parse(Console.ReadLine());
            _goals.Add(new ChecklistGoal(name, desc, pts, tgt, bon));
        }
    }
    public void ListGoalDetails()
    {
        for (int i = 0; i < _goals.Count; i++)
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
    }
    public void RecordEvent()
    {
        ListGoalDetails();
        Console.Write("Enter goal to record (number): ");
        int idx = int.Parse(Console.ReadLine()) - 1;
        int earned = _goals[idx].RecordEvent();
        _score += earned;
        Console.WriteLine($"Earned {earned} points.");
    }
    public void DisplayPlayerInfo()
    {
        Console.WriteLine($"Score: {_score}");
    }
    public void SaveGoals()
    {
        Console.Write("File name: ");
        string fn = Console.ReadLine();
        using (StreamWriter writer = new StreamWriter(fn))
        {
            writer.WriteLine(_score);
            foreach (var goal in _goals)
                writer.WriteLine(goal.GetStringRepresentation());
        }
        Console.WriteLine("Saved.");
    }
    public void LoadGoals()
    {
        Console.Write("File name: ");
        string fn = Console.ReadLine();
        using (StreamReader reader = new StreamReader(fn))
        {
            _score = int.Parse(reader.ReadLine());
            _goals.Clear();
            while (!reader.EndOfStream)
            {
                string[] parts = reader.ReadLine().Split('|');
                if (parts[0] == "SimpleGoal")
                    _goals.Add(new SimpleGoal(parts[1], parts[2], int.Parse(parts[3])));
                else if (parts[0] == "EternalGoal")
                    _goals.Add(new EternalGoal(parts[1], parts[2], int.Parse(parts[3])));
                else if (parts[0] == "ChecklistGoal")
                    _goals.Add(new ChecklistGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[5]), int.Parse(parts[6])));
            }
        }
        Console.WriteLine("Loaded.");
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