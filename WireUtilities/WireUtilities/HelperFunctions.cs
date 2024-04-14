using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using System;
using System.Reflection;

namespace WireUtilities
{
    public static class HelperFunctions
    {
        /// <summary>
        /// Extended method of GH_WireInteraction. TODO: Exceptions
        /// </summary>
        /// <param name="wireInteraction"></param>
        /// <param name="source">The parameter of the start grip at a wire.</param>
        /// <param name="target">The parameter of the end grip at a wire.</param>
        /// <param name="mode">Link modes at a wire, "Replace" of the new wiring, "Add" of the shift key wiring, "Remove" of the control key wiring.</param>
        public static void WireProperties(this GH_WireInteraction wireInteraction, out IGH_Param source, out IGH_Param target, out string mode)
        {
            Func<string, object> get_field = name => wireInteraction.GetType().GetField(name, BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(wireInteraction);

            source = get_field("m_source") as IGH_Param;
            target = get_field("m_target") as IGH_Param;
            mode = get_field("m_mode").ToString();
        }
    }
}
