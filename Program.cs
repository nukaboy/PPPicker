using System;
using Ghostscript.NET.Processor;
using Ghostscript.NET;
using System.IO;

namespace PPPicker
{
    class Program
    {


        static void Main(string[] args)
        {
            string inFile = args[0];
            string outFile = args[1];
            string[] pages = new string[args.Length - 2];
            for (int i = 2; i < args.Length; i++)
            {
                pages[i - 2] = args[i];
            }
            Console.WriteLine();
            GhostscriptProcessor gp = new GhostscriptProcessor(GhostscriptVersionInfo.GetLastInstalledVersion(), true);
            foreach (string p in pages)
            {
                gp.Processing += GPProcess;
                gp.StartProcessing(GSExtractSwitches(inFile, "temp" + p + ".pdf", p), null);
                while (gp.IsRunning) { }

            }
            gp.Processing += GPProcess;
            gp.StartProcessing(GSCombineSwitches(pages, outFile), null);
            while (gp.IsRunning) { }
            foreach (string p in pages)
            {
                DeleteTempFiles(p);
            }
        }

        static string[] GSExtractSwitches(string inFile, string outFile, string page)
        {
            return new string[]
                               {
                                   "-empty",
                                   "-sDEVICE=pdfwrite",
                                   "-dNOPAUSE",
                                   "-dBATCH",
                                   "-dSAFER",
                                    "-dFirstPage="+page,
                                    "-dLastPage="+page,
                                   "-sOutputFile=" + outFile,
                                   inFile
                               };
        }

        static string[] GSCombineSwitches(string[] inFiles, string outFile)
        {
            string[] pf = new string[]
                               {
                                   "-empty",
                                   "-sDEVICE=pdfwrite",
                                   "-dNOPAUSE",
                                   "-dBATCH",
                                   "-dSAFER",
                                   "-sOutputFile=" + outFile,
                               };
            int le = pf.Length + inFiles.Length;
            string[] rsw = new string[le];
            for (int i = 0; i < le; i++)
            {
                if (i < pf.Length)
                {
                    rsw[i] = pf[i];
                }
                else
                {
                    rsw[i] = "temp" + inFiles[i - pf.Length] + ".pdf";
                }
            }
            return rsw;
        }

        static void DeleteTempFiles(string page)
        {
            File.Delete("temp" + page + ".pdf");
        }

        static void GPProcess(object sender, GhostscriptProcessorProcessingEventArgs e)
        {

        }
    }
}
