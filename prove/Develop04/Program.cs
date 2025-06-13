using System;

// Exceeds requirements by:
// 1. Logging activities to activityLog with timestamps and durations.
// 2. Displaying a summary of all activities and total count when quitting.
class Program
{
    private List<Activity> activities = new List<Activity>();
    private List<string> activityLog = new List<string>();

    public Program()
    {
        activities.Add(new BreathingActivity("Breathing Activity", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.", 0, 5, 5));
        activities.Add(new ReflectionActivity("Reflection Activity", "This activity will help you relfect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.", 0, new List<string> { "Think of a time when you did something really difficult.", "Think of a time when you accomplished a goal.", "Think of a time when you faced one of your fears." }, new List<string> { "How did you feel when it was complete?", "What was your favorite thing about this experience?", "How did you feel about yourself afterwords?", "Did your perception change in any way?" }));
        activities.Add(new ListingActivity("Listing Activity", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.", 0, new List<string> { "When have you felt the Holy Ghost this month?", "Where have you seen the Lord's hands in your life today?", "What are the good things that happened this week?", "What made you happy this month?", "What are you looking forward to this year?" }, new List<string>()));
    }
    public void DisplayMenu()
    {
        Console.WriteLine("Menu Options:");
        Console.WriteLine("1. Start Breathing Activity");
        Console.WriteLine("2. Start Reflecting Activity");
        Console.WriteLine("3. Start Listing Activity");
        Console.WriteLine("4. Quit");
        Console.WriteLine();
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            DisplayMenu();
            Console.Write("Select a choice from the menu: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("How long, in seconds, would you like for your session?: ");
                string userInput = Console.ReadLine();
                int duration;
                bool isValid = int.TryParse(userInput, out duration) && duration > 0;

                if (isValid)
                {
                    activities[0].SetDuration(duration);
                    activities[0].RunActivity();
                    LogActivity(activities[0].GetName(), duration);
                }
                else
                {
                    Console.WriteLine("Invalid duration. Please enter a positive number.");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else if (choice == "2")
            {
                Console.Write("How long, in seconds, would you like for your session?: ");
                string userInput = Console.ReadLine();
                int duration;
                bool isValid = int.TryParse(userInput, out duration) && duration > 0;
                if (isValid)
                {
                    activities[1].SetDuration(duration);
                    activities[1].RunActivity();
                    LogActivity(activities[1].GetName(), duration);
                }
                else
                {
                    Console.WriteLine("Invalid duration. Please enter a positive number.");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else if (choice == "3")
            {
                Console.Write("How long, in seconds, would you like for your session?: ");
                string userInput = Console.ReadLine();
                int duration;
                bool isValid = int.TryParse(userInput, out duration) && duration > 0;
                if (isValid)
                {
                    activities[2].SetDuration(duration);
                    activities[2].RunActivity();
                    LogActivity(activities[2].GetName(), duration);
                }
                else
                {
                    Console.WriteLine("Invalid duration. Please enter a positive number.");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else if (choice == "4")
            {
                Console.Clear();
                Console.WriteLine("Activity Log Summary:");
                if (activityLog.Count == 0)
                {
                    Console.WriteLine("No activities completed this session.");
                }
                else
                {
                    foreach (string entry in activityLog)
                    {
                        Console.WriteLine(entry);
                    }
                    Console.WriteLine($"\nTotal activities completed: {activityLog.Count}");
                }
                Console.WriteLine("Thank you for using the Mindfulness Program. Goodbye!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please pick one of the four menu options.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

        }

    }

    private void LogActivity(string name, int duration)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logEntry = $"{date}: {name} for {duration} seconds";
        activityLog.Add(logEntry);
    }
    static void Main(string[] args)
    {
        Program program = new Program();
        program.Run();
    }
}

class Activity
{
    private string _name;
    private string _description;
    protected int _duration;


    public Activity(string name, string description, int duration)
    {
        _name = name;
        _description = description;
        _duration = duration;
    }

    public void SetDuration(int duration)
    {
        _duration = duration;
    }

    public void StartActivity()
    {
        Console.WriteLine();
        Console.WriteLine($"Welcome to the {GetName()}.");
        Console.WriteLine($"{GetDescription()}");
        Console.WriteLine();
        Console.WriteLine($"You have set this activity for {GetDuration()} seconds.");
        Console.WriteLine();
        Thread.Sleep(5000);
        Console.WriteLine("Prepare to begin...");
        ShowSpinner(3);
        Console.WriteLine();
    }
    public void EndActivity()
    {
        Console.WriteLine();
        Console.WriteLine("Good Job!");
        Console.WriteLine();
        Console.WriteLine($"You have completed another {GetDuration()} seconds of the {GetName()}.");
        ShowSpinner(5);
    }
    public virtual void RunActivity()
    {
        StartActivity();
        // Derived classes override to add activity-specific logic here
        EndActivity();
    }
    public void ShowSpinner(int seconds)
    {
        char[] spinner = { '|', '/', '-', '\\' };
        int interactions = seconds * 4;

        for (int i = 0; i < interactions; i++)
        {
            char currentSpinner = spinner[i % spinner.Length];
            Console.Write(currentSpinner);

            Thread.Sleep(250);
            Console.Write("\b");
        }

        Console.Write(" ");
        Console.Write("\b");
        Console.WriteLine();
    }
    public void ShowCountdown(int seconds)
    {
        for (int i = seconds; i >= 1; i--)
        {
            Console.WriteLine(" \b\b");
            Console.Write($"{i}");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
    public string GetName()
    {
        return _name;
    }
    public string GetDescription()
    {
        return _description;
    }
    public int GetDuration()
    {
        return _duration;
    }
}

class BreathingActivity : Activity
{
    private int _breatheInTime;
    private int _breatheOutTime;

    public BreathingActivity(string name, string description, int duration, int breatheInTime, int breatheOutTime) : base(name, description, duration)
    {
        _breatheInTime = breatheInTime;
        _breatheOutTime = breatheOutTime;
    }
    public override void RunActivity()
    {
        StartActivity();

        int elapsedTime = 0;
        while (elapsedTime < _duration)
        {
            if (elapsedTime + _breatheInTime <= _duration)
            {
                Console.WriteLine();
                Console.WriteLine("Breathe in...");
                ShowCountdown(_breatheInTime);
                Console.WriteLine();
                elapsedTime += _breatheInTime;
            }

            if (elapsedTime + _breatheOutTime <= _duration)
            {
                Console.WriteLine();
                Console.WriteLine("Breathe out...");
                ShowCountdown(_breatheOutTime);
                Console.WriteLine();
                elapsedTime += _breatheOutTime;
            }
        }

        Console.WriteLine();
        EndActivity();
    }
}
class ReflectionActivity : Activity
{
    private List<string> _prompts = new List<string>();
    private List<string> _questions = new List<string>();

    public ReflectionActivity(string name, string description, int duration, List<string> prompts, List<string> questions) : base(name, description, duration)
    {
        _prompts = prompts;
        _questions = questions;
    }
    public override void RunActivity()
    {
        StartActivity();

        Random random = new Random();
        int index = random.Next(0, _prompts.Count);
        Console.WriteLine();
        Console.WriteLine(_prompts[index]);
        ShowSpinner(5);
        Console.WriteLine();

        if (_prompts.Count <= 0)
        {
            Console.WriteLine("Error, no prompts entered.");
        }

        int elapsedTime = 5;
        while (elapsedTime < _duration)
        {
            if (elapsedTime + 5 <= _duration)
            {
                int questionIndex = random.Next(0, _questions.Count);
                Console.WriteLine(_questions[questionIndex]);
                ShowSpinner(5);
                elapsedTime += 5;
                Console.WriteLine();
            }
        }

        if (_questions.Count <= 0)
        {
            Console.WriteLine("Error, no questions entered.");
        }


        Console.WriteLine();
        EndActivity();
    }
}
class ListingActivity : Activity
{
    private List<string> _prompts = new List<string>();
    private List<string> _items = new List<string>();

    public ListingActivity(string name, string description, int duration, List<string> prompts, List<string> items) : base(name, description, duration)
    {
        _prompts = prompts;
        _items = items;
    }

    public override void RunActivity()
    {
        StartActivity();

        Random random = new Random();
        int index = random.Next(0, _prompts.Count);
        Console.WriteLine($"List as many responses as you can to the following prompt:");
        Console.WriteLine(_prompts[index]);
        ShowSpinner(5);
        Console.WriteLine();

        DateTime startTime = DateTime.Now;
        Console.WriteLine("Now enter as many items as you can until the time is up. Enter empty line to exit early.");
        Console.WriteLine();

        while ((DateTime.Now - startTime).TotalSeconds < _duration)
        {
            string userInput = Console.ReadLine();

            if (userInput != "")
            {
                _items.Add(userInput);
            }
            if (string.IsNullOrEmpty(userInput))
            {
                break;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"You listed {_items.Count} items.");
        _items.Clear();

        Console.WriteLine();
        EndActivity();
    }
}
