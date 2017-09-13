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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Essence.Geometry.Core.Byte;
using Essence.View.Controls.Osg;
using osg;
using osgEarthUtil;
using osgViewer;
using OSGDLL;

namespace Essence.View.Controls.Forms
{
    public class Drawing3DOSGControl : Essence.View.Forms.SimpleOSGControl
    {
        public const string BACKGROUND_COLOR = "BackgroundColor";

        public Drawing3DOSGControl()
        {
            this.SetStyle(ControlStyles.Selectable, true);
            this.UpdateStyles();
        }

        /// <summary>
        /// Color de fondo.
        /// </summary>
        public Color4b BackgroundColor
        {
            get { return this.colorFondo; }
            set
            {
                if (!Object.Equals(this.colorFondo, value))
                {
                    Color4b oldValue = this.colorFondo;
                    this.colorFondo = value;
                    //this.OnPropiedadModificada(BACKGROUND_COLOR, oldValue, value);

                    this.Invalidate();
                }
            }
        }

        #region private

        /// <summary>Color de fondo.</summary>
        private Color4b colorFondo = new Color4b(0, 0, 0, 0);

        /// <summary>Objetos OSG. Evita que se liberen.</summary>
        private readonly List<OSGObject> osgObjects = new List<OSGObject>();

        /// <summary>Required designer variable.</summary>
        private IContainer components = null;

        /// <summary>Menú de contexto.</summary>
        private ContextMenuStrip contextMenu;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Margin = Padding.Empty;
            this.Padding = Padding.Empty;
        }

        #endregion

        #endregion

        #region SimpleOSGControl

        protected override void CreateOSGWindow()
        {
            base.CreateOSGWindow();

            //ScreenCaptureHandler screenCaptureHandler = new ScreenCaptureHandler(new ScreenCaptureHandler.WriteToFile("Frame", "png", ScreenCaptureHandler.WriteToFile.SavePolicy.SEQUENTIAL_NUMBER), -1);
            //StatsHandler statsHandler = new StatsHandler();
            WindowSizeHandler windowSizeHandler = new WindowSizeHandler();

            osgViewer_View view = this.Viewer.asOsgViewerView();
            Camera camera = view.getCamera();

            view.addEventHandler(windowSizeHandler);

            this.osgObjects.Add(windowSizeHandler);

            camera.setClearColor(new Vec4f(0f, 0f, 0f, 1));

            CullSettings cullSettings = camera.asCullSettings();
            cullSettings.setComputeNearFarMode(CullSettings.ComputeNearFarMode.DO_NOT_COMPUTE_NEAR_FAR);
            //cullSettings.setNearFarRatio(0.00002);
            //cullSettings.setNearFarRatio(0.0001);
            //cullSettings.setSmallFeatureCullingPixelSize(-1.0f);

            DepthPartitionSettings dps = new DepthPartitionSettings(DepthPartitionSettings.DepthMode.FIXED_RANGE);
            dps._zNear = 1.0; // 0.1
            dps._zMid = 1e4; // 500 / 10
            dps._zFar = 2e8; // 20000 / 5000
            view.setUpDepthPartition(dps);

            // NOTA: no funciona correctamente con los shaders.
            if (!OSGUtils.UseShaders)
            {
                LogarithmicDepthBuffer buf = new LogarithmicDepthBuffer();
                buf.install(camera);
            }

            // Se eliminan todas las luces.
            view.setLightingMode(osg.View.LightingMode.NO_LIGHT);
            view.setLight(null);

            //EarthManipulator manipulator = new EarthManipulator();
            //manipulator.setViewpoint(new Viewpoint(-71.0763, 42.34425, 0, 24.261, -21.6, 3450.0), 5.0);
            //this.Vista.OsgManipulator = manipulator;

            /*// Se establece el manipulador por defecto: tipo trackball.
            TrackballManipulator manipulator = new MyTrackballManipulator();
            manipulator.setHomePosition(new Vec3d(-50, 0, 2), new Vec3d(0, 0, 0), new Vec3d(0, 0, 1));
            manipulator.setTransformation(new Vec3d(-50, 0, 2), new Vec3d(0, 0, 0), new Vec3d(0, 0, 1));
            //manipulator.home(0);
            //manipulator.setAutoComputeHomePosition(true);
            manipulator.setAllowThrow(false);
            manipulator.setVerticalAxisFixed(true);
            manipulator.setTrackballSize(1);

            this.Manipulator = manipulator;*/
        }

        #endregion

        #region UserControl

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
            this.Viewer.setThreadingModel(ViewerBase.ThreadingModel.SingleThreaded);
            this.Invalidate();
        }

        #endregion

        #region Component

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.contextMenu != null)
                {
                    // Se crea una copia para liberar despues los elementos.
                    ArrayList aux = new ArrayList(this.contextMenu.Items);

                    // Se vacia el menu.
                    this.contextMenu.Items.Clear();

                    // Se liberan.
                    foreach (ToolStripItem tsi in aux)
                    {
                        tsi.Dispose();
                    }

                    this.contextMenu.Dispose();
                    this.contextMenu = null;
                }

                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}