using System;
using System.Drawing;
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
            Console.WriteLine();
            string color = s.GetColor();

            string name = s.GetName();

            double area = s.GetArea();

            Console.WriteLine($"The {name} shape is {color} and its area is {area}.");
        }
    }
}

public abstract class Shape
{
    private string _name;
    private string _color;

    public Shape(string color, string name)
    {
        _color = color;
        _name = name;
    }

    public string GetColor()
    {
        return _color;
    }
    public string GetName()
    {
        return _name;
    }
    public void SetName(string name)
    {
        _name = name;
    }

    public void SetColor(string color)
    {
        _color = color;
    }

    public abstract double GetArea();
}

public class Square : Shape
{
    private double _side;

    public Square(string color, string name, double side) : base(color, name)
    {
        _side = side;
    }

    public override double GetArea()
    {
        return _side * _side;
    }
}

public class Rectangle : Shape
{
    private double _length;
    private double _width;

    public Rectangle(string color, string name, double length, double width) : base(color, name)
    {
        _length = length;
        _width = width;
    }

    public override double GetArea()
    {
        return _length * _width;
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