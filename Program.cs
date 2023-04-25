namespace MyGeometryLib;

internal class Program
{
    static void Main()
    {
        var t = new Triangle(3, 4, 5);

        Console.WriteLine(t);
        Console.WriteLine(t.GetArea());

    }
}
