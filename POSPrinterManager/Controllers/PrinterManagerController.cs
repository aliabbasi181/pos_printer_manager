using ESCPOS_NET;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using ESCPOS_NET;
using System.IO.Ports;
using System.Text;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
namespace POSPrinterManager.Controllers
{
    [ApiController]
    [Route("api/printer-manager")]
    public class PrinterManagerController : ControllerBase
    {
            [HttpGet("list-devices")]
            public async Task<IActionResult> ListDevices() 
            {
                string[] printerNames = PrinterSettings.InstalledPrinters.Cast<string>().ToArray();

                // Display the list of printers
                Console.WriteLine("List of installed printers:");
                foreach (string printerName in printerNames)
                {
                    Console.WriteLine(printerName);
                }
                return Ok(new {
                status = true,
                message = "Connected devices gest success",
                    data = printerNames
            });
            }

        [HttpGet("print/{printerName}")]
        public async Task<IActionResult> Print(string printerName)
        {

            var printer = new SerialPrinter(portName: printerName, baudRate: 115200);

            // In your program, register event handler to call the method when printer status changes:

            var e = new EPSON();
            printer.Write( // or, if using and immediate printer, use await printer.WriteAsync
              ByteSplicer.Combine(
                e.CenterAlign(),
                e.PrintLine("----------------------------------------"),
                e.SetBarcodeHeightInDots(360),
                e.SetBarWidth(BarWidth.Default),
                e.SetBarLabelPosition(BarLabelPrintPosition.None),
                e.PrintBarcode(BarcodeType.ITF, "0123456789"),
                e.PrintLine("----------------------------------------"),
                e.PrintLine("B&H PHOTO & VIDEO"),
                e.PrintLine("420 NINTH AVE."),
                e.PrintLine("NEW YORK, NY 10001"),
                e.PrintLine("(212) 502-6380 - (800)947-9975"),
                e.SetStyles(PrintStyle.Underline),
                e.PrintLine("www.bhphotovideo.com"),
                e.SetStyles(PrintStyle.None),
                e.PrintLine("----------------------------------------"),
                e.LeftAlign(),
                e.PrintLine("Order: 123456789        Date: 02/01/19"),
                e.PrintLine("----------------------------------------"),
                e.PrintLine("----------------------------------------"),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
                e.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
                e.PrintLine("----------------------------------------------------------------"),
                e.RightAlign(),
                e.PrintLine("SUBTOTAL         89.95"),
                e.PrintLine("Total Order:         89.95"),
                e.PrintLine("Total Payment:         89.95"),
                e.PrintLine("----------------------------------------"),
                e.LeftAlign(),
                e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                e.PrintLine("SOLD TO:                        SHIP TO:"),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("  FIRSTN LASTNAME                 FIRSTN LASTNAME"),
                e.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
                e.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
                e.PrintLine("  (123)456-7890                   (123)456-7890"),
                e.PrintLine("  CUST: 87654321"),
                e.PrintLine("----------------------------------------"),
                e.PrintLine("----------------------------------------")
              )
            );

            return Ok(new
            {
                status = true,
                message = "Print Success"
            });






            //// Check if the specified printer exists
            //if (PrinterExists(printerName))
            //{
            //    // Connect to the specified printer
            //    PrintDocument printDocument = new PrintDocument();
            //    printDocument.PrinterSettings.PrinterName = printerName;

            //    // Perform printing operations
            //    printDocument.PrintPage += PrintDocument_PrintPage;
            //    // For example:
            //    printDocument.Print();
            //    return Ok(new
            //    {
            //        status = true,
            //        message = "Print Success"
            //    });

            //    // or
            //    // printDocument.PrintPage += YourPrintPageHandler;
            //}
            //else
            //{
            //    Console.WriteLine("Printer does not exist.");
            //    return Ok(new
            //    {
            //        status = false,
            //        message = "Printer does not exist."
            //    });
            //}

        }

        private static void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Dummy invoice data
            string customerName = "John Doe";
            string invoiceNumber = "INV-123";
            string[] items = { "Item 1", "Item 2", "Item 3" };
            decimal[] prices = { 10.50m, 20.75m, 15.00m };

            // Font for the invoice
            Font font = new Font("Arial", 12, FontStyle.Regular);

            // Print the invoice data
            float y = 100; // Initial y position
            e.Graphics.DrawString("Invoice", font, Brushes.Black, 100, y);
            y += 30;
            e.Graphics.DrawString("Customer: " + customerName, font, Brushes.Black, 100, y);
            y += 20;
            e.Graphics.DrawString("Invoice Number: " + invoiceNumber, font, Brushes.Black, 100, y);
            y += 40;

            // Print items and prices
            for (int i = 0; i < items.Length; i++)
            {
                e.Graphics.DrawString(items[i], font, Brushes.Black, 100, y);
                e.Graphics.DrawString(prices[i].ToString("C"), font, Brushes.Black, 300, y);
                y += 20;
            }
        }

        static void StatusChanged(object sender, EventArgs ps)
        {
            var status = (PrinterStatusEventArgs)ps;
            Console.WriteLine($"Status: {status.IsPrinterOnline}");
            Console.WriteLine($"Has Paper? {status.IsPaperOut}");
            Console.WriteLine($"Paper Running Low? {status.IsPaperLow}");
            Console.WriteLine($"Cash Drawer Open? {status.IsCashDrawerOpen}");
            Console.WriteLine($"Cover Open? {status.IsCoverOpen}");
        }

        private bool PrinterExists(string printerName)
            {
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    if (printer == printerName)
                        return true;
                }
                return false;
            }
    }
}
