using System;
using System.Security.Cryptography.X509Certificates;

public class Program
{   
    static void Main(string[] args)
    {
       Console.WriteLine("Welcome to the Journal Program!");
       Journal journal= new Journal();
       while (true)
       {
        DisplayMenu(); 
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            journal.AddEntry();
        }
        else if (choice == "2")
        {
            journal.Display();
        }
        else if (choice == "3")
        {
            Console.Write("Enter filename to load: ");
            string filename = Console.ReadLine();
            journal.Read(filename);
        }
        else if (choice == "4")
        {
            Console.Write("Enter filename to load: ");
            string filename = Console.ReadLine();
            journal.Write(filename);
        }
        else if (choice == "5")
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid option. Please try again.");
        }
       }
    }
    static void DisplayMenu()
    {
        Console.WriteLine("1. Write");
        Console.WriteLine("2. Display");
        Console.WriteLine("3. Load");
        Console.WriteLine("4. Save");
        Console.WriteLine("5. Quit");
    }
}

public class Entry
{
    public DateTime date {get; set;}
    public string prompt {get; set;}
    public string response {get; set;}

    public void Display()
    {
        Console.WriteLine($"Date: {date}");
        Console.WriteLine(prompt);
        Console.WriteLine(response);
    }
    
}
public class Journal
{
    public string filename {get; set;}
    public List<Entry> _entries = new List<Entry>();

    public void Display()
    {
        foreach (var entry in _entries)
        {
            entry.Display();
        }
    }

    public void Read(string filename)
    {
        _entries.Clear();
        if (File.Exists(filename))
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                string[] parts = line.Split("|");
                if (parts.Length == 3)
                {
                    Entry entry = new Entry
                    {
                        date = DateTime.Parse(parts[0]),
                        prompt = parts[1],
                        response = parts[2]

                    };
                    _entries.Add(entry);
                }
            }
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }

    public void Write(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var entry in _entries)
            {
                writer.WriteLine($"{entry.date:yyyy-MM-dd HH:mm:ss}|{entry.prompt}|{entry.response}");
            }
        }
    }

    public void AddEntry()
    {
        string[]  prompts = new string[]
        {
            "What was the best part of my day today?",
            "Where have I seen God's hands in my life today?",
            "What was the strongest emotion I felt today?",
            "Who did I talk to today?",
            "If I could do one thing over today, what would it be?",
            "What was the weather like today?",
        };
        Random rand = new Random();
        string selectPrompt = prompts[rand.Next(prompts.Length)];

        Console.WriteLine($"Prompt: {selectPrompt}");
        string response = Console.ReadLine();

        Entry newEntry = new Entry
        {
            date = DateTime.Now,
            prompt = selectPrompt,
            response = response
        };
        _entries.Add(newEntry);
    }
   
}
