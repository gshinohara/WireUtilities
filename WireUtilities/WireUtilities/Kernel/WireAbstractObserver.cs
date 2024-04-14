using Grasshopper;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WireUtilities.Kernel
{
    public abstract class WireAbstractObserver : GH_AssemblyPriority
    {
        private TaskCompletionSource<GH_WireInteraction> m_tcs_Interaction = new TaskCompletionSource<GH_WireInteraction>();
        private TaskCompletionSource<bool> m_tcs_TargetChanged = new TaskCompletionSource<bool>();
        private event EventHandler TargetChanged;

        private IGH_Param m_target = null;
        private IGH_Param Target
        {
            get => m_target;
            set
            {
                if (m_target != value)
                    TargetChanged.Invoke(null, new EventArgs());
                m_target = value;
            }
        }

        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.CanvasCreated += Instances_CanvasCreated;
            return GH_LoadingInstruction.Proceed;
        }

        private void Instances_CanvasCreated(GH_Canvas canvas)
        {
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            TargetChanged += (sender, e) => m_tcs_TargetChanged.TrySetResult(true);
        }

        private async void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            while (true)
            {
                m_tcs_Interaction = new TaskCompletionSource<GH_WireInteraction>();
                m_tcs_TargetChanged = new TaskCompletionSource<bool>();
                GH_WireInteraction wireInteraction = await m_tcs_Interaction.Task;
                await m_tcs_TargetChanged.Task;
                ActionPreWire(wireInteraction);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is GH_Canvas canvas && canvas.ActiveInteraction is GH_WireInteraction wireInteraction)
            {
                wireInteraction.WireProperties(out IGH_Param source, out IGH_Param target, out string mode, out bool isDragFromInput);
                Target = target;
                if (source != null && target != null)
                    m_tcs_Interaction.TrySetResult(wireInteraction);
            }
        }

        /// <summary>
        /// An action after wiring on a grip.
        /// </summary>
        /// <param name="wireInteraction">Active wire interaction.</param>
        protected abstract void ActionPreWire(GH_WireInteraction wireInteraction);
    }
}
