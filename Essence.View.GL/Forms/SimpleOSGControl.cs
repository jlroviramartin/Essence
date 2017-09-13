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
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using osg;
using osgEarthUtil;
using osgGA;
using osgViewer;
using EventHandler = System.EventHandler;
using Timer = System.Windows.Forms.Timer;

namespace Essence.View.Forms
{
    public class SimpleOSGControl : UserControl
    {
        #region Miembros publicos

        public SimpleOSGControl()
        {
            // Se inicializa el componente: lo utiliza VisualStudio.
            this.InitializeComponent();

            this.Name = "SimpleOSGControl";

            this.ResizeRedraw = true;
            this.DoubleBuffered = false;

            // Se modifican los estilos.
            this.SetStyle(ControlStyles.Opaque
                          | ControlStyles.ResizeRedraw
                          | ControlStyles.UserPaint
                          | ControlStyles.AllPaintingInWmPaint,
                          true);
            // No se emite Click ni DoubleClick.
            this.SetStyle(ControlStyles.StandardClick
                          | ControlStyles.StandardDoubleClick,
                          false);
            // Se procesa el control del raton.
            this.SetStyle(ControlStyles.UserMouse,
                          true);
            // Es seleccionable.
            this.SetStyle(ControlStyles.Selectable,
                          true);
            // No se utiliza DoubleBuffer.
            this.SetStyle(ControlStyles.DoubleBuffer
                          | ControlStyles.OptimizedDoubleBuffer,
                          false);
            this.UpdateStyles();

            this.EnableVSync = false;
        }

        /// <summary>Vista.</summary>
        public Viewer Viewer
        {
            get { return this.viewer; }
            set
            {
                if (this.viewer != value)
                {
                    this.ResetSceneData();
                    this.viewer = value;
                    this.SetSceneData();
                }
            }
        }

        /// <summary>Raiz.</summary>
        public Group Root
        {
            get { return this.root; }
            set
            {
                if (this.root != value)
                {
                    this.ResetSceneData();
                    this.root = value;
                    this.SetSceneData();
                }
            }
        }

        /// <summary>Camara.</summary>
        public Camera Camera
        {
            get { return this.camera; }
            /*private set
            {
                if (this.camera != value)
                {
                    this.camera = value;
                    this.viewer.asOsgViewerView().setCamera(this.camera);
                }
            }*/
        }

        /*/// <summary>Camara.</summary>
        public Camera PreCamera
        {
            get { return this.preCamera; }
            private set
            {
                if (this.preCamera != value)
                {
                    if (this.preCamera != null)
                    {
                        Viewer.asView(this.viewer).removeSlave(this.preCamera);
                    }
                    this.preCamera = value;
                    if (this.preCamera != null)
                    {
                        Viewer.asView(this.viewer).addSlave(this.preCamera);
                    }
                }
            }
        }*/

        /// <summary>Manipulador de la camara.</summary>
        public CameraManipulator Manipulator
        {
            get { return this.manipulator; }
            set
            {
                if (this.manipulator != value)
                {
                    this.manipulator = value;
                    this.viewer.asOsgViewerView().setCameraManipulator(this.manipulator);
                }
            }
        }

        /// <summary>
        /// Genera un frame.
        /// </summary>
        public void Frame()
        {
            this.DoFrame();
        }

        /// <summary>
        /// Indica que pare la generación de frames.
        /// </summary>
        public void PauseOSG()
        {
            if (useThread)
            {
                if (this.pauseOSGThread != null)
                {
                    this.pauseOSGThread.Reset();
                }
            }
            else if (useTimer)
            {
                if (this.timerPintar != null)
                {
                    this.timerPintar.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Indica que continue la generación de frames.
        /// </summary>
        public void ContinueOSG()
        {
            if (useThread)
            {
                if (this.pauseOSGThread != null)
                {
                    this.pauseOSGThread.Set();
                }
            }
            else if (useTimer)
            {
                if (this.timerPintar != null)
                {
                    this.timerPintar.Enabled = true;
                }
            }
        }

        public event EventHandler UpdateOSG;

        #endregion Miembros publicos _______________________________________________________________

        #region Miembros protegidos

        protected bool EnableVSync { get; set; }

        /// <summary>Contexto grafico.</summary>
        protected GraphicsContext GraphicsContext
        {
            get { return this.gc; }
        }

        /// <summary>
        /// Lanza el evento <c>UpdateOSG</c>.
        /// </summary>
        protected virtual void OnUpdateOSG(EventArgs args)
        {
            if (this.UpdateOSG != null)
            {
                this.UpdateOSG(this, args);
            }
        }

        /// <summary>
        /// Crea la ventana de <c>OSG</c>.
        /// </summary>
        protected virtual void CreateOSGWindow()
        {
            GraphicsContext.Traits traits = this.CreateTraits();

            Trace.WriteLine("Traits:");
            Trace.WriteLine(ToString(traits));

            this.gc = GraphicsContext.createGraphicsContext(traits);

            if (!this.gc.valid())
            {
                Trace.WriteLine("El GraphicsContext {0} no es valido", this.gc.ToString());
            }

            // Se establece el 'viewer'.
            Viewer viewer = new Viewer();
            viewer.asOsgViewerView().setLightingMode(osg.View.LightingMode.SKY_LIGHT);
            this.Viewer = viewer;

            // Se establece la camara.
            Camera camera = this.Viewer.asOsgViewerView().getCamera();
            camera.setGraphicsContext(this.gc);
            camera.setViewport(new Viewport(0, 0, traits.width, traits.height));
            camera.setProjectionMatrixAsPerspective(30.0f, (double)traits.width / (double)traits.height, 0.5, 1000.0);
            camera.setDrawBuffer((uint)OsgModule.GL_FRONT);
            camera.setReadBuffer((uint)OsgModule.GL_FRONT);
            camera.setClearColor(new Vec4f(0.2f, 0.2f, 0.2f, 1));
            //camera.asCullSettings().setComputeNearFarMode(CullSettings.ComputeNearFarMode.DO_NOT_COMPUTE_NEAR_FAR);
            camera.setClearMask((uint)(OsgModule.GL_COLOR_BUFFER_BIT
                                       | OsgModule.GL_DEPTH_BUFFER_BIT
                                    /*| OsgModule.GL_STENCIL_BUFFER_BIT*/));

            //DisplaySettings settings = this.CreateDisplaySettings();
            //camera.setDisplaySettings(settings);
            //osgViewer.Viewer.asView(this.Viewer).setDisplaySettings(settings);

            //this.Camera.attach(Camera.BufferComponent.STENCIL_BUFFER,));

            //this.Camera = camera;
            this.camera = camera;

            // Se establece el manipulador por defecto: tipo trackball.
            TrackballManipulator manipulator = new TrackballManipulator();
            manipulator.setHomePosition(new Vec3d(-50, 0, 2), new Vec3d(0, 0, 0), new Vec3d(0, 0, 1));
            manipulator.setTransformation(new Vec3d(-50, 0, 2), new Vec3d(0, 0, 0), new Vec3d(0, 0, 1));
            //manipulator.home(0);
            //manipulator.setAutoComputeHomePosition(true);
            manipulator.setAllowThrow(false);
            manipulator.setVerticalAxisFixed(true);
            manipulator.setTrackballSize(1);

            this.Manipulator = manipulator;
        }

        protected virtual GraphicsContext.Traits CreateTraits()
        {
            Referenced windata = GraphicsWindowWin32.WindowData.create(this.Handle);

            GraphicsContext.Traits traits = new GraphicsContext.Traits();
            traits.x = 0;
            traits.y = 0;
            traits.width = this.Width;
            traits.height = this.Height;
            traits.supportsResize = true;
            traits.windowDecoration = false;
            traits.doubleBuffer = true;
            //traits.sharedContext = new GraphicsContextObserver();
            traits.inheritedWindowData = new ReferencedRef(windata);

            //traits.setInheritedWindowPixelFormat = true;
            traits.setInheritedWindowPixelFormat = false;
            traits.stencil = stencil;
            traits.depth = depth;
            traits.red = red;
            traits.green = green;
            traits.blue = blue;
            traits.alpha = alpha;
            traits.samples = (uint)numSamples; // to make anti-aliased
            traits.sampleBuffers = (uint)((numSamples > 0) ? 1 : 0);

            // disable vertical sync.
            traits.vsync = this.EnableVSync;
            traits.windowName = this.Name;
            //traits.useMultiThreadedOpenGLEngine

            traits.swapMethod = swapMethod;

            //traits.glContextVersion = version;

            return traits;
        }

        /// <summary>
        /// Crea la raiz de la escena.
        /// </summary>
        protected virtual void CreateOSGRoot()
        {
            // Se crea el nodo raiz.
            this.Root = new Group();

            // Realize the Viewer
            this.Viewer.realize();

            this.Viewer.setDone(false);

            // Se configura la tarea 'OSG' si procede.
            if (useThread)
            {
                Debug.WriteLine("[SimpleOSGControl] useThread Priority={0} Name={1}",
                                threadPriority, NOMBRE_OSG);

                this.osgThread = new Thread(this.OSGThread);
                this.osgThread.IsBackground = true;
                this.osgThread.Priority = threadPriority;
                this.osgThread.Name = NOMBRE_OSG;

                this.osgThread.Start();
            }
            else if (useTimer)
            {
                Debug.WriteLine("[SimpleOSGControl] useTimer Interval={0}",
                                timerInterval);

                this.timerPintar = new Timer();
                this.timerPintar.Tick += this.TimerPintar_Tick;
                this.timerPintar.Interval = timerInterval;
                this.timerPintar.Enabled = false;
                this.timerPintar.Start();
            }
            else if (useGraphicsThread)
            {
                Debug.WriteLine("[SimpleOSGControl] useGraphicsThread");

                this.gc.createGraphicsThread();
            }

            if (this.Visible || this.Focused)
            {
                this.ContinueOSG();
            }
        }

        #endregion Miembros protegidos _____________________________________________________________

        #region Miembros privados

        /// <summary>Nombre del hilo principal.</summary>
        private const string NOMBRE_OSG = "Hilo ventana OSG";

        /// <summary>
        /// Inicializa el componente.
        /// </summary>
        private void InitializeComponent()
        {
        }

        private void ResetSceneData()
        {
            if (this.Viewer != null)
            {
                this.Viewer.asOsgViewerView().setSceneData(null);
            }
        }

        private void SetSceneData()
        {
            if (this.Viewer != null)
            {
                this.Viewer.asOsgViewerView().setSceneData(this.Root);
            }
        }

        /// <summary>
        /// Tarea que genera los frames.
        /// </summary>
        private void OSGThread()
        {
            this.Viewer.setDone(false);

            while (this.DoFrame())
            {
                // Hasta el infinito..
            }
        }

        /// <summary>
        /// Escucha el evento <c>TimerPintar.Tick</c>.
        /// </summary>
        private void TimerPintar_Tick(object sender, EventArgs args)
        {
            this.DoFrame();
        }

        private bool DoFrame()
        {
            if (!this.Viewer.done())
            {
                try
                {
                    this.OnUpdateOSG(EventArgs.Empty);
                    //this.viewer.eventTraversal();
                    //this.viewer.updateTraversal();
                    //this.viewer.renderingTraversals();
                    this.Viewer.frame();
                }
                catch (Exception e)
                {
                    Debug.Write(e);
                }
            }
            return true;
        }

        /*private void TimerPintar_Tick(object sender, EventArgs args)
        {
            Geode cylGeode = this.LoadCylinder();
            Geode boxGeode = this.LoadBox();

            this.root.addChild(cylGeode);
            this.root.addChild(boxGeode);

            StateSet cylState = cylGeode.getOrCreateStateSet();
            CullFace cfCyl = new CullFace();
            cylState.setAttributeAndModes(cfCyl);

            StateSet boxState = boxGeode.getOrCreateStateSet();
            CullFace cfBox = new CullFace();
            boxState.setAttributeAndModes(cfBox);

            StateSet state = this.root.getOrCreateStateSet();

            ColorMask colorMask = new ColorMask();
            Depth depth = new Depth();
            Stencil stencil = new Stencil();

            state.setAttributeAndModes(colorMask, (uint)StateAttribute.Values.ON);
            state.setAttributeAndModes(depth, (uint)StateAttribute.Values.OFF);
            state.setAttributeAndModes(stencil, (uint)StateAttribute.Values.OFF);

            if (!this.viewer.done())
            {
                // Turn off writing to the colour buffer, so nothing is rendered
                // to the screen yet
                colorMask.setMask(false, false, false, false);

                // Clear stencil buffer to O ??

                // Turn on depth testing
                state.setAttribute(depth, (uint)StateAttribute.Values.ON);

                // Render the back  face of cylinder (object a) into depth buffer
                // Ensure the box (object b ) isn't rendered
                cfCyl.setMode(CullFace.Mode.FRONT);
                cfBox.setMode(CullFace.Mode.FRONT_AND_BACK);
                this.viewer.frame();

                // Disable writing to the depth buffer
                depth.setWriteMask(false);

                // Set the stencil buffer to increment if the depth test passes
                //enable the stencil test
                stencil.setWriteMask(1);
                state.setAttribute(stencil, (uint)StateAttribute.Values.ON);

                //set the stencil functions
                stencil.setFunction(Stencil.Function.ALWAYS, 0, 0);

                stencil.setOperation(Stencil.Operation.KEEP, Stencil.Operation.KEEP, Stencil.Operation.INCR);

                // Render the front faces of the box object b
                cfCyl.setMode(CullFace.Mode.FRONT_AND_BACK);
                cfBox.setMode(CullFace.Mode.BACK);
                this.viewer.frame();

                // Set the stencil buffer to decrement if the depth test passes
                stencil.setOperation(Stencil.Operation.KEEP, Stencil.Operation.KEEP, Stencil.Operation.DECR);

                // Render the back face of the box (object b)
                cfBox.setMode(CullFace.Mode.FRONT);
                this.viewer.frame();

                // Set the stencil buffer to pass where values aren't equal to 0
                stencil.setFunction(Stencil.Function.NOTEQUAL, 0, 1);

                // Turn off writing to the stencil buffer
                stencil.setWriteMask(0);

                // Turn on writing to the colour buffer
                colorMask.setMask(true, true, true, true);

                // Turn off depth testing
                state.setAttribute(depth, (uint)StateAttribute.Values.OFF);

                // Render back faces of cylinder object a
                cfCyl.setMode(CullFace.Mode.FRONT);
                cfBox.setMode(CullFace.Mode.FRONT_AND_BACK);

                this.viewer.frame();
            }
        }*/

        private void KillGLWindow()
        {
            if (this.gc != null)
            {
                this.gc.releaseContext();
                this.gc = null;
            }

            if (this.gc != null)
            {
                this.gc.Dispose();
                this.gc = null;
            }
        }

        private static readonly uint stencil = 8;
        private static readonly uint depth = 24;
        private static readonly uint red = 8;
        private static readonly uint green = 8;
        private static readonly uint blue = 8;
        private static readonly uint alpha = 8;
        private static readonly int numSamples = 4;

        private static readonly DisplaySettings.SwapMethod swapMethod = DisplaySettings.SwapMethod.SWAP_DEFAULT;

        private static readonly bool useThread = false;
        private static readonly ThreadPriority threadPriority = ThreadPriority.Lowest;

        private static readonly bool useTimer = true;
        private static readonly int timerInterval = 25;

        private static readonly bool useGraphicsThread = false;

        private static string ToString(GraphicsContext.Traits traits)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("x, y, width, height: {0}, {1}, {2}, {3}", traits.x, traits.y, traits.width, traits.height).AppendLine();
            builder.AppendFormat("windowName: {0}", traits.windowName).AppendLine();
            builder.AppendFormat("windowDecoration: {0}", traits.windowDecoration).AppendLine();
            builder.AppendFormat("supportsResize: {0}", traits.supportsResize).AppendLine();
            builder.AppendFormat("RGBA: {0}, {1}, {2}, {3}", traits.red, traits.green, traits.blue, traits.alpha).AppendLine();
            builder.AppendFormat("depth: {0}", traits.depth).AppendLine();
            builder.AppendFormat("stencil: {0}", traits.stencil).AppendLine();
            builder.AppendFormat("sampleBuffers: {0}", traits.sampleBuffers).AppendLine();
            builder.AppendFormat("samples: {0}", traits.samples).AppendLine();
            builder.AppendFormat("pbuffer: {0}", traits.pbuffer).AppendLine();
            builder.AppendFormat("quadBufferStereo: {0}", traits.quadBufferStereo).AppendLine();
            builder.AppendFormat("doubleBuffer: {0}", traits.doubleBuffer).AppendLine();
            builder.AppendFormat("target: {0}", traits.target).AppendLine();
            builder.AppendFormat("format: {0}", traits.format).AppendLine();
            builder.AppendFormat("level: {0}", traits.level).AppendLine();
            builder.AppendFormat("face: {0}", traits.face).AppendLine();
            builder.AppendFormat("mipMapGeneration: {0}", traits.mipMapGeneration).AppendLine();
            builder.AppendFormat("vsync: {0}", traits.vsync).AppendLine();
            builder.AppendFormat("swapGroupEnabled: {0}", traits.swapGroupEnabled).AppendLine();
            builder.AppendFormat("swapGroup: {0}", traits.swapGroup).AppendLine();
            builder.AppendFormat("swapBarrier: {0}", traits.swapBarrier).AppendLine();
            builder.AppendFormat("useMultiThreadedOpenGLEngine: {0}", traits.useMultiThreadedOpenGLEngine).AppendLine();
            builder.AppendFormat("useCursor: {0}", traits.useCursor).AppendLine();
            builder.AppendFormat("glContextVersion: {0}", traits.glContextVersion).AppendLine();
            builder.AppendFormat("glContextFlags: {0}", traits.glContextFlags).AppendLine();
            builder.AppendFormat("glContextProfileMask: {0}", traits.glContextProfileMask).AppendLine();
            //getContextVersion(SWIGTYPE_p_unsigned_int major, SWIGTYPE_p_unsigned_int minor)
            builder.AppendFormat("sharedContext: {0}", traits.sharedContext).AppendLine();
            builder.AppendFormat("inheritedWindowData: {0}", traits.inheritedWindowData).AppendLine();
            builder.AppendFormat("setInheritedWindowPixelFormat: {0}", traits.setInheritedWindowPixelFormat).AppendLine();
            builder.AppendFormat("overrideRedirect: {0}", traits.overrideRedirect).AppendLine();
            builder.AppendFormat("swapMethod: {0}", traits.swapMethod).AppendLine();

            GraphicsContext.ScreenIdentifier sidentifier = traits.asScreenIdentifier();
            builder.AppendFormat("displayName: {0}", sidentifier.displayName()).AppendLine();
            builder.AppendFormat("hostName: {0}", sidentifier.hostName).AppendLine();
            builder.AppendFormat("displayNum: {0}", sidentifier.displayNum).AppendLine();
            builder.AppendFormat("screenNum: {0}", sidentifier.screenNum).AppendLine();

            return builder.ToString();
        }

        /// <summary>Timer de OSG.</summary>
        private Timer timerPintar;

        /// <summary>Tarea principal de OSG.</summary>
        private Thread osgThread;

        /// <summary><see cref="http://stackoverflow.com/questions/142826/is-there-a-way-to-indefinitely-pause-a-thread"/></summary>
        private readonly ManualResetEvent pauseOSGThread = new ManualResetEvent(false);

        ///// <summary><see cref="http://stackoverflow.com/questions/142826/is-there-a-way-to-indefinitely-pause-a-thread"/></summary>
        //private ManualResetEvent shutdownSGThread = new ManualResetEvent(false);

        /// <summary>Vista.</summary>
        private Viewer viewer;

        /// <summary>Nodo raiz.</summary>
        private Group root;

        /// <summary>Camara.</summary>
        private Camera camera;

        /*/// <summary>Camara.</summary>
        private Camera preCamera;*/

        /// <summary>Manipulador.</summary>
        private CameraManipulator manipulator;

        /// <summary>Contexto grafico.</summary>
        private GraphicsContext gc;

#if false
        protected readonly ReaderWriterLock rwlock = new ReaderWriterLock();
#endif

        #endregion Miembros privados _______________________________________________________________

        #region Miembros Control

        protected override void OnLoad(EventArgs args)
        {
            this.CreateOSGWindow();
            this.CreateOSGRoot();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // No se hace nada.
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            // No se hace nada.
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            this.PauseOSG();
            if (useThread)
            {
                if (this.osgThread != null)
                {
                    this.osgThread.Abort();
                }
            }

            this.KillGLWindow();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            this.PauseOSG();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            this.ContinueOSG();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.Visible)
            {
                this.ContinueOSG();
            }
            else
            {
                this.PauseOSG();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (this.timerPintar != null)
            {
                this.timerPintar.Dispose();
            }
        }

        #endregion Miembros Control ________________________________________________________________
    }
}