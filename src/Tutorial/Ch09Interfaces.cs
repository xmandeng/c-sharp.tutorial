namespace Tutorial;

// Chapter 9: interfaces, abstract classes, inheritance, polymorphism.

// Interface: a list of members a class promises to provide.
// Normally it contains no code.
// Python equivalent: typing.Protocol or an ABC with only abstract methods.
public interface IShape
{
    string Name { get; }
    double Area();
}

// Abstract class: cannot be created directly. Shares code with subclasses
// and leaves some members for them to fill in.
public abstract class Shape : IShape
{
    public abstract string Name { get; }      // subclasses MUST override
    public abstract double Area();

    // virtual: subclasses MAY override
    public virtual string Describe() => $"{Name} with area {Area():F2}";
}

public class Circle : Shape                   // Python: class Circle(Shape):
{
    public double Radius { get; }

    public Circle(double radius) => Radius = radius;

    public override string Name => "Circle";
    public override double Area() => Math.PI * Radius * Radius;
}

public class Rectangle : Shape
{
    public double Width { get; }
    public double Height { get; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override string Name => "Rectangle";
    public override double Area() => Width * Height;
}

// sealed: nothing may inherit from Square.
// : base(...) calls the parent constructor (Python: super().__init__(...))
public sealed class Square : Rectangle
{
    public Square(double side) : base(side, side) { }

    public override string Name => "Square";
    public override string Describe() => base.Describe() + " (all sides equal)";
}

public static class Ch09Interfaces
{
    // Accepts any sequence of anything that implements IShape
    public static double TotalArea(IEnumerable<IShape> shapes)
    {
        double total = 0;
        foreach (var shape in shapes)
        {
            total += shape.Area();            // each object runs its own Area()
        }
        return total;
    }

    public static void Run()
    {
        var shapes = new List<Shape>
        {
            new Circle(1), new Rectangle(2, 3), new Square(2),
        };

        foreach (var shape in shapes)
        {
            Console.WriteLine(shape.Describe());
        }
        Console.WriteLine($"Total area: {TotalArea(shapes):F2}");

        // Type test plus cast in one step (Python: isinstance)
        foreach (var shape in shapes)
        {
            if (shape is Circle c)
            {
                Console.WriteLine($"found a circle of radius {c.Radius}");
            }
        }

        // Pattern matching on type. First matching arm wins.
        string kind = shapes[2] switch
        {
            Square sq => $"square of side {sq.Width}",
            Rectangle r => $"rectangle {r.Width} x {r.Height}",
            _ => "something else",
        };
        Console.WriteLine(kind);
    }
}
