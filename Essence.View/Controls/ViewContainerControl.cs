using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using Essence.View.Views;
using Essence.Util.Collections;

namespace Essence.View.Controls
{
    public class ViewContainerControl<TControl> : ViewControl<TControl>,
                                                  IViewContainer
        where TControl : Control, new()
    {
        public ViewContainerControl()
        {
            this.views.ListChanged += this.Views_ListChanged;
        }

        public virtual void SetConstraints(IView view, object constraints)
        {
            int index = this.views.IndexOf(view);
            Contract.Assert(index >= 0);
            this.constraints[index] = constraints;
        }

        #region private

        private void Views_ListChanged(object sender, ListEventArgs args)
        {
            this.DoUpdateContent(
                () =>
                {
                    args.ForEach<IView>(
                        (v, i) =>
                        {
                            IRefControl refControl = (IRefControl)v;
                            this.Control.Controls.Add(refControl.Control);
                            v.Container = this;

                            this.constraints.Insert(i, null);
                        },
                        (v, i) =>
                        {
                            IRefControl refControl = (IRefControl)v;
                            this.Control.Controls.Remove(refControl.Control);
                            v.Container = null;

                            this.constraints.RemoveAt(i);
                        });

                    // Se reordena.
                    int index = 0;
                    foreach (Control control in this.Views.Cast<IRefControl>().Select(rc => rc.Control))
                    {
                        this.Control.Controls.SetChildIndex(control, index++);
                    }
                });
        }

        private void Control_ControlAdded(object sender, ControlEventArgs args)
        {
            this.DoUpdateContent(
                () =>
                {
                    IView view = GetView(args.Control);
                    int index = this.Control.Controls.IndexOf(args.Control);
                    this.Views.Insert(index, view);
                });
        }

        private void Control_ControlRemoved(object sender, ControlEventArgs args)
        {
            this.DoUpdateContent(
                () =>
                {
                    IView view = GetView(args.Control);
                    int index = this.Control.Controls.IndexOf(args.Control);
                    this.Views.RemoveAt(index);
                });
        }

        private void DoUpdateContent(Action action)
        {
            if (this.updatingContent)
            {
                return;
            }
            this.updatingContent = true;
            action();
            this.updatingContent = false;
        }

        private bool updatingContent;
        private readonly EventList<IView> views = new EventList<IView>();
        private readonly List<object> constraints = new List<object>();

        #endregion

        #region ViewControl<TControl>

        protected override void AttachControl()
        {
            base.AttachControl();
            this.Control.ControlAdded += this.Control_ControlAdded;
            this.Control.ControlRemoved += this.Control_ControlRemoved;
        }

        protected override void DeattachControl()
        {
            this.Control.ControlAdded -= this.Control_ControlAdded;
            this.Control.ControlRemoved -= this.Control_ControlRemoved;
            base.DeattachControl();
        }

        #endregion

        #region IViewContainer

        public IList<IView> Views
        {
            get { return this.views; }
        }

        public void AddView(IView view, object constraints)
        {
            this.views.Add(view);
            this.SetConstraints(view, constraints);
        }

        public void InsertView(int index, IView view, object constraints)
        {
            this.views.Insert(index, view);
            this.SetConstraints(view, constraints);
        }

        #endregion
    }
}