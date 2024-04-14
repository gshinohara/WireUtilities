using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace WireUtilities.Kernel
{
    public class WireUtilitiesInfo : GH_AssemblyInfo
    {
        public override string Name => "WiringUtilities";

        public override Bitmap Icon => null;

        public override string Description => "";

        public override Guid Id => new Guid("9259079c-11d0-46f4-b937-91c1f219e084");

        public override string AuthorName => "";

        public override string AuthorContact => "";
    }
}