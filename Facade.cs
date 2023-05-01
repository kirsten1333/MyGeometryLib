using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyGeometryLib
{
    public class Facade //Над названием можно поработать
    {
        //Также эти методы можно было вынести в отдельный класс, оставив фасад максимально тонким,
        //это было бы логичнее, но в данном примере не критично

        /// <summary>
        /// Получает площадь первой попавшейся фигуры с подходящим конструктором
        /// </summary>
        /// <param name="values"> Параметры на основынии которых будет создаваться фигура</param>
        /// <returns>Площадь фигуры</returns>
        /// <exception cref="ArgumentException">Если ни один конструктор не подходит, то выдаёт ошибку</exception>
        public static double CalculateArea(params double[] values)
        {
            if (values.Length == 0) return 0;

            var ctors = GetCtors(values);

            List<IFigure> figures = GetFigures(values, ctors);

            var area = figures.First().GetArea(); //Можно было сделать выбор того, какую фигуру выбрать, но для этого нужен UI,
            // UI тут не предусмотрен
            return area;
        }

        private static List<IFigure> GetFigures(double[] values, List<ConstructorInfo> ctors)
        {
            return ctors
                .Select(ctor => ctor.Invoke(values.Cast<object>().ToArray()))
                .Cast<IFigure>()
                .ToList();
        }

        private static List<ConstructorInfo> GetCtors(double[] values)
        {
            //Также тут можно было бы сделать приведение всех конструктороа фигур (при возможности) к double
            //CTOR(Point,Point,double) === CTOR(double * 5)
            //т.к. Point === (double, double)
            //Во1, это вызывает больше проблем, чем возможностей
            //Во2, неоднозначность слишком велика, работать с конкретными типами - проще
            //Во3, Реализация вызывает вопросы
            var type = typeof(IFigure);
            var typeArr = new Type[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                typeArr[i] = typeof(double);
            }

            List<ConstructorInfo?> ctors =
                AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.GetConstructor(typeArr))
                .Where(x => x != null)
                .ToList();

            if (!ctors.Any()) //Тут можно было выдавать ноль,
                              //но я считаю попытку вызвать неподходящий конструктор - исключительным случаем
                              //так как площадь фигуры с четырьмя сторонами не всегда равна нулю
                              //у нас же, просто, нет конструктора, соответствующего такой ситуации
            {
                throw new ArgumentException("Fail to get correct CTOR," +
                " try another count of arguments," +
                " or create new CTOR with IFigure interface");
            }

            return ctors!;
        }
    }
}
