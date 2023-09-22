using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Plan
{
    public class PlanInfo : GH_AssemblyInfo
    {
        public override string Name => "Plan";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("3F319F9A-1AA2-49C0-B42F-00A801299D5C");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}