using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();

        while (true)
        {
            Console.Write("Enter number: ");
            string userInput = Console.ReadLine();
            
            if (!int.TryParse(userInput, out int enterNumber))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            if (enterNumber== 0)
            {
                break;
            }

            numbers.Add(enterNumber);

        }

        double sum = 0;
        int largest = numbers[0];
        foreach (int number in numbers)
        {
            sum += number;
            if (number > largest)
            {
                largest = number;
            }
        }

        double average = sum / numbers.Count;

        Console.WriteLine($"The sum is: {sum}");
        Console.WriteLine($"The average is: {average}");
        Console.WriteLine($"The largest number is: {largest}");
        
    }
}