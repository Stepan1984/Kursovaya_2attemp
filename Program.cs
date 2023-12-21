using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp
{
    class Program
    {
        public static Random rnd = new Random();
        const double EPS = 1e-6;

        public interface IForKurs
        {
            List<Func<string>> GenerateDelegateList();
            public byte[] GetByteArray()
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, this);
                    return ms.ToArray();
                }
            }
        }

        public interface IJuicable
        {
            bool Juicable { get; set; }
            string Juice();
        }
        public interface IGarden
        {
            bool Ripe { get; set; }
            string Eat();
            string Cut();

        }

        [Serializable]
        public class Birch : IJuicable, IForKurs
        {
            public bool Juicable { get; set; }

            public string Juice()
            {
                if (Juicable)
                    return "Пьём берёзовый сок";
                else return "Грызём кору берёзы";
            }
            public Birch()
            {
                Juicable = true;
            }

            public List<Func<string>> GenerateDelegateList()
            {
                return new List<Func<string>>() { Juice };
            }
        }

        [Serializable]
        public class Cabbage : IGarden, IForKurs
        {
            public bool Ripe { get; set; }
            protected string type = "рыночный";
            public string Type { get { return type; } set { type = value; } }
            public string Eat()
            {
                if (Ripe)
                    return "Хрустим капустой";
                else return "Плюёмся капустой";
            }
            public string Cut()
            {
                return "Режем капусту";
            }

            public Cabbage()
            {
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
            }

            public Cabbage(string newType)
            {
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
                Type = newType;
            }

            public List<Func<string>> GenerateDelegateList()
            {
                return new List<Func<string>>() { Eat, Cut };
            }

        }

        [Serializable]
        public class Lemon : IGarden, IForKurs
        {
            public bool Ripe { get; set; }

            protected int sourness;
            public int Sourness { get { return sourness; } set { if (value > 0) sourness = value; } }

            protected string type = "рыночный";
            public string Type { get { return type; } set { type = value; } }

            public bool Seeds { get; set; }

            public string Eat()
            {
                if (Ripe)
                    return "Макунли лимон в сахар и наслаждаемся вкусом";
                else return "Плюёмся неспелым лимоном";
            }

            public string Cut() { return "Порезали лимон"; }

            public string Smash() { return "Выжали лимон"; }

            public virtual void ClassName() { Console.WriteLine(GetType()); }

            public static string WhenPlant()
            { return "Сажать надо вовремя☺"; }

            public override string ToString()
            {
                return String.Format("{4} \n Сорт:{0} \n Кислотность:{1} \n Спелый:{2} \n Семечки:{3}", Type, Sourness, Ripe ? "Да" : "Нет", Seeds ? "есть" : "нет", "лимон");
            }
            public Lemon()
            {
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
                Sourness = 1;
                Seeds = true;
            }

            public Lemon(string newType, int newSourness, bool containsSeeds)
            {
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
                Type = newType;
                Sourness = newSourness;
                Seeds = containsSeeds;
            }

            public List<Func<string>> GenerateDelegateList()
            {
                return new List<Func<string>>() { Eat, Cut, Smash, WhenPlant };
            }
        }

        [Serializable]
        public abstract class seedbed : IGarden // класс грядка 
        {


            public bool Ripe { get; set; }

            protected string type = "рыночный";
            public string Type { get { return type; } set { type = value; } }

            public bool Seeds { get; set; }

            public virtual void ClassName() { Console.WriteLine(GetType()); }

            public string Eat()
            {
                return "Съели";
            }

            public string Cut() { return "Порезали"; }

            public static string WhenPlant()
            { return "Сажать надо вовремя☺"; }

        }


        [Serializable]
        public class Watermelon : seedbed, IForKurs
        {


            public double Weight { get; set; }

            public override void ClassName()
            {
                base.ClassName();
            }

            public virtual string Knock()
            { return "Тук-тук"; }

            public override string ToString()
            {
                return String.Format("{4} \n Сорт:{0} \n Вес:{1} \n Спелый:{2} \n Семечки:{3}", Type, Weight, Ripe ? "Да" : "Нет", Seeds ? "есть" : "нет", "арбуз");
            } // перегрузка ToString

            public Watermelon(double w = 1, bool s = true, string t = "рыночный")
            {
                if (t.GetHashCode() == 0) throw new Exception("Недопустимый сорт");
                Weight = w;
                Seeds = s;
                Type = t;
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
            }
            public Watermelon()
            {
                Weight = 0;
                Type = "базовый";
                Seeds = true;
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
            }
            public Watermelon(string t, double w)
            {
                if (t.GetHashCode() == 0) throw new Exception("Недопустимый сорт");
                Weight = w;
                Seeds = true;
                Type = t;
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
            }

            // перегрузки
            public static bool operator >(Watermelon a, Watermelon b)
            {
                if (a.Weight > b.Weight) return true;
                else return false;
            }
            public static bool operator <(Watermelon a, Watermelon b)
            {
                if (a.Weight < b.Weight) return true;
                else return false;
            }
            public static string operator +(Watermelon a, Watermelon b)
            {
                return "арбуз + арбуз = 🚀";
            }

            public List<Func<string>> GenerateDelegateList()
            {
                return new List<Func<string>>() { Knock, Eat, Cut };
            }
        }


        [Serializable]
        public class Melon : seedbed, IForKurs
        {
            protected static string[] colorsArray = new string[4] { "жёлтый", "зелёный", "коричневый", "белый" };

            public static string[] ColorsArray { get { return colorsArray; } }

            protected int color;
            public int Color { get { return color; } set { if (value >= 1 && value <= 4) color = value; } }

            public string Roll()
            {
                return "Катится......";
            }

            public override void ClassName()
            {
                base.ClassName();
            }

            public override string ToString()
            {
                return String.Format("{4} \n Сорт:{0} \n Цвет:{1} \n Спелый:{2} \n Семечки:{3}", Type, colorsArray[Color], Ripe ? "Да" : "Нет", Seeds ? "есть" : "нет", "дыня");
            } // перегрузка ToString

            public Melon()
            {
                color = 0;
                Type = "базовый";
                Seeds = true;
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
            }

            public Melon(string t, int c, bool s)
            {
                Type = t;
                Color = c;
                Seeds = s;
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
            }

            public List<Func<string>> GenerateDelegateList()
            {
                return new List<Func<string>>() { Roll, Eat, Cut };
            }
        }


        [Serializable]
        public class Grape : seedbed, IForKurs
        {
            private int amount;
            public int Amount { get { return amount; } set { if (value > 0) amount = value; } }

            public string EatAgressively()
            {
                return "Яростно запихиваем виноградины в рот";
            }


            public override void ClassName()
            {
                base.ClassName();
            }

            public override string ToString()
            {
                return String.Format("{4} \n Сорт:{0} \n Количество виноградин:{1} \n Спелый:{2} \n Семечки:{3}", Type, Amount, Ripe ? "Да" : "Нет", Seeds ? "есть" : "нет", "виноград");
            } // перегрузка ToString

            public Grape(string t, int a, bool s)
            {
                Type = t;
                Amount = a;
                Seeds = s;
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
            }
            public Grape()
            {
                Amount = 1;
                Type = "базовый";
                Seeds = true;
                Ripe = Convert.ToBoolean(rnd.Next() % 2);
            }

            public List<Func<string>> GenerateDelegateList()
            {
                return new List<Func<string>>() { EatAgressively, Eat, Cut };
            }

        }

        static void Main(string[] args)
        {

            List<IGarden> bed = new List<IGarden>();

            int menu, submenu, length, index;
            do
            {
                Console.Clear();
                Console.WriteLine("1.Добавить плод");
                Console.WriteLine("2.Вывести список плодов");
                Console.WriteLine("3.Свойства плода из списка");
                Console.WriteLine("4.Методы плода из списка");
                Console.WriteLine("5.Передать объект плода в функцию");
                Console.WriteLine("6.Выход");
                if (Int32.TryParse(Console.ReadLine(), out menu) && (menu < 1 || menu > 7))
                    continue;
                Console.Clear();

                length = bed.Count;
                switch (menu)
                {
                    case 1: // создать объект
                        int type = ChooseType();
                        Console.Clear();
                        bed.Add(CreateNewObj(type));
                        Exit();
                        break;
                    case 2: // вывод списка
                        if (length <= 0)
                            Console.WriteLine("Список пуст");
                        else
                        {
                            int i = 0;
                            foreach (var item in bed)
                                Console.WriteLine("{0}: {1}", ++i, item);
                        }
                        Exit();
                        break;
                    case 3: // вывод свойств
                        if (length < 1)
                            Console.WriteLine("Список пуст");
                        else
                        {
                            index = GetIndex(length);
                            Console.Clear();
                            Console.WriteLine(bed[index]);
                        }
                        Exit();
                        break;

                    case 4: // выполнение методов
                        if (length <= 0)
                            Console.WriteLine("Список пуст");
                        else
                        {
                            index = GetIndex(length);
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("1.Когда сажать (статический метод)");
                                Console.WriteLine("2.Съесть");
                                Console.WriteLine("3.Порезать");
                                switch (bed[index])
                                {
                                    case Watermelon: Console.WriteLine("4.Постучать"); break;
                                    case Melon: Console.WriteLine("4.Катнуть"); break;
                                    case Grape: Console.WriteLine("4.Агрессивно хомячить"); break;
                                    case Lemon: Console.WriteLine("4.Выжать"); break;
                                }
                                Console.WriteLine("5.назад");
                                if (Int32.TryParse(Console.ReadLine(), out submenu) && (submenu < 1 || submenu > 5))
                                    continue;
                                Console.Clear();
                                switch (submenu)
                                {
                                    case 1:
                                        if ((bed[index]) is Lemon)
                                            Lemon.WhenPlant();
                                        else
                                            seedbed.WhenPlant();
                                        Exit();
                                        break;
                                    case 2: bed[index].Eat(); Exit(); break;
                                    case 3: bed[index].Cut(); Exit(); break;
                                    case 4:
                                        switch (bed[index])
                                        {
                                            case Watermelon: ((Watermelon)bed[index]).Knock(); break;
                                            case Melon: ((Melon)bed[index]).Roll(); break;
                                            case Grape: ((Grape)bed[index]).EatAgressively(); break;
                                            case Lemon: ((Lemon)bed[index]).Smash(); break;
                                        }
                                        Exit();
                                        break;
                                }
                            } while (submenu != 5);

                        }
                        Exit();
                        break;
                    case 5: // передать в функцию
                        if (length <= 0)
                            Console.WriteLine("Список пуст");
                        else
                        {
                            index = GetIndex(length);
                            Console.Clear();
                            Foo(bed[index]);
                        }
                        Exit();
                        break;
                }
            }
            while (menu != 6);
        }

        public static void Exit()
        {
            Console.WriteLine("нажмите любую клавишу для возварата в меню");
            Console.ReadKey();
        }

        public static IGarden CreateNewObj(int type)
        {
            string? t = null;
            string? stmp = null;
            bool s;

            int c = -1;
            double w = -1;
            bool newtaste = false;
            int a = -1;

            Console.Clear();
            Console.WriteLine("Введите сорт:\n");
            while (t is null || t.Length < 2)
                t = Console.ReadLine();

            Console.Clear();
            if (type == 0)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Введите вес арбуза(кг):\n");
                } while (!Double.TryParse(Console.ReadLine(), out w) || w < EPS);
            }
            if (type == 1)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Цвет дыни");
                    for (int i = 0; i < Melon.ColorsArray.Length; i++)
                        Console.WriteLine("{0}.{1}", i + 1, Melon.ColorsArray[i]);

                } while (!Int32.TryParse(Console.ReadLine(), out c) || c < 1 || c > 4);
                c--;
            }

            if (type == 2)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Количество виноградин: ");
                } while (!Int32.TryParse(Console.ReadLine(), out a) || a < 1);
            }

            if (type == 3)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Сладкий или кислый (с/к):\n");
                    stmp = Console.ReadLine();
                } while (stmp != "с" && stmp != "С" && stmp != "к" && stmp != "К");
                if (stmp == "к" || stmp == "К")
                    newtaste = false;
                else newtaste = true;
            }
            if (type == 4)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Степень кислотности: ");
                } while (!Int32.TryParse(Console.ReadLine(), out a) || a < 1);
            }

            do
            {
                Console.Clear();
                Console.WriteLine("Внутри есть семечки (д/н):\n");
                stmp = Console.ReadLine();
            } while (stmp != "д" && stmp != "Д" && stmp != "н" && stmp != "Н");
            if (stmp == "Д" || stmp == "д")
                s = true;
            else s = false;
            switch (type)
            {
                case 0:
                    return new Watermelon(w, s, t);
                case 1:
                    return new Melon(t, c, s);
                case 2:
                    return new Grape(t, a, s);
                case 3:
                    return new Lemon(t, a, s);
            }
            return null;
        }
        public static int ChooseType()
        {
            // арбуз - 0
            // дыня - 1
            // виноград - 2
            // лимон - 3
            int answ = -1;
            Console.WriteLine("1.арбуз");
            Console.WriteLine("2.дыня");
            Console.WriteLine("3.виноград");
            Console.WriteLine("4.лимон");
            while (answ < 1 || answ > 4)
                Int32.TryParse(Console.ReadLine(), out answ);
            return --answ;
        }

        public static int GetIndex(int length)
        {
            if (length < 1)
                return -1;
            int index;
            do
            {
                Console.Clear();
                Console.WriteLine("Введите номер элемента в списке: ");

            } while (!Int32.TryParse(Console.ReadLine(), out index) || index < 1 || index > length);
            return --index;
        }

        public static void Foo(IGarden plant)
        {
            Console.WriteLine("Функция получила растение");
            Console.WriteLine(plant);
        }

    }
}
