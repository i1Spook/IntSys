using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntSysA1
{
    class Program
    {
        //Aufgabe 1
        //public static void Add(double x, double y) { Console.WriteLine(x + y); }
        //public static void Mult(double x, double y) { Console.WriteLine(x * y); }
        //delegate void MyDelegate(double x, double y);
        static void Main(string[] args)
        {
            ////Aufgabe 2
            //double x = Convert.ToInt32(Console.ReadLine());
            //double y = Convert.ToInt32(Console.ReadLine());
            //MyDelegate RechenDelegate = null;

            ////Operatoren
            //RechenDelegate = Add;
            //RechenDelegate += Mult;
            //RechenDelegate(x,y);

            ////Delegate Methode
            //MyDelegate AddDelegate = Add;
            //MyDelegate MultDelegate = Mult;
            //RechenDelegate = (MyDelegate) Delegate.Combine(AddDelegate, MultDelegate);
            //RechenDelegate(x, y);

            ////Anonyme Methode
            //RechenDelegate = delegate (double x1, double y1) { Console.WriteLine(x1 + y1); };
            //RechenDelegate += delegate (double x1, double y1) { Console.WriteLine((x1 * y1)); };
            //RechenDelegate(x, y);



        }
    }
    public delegate void MeinEventHandler();
    class ConsoleApp
    {
        public event MeinEventHandler StudentEvent;
        Student Student1 = new Student();

    }
    class Student
    {
        protected int matrikelNummer = 123456;
        
        public int matrikelNummerProp
        {
            get { return matrikelNummer; }
        }


    }
}
