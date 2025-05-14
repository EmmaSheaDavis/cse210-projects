using System;

class Program
{
    static void Main(string[] args)
    {
        //This is a program to compute the area of a circle.

        //Get the radius of the circle from the user.
        Console.Write("Please enter the raidus: ");
        string text = Console.ReadLine();
        double radius = double.Parse(text);

        //Compute area of circle
        double area = Math.PI * radius * radius;

        //Display result for user to see
        Console.WriteLine($"Area of the circle: {area}");
    }
}