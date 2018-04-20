using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            X y = new Y();
            y.Test1();
        }
    }

    class X
    {
        public void Test1()
        {
            Console.WriteLine("Test1");
        }
    }

    class Y : X
    {
        public new void Test1()
        {
            Console.WriteLine("Test2");
        }

        public void Test2()
        {
            base.Test1();
        }
    }
}
