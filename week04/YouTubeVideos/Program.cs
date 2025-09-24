using System;
using System.Collections.Generic;

public class Video
{
    public string Title { get; set; }
    public int Duration { get; set; } 
    public int Views { get; set; }
    public int Likes { get; set; }
    public string Url { get; set; }

    public void Play()
    {
        Views++;
        Console.WriteLine($"Playing video: {Title}");
    }

    public void Like()
    {
        Likes++;
        Console.WriteLine($"You liked {Title}. Total likes: {Likes}");
    }

    public void Share()
    {
        Console.WriteLine($"Sharing video: {Title} ({Url})");
    }
}

public class Channel
{
    public string Name { get; set; }
    public int Subscribers { get; set; }
    public List<Video> Videos { get; set; } = new List<Video>();

    public void AddVideo(Video video)
    {
        if (!Videos.Contains(video))
        {
            Videos.Add(video);
            Console.WriteLine($"Added video '{video.Title}' to channel '{Name}'");
        }
    }

    public void RemoveVideo(Video video)
    {
        if (Videos.Contains(video))
        {
            Videos.Remove(video);
            Console.WriteLine($"Removed video '{video.Title}' from channel '{Name}'");
        }
    }

    public void ListVideos()
    {
        Console.WriteLine($"Videos in {Name}:");
        foreach (var video in Videos)
        {
            Console.WriteLine($"- {video.Title}");
        }
    }
}

public class Playlist
{
    public string Name { get; set; }
    public List<Video> Videos { get; set; } = new List<Video>();

    public void AddVideo(Video video)
    {
        if (!Videos.Contains(video))
        {
            Videos.Add(video);
            Console.WriteLine($"Added video '{video.Title}' to playlist '{Name}'");
        }
    }

    public void RemoveVideo(Video video)
    {
        if (Videos.Contains(video))
        {
            Videos.Remove(video);
            Console.WriteLine($"Removed video '{video.Title}' from playlist '{Name}'");
        }
    }

    public void PlayAll()
    {
        Console.WriteLine($"Playing playlist: {Name}");
        foreach (var video in Videos)
        {
            video.Play();
        }
    }
}

public class User
{
    public string Username { get; set; }
    public List<Channel> SubscribedChannels { get; set; } = new List<Channel>();
    public List<Video> LikedVideos { get; set; } = new List<Video>();

    public void Subscribe(Channel channel)
    {
        if (!SubscribedChannels.Contains(channel))
        {
            SubscribedChannels.Add(channel);
            channel.Subscribers++;
            Console.WriteLine($"{Username} subscribed to {channel.Name}");
        }
    }

    public void Unsubscribe(Channel channel)
    {
        if (SubscribedChannels.Contains(channel))
        {
            SubscribedChannels.Remove(channel);
            channel.Subscribers = Math.Max(0, channel.Subscribers - 1);
            Console.WriteLine($"{Username} unsubscribed from {channel.Name}");
        }
    }

    public void LikeVideo(Video video)
    {
        if (!LikedVideos.Contains(video))
        {
            video.Like();
            LikedVideos.Add(video);
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
       
        Video video1 = new Video { Title = "C# Tutorial", Url = "http://example.com", Duration = 10 };
        Video video2 = new Video { Title = "OOP Concepts", Url = "http://example.com", Duration = 15 };

        
        Channel channel1 = new Channel { Name = "Code Academy" };
        channel1.AddVideo(video1);
        channel1.AddVideo(video2);
        channel1.ListVideos();

        
        User user1 = new User { Username = "Kishie" };
        user1.Subscribe(channel1);
        user1.LikeVideo(video1);

        
        Playlist playlist1 = new Playlist { Name = "My Favorites" };
        playlist1.AddVideo(video1);
        playlist1.AddVideo(video2);
        playlist1.PlayAll();

        Console.WriteLine("\nSimulation complete.");
    }
}