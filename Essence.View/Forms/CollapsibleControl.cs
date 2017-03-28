using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Essence.View.Resources;

namespace Essence.View.Forms
{
    public class CollapsibleControl : Panel
    {
        public CollapsibleControl()
        {
            this.InitializeComponent();

            this.HeaderFont = new Font(this.Font, FontStyle.Bold);
            this.HeaderTextColor = Color.Black;
        }

        [Browsable(false)]
        public new Color BackColor
        {
            get { return Color.Transparent; }
            set
            {
                /*base.BackColor = Color.Transparent;*/
            }
        }

        [DefaultValue(false)]
        [Description("Collapses the control when set to true")]
        [Category("CollapsiblePanel")]
        public bool Collapse
        {
            get { return this.collapse; }
            set
            {
                // If using animation make sure to ignore requests for collapse or expand while a previous
                // operation is in progress.
                if (this.useAnimation)
                {
                    // An operation is already in progress.
                    if (this.timerAnimation.Enabled)
                    {
                        return;
                    }
                }
                this.collapse = value;
                this.CollapseOrExpand();
                this.Refresh();
            }
        }

        [DefaultValue(50)]
        [Category("CollapsiblePanel")]
        [Description("Specifies the speed (in ms) of Expand/Collapse operation when using animation. UseAnimation property must be set to true.")]
        public int AnimationInterval
        {
            get { return this.timerAnimation.Interval; }
            set
            {
                // Update animation interval only during idle times.
                if (!this.timerAnimation.Enabled)
                {
                    this.timerAnimation.Interval = value;
                }
            }
        }

        [DefaultValue(false)]
        [Category("CollapsiblePanel")]
        [Description("Indicate if the panel uses amination during Expand/Collapse operation")]
        public bool UseAnimation
        {
            get { return this.useAnimation; }
            set { this.useAnimation = value; }
        }

        [DefaultValue(true)]
        [Category("CollapsiblePanel")]
        [Description("When set to true draws panel borders, and shows a line separating the panel's header from the rest of the control")]
        public bool ShowHeaderSeparator
        {
            get { return this.showHeaderSeparator; }
            set
            {
                this.showHeaderSeparator = value;
                this.Refresh();
            }
        }

        [DefaultValue(false)]
        [Category("CollapsiblePanel")]
        [Description("When set to true, draws a panel with rounded top corners, the radius can bet set through HeaderCornersRadius property")]
        public bool RoundedCorners
        {
            get { return this.roundedCorners; }
            set
            {
                this.roundedCorners = value;
                this.Refresh();
            }
        }

        [DefaultValue(10)]
        [Category("CollapsiblePanel")]
        [Description("Top corners radius, it should be in [1, 15] range")]
        public int HeaderCornersRadius
        {
            get { return this.headerCornersRadius; }

            set
            {
                if (value < 1 || value > 15)
                {
                    throw new ArgumentOutOfRangeException("HeaderCornersRadius", value, "Value should be in range [1, 90]");
                }
                else
                {
                    this.headerCornersRadius = value;
                    this.Refresh();
                }
            }
        }

        [DefaultValue(false)]
        [Category("CollapsiblePanel")]
        [Description("Enables the automatic handling of text that extends beyond the width of the label control.")]
        public bool HeaderTextAutoEllipsis
        {
            get { return this.headerTextAutoEllipsis; }
            set
            {
                this.headerTextAutoEllipsis = value;
                this.Refresh();
            }
        }

        [Category("CollapsiblePanel")]
        [Description("Text to show in panel's header")]
        public string HeaderText
        {
            get { return this.Text; }
            set
            {
                this.Text = value;
                this.Refresh();
            }
        }

        [Category("CollapsiblePanel")]
        [Description("Color of text header, and panel's borders when ShowHeaderSeparator is set to true")]
        public Color HeaderTextColor
        {
            get { return this.ForeColor; }
            set
            {
                this.ForeColor = value;
                this.Refresh();
            }
        }

        [Category("CollapsiblePanel")]
        [Description("Image that will be displayed in the top left corner of the panel")]
        public Image HeaderImage
        {
            get { return this.headerImage; }
            set
            {
                this.headerImage = value;
                this.Refresh();
            }
        }

        [Category("CollapsiblePanel")]
        [Description("The font used to display text in the panel's header.")]
        public Font HeaderFont
        {
            get { return this.Font; }
            set
            {
                this.Font = value;
                this.Refresh();
            }
        }

        public int Gap
        {
            get { return this.gap; }
            set
            {
                this.gap = value;
                this.Refresh();
            }
        }

        public Color LeftColor
        {
            get { return this.leftColor; }
            set
            {
                this.leftColor = value;
                this.Refresh();
            }
        }

        public Color RightColor
        {
            get { return this.rightColor; }
            set
            {
                this.rightColor = value;
                this.Refresh();
            }
        }

        public event EventHandler CollapsedOrExpanded;

        #region protected

        protected virtual void OnCollapsedOrExpanded(EventArgs args)
        {
            if (this.CollapsedOrExpanded != null)
            {
                this.CollapsedOrExpanded(this, args);
            }
        }

        #endregion

        #region private

        private void DrawHeaderCorners(Graphics g, Brush brush, float x, float y, float width, float height, float radius)
        {
            GraphicsPath gp = new GraphicsPath();

            gp.AddLine(x + radius, y, x + width - (radius * 2), y); // Line
            gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90); // Corner
            gp.AddLine(x + width, y + radius, x + width, y + height); // Line
            gp.AddLine(x + width, y + height, x, y + height); // Line
            gp.AddLine(x, y + height, x, y + radius); // Line
            gp.AddArc(x, y, radius * 2, radius * 2, 180, 90); // Corner
            gp.CloseFigure();
            g.FillPath(brush, gp);
            if (this.ShowHeaderSeparator)
            {
                g.DrawPath(new Pen(this.HeaderTextColor), gp);
            }
            gp.Dispose();
        }

        private void DrawHeaderPanel(PaintEventArgs e)
        {
            Rectangle headerRect = this.ClientRectangle;
            LinearGradientBrush headerBrush = new LinearGradientBrush(headerRect, this.LeftColor, this.RightColor, LinearGradientMode.Horizontal);

            if (!this.RoundedCorners)
            {
                e.Graphics.FillRectangle(headerBrush, headerRect.X, headerRect.Y, headerRect.Width - 1, headerRect.Height - 1);
                if (this.ShowHeaderSeparator)
                {
                    e.Graphics.DrawRectangle(new Pen(this.HeaderTextColor), headerRect.X, headerRect.Y, headerRect.Width - 1, headerRect.Height - 1);
                }
            }
            else
            {
                this.DrawHeaderCorners(e.Graphics, headerBrush, headerRect.X, headerRect.Y, headerRect.Width - 1, headerRect.Height - 1, this.HeaderCornersRadius);
            }

            int headerRectHeight = this.Height;

            // Draw header image.
            if (this.HeaderImage != null)
            {
                this.pictureBoxImage.Image = this.HeaderImage;
                this.pictureBoxImage.Visible = true;
            }
            else
            {
                this.pictureBoxImage.Image = null;
                this.pictureBoxImage.Visible = false;
            }

            // Calculate header string position.
            if (!string.IsNullOrEmpty(this.HeaderText))
            {
                this.useToolTip = false;

                int delta = this.pictureBoxExpandCollapse.Width + 5;
                int offset = 0;
                if (this.HeaderImage != null)
                {
                    offset = headerRectHeight;
                }

                Size headerTextSize = TextRenderer.MeasureText(this.HeaderText, this.HeaderFont);
                if (this.HeaderTextAutoEllipsis && (headerTextSize.Width >= headerRect.Width - (delta + offset)))
                {
                    RectangleF rectLayout = new RectangleF(headerRect.X + offset,
                                                           (headerRect.Height - headerTextSize.Height) / 2f,
                                                           headerRect.Width - delta,
                                                           headerTextSize.Height);

                    StringFormat format = new StringFormat();
                    format.Trimming = StringTrimming.EllipsisWord;

                    e.Graphics.DrawString(this.HeaderText, this.HeaderFont, new SolidBrush(this.HeaderTextColor), rectLayout, format);

                    this.toolTipRectangle = rectLayout;
                    this.useToolTip = true;
                }
                else
                {
                    PointF headerTextPosition = new PointF((offset + headerRect.Width - headerTextSize.Width) / 2f,
                                                           (headerRect.Height - headerTextSize.Height) / 2f);

                    e.Graphics.DrawString(this.HeaderText, this.HeaderFont, new SolidBrush(this.HeaderTextColor), headerTextPosition);

                    if (this.gap > 0)
                    {
                        // evaluates text size
                        SizeF textSize = e.Graphics.MeasureString(this.HeaderText, this.HeaderFont);

                        int y = (int)headerTextPosition.Y + (int)textSize.Height / 2;

                        int x0 = 0;
                        int x1 = (this.Width - (int)textSize.Width) / 2 - this.gap;
                        this.Draw3DLine(e.Graphics, x0, y, x1, y);

                        x0 = (this.Width + (int)textSize.Width) / 2 + this.gap;
                        x1 = this.Width;
                        this.Draw3DLine(e.Graphics, x0, y, x1, y);
                    }
                }
            }
        }

        private void Draw3DLine(Graphics g, int x1, int y1, int x2, int y2)
        {
            g.DrawLine(SystemPens.ControlDark, x1, y1, x2, y2);
            g.DrawLine(SystemPens.ControlLightLight, x1, y1 + 1, x2, y2 + 1);
        }

        private void pictureBoxExpandCollapse_Click(object sender, EventArgs e)
        {
            this.Collapse = !this.Collapse;
        }

        private void CollapseOrExpand()
        {
            if (!this.UseAnimation)
            {
                if (this.Collapse)
                {
                    this.pictureBoxExpandCollapse.Image = MainResources.expand.ToBitmap();
                }
                else
                {
                    this.pictureBoxExpandCollapse.Image = MainResources.collapse.ToBitmap();
                }
            }
            else
            {
                this.timerAnimation.Enabled = true;
                this.timerAnimation.Start();
            }

            this.OnCollapsedOrExpanded(EventArgs.Empty);
        }

        private void pictureBoxExpandCollapse_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.timerAnimation.Enabled)
            {
                if (!this.Collapse)
                {
                    this.pictureBoxExpandCollapse.Image = MainResources.collapse_hightlight.ToBitmap();
                }
                else
                {
                    this.pictureBoxExpandCollapse.Image = MainResources.expand_highlight.ToBitmap();
                }
            }
        }

        private void pictureBoxExpandCollapse_MouseLeave(object sender, EventArgs e)
        {
            if (!this.timerAnimation.Enabled)
            {
                if (!this.Collapse)
                {
                    this.pictureBoxExpandCollapse.Image = MainResources.collapse.ToBitmap();
                }
                else
                {
                    this.pictureBoxExpandCollapse.Image = MainResources.expand.ToBitmap();
                }
            }
        }

        private void pnlHeader_MouseHover(object sender, EventArgs e)
        {
            if (this.useToolTip)
            {
                Point p = this.PointToClient(Control.MousePosition);
                if (this.toolTipRectangle.Contains(p))
                {
                    this.toolTip.Show(this.HeaderText, this, p);
                }
            }
        }

        private void pnlHeader_MouseLeave(object sender, EventArgs e)
        {
            if (this.useToolTip)
            {
                Point p = this.PointToClient(Control.MousePosition);
                if (!this.toolTipRectangle.Contains(p))
                {
                    this.toolTip.Hide(this);
                }
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.components = new System.ComponentModel.Container();
            this.pictureBoxExpandCollapse = new System.Windows.Forms.PictureBox();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            //this.timerAnimation = new System.Windows.Forms.Timer(this.components);
            this.timerAnimation = new System.Windows.Forms.Timer();
            //this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExpandCollapse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.Controls.Add(this.pictureBoxExpandCollapse);
            this.Controls.Add(this.pictureBoxImage);
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "pnlHeader";
            this.Size = new System.Drawing.Size(249, 30);
            this.TabIndex = 0;
            this.MouseLeave += new System.EventHandler(this.pnlHeader_MouseLeave);
            this.MouseHover += new System.EventHandler(this.pnlHeader_MouseHover);
            // 
            // pictureBoxExpandCollapse
            // 
            this.pictureBoxExpandCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxExpandCollapse.Image = MainResources.collapse.ToBitmap();
            this.pictureBoxExpandCollapse.Location = new System.Drawing.Point(0, 5); // Point(224, 5)
            this.pictureBoxExpandCollapse.Name = "pictureBoxExpandCollapse";
            this.pictureBoxExpandCollapse.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxExpandCollapse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxExpandCollapse.TabIndex = 2;
            this.pictureBoxExpandCollapse.TabStop = false;
            this.pictureBoxExpandCollapse.MouseLeave += new System.EventHandler(this.pictureBoxExpandCollapse_MouseLeave);
            this.pictureBoxExpandCollapse.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxExpandCollapse_MouseMove);
            this.pictureBoxExpandCollapse.Click += new System.EventHandler(this.pictureBoxExpandCollapse_Click);
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxImage.TabIndex = 1;
            this.pictureBoxImage.TabStop = false;
            this.pictureBoxImage.Visible = false;
            // 
            // timerAnimation
            // 
            //this.timerAnimation.Interval = 50;
            //this.timerAnimation.Tick += new System.EventHandler(this.timerAnimation_Tick);
            // 
            // CollapsiblePanel
            // 
            this.Name = "CollapsiblePanel";
            this.Size = new System.Drawing.Size(250, 150);
            this.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExpandCollapse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxExpandCollapse;
        private PictureBox pictureBoxImage;
        private Timer timerAnimation;
        private ToolTip toolTip;

        private bool collapse;
        private bool useAnimation;
        private bool showHeaderSeparator = true;
        private bool roundedCorners;
        private int headerCornersRadius = 10;
        private bool headerTextAutoEllipsis;
        private Image headerImage;

        private RectangleF toolTipRectangle = new RectangleF();
        private bool useToolTip;

        private int gap;

        private Color leftColor = Color.Snow;
        private Color rightColor = Color.LightBlue;

        #endregion

        #region Control

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DrawHeaderPanel(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Refresh();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Refresh();
        }

        #endregion
    }
}