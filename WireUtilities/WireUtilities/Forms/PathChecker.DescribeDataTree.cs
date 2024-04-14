using Eto.Drawing;
using Eto.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Collections;
using System.Linq;

namespace WireUtilities.Forms
{
    internal static class DescribeDataTree
    {
        /// <summary>
        ///  The helper function to make description of a param path 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="structure"></param>
        public static DynamicLayout PathDescription(this IGH_Param param)
        {
            DynamicLayout layout = new DynamicLayout();
            IGH_Structure structure = param.VolatileData;

            layout.DescribeTopLevel(param);
            layout.DescribeBranches(structure);
            layout.DescribePathCount(structure);

            return layout;
        }

        private static void DescribeTopLevel(this DynamicLayout layout, IGH_Param param)
        {
            Label paramname = new Label
            {
                Text = param.Name + (param.GetParentComponent(out IGH_Component component) ? $" ({component.Name})" : ""),
                Font = new Font(SystemFont.Bold)
            };
            Label access = new Label
            {
                Text = $" Access : {param.Access}",
                TextColor = Color.Parse("Gray"),
                Font = new Font(SystemFont.Bold),
            };
            layout.AddSeparateRow(paramname, null, access);
        }

        private static void DescribePathCount(this DynamicLayout layout, IGH_Structure structure)
        {
            Label pathcount = new Label
            {
                Text = $"Path Count : {structure.PathCount}",
                Font = new Font(SystemFont.Bold),
                TextColor = Color.Parse("Gray")
            };
            layout.AddSeparateRow(null, pathcount);
        }

        private static void DescribeBranches(this DynamicLayout layout, IGH_Structure structure)
        {
            int counter = 0;
            foreach (GH_Path path in structure.Paths)
            {
                bool stopper = false;
                if (counter < 6)
                {
                    IList list = structure.get_Branch(path);

                    Label pathLabel = new Label { Text = $"\t{path.ToString(true)}" };
                    Label listCountLabel = new Label { Text = $"List Count : {list.Count}", TextColor = Color.Parse("Gray") };
                    layout.AddSeparateRow(pathLabel, null, listCountLabel);

                    Label itemsLabel = new Label { Text = "\t\t" };
                    if (list.Count == 0)
                    {
                        itemsLabel.Text += "No items";
                        itemsLabel.TextColor = Color.Parse("Red");
                    }
                    else
                    {
                        itemsLabel.TextColor = Color.Parse("Gray");
                        foreach (var item in list)
                        {
                            if (item is Grasshopper.Kernel.Types.IGH_Goo goo)
                                itemsLabel.Text += $"{goo.TypeName}, ";
                            if (itemsLabel.Text.Count() > 60)
                            {
                                itemsLabel.Text += ".....";
                                break;
                            }
                        }
                        itemsLabel.Text = itemsLabel.Text.Substring(0, itemsLabel.Text.Length - 2);
                    }
                    layout.AddSeparateColumn(itemsLabel);
                }
                else
                    stopper = true;

                if (stopper)
                {
                    layout.AddSeparateColumn(new Label { Text = "..." });
                    break;
                }
                counter++;
            }
        }
    }
}
