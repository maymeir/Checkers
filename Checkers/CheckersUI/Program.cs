using System;
using System.Windows.Forms;

namespace CheckersUI
{
    public static class Program
    {


            [STAThread]
            public static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(true);
                Application.Run(new StartGameForm());
            }
       
    }
}
