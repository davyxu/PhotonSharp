using System;
using System.Windows.Forms;
using Photon.Model;
using Photon.VM;
using System.Drawing;

namespace PhotonToy
{
    public partial class MainForm : Form
    {
        DebugBox _debugBox;

        public MainForm()
        {
            _debugBox = new DebugBox(this);

            InitializeComponent();
            CodeViewInit();

            
            _debugBox.OnBreak += OnBreak;
            _debugBox.OnLoad += OnLoad;

            _debugBox.Load("Closure.pho");
        }

        static readonly SolidBrush textBg = new SolidBrush(Color.Black);

        void CodeViewInit()
        {
            codeList.DrawMode = DrawMode.OwnerDrawVariable;
            codeList.DrawItem += ( sender, e ) => 
            {
                if ( e.Index == -1 )
                {
                    return;
                }

                var item = codeList.Items[e.Index] as string;
                if (item == null)
                    return;

                e.Graphics.DrawString( item,codeList.Font,textBg, e.Bounds );
            };
        }

        void OnBreak( VMachine vm )
        {            
            
        }

        void OnLoad(VMachine vm)
        {
            UpdateCode(_debugBox.Source);
        }

        void UpdateCode( SourceFile file )
        {
            codeList.Items.Clear();

            foreach( var line in file.Lines )
            {
                codeList.Items.Add(line);
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "pho files (*.pho)|*.pho";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _debugBox.Load( dialog.FileName);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_debugBox.State == Photon.VM.State.Breaking)
            {
                _debugBox.Resume();
            }
            else
            {
                _debugBox.Start();
            }
            
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _debugBox.Stop();
        }

        private void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_debugBox.State == Photon.VM.State.Breaking)
            {
                _debugBox.Resume();
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _debugBox.Stop();
        }
    }
}
