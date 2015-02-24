using System;
using Photon.AST;
using Photon.OpCode;
using Photon.Scanner;


namespace Photon.Compiler
{
    public partial class Compiler
    {

        bool CompileStmt(CommandSet cm, Node n, bool lhs)
        {
            if (n is ReturnStmt)
            {
                var v = n as ReturnStmt;

                CompileNode(cm, v.Results[0], false);
                cm.Add(new Command(Opcode.Ret));
                return true;
            }


            if (n is BlockStmt)
            {
                var v = n as BlockStmt;


                foreach (var b in v.Stmts)
                {
                    CompileNode(cm, b, false);
                }

                return true;
            }

            if (n is AssignStmt)
            {
                var v = n as AssignStmt;

                foreach( var e in v.RHS)
                {
                    CompileNode(cm, e, false);
                }

                foreach (var e in v.LHS)
                {
                    CompileNode(cm, e, true);
                }

                return true;
            }

            if ( n is IfStmt )
            {
                var v = n as IfStmt;

                CompileNode(cm, v.Condition, false);

                var jnzCmd = cm.Add(new Command(Opcode.Jnz, 0));

                CompileNode(cm, v.Body, false);

                var jmpCmd = cm.Add(new Command(Opcode.Jmp, 0));

                // false body跳入
                jnzCmd.DataA = cm.CurrGenIndex;

                if ( v.ElseBody.Stmts.Count > 0 )
                {
                    CompileNode(cm, v.ElseBody, false);
                }

                // true body执行完毕跳出
                jmpCmd.DataA = cm.CurrGenIndex;

                
                return true;
            }

            if (n is ForStmt)
            {
                var v = n as ForStmt;

                var loopStart = cm.CurrGenIndex;

                CompileNode(cm, v.Condition, false);

                var jnzCmd = cm.Add(new Command(Opcode.Jnz, 0));

                CompileNode(cm, v.Body, false);

                cm.Add(new Command(Opcode.Jmp, loopStart));

                // false body跳入
                jnzCmd.DataA = cm.CurrGenIndex;


                return true;
            }

            return false;
        }
    }
}
