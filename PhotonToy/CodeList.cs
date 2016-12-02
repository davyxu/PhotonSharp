
using Photon;
using SharpLexer;
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




        static int currIndex = 0;
        static AssemblyLocation lastAL;
        static Executable Exe;

        internal static void Init( Executable e)
        {
            Exe = e;
            lastAL.FuncID = -1;
            lastAL.FuncName = string.Empty;
            lastAL.CodePos = TokenPos.Invalid;
            lastAL.PC = -1;
        }
        

        internal static void SetCurrLine(this ListBox self, AssemblyLocation al)
        {

            if (lastAL.IsDiff(al) )
            {
                ShowCode(self, al, Exe);
                lastAL = al;
            }


            int index = -1;
            if (!string.IsNullOrEmpty(al.CodePos.SourceName))
            {
                _listIndexByLocation.TryGetValue(al, out index);
            }

            
            if ( self.Items.Count > 0 )
            {
                currIndex = index;
                self.SelectedIndex = index;
                self.Refresh();
            }
            
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

        internal static void ShowCode(this ListBox self, AssemblyLocation al,  Executable exec)
        {


            var sf = exec.GetSourceFile(al.CodePos.SourceName);

            if (sf != null)
            {
                _listIndexByLocation.Clear();
                self.Items.Clear();

                ShowCommandSet(self, sf, al, exec);
            }

            
        }

        static void ShowCommandSet(ListBox self, SourceFile file, AssemblyLocation al, Executable exec)
        {
            int currSourceLine = 0;
            al.PC = 0;

            foreach (var c in al.Commands)
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

                    AddCodeLine(self, al, CodeType.Source, "{0,3}({1} {2})| {3}", currSourceLine, al.FuncName, al.FuncDefPos, file.GetLine(currSourceLine));                    
                }
                
                // 显示汇编
                AddCodeLine(self, al, CodeType.Assembly, "{0,3}| {1}", al.PC, c.ToString());

                al.PC++;
            }
        }
    }
}
