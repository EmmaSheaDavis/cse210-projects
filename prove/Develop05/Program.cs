using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

class Program
{
    private QuestManager _questManager;

    public void DisplayMenu()
    {
        Console.WriteLine("Menu Options:");
        Console.WriteLine("1. Create New Goal");
        Console.WriteLine("2. List Goals");
        Console.WriteLine("3. Save Goals");
        Console.WriteLine("4. Load Goals");
        Console.WriteLine("5. Record Event");
        Console.WriteLine("6. Quit");
        Console.WriteLine();
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            _questManager.DisplayScore();
            Console.WriteLine();
            DisplayMenu();
            Console.WriteLine("Select a choice from the menu: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("The types of Goals are:");
                Console.WriteLine("1. Simple Goal");
                Console.WriteLine("2. Eternal Goal");
                Console.WriteLine("3. Cheklist Goal");
                Console.WriteLine();

                Console.Write("Which type of goal would you like to create? ");
                string goalType = Console.ReadLine();
                Console.Write("Enter goal name: ");
                string name = Console.ReadLine();
                Console.Write("Enter goal description: ");
                string description = Console.ReadLine();
                Console.Write("Enter points for goal: ");
                int.TryParse(Console.ReadLine(), out int points);

                if (goalType == "3")
                {
                    Console.Write("Enter target count: ");
                    int.TryParse(Console.ReadLine(), out int targetCount);
                    Console.Write("Enter bonus points: ");
                    int.TryParse(Console.ReadLine(), out int bonusPoints);
                    _questManager.CreateGoal(goalType, name, description, points, targetCount, bonusPoints);
                }
                else if (goalType == "1" || goalType == "2")
                {
                    _questManager.CreateGoal(goalType, name, description, points);
                }
                else
                {
                    Console.WriteLine("Invalide goal option. Please pick 1, 2, or 3.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else if (choice == "2")
            {
                Console.Clear();
                _questManager.DisplayGoals();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else if (choice == "3")
            {
                Console.Clear();
                _questManager.SaveToFile("goals.txt");
                Console.WriteLine("Goals Saved!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else if (choice == "4")
            {
                Console.Clear();
                _questManager.LoadFromFile("goals.txt");
                Console.WriteLine("Goals loaded!");
                Console.WriteLine("Press any key to conintue...");
                Console.ReadKey();
            }
            else if (choice == "5")
            {
                Console.Clear();
                _questManager.DisplayGoals();

                Console.Write("Enter Goal Number: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int goalNumber))
                {

                    int goalIndex = goalNumber - 1;
                    _questManager.RecordGoalEvent(goalIndex);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else if (choice == "6")
            {
                Console.Clear();
                Console.WriteLine("Thank you for using the Goal Program.");
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please pick a choice from the six menu options:");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
    static void Main(string[] args)
    {
        Program program = new Program();
        program._questManager = new QuestManager();
        program.Run();
    }
}

public abstract class Goal
{
    public Goal(string name, string description, int points)
    {
        Name = name;
        Description = description;
        Points = points;
    }
    protected string Name { get; private set; }
    protected string Description { get; private set; }
    protected int Points { get; private set; }

    public abstract bool IsComplete();
    public abstract int RecordGoal();
    public abstract string GoalStatus();
    public abstract string SaveGoal();
}

public class SimpleGoal : Goal
{
    private bool _isComplete;
    public SimpleGoal(string name, string description, int points) : base(name, description, points)
    {
        _isComplete = false;
    }

    public override bool IsComplete()
    {
        return _isComplete;
    }
    public override int RecordGoal()
    {
        if (!_isComplete)
        {
            _isComplete = true;
            return Points;
        }
        else
        {
            return 0;
        }
    }
    public override string GoalStatus()
    {
        return $"{Name}: {Description} (Points: {Points})";   
    }
    public override string SaveGoal()
    {
        return $"Simple Goal/ {Name}/ {Description}/ {Points}/ completed: {_isComplete}";
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points) : base(name, description, points)
    { }
    public override bool IsComplete()
    {
        return false;
    }
    public override int RecordGoal()
    {
        return Points;
    }
    public override string GoalStatus()
    {
        return $"{Name}: {Description} (Points: {Points})";  
    }
    public override string SaveGoal()
    {
        return $"Eternal Goal/ {Name}/ {Description}/ {Points}/ completed: false";
    }
}

public class ChecklistGoal : Goal
{
    private int _timesCompleted;
    private int _targetCount;
    private int _bonusPoints;
    public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints) : base(name, description, points)
    {
        _targetCount = targetCount;
        _bonusPoints = bonusPoints;
        _timesCompleted = 0;
    }
    public override bool IsComplete()
    {
        if (_timesCompleted >= _targetCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override int RecordGoal()
    {
        if (_timesCompleted < _targetCount)
        {
            _timesCompleted++;
            if (_timesCompleted == _targetCount)
            {
                return Points + _bonusPoints;
            }
            return Points;
        }
        return 0;
    }
    public override string GoalStatus()
    {
        return $"{Name}: {Description} (Points: {Points}, Completed {_timesCompleted}/{_targetCount}, {_bonusPoints} Bonus points upon completion.)";
    }
    public override string SaveGoal()
    {
        return $"Checklist Goal/ {Name}/ {Description}/ {Points}/ {_timesCompleted}/ {_targetCount}/ {_bonusPoints}";
    }
}

public class QuestManager
{
    private List<Goal> _goals = new List<Goal>();
    private int _score;
    private int _level;

    public QuestManager()
    {
        _score = 0;
        _level = 0;
    }

    public void CreateGoal(string goalType, string name, string description, int points, int targetCount = 0, int bonusPoints = 0)
    {
        Goal goal = goalType switch
        {
            "1" => new SimpleGoal(name, description, points),
            "2" => new EternalGoal(name, description, points),
            "3" => new ChecklistGoal(name, description, points, targetCount, bonusPoints),
            _ => null
        };
        if (goal != null)
        {
            _goals.Add(goal);
            Console.WriteLine("Goal created successfully!");
        }
        else
        {
            Console.WriteLine("Invalid goal type.");
        }
    }

    public void RecordGoalEvent(int goalIndex)
    {
        if (goalIndex >= 0 && goalIndex < _goals.Count)
        {
            Goal goal = _goals[goalIndex];
            if (goal.IsComplete())
            {
                Console.WriteLine($"The goal '{goal.GoalStatus()}' is already complete and cannot be recorded again.");
            }
            else
            {
                int previousScore = _score;
                _score += goal.RecordGoal();
                Console.WriteLine("Goal Recorded!");
                CheckLevelUp(previousScore);
            }
        }
        else
        {
            Console.WriteLine("Invalid goal number.");
        }
    }
    public void DisplayGoals()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals have been recorded/saved/loaded. Please create a goal first or load previously saved goals.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        else
        {
            int index = 1;
            foreach (Goal goal in _goals)
            {
                if (goal.IsComplete())
                {
                    Console.WriteLine($"{index}.[X]" + goal.GoalStatus());
                    index++;
                }
                else
                {
                    Console.WriteLine($"{index}.[ ]" + goal.GoalStatus());
                    index++;
                }
            }
        }
    }
    public void DisplayScore()
    {
        Console.WriteLine($"Current Score: {_score}");
        Console.WriteLine($"Current Level: {CalculateLevel()}");
    }
    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine(_score);
            foreach (Goal goal in _goals)
            {
                writer.WriteLine(goal.SaveGoal());
            }
        }
    }
    public void LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            return;
        }
        _goals.Clear();
        using (StreamReader reader = new StreamReader(filename))
        {
            _score = int.Parse(reader.ReadLine());
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split('/');
                string type = parts[0].Trim();
                string name = parts[1].Trim();
                string description = parts[2].Trim();
                int points = int.Parse(parts[3].Trim());

                if (type == "Simple Goal")
                {
                    bool isComplete = bool.Parse(parts[4].Split(':')[1].Trim());
                    var goal = new SimpleGoal(name, description, points);
                    if (isComplete) goal.RecordGoal();
                    _goals.Add(goal);
                }
                else if (type == "Eternal Goal")
                {
                    _goals.Add(new EternalGoal(name, description, points));
                }
                else if (type == "Checklist Goal")
                {
                    int timesCompleted = int.Parse(parts[4].Trim());
                    int targetCount = int.Parse(parts[5].Trim());
                    int bonusPoints = int.Parse(parts[6].Trim());
                    var goal = new ChecklistGoal(name, description, points, targetCount, bonusPoints);
                    for (int i = 0; i < timesCompleted; i++)
                    {
                        goal.RecordGoal();
                    }
                    _goals.Add(goal);
                }
            }
        }
    }
    public int CalculateLevel()
    {
        return _score / 1000 + 1;
    }
    public void CheckLevelUp(int previousScore)
    {
        int oldLevel = previousScore / 1000 + 1;
        int newLevel = _score / 1000 + 1;
        if (newLevel > oldLevel)
            Console.WriteLine($"Congratulations! You've reached Level {newLevel}!");
    }
}