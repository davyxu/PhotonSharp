
using Photon;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhotonToy
{
    enum CodeType
    {
        Empty,
        Assembly,
        Source,
    }

    class CodeLine
    {
        public string Code;
        public CodeType Type;

        public CodeLine(CodeType t, string code)
        {
            Type = t;
            Code = code;
        }

        public override string ToString()
        {
            return Code;
        }
    }

    static class CodeList
    {
        static readonly SolidBrush textBrush = new SolidBrush(Color.Black);
        static readonly SolidBrush normalBGBrush = new SolidBrush(Color.White);
        static readonly SolidBrush runningLineBrush = new SolidBrush(Color.Yellow);

        const int TagRowWidth = 30;

        internal static void HookDraw( this ListBox self )
        {
            self.DrawMode = DrawMode.OwnerDrawVariable;
            self.DrawItem += (sender, e) =>
            {
                if (e.Index == -1)
                {
                    return;
                }

                var item = self.Items[e.Index] as CodeLine;
                if (item == null)
                    return;

                

                var b = e.Bounds;

                b.Offset(TagRowWidth, 0);

                if (e.Index == currIndex)
                {
                    e.Graphics.FillRectangle(runningLineBrush, b);
                }

                e.Graphics.DrawString(item.ToString(), self.Font, textBrush, b);
            };
        }

        internal static void ShowCode(this ListBox self, Executable exec )
        {
            _listIndexByLocation.Clear();
            self.Items.Clear();

            foreach (var cs in exec.CmdSet)
            {
                ShowCommandSet(self, exec.Source, cs);
            }
        }


        static int currIndex = 0;

        internal static void SetCurrLine(this ListBox self, AssemblyLocation al)
        {
            int index;
            if ( !_listIndexByLocation.TryGetValue(al, out index) )
            {
                index = -1;
            }

            currIndex = index;
            self.SelectedIndex = index;
            self.Refresh();
        }


        static Dictionary<AssemblyLocation, int> _listIndexByLocation = new Dictionary<AssemblyLocation, int>();
        static void AddCodeLine( ListBox self, AssemblyLocation al, CodeType t, string fmt, params object[] args )
        {
            var code = string.Format(fmt, args);
            self.Items.Add(new CodeLine(t, code));

            if ( t == CodeType.Assembly )
            {
                _listIndexByLocation.Add(al, self.Items.Count - 1);
            }
            
        }

        static void ShowCommandSet( ListBox self, SourceFile file, CommandSet cmdset )
        {
            int currSourceLine = 0;

            AssemblyLocation al;
            al.CmdSetID = cmdset.ID;
            al.Pos = 0;

            foreach (var c in cmdset.Commands)
            {
                // 到新的源码行
                if (c.CodePos.Line > currSourceLine)
                {
                    // 每个源码下的汇编成为一块, 块与块之间空行
                    if (currSourceLine != 0)
                    {
                        AddCodeLine(self, al,CodeType.Empty, string.Empty);
                    }

                    currSourceLine = c.CodePos.Line;

                    // 显示源码

                    AddCodeLine(self, al, CodeType.Source, "{0,3}({1})| {2}", currSourceLine, cmdset.ToString(), file.GetLine(currSourceLine));                    
                }
                
                // 显示汇编
                AddCodeLine(self, al, CodeType.Assembly, "{0,3}| {1}", al.Pos, c.ToString());

                al.Pos++;
            }
        }
    }
}
