using System;
using System.Linq;
using System.Windows.Forms;

namespace ConsoleMessageBox
{
    /// <summary>
    /// Gibt alle Kommandozeilenparameter durch NewLine getrennt
    /// in einer MessageBox aus. Bei mehr als einem Kommandozeilenparameter
    /// wird der letzte übergebene Parameter für die Überschrift der
    /// MessageBox verwendet.
    /// </summary>
    /// <remarks>
    /// File: Program.cs
    /// Autor: Erik Nagel
    ///
    /// 20.03.2013 Erik Nagel: erstellt
    /// </remarks>
    public class Program
    {
        static void Main(string[] args)
        {
            // DEBUG: MessageBox.Show(String.Join(" -|- ", args));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            string header;
            string msg = "";
            MessageBoxIcon icon;
            string delim = "";
            int aufrufCounter;

            if (args.Length < 3)
            {
                SyntaxAndOut("");
            }
            if (!Int32.TryParse(args[0], out aufrufCounter))
            {
                SyntaxAndOut("Der AufrufCounter ist nicht numerisch!");
            }
            args = args.Skip(3).ToArray();
            header = "Information";
            icon = MessageBoxIcon.Information;
            if (aufrufCounter < 0)
            {
                header = "Entwarnung";
                msg = "Das Problem ist behoben. Die ursprüngliche Meldung war:";
                delim = Environment.NewLine;
            }
            for (int i = 0; i < args.Length - 1; i++)
            {
                msg += delim + args[i];
                delim = Environment.NewLine;
            }
            if (args.Length < 2)
            {
                msg += delim + args[0]; // Es wurde nur die Meldung übergeben.
            }
            else
            {
                if (aufrufCounter >= 0)
                {
                    // letzter (User-)Parameter wird zur Überschrift, wenn keine Entwarnung vorliegt.
                    header = args[args.Length - 1];
                }
            }
            if (header.ToUpper().Contains("ERROR") || header.ToUpper().Contains("FEHLER")
                || header.ToUpper().Contains("EXCEPTION"))
            {
                icon = MessageBoxIcon.Error;
            }
            else
            {
                if (header.ToUpper().StartsWith("WARN"))
                {
                    header = "Warnung";
                    icon = MessageBoxIcon.Exclamation;
                }
            }

            MessageBox.Show(msg.Replace("#", Environment.NewLine), header, MessageBoxButtons.OK, icon);
        }

        private static void SyntaxAndOut(string message)
        {
            string intro = String.IsNullOrEmpty(message) ? "" : message + Environment.NewLine;
            MessageBox.Show(intro + "Mit diesem Vishnu-Worker können Meldungen in einer MessageBox ausgegeben werden."
                            + Environment.NewLine + String.Format($"Aufruf: {System.Windows.Forms.Application.ProductName} <Aufruf-Zähler> <Tree-Info> <Node-Info> <TreeEvent-Info> [Nachricht-Zeile[0..n]] [Überschrift]")
                            + Environment.NewLine + String.Format($"Beispiel: {System.Windows.Forms.Application.ProductName} -1 \"Tree 1\" \"Root\" \"Server-1: Root:False\" \"Info\""));
            Environment.Exit(0);
        }
    }
}
