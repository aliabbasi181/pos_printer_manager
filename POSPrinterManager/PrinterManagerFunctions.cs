using System;
using System.Drawing.Printing;

namespace POSPrinterManager
{
    public class PrinterManagerFunctions
    {
        public static async Task<bool> loadDevices() {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {

            }
            return true;
        }
    }
}
