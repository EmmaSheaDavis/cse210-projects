using System;
using System.Drawing;
using System.Dynamic;
using System.Formats.Asn1;
using System.Reflection.Metadata.Ecma335;

class Program
{
    static void Main(string[] args)
    {
        List<Shape> shapes = new List<Shape>();

        Square s1 = new Square("Purple", "Square", 4);
        shapes.Add(s1);

        Rectangle s2 = new Rectangle("Red", "Rectangle", 5, 7);
        shapes.Add(s2);

        Circle s3 = new Circle("Blue", "Circle", 2.5);
        shapes.Add(s3);

        foreach (Shape s in shapes)
        {
            DisplayShape(s);
        }
    }
    public static void DisplayShape(Shape shapes)
    {

        string color = shapes.Color;

        string name = shapes.Name;

        double area = shapes.GetArea();

        Console.WriteLine($"The {name} shape is {color} and its area is {area}.");
        Console.WriteLine();
    }

}

public abstract class Shape
{
    // private string _name;
    // private string _color;

    public Shape(string color, string name)
    {
        Color = color;
        Name = name;
    }
    public string Color { get; set; }
    public string Name { get; set; }

    public abstract double GetArea();
}

public class Square : Shape
{
    private double Side { get; set; }

    public Square(string color, string name, double side) : base(color, name)
    {
        Side = side;
    }

    public override double GetArea()
    {
        return Side * Side;
    }
}

public class Rectangle : Shape
{
    private double Length{ get; set; }
    private double Width{ get; set; }

    public Rectangle(string color, string name, double length, double width) : base(color, name)
    {
        Length = length;
        Width = width;
    }

    public override double GetArea()
    {
        return Length * Width;
    }
}

public class Circle : Shape
{
    private double _radius;

    public Circle(string color, string name, double radius) : base(color, name)
    {
        _radius = radius;
    }

    public override double GetArea()
    {
        return Math.PI * _radius * _radius;
    }
}