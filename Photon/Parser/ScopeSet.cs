using SharpLexer;

namespace Photon
{
    partial class Parser
    {
        Scope _topScope;
        Scope _global;

        public Scope PackageScope
        {
            get { return _global; }
        }

        void InitScope( )
        {
            OpenScope( ScopeType.Package, TokenPos.Invalid );
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
                throw new ParseException(string.Format("{0} redeclared, pre define: {1}", name, pre.DefinePos), pos);
            }

            Symbol data = new Symbol();
            data.Name = name;
            data.Decl = n;
            data.DefinePos = pos;
            data.Usage = usage;
            

            s.Insert(data );

            if ( n != null )
            {
                var ident = n as Ident;

                if (ident == null)
                    return data;

                ident.Symbol = data;
            }

            return data;
        }

        void Resolve( Node x, Scope beginScope = null )
        {
            var ident = x as Ident;

            if (ident == null)
                return;

            if ( beginScope == null )
            {
                beginScope = _topScope;
            }

            ident.BaseScope = beginScope;

            for (var s = beginScope; s != null; s = s.Outter)
            {
                var data = s.FindSymbol(ident.Name);
                if ( data != null )
                {
                    ident.Symbol = data;

                    // 从闭包开始搜索, 如果已经不是在本层找到, 一定是upvalue
                    if (beginScope.Type == ScopeType.Closure && s != beginScope)
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
