using System;
using System.Dynamic;

//design for Develop02, the design for the journal program
class Program
{   
    
    static void Main(string[] args)
    {
       Console.WriteLine("Welcome to the Journal Program!");
    }
    static void DisplayMenu()
    {
        Console.WriteLine("1. Write");
        Console.WriteLine("2. Dispaly");
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
   
}





