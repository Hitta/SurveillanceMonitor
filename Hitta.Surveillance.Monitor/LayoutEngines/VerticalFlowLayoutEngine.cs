using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Hitta.Surveillance.Monitor.LayoutEngines
{
    public class VerticalFlowLayoutEngine : LayoutEngine
    {
        static int GetMinAmountOfColumns(IEnumerable<Control> controls, Rectangle bounds)
        {
            var columnCountQueue = new Queue<Control>(controls.Reverse());

            int columns = 0;
            while (true)
            {
                int stackHeight = 0;

                if (columnCountQueue.DequeueWhile(control => (stackHeight == 0 | (stackHeight += control.Margin.Top + control.Margin.Bottom + control.MinimumSize.Height) <= bounds.Height)).Count() == 0)
                    break;

                columns++;
            }
            return columns;
        }
        static IList<IList<Control>> InitControlColumns(IList<Control> childControls, int columns)
        {
            var controls = new List<IList<Control>>();
            controls.Add(childControls);

            for (int i = 1; i < columns; i++)
            {
                controls.Add(new List<Control>());
            }
            return controls;
        }
        
        static IEnumerable<Control> getVisibleChildControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Visible) yield return control;
            }
        }

        public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
        {
            Control parent = container as Control;

            if (parent == null) return base.Layout(container, layoutEventArgs);

            Rectangle parentDisplayRectangle = parent.DisplayRectangle;

            IList<Control> childControls = getVisibleChildControls(parent).ToList();

            if (childControls.Count == 0) return base.Layout(container, layoutEventArgs);

            int maxLeftMargin = childControls.Max(control => control.Margin.Left);
            int maxRightMargin = childControls.Max(control => control.Margin.Right);

            int columns = GetMinAmountOfColumns(childControls, parentDisplayRectangle);
            var controlColumns = InitControlColumns(childControls, columns);

            int childWidth = (parentDisplayRectangle.Width - (columns * (maxLeftMargin + maxRightMargin))) / columns;


            bool controlsMoved = true;
            while (controlsMoved)
            {
                controlsMoved = false;
                for (int column = 0; column < controlColumns.Count - 1; column++)
                {
                    Control topControl = controlColumns[column].Last();
                    int topControlHeight = topControl.Margin.Top + topControl.Margin.Bottom + topControl.MinimumSize.Height;
                    int columnHeight = controlColumns[column].Sum(control => control.Margin.Top + control.Margin.Bottom + control.MinimumSize.Height);
                    int succeedingColumnHeight = controlColumns[column + 1].Sum(control => control.Margin.Top + control.Margin.Bottom + control.MinimumSize.Height);

                    int heightDiff = Math.Abs(columnHeight - succeedingColumnHeight);

                    if(Math.Abs((columnHeight - topControlHeight) - (succeedingColumnHeight + topControlHeight)) <= heightDiff)
                    {
                        //move control!
                        controlColumns[column + 1].Insert(0, controlColumns[column].Last());
                        controlColumns[column].Remove(controlColumns[column].Last());
                        controlsMoved = true;
                    }
                }
            }


            for (int column = 0; column < columns; column++ )
            {
                double controlsHeight = controlColumns[column].Sum(control => control.MinimumSize.Height);
                double marginsHeight = controlColumns[column].Sum(control => control.Margin.Top + control.Margin.Bottom);
                double scaleFactor = (parentDisplayRectangle.Height - marginsHeight) / controlsHeight;
                Point childOffset = new Point((childWidth + maxLeftMargin + maxRightMargin) * column + maxLeftMargin + parentDisplayRectangle.Left, parentDisplayRectangle.Top);

                foreach (var control in controlColumns[column])
                {
                    childOffset.Offset(0, control.Margin.Top);
                    control.Location = new Point(childOffset.X, childOffset.Y);
                    control.Height = (int)(control.MinimumSize.Height * scaleFactor);
                    control.Width = childWidth;
                    childOffset.Offset(0, control.Height + control.Margin.Bottom);
                }
            }

            return false;
        }
    }
}
