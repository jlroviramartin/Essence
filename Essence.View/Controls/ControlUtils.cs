using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using Essence.View.Views;

namespace Essence.View.Controls
{
    public static class ControlUtils
    {
        public static void ApplyConstraints(IView parent, IView child, object constraints)
        {
            Contract.Assert(parent is IRefControl && child is IRefControl);
            ApplyConstraints(((IRefControl) parent).Control, ((IRefControl)child).Control, constraints);
        }

        public static void ApplyConstraints(Control parent, Control child, object constraints)
        {
            if (constraints is DockConstraints)
            {
                ApplyConstraints(parent, child, (DockConstraints) constraints);
                return;
            }
            throw new NotSupportedException();
        }

        public static void ApplyConstraints(Control parent, Control control, DockConstraints constraints)
        {
            switch (constraints)
            {
                case DockConstraints.None:
                    control.Dock = DockStyle.None;
                    break;
                case DockConstraints.Top:
                    control.Dock = DockStyle.Top;
                    break;
                case DockConstraints.Bottom:
                    control.Dock = DockStyle.Bottom;
                    break;
                case DockConstraints.Left:
                    control.Dock = DockStyle.Left;
                    break;
                case DockConstraints.Right:
                    control.Dock = DockStyle.Right;
                    break;
                case DockConstraints.Fill:
                    control.Dock = DockStyle.Fill;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }
}