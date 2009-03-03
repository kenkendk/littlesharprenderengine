using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LittleSharpRenderEngine;
using Topology.Geometries;

namespace LSRETester
{
    public partial class frmMap : Form
    {
        private List<IProvider> m_layers;
        private IEnvelope m_env;
        private IEnvelope m_origEnv;
        private IPoint m_downCoord;
        private System.Drawing.Point m_downPoint;
        private Image m_origImage;
        private Tools m_currentTool = Tools.ZoomIn;

        private enum Tools
        {
            ZoomIn,
            ZoomOut,
            Pan
        }

        public frmMap(IEnvelope envelope, params IProvider[] layers) : this()
        {
            m_layers = new List<IProvider>(layers);
            m_env = m_origEnv = envelope;
        }

        private frmMap()
        {
            InitializeComponent();
        }

        //TODO: Use onDraw instead of this...
        private void Form2_Resize(object sender, EventArgs e)
        {
            LittleSharpRenderEngine.LittleSharpRenderEngine eng = new LittleSharpRenderEngine.LittleSharpRenderEngine(m_env, null, pictureBox1.Size, Color.White);
            foreach (LittleSharpRenderEngine.IProvider provider in m_layers)
                eng.RenderFeatures(null, provider.GetFeatures(m_env, null, float.NaN));		//Be sure to crash, when we implement the scale ;) MUAHAHAHAH!

            pictureBox1.Image = eng.Bitmap;
        }

        private void Form2_SizeChanged(object sender, EventArgs e)
        {
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Form2_Resize(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            double x = (PointToClient(Cursor.Position).X * (m_env.Width / pictureBox1.Width)) + m_env.MinX;
            double y = (PointToClient(Cursor.Position).Y * (m_env.Height / pictureBox1.Height)) + m_env.MinY;

            CursorPosition.Text = "X: " + x.ToString("0.0000") + ", Y: " + y.ToString("0.0000");

            if (m_downCoord != null)
            {
                switch (m_currentTool)
                {
                    case Tools.Pan:
                        System.Drawing.Bitmap b3 = new Bitmap(m_origImage.Size.Width, m_origImage.Size.Height);
                        using (Graphics g = Graphics.FromImage(b3))
                        {
                            g.Clear(Color.White);
                            g.DrawImageUnscaled(m_origImage, new System.Drawing.Point(e.X - m_downPoint.X, e.Y - m_downPoint.Y));
                        }
                        pictureBox1.Image = b3;

                        break;
                    case Tools.ZoomIn:
                    case Tools.ZoomOut:

                        System.Drawing.Bitmap b2 = new Bitmap(m_origImage);
                        using (Graphics g = Graphics.FromImage(b2))
                        {
                            int minx = Math.Min(e.X, m_downPoint.X);
                            int miny = Math.Min(e.Y, m_downPoint.Y);
                            int maxx = Math.Max(e.X, m_downPoint.X);
                            int maxy = Math.Max(e.Y, m_downPoint.Y);

                            Rectangle rect = new Rectangle(minx, miny, maxx - minx, maxy -miny);

                            using(Brush b = new SolidBrush(Color.FromArgb(64, Color.Blue)))
                                g.FillRectangle(b, rect);
                            g.DrawRectangle(System.Drawing.Pens.DarkBlue, rect);
                        }

                        pictureBox1.Image = b2;


                        break;
                }
            }
        }

        private void UnzoomButton_Click(object sender, EventArgs e)
        {
            m_env = m_origEnv;
            Form2_Resize(sender, e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            double x = (e.X  * (m_env.Width / pictureBox1.Width)) + m_env.MinX;
            double y = (e.Y * (m_env.Height / pictureBox1.Height)) + m_env.MinY;
            m_origImage = pictureBox1.Image;
            m_downCoord = new Topology.Geometries.Point(x, y);
            m_downPoint = new System.Drawing.Point(e.X, e.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            double curx = (e.X * (m_env.Width / pictureBox1.Width)) + m_env.MinX;
            double cury = (e.Y * (m_env.Height / pictureBox1.Height)) + m_env.MinY;
            double downx = (m_downPoint.X * (m_env.Width / pictureBox1.Width)) + m_env.MinX;
            double downy = (m_downPoint.Y * (m_env.Height / pictureBox1.Height)) + m_env.MinY;

            
            double w = m_env.Width;
            double h = m_env.Height;
            double x = m_env.Centre.X;
            double y = m_env.Centre.Y;

            if (m_downCoord != null)
            {
                switch (m_currentTool)
                {
                    case Tools.Pan:
                        //Move center
                        x += (downx - curx);
                        y += (downy - cury);

                        m_env = new Topology.Geometries.Envelope(x - (w / 2), x + (w / 2), y - (h / 2), y + (h / 2));
                        break;
                    case Tools.ZoomIn:
                        //Is it a click?
                        if (Math.Abs(m_downPoint.X - e.X) < 2 && Math.Abs(m_downPoint.Y - e.Y) < 2)
                        {
                            w /= 2;
                            h /= 2;
                            m_env = new Topology.Geometries.Envelope(x - (w / 2), x + (w / 2), y - (h / 2), y + (h / 2));
                        }
                        else
                        {
                            m_env = new Topology.Geometries.Envelope(curx, downx, cury, downy);
                        }

                        break;
                    case Tools.ZoomOut:
                        //Is it a click?

                        double hr = 2;
                        double wr = 2;

                        if (Math.Abs(m_downPoint.X - e.X) < 2 && Math.Abs(m_downPoint.Y - e.Y) < 2)
                        {
                            //Use factor 2
                        }
                        else
                        {
                            wr = Math.Abs(m_env.Width) / Math.Abs(curx - downx);
                            hr = Math.Abs(m_env.Height) / Math.Abs(cury - downy);
                        }

                        w *= wr;
                        h *= hr;
                        m_env = new Topology.Geometries.Envelope(x - (w / 2), x + (w / 2), y - (h / 2), y + (h / 2));

                        break;
                }
            }

            m_origImage = null;
            m_downCoord = null;

            Form2_Resize(sender, e);
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            ZoomOutButton.Checked = PanButton.Checked = false;
            ZoomInButton.Checked = true;
            m_currentTool = Tools.ZoomIn;
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            ZoomInButton.Checked = PanButton.Checked = false;
            ZoomOutButton.Checked = true;
            m_currentTool = Tools.ZoomOut;
        }

        private void PanButton_Click(object sender, EventArgs e)
        {
            ZoomOutButton.Checked = ZoomInButton.Checked = false;
            PanButton.Checked = true;
            m_currentTool = Tools.Pan;
        }
    }
}