using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CompressString;

namespace Snake
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string testing = "";
            for (int i = 0; i < 36; i += 1)
            {
                testing += 'a';
            }
            int length = testing.Length;
            testing = StringCompressor.CompressString(testing);
            length = testing.Length;
            testing = StringCompressor.DecompressString(testing);
            length = testing.Length;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new gameWindowForm());
        }
    }
}
