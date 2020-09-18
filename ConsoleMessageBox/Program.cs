using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string msg = "";
            string delim = "";
            string aufrufInfo = "Aufruf-Zähler fehlt!";
            string countString = "";
            int aufrufCounter = 0;
            if (args.Length < 3)
            {
                SyntaxAndOut("");
            }
            countString = args[0];
            if (!Int32.TryParse(countString, out aufrufCounter))
            {
                SyntaxAndOut("Der AufrufCounter ist nicht numerisch!");
            }
            args = args.Skip(3).ToArray();
            if (aufrufCounter < 0)
            {
                aufrufInfo = "Das Problem ist behoben. Die ursprüngliche Meldung war:";
                delim = Environment.NewLine;
            }
            else
            {
                //aufrufInfo = countString + ". Warnung";
                aufrufInfo = "";
            }
            msg = aufrufInfo;
            for (int i = 0; i < args.Length - 1; i++)
            {
                msg += delim + args[i];
                delim = Environment.NewLine;
            }
            MessageBoxIcon icon = MessageBoxIcon.Information;
            if (msg != aufrufInfo) // letzter (User-)Parameter wird zur Überschrift.
            {
                string header = args[args.Length - 1];
                if (aufrufCounter < 0)
                {
                    header = "Entwarnung";
                }
                if (header.ToUpper().Contains("ERROR") || header.ToUpper().Contains("FEHLER") || header.ToUpper().Contains("EXCEPTION"))
                {
                    icon = MessageBoxIcon.Error;
                }
                MessageBox.Show(msg.Replace("#", Environment.NewLine), header, MessageBoxButtons.OK, icon);
            }
            else // letzter (User-)Parameter ist die Meldung
            {
                if (args.Length > 0)
                {
                    msg += delim + args[args.Length - 1];
                    string header = "Information";
                    if (msg.ToUpper().Contains("ERROR") || msg.ToUpper().Contains("FEHLER") || msg.ToUpper().Contains("EXCEPTION"))
                    {
                        icon = MessageBoxIcon.Error;
                        header = "Fehler";
                    }
                    MessageBox.Show(msg.Replace("#", Environment.NewLine), header, MessageBoxButtons.OK, icon);
                }
            }
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
