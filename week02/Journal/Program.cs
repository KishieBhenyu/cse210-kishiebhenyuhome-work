using System;

namespace JournalApp
{
    public class Entry
    {
        public string Date { get; set; }
        public string Prompt { get; set; }
        public string Response { get; set; }

        public Entry(string date, string prompt, string response)
        {
            Date = date;
            Prompt = prompt;
            Response = response;
        }

        public void DisplayEntry()
        {
            Console.WriteLine($"Date: {Date}");
            Console.WriteLine($"Prompt: {Prompt}");
            Console.WriteLine($"Response: {Response}");
            Console.WriteLine("-----------------------------------");
        }

        public string ToSaveFormat()
        {
            return $"{Date}|{Prompt}|{Response}";
        }

        public static Entry FromSaveFormat(string line)
        {
            string[] parts = line.Split('|');
            if (parts.Length == 3)
            {
                return new Entry(parts[0], parts[1], parts[2]);
            }
            return null;
        }
    }
}


namespace JournalApp
{
    public class Journal
    {
        private List<Entry> entries = new List<Entry>();

        public void AddEntry(Entry entry)
        {
            entries.Add(entry);
        }

        public void DisplayJournal()
        {
            if (entries.Count == 0)
            {
                Console.WriteLine("No journal entries yet.");
                return;
            }

            foreach (Entry entry in entries)
            {
                entry.DisplayEntry();
            }
        }

        public void SaveToFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (Entry entry in entries)
                {
                    writer.WriteLine(entry.ToSaveFormat());
                }
            }
            Console.WriteLine($"Journal saved to {filename}");
        }

        public void LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return;
            }

            entries.Clear();
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                Entry entry = Entry.FromSaveFormat(line);
                if (entry != null)
                {
                    entries.Add(entry);
                }
            }
            Console.WriteLine($"Journal loaded from {filename}");
        }
    }
}


namespace JournalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal();
            List<string> prompts = new List<string>
            {
                "Who was the most interesting person I interacted with today?",
                "What was the best part of my day?",
                "How did I see the hand of the Lord in my life today?",
                "What was the strongest emotion I felt today?",
                "If I had one thing I could do over today, what would it be?"
            };

            bool running = true;

            while (running)
            {
                Console.WriteLine("\nJournal Menu");
                Console.WriteLine("1. Write a new entry");
                Console.WriteLine("2. Display the journal");
                Console.WriteLine("3. Save the journal to a file");
                Console.WriteLine("4. Load the journal from a file");
                Console.WriteLine("5. Quit");
                Console.Write("Choose an option: ");
                
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Random rand = new Random();
                        string prompt = prompts[rand.Next(prompts.Count)];
                        Console.WriteLine($"\nPrompt: {prompt}");
                        Console.Write("Your response: ");
                        string response = Console.ReadLine();
                        string date = DateTime.Now.ToShortDateString();
                        Entry newEntry = new Entry(date, prompt, response);
                        journal.AddEntry(newEntry);
                        break;

                    case "2":
                        Console.WriteLine("\n--- Journal Entries ---");
                        journal.DisplayJournal();
                        break;

                    case "3":
                        Console.Write("Enter filename to save: ");
                        string saveFile = Console.ReadLine();
                        journal.SaveToFile(saveFile);
                        break;

                    case "4":
                        Console.Write("Enter filename to load: ");
                        string loadFile = Console.ReadLine();
                        journal.LoadFromFile(loadFile);
                        break;

                    case "5":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }
        }
    }
}