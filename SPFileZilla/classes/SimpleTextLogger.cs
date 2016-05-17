using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BandR
{
    public static class SimpleTextLogger
    {

        private const bool USE_LOG = true; // #changeme should be false

        /// <summary>
        /// </summary>
        public static void Write(string msg)
        {
            try
            {
                if (System.Environment.MachineName == "PERSEUS" || USE_LOG)
                {
                    System.IO.File.AppendAllText("c:\\temp\\spfilezilla.log.txt", DateTime.Now.ToString("o") + ": " + msg + Environment.NewLine);
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

    }
}
