using System;
using System.Windows.Forms;
using Photon;

namespace PhotonToy
{
    public partial class MainForm : Form
    {
        DebugBox _debugBox;

        string _currFile;

        public MainForm(string[] args)
        {
            _debugBox = new DebugBox(this);

            InitializeComponent();
            codeList.HookDraw();

            
            _debugBox.OnBreak += OnBreak;
            _debugBox.OnLoad += OnLoad;
            _debugBox.OnError += e  => {
                MessageBox.Show(e);
            };

            if (args.Length > 0 )
            {
                _currFile = args[0];
                _debugBox.Start(_currFile);
            }

        }

        void RefreshRegisterCategoryList(Executable exe)
        {
            var preselc = pkgSel.SelectedIndex;

            pkgSel.Items.Clear();
            pkgSel.Items.Add("local");

            for (int i = 0; i < exe.PackageCount; i++)
            {
                var pkg = exe.GetPackage(i);
                pkgSel.Items.Add(pkg.Name);
            }

            if ( preselc == -1)
            {
                pkgSel.SelectedIndex = 0;
            }
            else
            {
                pkgSel.SelectedIndex = preselc;
            }


        }

        void OnLoad(Executable exe)
        {
            RefreshRegisterCategoryList(exe);

            CodeList.Init(exe);
        }

        void OnBreak( VMState vms )
        {
            
            codeList.SetCurrLine(vms.Location);

            registerList.Items.Clear();
            foreach( var str in vms.Register)
            {
                registerList.Items.Add(str);
            }

            dataStackList.Items.Clear();
            foreach (var str in vms.DataStack)
            {
                dataStackList.Items.Add(str);
            }

            callStackList.Items.Clear();
            foreach (var str in vms.CallStack)
            {
                callStackList.Items.Add(str);
            }

            if ( string.IsNullOrEmpty(vms.RegPackage) )
            {
                pkgSel.SelectedIndex = 0;
            }
            else
            {
                if (pkgSel.SelectedItem as string != vms.RegPackage )
                {
                    foreach (var i in pkgSel.Items)
                    {
                        var pkgName = i as string;
                        if (pkgName == vms.RegPackage)
                        {
                            pkgSel.SelectedItem = i;
                            break;
                        }
                    }
                }
  

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
                _currFile = dialog.FileName;
                _debugBox.Stop();                
                _debugBox.Start(_currFile);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_debugBox.State == State.Breaking)
            {
                _debugBox.Operate(DebuggerMode.Continue);
            }
            else
            {
                _debugBox.Start(_currFile);
            }
            
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _debugBox.Stop();
        }

        private void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_debugBox.State == State.Breaking)
            {
                _debugBox.Operate(DebuggerMode.StepOver);
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _debugBox.Stop();
        }

        private void stepIntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _debugBox.Operate(DebuggerMode.StepIn);            
        }

        private void stepOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _debugBox.Operate(DebuggerMode.StepOut);            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _debugBox.Stop();
            _debugBox.Start(_currFile);
        }

        private void pkgSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pkgName = pkgSel.SelectedItem as string;
            if ( pkgName == "local")
            {
                pkgName = string.Empty;
            }
            _debugBox.SwitchRegister(pkgName);
        }
    }
}
