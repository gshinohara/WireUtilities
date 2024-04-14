using Eto.Drawing;
using Eto.Forms;
using Grasshopper.Kernel;
using Rhino.UI;
using System;

namespace WireUtilities.Forms
{
    /// <summary>
    /// Checks path in wiring, just showing information.
    /// </summary>
    public class PathChecker : Form
    {
        /// <summary>
        /// Left parameter in wiring.
        /// </summary>
        private IGH_Param m_left;
        /// <summary>
        /// Right parameter in wiring.
        /// </summary>
        private IGH_Param m_right;

        /// <summary>
        /// Construct a form instance.
        /// </summary>
        /// <param name="left">Left parameter in wiring.</param>
        /// <param name="right">Right parameter in wiring.</param>
        public PathChecker(IGH_Param left, IGH_Param right)
        {
            m_left = left;
            m_right = right;
        }

        /// <summary>
        /// Construct this layout.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            var pt = GH_Convert.ToPoint(Grasshopper.Instances.ActiveCanvas.Viewport.ProjectPoint(m_left.Attributes.InputGrip));

            Title = "Remark Path Match";
            AutoSize = true;
            Resizable = false;
            Padding = 15;
            Topmost = true;
            Location = new Point(pt.X, pt.Y);

            Button close = new Button { Text = "Close" };
            close.Click += (sender, arg) => Close();

            DynamicLayout layout = new DynamicLayout { Spacing = new Size(20, 5) };

            layout.AddSeparateRow(new Label
            {
                Text = "Too much difference in matching data trees.",
                TextAlignment = TextAlignment.Center,
            });
            layout.AddSeparateRow(null);
            layout.AddSeparateRow(new ImageView { Image = ImageFunction.DrawImage(m_left, m_right).ToEto(), Width = 500, Height = 300 });
            layout.AddSeparateRow(null);
            layout.AddSeparateRow(m_right.PathDescription());
            layout.AddSeparateRow(null);
            layout.AddSeparateRow(new Rhino.UI.Controls.Divider());
            layout.AddSeparateRow(null);
            layout.AddSeparateRow(m_left.PathDescription());
            layout.AddSeparateRow(null);
            layout.AddSeparateRow(null, close, null);

            Content = layout;

            base.OnShown(e);
        }
    }
}
