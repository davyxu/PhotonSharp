

using SharpLexer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon
{
    partial class CodeParser
    {
        Scope _topScope;
        Scope _global;

        public Scope GlobalScope
        {
            get { return _global; }
        }

        void InitScope( )
        {
            OpenScope( ScopeType.Global, TokenPos.Invalid );
            _global = _topScope;
        }

        Scope OpenScope( ScopeType type, TokenPos pos )
        {
            var s = new Scope(_topScope, type, pos );
            _topScope = s;
            return s;
        }

        void CloseScope( )
        {
            _topScope = _topScope.Outter;
        }

        internal Symbol Declare(Node n, Scope s, string name, TokenPos pos, SymbolUsage usage )
        {
            var pre = s.FindSymbol(name);
            if ( pre != null )
            {
                Error(string.Format("{0} redeclared, pre define: {1}", name, pre.DefinePos), pos);
            }

            Symbol data = new Symbol();
            data.Name = name;
            data.Decl = n;
            data.DefinePos = pos;
            data.Usage = usage;

            

            s.Insert(data, (usage != SymbolUsage.Func && usage != SymbolUsage.Delegate));

            if ( n != null )
            {
                var ident = n as Ident;

                if (ident == null)
                    return data;

                ident.ScopeInfo = data;
            }

            return data;
        }

        void Resolve( Node x )
        {
            var ident = x as Ident;

            if (ident == null)
                return;

            ident.BaseScope = _topScope;

            for (var s = _topScope; s != null; s = s.Outter)
            {
                var data = s.FindSymbol(ident.Name);
                if ( data != null )
                {
                    ident.ScopeInfo = data;

                    // 从闭包开始搜索, 如果已经不是在本层找到, 一定是upvalue
                    if ( _topScope.Type == ScopeType.Closure &&  s != _topScope )
                    {
                        ident.UpValue = true;
                    }

                    return;
                }
            }

            int a = 1;
            // 这里不再检查symbol是否存在, 而是放到AST里去
            // 这里检查将阻碍颠倒顺序函数定义
            //Error(string.Format("undeclared symbol {0}", ident.Name), ident.DefinePos );

        }
    }
}
