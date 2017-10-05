using System;
using System.Collections.Generic;
using Essence.Geometry.Core;
using Essence.Geometry.Core.Double;
using Essence.Util;
using Essence.Util.Events;

namespace Essence.View.Models
{
    public interface IRenderContext3D : INotifyPropertyChangedEx
    {
        IServiceProvider ServiceProvider { get; }

        ITransform3 ModelView { get; }

        BoundingBox3d BoundingBoxInView { get; }
        BoundingBox3d BoundingBoxInModel { get; }

        void Transform(ITransform3 transform);

        void PushAttributes(ulong attribs);

        void PopAttributes();

        event EventHandler<AttributesEventArgs> PushingAttributes;
        event EventHandler<AttributesEventArgs> PopingAttributes;
    }

    public class AttributesEventArgs : EventArgs
    {
        public AttributesEventArgs(ulong attributes)
        {
            this.Attributes = attributes;
        }

        public ulong Attributes { get; set; }
    }

    public class RenderContext3D : AbsNotifyPropertyChanged, IRenderContext3D
    {
        public const string MODEL_VIEW = "ModelView";

        #region protected

        protected virtual bool Push(ulong attrib)
        {
            switch (((RenderAttrib)attrib) & RenderAttrib.Mask)
            {
                case RenderAttrib.ModelView:
                    this.PushValue(this.ModelView);
                    return true;

                case RenderAttrib.BoundingBoxInView:
                    this.PushValue(this.BoundingBoxInView);
                    return true;
            }
            return false;
        }

        protected virtual bool Pop(ulong attrib)
        {
            switch (((RenderAttrib)attrib) & RenderAttrib.Mask)
            {
                case RenderAttrib.ModelView:
                    this.modelView = (ITransform3)this.PopValue();
                    return true;

                case RenderAttrib.BoundingBoxInView:
                    this.boundingBoxInView = (BoundingBox3d)this.PopValue();
                    this.boundingBoxInModel = null;
                    return true;
            }
            return false;
        }

        protected void PushValue(object value)
        {
            this.values.Push(value);
        }

        protected object PopValue()
        {
            return this.values.Pop();
        }

        protected void OnPushingAttributes(AttributesEventArgs args)
        {
            if (this.PushingAttributes != null)
            {
                this.PushingAttributes(this, args);
            }
        }

        protected void OnPopingAttributes(AttributesEventArgs args)
        {
            if (this.PopingAttributes != null)
            {
                this.PopingAttributes(this, args);
            }
        }

        #endregion

        #region private

        private ITransform3 modelView;
        private BoundingBox3d boundingBoxInView;
        private BoundingBox3d? boundingBoxInModel;

        private readonly Stack<object> values = new Stack<object>();

        #endregion

        #region IRenderContext3D

        public IServiceProvider ServiceProvider { get; set; }

        public ITransform3 ModelView
        {
            get { return this.modelView; }
        }

        public BoundingBox3d BoundingBoxInView
        {
            get { return this.boundingBoxInView; }
        }

        public BoundingBox3d BoundingBoxInModel
        {
            get
            {
                if (this.boundingBoxInModel == null)
                {
                    this.boundingBoxInModel = this.ModelView.DoTransform(this.BoundingBoxInView);
                }
                return (BoundingBox3d)this.boundingBoxInModel;
            }
        }

        public void Transform(ITransform3 transform)
        {
            ITransform3 oldValue = this.modelView;
            this.modelView = this.modelView.Concat(transform);
            this.OnPropertyChanged(MODEL_VIEW, oldValue, this.modelView);
        }

        public void PushAttributes(ulong attribs)
        {
            List<ulong> aux = new List<ulong>();
            FlagsUtils.DecomposeFlag(attribs, aux);

            foreach (ulong attrib in aux)
            {
                this.Push(attrib);
            }

            this.PushValue(attribs);

            this.OnPushingAttributes(new AttributesEventArgs(attribs));
        }

        public void PopAttributes()
        {
            ulong attribs = (ulong)this.PopValue();

            List<ulong> aux = new List<ulong>();
            FlagsUtils.DecomposeFlag(attribs, aux);
            aux.Reverse();

            foreach (ulong attrib in aux)
            {
                this.Pop(attrib);
            }

            this.OnPopingAttributes(new AttributesEventArgs(attribs));
        }

        public event EventHandler<AttributesEventArgs> PushingAttributes;
        public event EventHandler<AttributesEventArgs> PopingAttributes;

        #endregion
    }

    [Flags]
    public enum RenderAttrib : ulong
    {
        None = 0x0,

        ModelView = 0x01,
        BoundingBoxInView = 0x02,

        Mask = 0x03
    }
}