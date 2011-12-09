using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SharpNeatLib.Experiments;
using SharpNeatLib.Xml;
using SharpNeatLib.NeatGenome;
using SharpNeatLib.NeuralNetwork;
using System.Xml;
using SharpNeatLib.NeatGenome.Xml;
using SharpNeatLib.CPPNs;

namespace SkirmishVisualization
{
    public partial class Form1 : Form
    {
        World w=new World();
        bool drawPie = true;
        bool isMulti = true;
        INetwork network=null;
        NeatGenome seedGenome = null;
        SkirmishSubstrate substrate;
        int timer = 0;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.Size = new Size((int)w.height, (int)w.width);
            SkirmishExperiment.multiple = true;
            isMulti = true;
            this.toggleMultiToolStripMenuItem.Checked = isMulti;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            w.drawWorld(e.Graphics,drawPie);
            e.Graphics.DrawString(timer.ToString(), new Font("Tahoma", 40), Brushes.Black, 50, 50);
            string s="";
            if (isMulti && w.bigBrain != null)
            {
                for (int j = 0; j < 5; j++)
                {

                    s += w.bigBrain.GetOutputSignal(j * 3);
                    s += " " + w.bigBrain.GetOutputSignal(j * 3 + 1);
                    s += " " + w.bigBrain.GetOutputSignal(j * 3 + 2);
                    s += Environment.NewLine;
                }
            }
            e.Graphics.DrawString(s, new Font("Tahoma", 12), Brushes.Black, 50, 100);
           
            //this.menuStrip1.Visible = false;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'm')
                drawPie = !drawPie;
            else if (e.KeyChar == '/')
            {
                if (!isMulti)
                    w.go(100);
                else
                    w.goMulti(100);
                timer += 100;
            }
            else if (e.KeyChar == '1')
            {
                w = SkirmishNetworkEvaluator.world1(network);
                timer = 0;
            }
            else if (e.KeyChar == '2')
            {
                w = SkirmishNetworkEvaluator.world2(network);
                timer = 0;
            }
            else if (e.KeyChar == '3')
            {
                w = SkirmishNetworkEvaluator.world3(network);
                timer = 0;
            }
            else if (e.KeyChar == '4')
            {
                w = SkirmishNetworkEvaluator.world4(network);
                timer = 0;
            }
            else if (e.KeyChar == '5')
            {
                w = SkirmishNetworkEvaluator.world5(network);
                timer = 0;
            }
            else if (e.KeyChar == 'q')
            {
                w = SkirmishNetworkEvaluator.pointWorldVar(network, 3*(float)Math.PI / 8.0f);
                timer = 0;
            }
            else if (e.KeyChar == 'w')
            {
                w = SkirmishNetworkEvaluator.pointWorldVar(network, (float)Math.PI/4.0f);
                timer = 0;
            }
            else if (e.KeyChar == 'e')
            {
                w = SkirmishNetworkEvaluator.pointWorldVar(network, (float)Math.PI / 8.0f);
                timer = 0;
            }
            else if (e.KeyChar == 'a')
            {
                w = SkirmishNetworkEvaluator.diamondWorldVar(network, 75);
                timer = 0;
            }
            else if (e.KeyChar == 's')
            {
                w = SkirmishNetworkEvaluator.diamondWorldVar(network, 100);
                timer = 0;
            }
            else if (e.KeyChar == 'd')
            {
                w = SkirmishNetworkEvaluator.diamondWorldVar(network, 125);
                timer = 0;
            }
            else if (e.KeyChar == 'z')
            {
                w = SkirmishNetworkEvaluator.squareWorldVar(network, 75);
                timer = 0;
            }
            else if (e.KeyChar == 'x')
            {
                w = SkirmishNetworkEvaluator.squareWorldVar(network, 100);
                timer = 0;
            }
            else if (e.KeyChar == 'c')
            {
                w = SkirmishNetworkEvaluator.squareWorldVar(network, 125);
                timer = 0;
            }
            else if (e.KeyChar == 'r')
            {
                w = SkirmishNetworkEvaluator.lWorldVar(network, 75);
                timer = 0;
            }
            else if (e.KeyChar == 't')
            {
                w = SkirmishNetworkEvaluator.lWorldVar(network, 100);
                timer = 0;
            }
            else if (e.KeyChar == 'y')
            {
                w = SkirmishNetworkEvaluator.lWorldVar(network, 125);
                timer = 0;
            }
            else
            {
                if (!isMulti)
                    w.timeStep();
                else
                    w.timeStepMulti();
                timer++;
            }
                
            Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            w.addEnemy(new Prey(e.X, e.Y, w.agentSize, w.agentSize));
            Invalidate();
        }

        private void loadGenomeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult res=openFileDialog1.ShowDialog(this);
            if (res == DialogResult.OK  || res==DialogResult.Yes)
            {
                string filename = openFileDialog1.FileName;
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filename);
                    seedGenome = XmlNeatGenomeReaderStatic.Read(doc);
                    setupSubstrate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

        private void toggleMultiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isMulti = !isMulti;
            SkirmishExperiment.multiple = isMulti;
            toggleMultiToolStripMenuItem.Checked = isMulti;
            setupSubstrate();
            
        }

        private void setupSubstrate()
        {
            if (isMulti)
            {
                substrate = new SkirmishSubstrate(25, 15, 25, HyperNEATParameters.substrateActivationFunction);
                network = substrate.generateMultiGenomeModulus(seedGenome.Decode(null), 5).Decode(null);
            }
            else
            {
                substrate = new SkirmishSubstrate(5, 3, 5, HyperNEATParameters.substrateActivationFunction);
                network = substrate.generateGenome(seedGenome.Decode(null)).Decode(null);
            }
        }

    }
}