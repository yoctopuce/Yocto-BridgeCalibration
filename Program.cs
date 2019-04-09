using System;

using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            constants.init(args);
            string errsmg = "";
            YAPI.RegisterLogFunction(LogManager.APIlog);
            if (YAPI.InitAPI(YAPI.DETECT_NONE, ref errsmg) == YAPI.SUCCESS)
            {
               
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else MessageBox.Show("Yoctopuce API init error:" + errsmg);
        }
    }
}
