using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Hitta.Surveillance.Monitor.LayoutEngines
{
    internal class HorizontalLayoutEngine : LayoutEngine
    {
        static IEnumerable<Control> GetVisibleChildControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Visible) yield return control;
            }
        }

        static LayoutData GetLayoutData(ICollection<Control> childControls, Rectangle bounds)
        {
            int totalMargin = childControls.Sum(control => control.Margin.Left + control.Margin.Right);
            int largestTopMargin = childControls.Max(control => control.Margin.Top);
            int largestBottomMargin = childControls.Max(control => control.Margin.Bottom);

            int childWidth = (bounds.Width - totalMargin) / childControls.Count;
            int childHeight = (bounds.Height) - (largestBottomMargin + largestTopMargin);

            return new LayoutData { ChildWidth = childWidth, ChildHeight = childHeight, LargestTopMargin = largestTopMargin, LargestBottomMargin = largestBottomMargin };
        }

        public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
        {
            Control parent = container as Control;

            if (parent == null) return base.Layout(container, layoutEventArgs);

            IList<Control> childControls = GetVisibleChildControls(parent).ToList();
            if (childControls.Count == 0) return base.Layout(container, layoutEventArgs);


            Rectangle parentDisplayRectangle = parent.DisplayRectangle;
            Point nextControlLocation = parentDisplayRectangle.Location;

            LayoutData layoutData = GetLayoutData(childControls, parentDisplayRectangle);

            nextControlLocation.Offset(0, layoutData.LargestTopMargin);

            foreach (Control childControl in childControls)
            {
                nextControlLocation.Offset(childControl.Margin.Left, 0);

                childControl.Height = layoutData.ChildHeight;
                childControl.Width = layoutData.ChildWidth;
                childControl.Location = nextControlLocation;

                nextControlLocation.Offset(childControl.Margin.Right + layoutData.ChildWidth, 0);
            }

            return false;
        }

        class LayoutData
        {
            public int ChildWidth { get; set; }
            public int ChildHeight { get; set; }
            public int LargestTopMargin { get; set; }
            public int LargestBottomMargin { get; set; }
        }
    }
}
