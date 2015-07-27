using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gitIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            var contents = args[0];

            var integration = new GitIntegration();
            var url = integration.CreateGist(contents);

            GoToGist(url.Result);
        }

        private static void GoToGist(string target)
        {
            if (String.IsNullOrWhiteSpace(target))
            {
                //ShowStatus($"No link provided");
                return;
            }
            try
            {
                System.Diagnostics.Process.Start(target);
                //ShowStatus($"Gist saved in {target}");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    //ShowStatus($"Unable to open a web browser.");
                }
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
            }
            catch (System.Exception other)
            {
                //ShowStatus($"Another error.");
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
            }
        }
    }
}
