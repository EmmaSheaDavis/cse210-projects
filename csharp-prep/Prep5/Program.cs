using System;

class Program
{
    static void Main(string[] args)
    {
       DisplayWelcome();
       string userName = PromptUserName();
       int number = PromptUserNumber();
       int numberSquared = SquareNumber(number);
       DisplayResult(userName, numberSquared);
    }
    static void DisplayWelcome()
    {
        Console.WriteLine("Welcome to the program!");
    }
    static string PromptUserName()
    {
        Console.Write("Please enter your name: ");
        string userName = Console.ReadLine();
        return userName;
    }
    static int PromptUserNumber()
    {
        Console.Write("Please enter your favorite number: ");
        string userInput = Console.ReadLine();
        int number = int.Parse(userInput);
        return number;
    }
    static int SquareNumber(int number)
    {
        int numberSquared = number * number;
        return numberSquared;
    }
    static void DisplayResult(string userName, int numberSquared)
    {
        Console.WriteLine($"{userName}, the square of your number is {numberSquared}.");
    }
}