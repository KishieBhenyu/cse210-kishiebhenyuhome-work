using System;
using System.Collections.Generic;

// Base Activity class with encapsulation and abstraction
public abstract class Activity
{
    private string _date;
    private int _minutes;

    public Activity(string date, int minutes)
    {
        _date = date;
        _minutes = minutes;
    }

    public string GetDate() => _date;
    public int GetMinutes() => _minutes;

    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    // Shared summary method that uses virtual methods
    public virtual string GetSummary()
    {
        return $"{_date} {GetType().Name} ({_minutes} min): Distance {GetDistance():F2} miles, Speed {GetSpeed():F2} mph, Pace: {GetPace():F2} min per mile";
    }
}

// Running activity inherits from Activity
public class Running : Activity
{
    private double _distance; // miles

    public Running(string date, int minutes, double distance)
        : base(date, minutes)
    {
        _distance = distance;
    }

    public override double GetDistance() => _distance;
    public override double GetSpeed() => (_distance / GetMinutes()) * 60;
    public override double GetPace() => GetMinutes() / _distance;
}

// Cycling activity inherits from Activity
public class Cycling : Activity
{
    private double _speed; // mph

    public Cycling(string date, int minutes, double speed)
        : base(date, minutes)
    {
        _speed = speed;
    }

    public override double GetDistance() => _speed * GetMinutes() / 60.0;
    public override double GetSpeed() => _speed;
    public override double GetPace() => (_speed > 0) ? GetMinutes() / GetDistance() : 0;
}

// Swimming activity inherits from Activity
public class Swimming : Activity
{
    private int _laps; // count

    public Swimming(string date, int minutes, int laps)
        : base(date, minutes)
    {
        _laps = laps;
    }

    public override double GetDistance()
    {
        // Each lap is 50 meters; convert meters to miles
        return (_laps * 50.0) / 1609.34;
    }

    public override double GetSpeed()
    {
        double distance = GetDistance();
        return (distance / GetMinutes()) * 60.0;
    }

    public override double GetPace()
    {
        double distance = GetDistance();
        return (distance > 0) ? GetMinutes() / distance : 0;
    }
}

// Main Program
class Program
{
    static void Main()
    {
        // Create activities of all three types
        var activities = new List<Activity>
        {
            new Running("03 Nov 2022", 30, 3.0),
            new Cycling("03 Nov 2022", 30, 6.0),
            new Swimming("03 Nov 2022", 30, 20) // 20 laps in 30 minutes
        };

        // Print summaries using polymorphism
        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}