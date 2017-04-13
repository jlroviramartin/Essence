// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Essence.Util.Services;
using Essence.View.Controls.Forms;
using Essence.View.Models;
using Essence.View.Views;
using osg;
using osgGA;
using osgViewer;

namespace Essence.View.Controls
{
    public class Drawing3DViewControl<TControl> : ViewControl<TControl>, IDrawing3DView
        where TControl : Drawing3DOSGControl, new()
    {
        public const string OSG_MANIPULATOR = "OsgManipulator";
        public const string DRAWING_MODEL = "DrawingModel";

        /// <summary>
        ///     Manipulador de la camara.
        /// </summary>
        public CameraManipulator OsgManipulator
        {
            get { return this.manipulator; }
            set
            {
                if (this.manipulator != value)
                {
                    CameraManipulator oldValue = this.manipulator;
                    this.manipulator = value;

                    if (this.manipulator != null)
                    {
                        /*// Se actualiza el nodo (del manipulador de la camara).
                        if (this.DrawingModel != null)
                        {
                            this.manipulator.setNode(((ModeloDibujo3D)this.ModeloDibujo).CameraNode);
                        }*/
                    }

                    this.Control.Manipulator = this.manipulator;
                    this.OnPropertyChanged(OSG_MANIPULATOR, oldValue, value);
                }
            }
        }

        /// <summary>
        ///     <c>Viewer</c> de la vista.
        /// </summary>
        public Viewer OsgViewer
        {
            get { return this.Control.Viewer; }
        }

        /// <summary>
        ///     <c>Camera</c> de la vista.
        /// </summary>
        public Camera OsgCamera
        {
            get { return this.OsgViewer.asOsgViewerView().getCamera(); }
        }

        /// <summary>
        ///     <c>Group</c> raiz de la vista.
        /// </summary>
        public Group OsgRoot
        {
            get { return this.Control.Root; }
        }

        #region private

        private void DrawingModel_Invalidate(object sender, Invalidate3DEventArgs args)
        {
        }

        private void Control_UpdateOSG(object sender, EventArgs args)
        {
            if (this.DrawingModel != null)
            {
                RenderContext3D context = new RenderContext3D();

                MapServiceProvider serviceProvider = new MapServiceProvider();
                serviceProvider.Register<Group>(this.OsgRoot);

                context.ServiceProvider = serviceProvider;
                this.DrawingModel.Update(context);
            }
        }

        private IDrawing3DModel drawingModel;

        /// <summary>Camara de dibujo asociado a la vista.</summary>
        //private readonly Camara3D camara;
        /// <summary>Manipulador de la camara.</summary>
        private CameraManipulator manipulator;

        private Node cameraNode;
        private NodeCallback cameraNodeCallback;

        #endregion

        #region IDrawing2DView

        public IDrawing3DModel DrawingModel
        {
            get { return this.drawingModel; }
            set
            {
                if (this.drawingModel != value)
                {
                    if (this.drawingModel != null)
                    {
                        this.drawingModel.Invalidate -= this.DrawingModel_Invalidate;

                        //this.Control.Root.removeChild(this.cameraNode);
                        this.Control.Root = null;
                    }
                    IDrawing3DModel oldValue = this.drawingModel;
                    this.drawingModel = value;
                    if (this.drawingModel != null)
                    {
                        this.Control.Root = new Group();
                        //this.Control.Root.addChild(this.cameraNode);

                        this.drawingModel.Invalidate += this.DrawingModel_Invalidate;

                        // Se actualiza el nodo (del manipulador de la camara).
                        if (this.OsgManipulator != null)
                        {
                            //this.OsgManipulator.setNode(this.cameraNode);
                        }

                        //this.OsgCamera.addCullCallback(new AutoClipPlaneCullCallback(mapNode));
                    }
                    this.OnPropertyChanged(DRAWING_MODEL, oldValue, value);
                }
            }
        }

        #endregion

        #region ViewControl

        protected override void AttachControl()
        {
            base.AttachControl();
            this.Control.UpdateOSG += this.Control_UpdateOSG;
        }

        protected override void DeattachControl()
        {
            this.Control.UpdateOSG -= this.Control_UpdateOSG;
            base.DeattachControl();
        }

        #endregion
    }
}