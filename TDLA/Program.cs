using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TDLA.DBController;
using TDLA.ImGUI;

namespace TDLA
{
    class Program
    {
        public static UI ui = new UI();

        static void Main(string[] args)
        {
            Console.WriteLine("Start!\n");

            Rendering.RenderUI();
        }
    }
}
