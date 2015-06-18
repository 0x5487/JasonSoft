using System;
using System.Diagnostics;

namespace JasonSoft
{
    public class DosCommand
    {
        public DosCommand(String command)
        {
            _command = command;
        }

        public DosCommand(String command, String arugment)
        {
            _command = command;
            _arugment = arugment;
        }

        private String _command, _arugment;

        public String ErrorMessage { get; private set; }
        public String OutputMessage { get; private set; }

        public Boolean Start()
        {
            //FileInfo commandFile = new FileInfo(_commandFullPath);
            //if(commandFile.Exists == false) throw new FileNotFoundException();

            Boolean result = false;

            Process p = new Process();
            p.StartInfo.FileName = _command;
            if(_arugment.IsNullOrEmpty() == false) p.StartInfo.Arguments = _arugment;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;

            // These two optional flags ensure that no DOS window appears
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();
            ErrorMessage = p.StandardError.ReadToEnd();
            OutputMessage = p.StandardOutput.ReadToEnd();
            if(p.ExitCode == 0) result = true;
            p.Close();
            p.Dispose();

            return result;
        }
    }
}
