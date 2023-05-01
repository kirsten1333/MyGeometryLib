namespace MyGeometryLib;

internal class Program
{
    static void Main()
    {
        //Создание треугольника и получение его площади типичным способом
        Triangle triangle = new(3, 4, 5);
        Console.WriteLine(triangle);
        Console.WriteLine(triangle.GetArea());

        //Создание окружности и получение его площади типичным способом
        Circle circle = new(100);
        Console.WriteLine(circle);
        Console.WriteLine(circle.GetArea());

        //Получение площади фигуры с тремя сторонами (получится треугольник)
        Console.WriteLine(Facade.CalculateArea(3, 4, 5));

        //Получение площади фигуры с тремя сторонами (получится окружность)
        Console.WriteLine(Facade.CalculateArea(100));
    }
}
