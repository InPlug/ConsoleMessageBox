using System.Text;
using NetEti.ApplicationEnvironment;

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

            EvaluateParametersOrDie(args, out string message, out string caption, out MessageBoxButtons messageBoxButton, out MessageBoxIcon messageBoxIcon);

            MessageBox.Show(message, caption, messageBoxButton, messageBoxIcon);
        }

        private static void EvaluateParametersOrDie(string[] args, out string message, out string caption, out MessageBoxButtons messageBoxButton, out MessageBoxIcon messageBoxIcon)
        {
            CommandLineAccess commandLineAccess = new();

            messageBoxIcon = MessageBoxIcon.None;
            messageBoxButton = MessageBoxButtons.OK;
            string? tmpStr = commandLineAccess.GetStringValue("EscalationCounter", "0");
            bool isResetting = Int32.TryParse(tmpStr, out int escalationCounter) && escalationCounter < 0;

            caption = commandLineAccess.GetStringValue("Caption", "Information") ?? "Information";
            StringBuilder sb = new StringBuilder();

            string msg = commandLineAccess.GetStringValue("Message", null)
                ?? Die<string>("Es muss ein Meldungstext mitgegeben werden.", commandLineAccess.CommandLine);

            string resolvedPrefix = commandLineAccess.GetStringValue(
                "ResolvedPrefix", "Das Problem ist behoben. Die ursprüngliche Meldung war:") ?? "";

            tmpStr = commandLineAccess.GetStringValue("MessageNewLine", null);
            string[] messageLines;
            if (!String.IsNullOrEmpty(tmpStr))
            {
                messageLines = msg.Split(tmpStr);
            }
            else
            {
                messageLines = new string[1] { msg };
            }
            string delim = "";
            for (int i = 0; i < messageLines.Count(); i++)
            {
                sb.Append(delim + messageLines[i]);
                delim = Environment.NewLine;
            }
            message = sb.ToString();
            if (isResetting)
            {
                message = resolvedPrefix
                + Environment.NewLine
                        + message;
            }

            messageBoxIcon = MessageBoxIcon.Information;
            if (caption.ToUpper().Contains("ERROR") || caption.ToUpper().Contains("FEHLER")
                || caption.ToUpper().Contains("EXCEPTION"))
            {
                messageBoxIcon = MessageBoxIcon.Error;
            }
            else
            {
                if (caption.ToUpper().StartsWith("WARN") == true)
                {
                    messageBoxIcon = MessageBoxIcon.Exclamation;
                }
            }
            messageBoxButton = MessageBoxButtons.OK;

            if (isResetting)
            {
                messageBoxIcon = MessageBoxIcon.Information;
                caption = "(" + caption + ")";
            }
        }

        private static T Die<T>(string? message, string? commandLine = null)
        {
            string usage = "Syntax:"
                + Environment.NewLine
                + "\t-Message=<Nachricht>"
                + Environment.NewLine
                + "\t[-Caption=<Überschrift>]"
                + Environment.NewLine
                + "\t[-MessageNewLine=<NewLine-Kennung>]"
                + Environment.NewLine
                + "\t[-EscalationCounter={-n;+n} (negativ: Ursache behoben)]"
                + Environment.NewLine
                + "\t[-ResolvedPrefix=<Vorangestellter Kurztext bei negativem EscalationCounter>]"
                + Environment.NewLine
                + "Beispiel:"
                + Environment.NewLine
                + "\t-Message=\"Server-1:#Zugriffsproblem#Connection-Error...\""
                + Environment.NewLine
                + "\t-Caption=\"SQL-Exception\""
                + Environment.NewLine
                + "\t-ResolvedPrefix=\"No longer valid:\""
                + Environment.NewLine
                + "\t-MessageNewLine=\"#\"";
            if (commandLine != null)
            {
                usage = "Kommandozeile: " + commandLine + Environment.NewLine + usage;
            }
            MessageBox.Show(message + Environment.NewLine + usage);
            throw new ArgumentException(message + Environment.NewLine + usage);
        }

    }
}