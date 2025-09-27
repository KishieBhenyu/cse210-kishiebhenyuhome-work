using System;
using System.Collections.Generic;

public class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthInSeconds { get; set; }
    private List<Comment> comments = new List<Comment>();

    public Video(string title, string author, int lengthInSeconds)
    {
        Title = title;
        Author = author;
        LengthInSeconds = lengthInSeconds;
    }

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return comments.Count;
    }

    public void DisplayVideoDetails()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {LengthInSeconds} seconds");
        Console.WriteLine($"Number of comments: {GetNumberOfComments()}");

        Console.WriteLine("Comments:");
        foreach (var comment in comments)
        {
            Console.WriteLine($" - {comment.CommenterName}: {comment.Text}");
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create videos
        Video video1 = new Video("Learning C# Basics", "TechGuru", 600);
        video1.AddComment(new Comment("Alice", "Great explanation!"));
        video1.AddComment(new Comment("Bob", "Very helpful, thanks."));
        video1.AddComment(new Comment("Charlie", "Looking forward to more tutorials."));

        Video video2 = new Video("Top 10 Travel Destinations", "Wanderlust", 900);
        video2.AddComment(new Comment("David", "I want to visit all of them!"));
        video2.AddComment(new Comment("Eva", "Amazing video quality."));
        video2.AddComment(new Comment("Frank", "You forgot Bali!"));

        Video video3 = new Video("Healthy Recipes", "ChefMaster", 750);
        video3.AddComment(new Comment("Grace", "Delicious and easy to make!"));
        video3.AddComment(new Comment("Henry", "Tried this yesterday, turned out great."));
        video3.AddComment(new Comment("Ivy", "Please share more vegetarian options."));

        // Store videos in a list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Display all videos and their comments
        foreach (var video in videos)
        {
            video.DisplayVideoDetails();
        }
    }
}