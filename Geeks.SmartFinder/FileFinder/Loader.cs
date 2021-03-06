using System.ComponentModel;
using System.Windows.Forms;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    internal abstract class Loader : BackgroundWorker
    {
        public abstract void LoadOptions();

        public abstract void DisplayOptions(ContextMenuStrip mnuOptions);

        public abstract void OptionClicked(ToolStripMenuItem toolStripMenuItem, ref bool searchAgain, ref bool loadAgain);
    }
}
