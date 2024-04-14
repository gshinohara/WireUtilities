using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using WireUtilities;
using WireUtilities.Forms;
using WireUtilities.Kernel;

namespace WireUtilities_Test
{
    public class WireObserver_Test : WireAbstractObserver
    {
        protected override void ActionPreWire(GH_WireInteraction wireInteraction)
        {
            wireInteraction.WireProperties(out IGH_Param source, out IGH_Param target, out string mode, out bool isDragFromInput);
            if (source != null && target != null && mode != null)
            {
                Rhino.RhinoApp.WriteLine($"Wire {mode}! source: {source.Name}, target: {target.Name}");

                IGH_Param left, right;
                if (isDragFromInput)
                {
                    left = target;
                    right = source;
                }
                else
                {
                    left = source;
                    right = target;
                }

                PathChecker pathChecker = new PathChecker(left, right);
                pathChecker.Show();
            }
        }
    }
}
