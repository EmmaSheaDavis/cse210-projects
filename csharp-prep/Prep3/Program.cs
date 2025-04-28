using System;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        int number = randomGenerator.Next(1,100);
        int guess = 0;
       

        while (guess != number)
        {
            Console.Write("What is your guess? ");

            string userInput = Console.ReadLine();
            guess = int.Parse(userInput);

            if (guess > number)
            {
                Console.WriteLine("Lower");
            }
            else if (guess < number)
            {
                Console.WriteLine("Higher");
            }

        }
        Console.WriteLine("You guessed it!");
        


    }
}