using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your grade percentage? ");
        string userInput = Console.ReadLine();
        int percentage = int.Parse(userInput);

        string letter = "";

        string sign = "";
        int lastDigit = (int)percentage % 10;

        if (percentage >= 90)
        {
            letter = "A";
        }
        else if (percentage >= 80)
        {
            letter = "B";
        }
        else if (percentage >= 70)
        {
            letter = "C";
        }
        else if (percentage >= 60)
        {
            letter = "D";
        }
        else 
        {
            letter = "F";
        }

        if (lastDigit >= 7)
        {
            sign = "+";
        }
        else if (lastDigit < 3)
        {
            sign = "-";
        }
        else{
            sign = "";
        }

        if (letter == "A" || letter == "F")
        {
            sign = "";
        }

        Console.WriteLine($"Youre Grade: {letter}{sign}");

        if (percentage >= 70)
        {
            Console.WriteLine("You have passed the class!");
        }
        else{
            Console.WriteLine("You have failed the class. You'll get it next time!");
        }
    }
}