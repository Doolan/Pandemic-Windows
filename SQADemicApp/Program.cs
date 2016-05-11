using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQADemicApp
{
    static class Program
    {
        public static string[] RolesArray;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SetupGameForm());

            if (RolesArray == null) return;

            var form1 = new GameBoard(RolesArray);
            Application.Run(form1);
        }
    }
}
