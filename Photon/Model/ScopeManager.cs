using SharpLexer;
using MarkSerializer;

namespace Photon
{

    class ScopeManager : IMarkSerializable
    {
        Scope _topScope;
        
        Scope _global;


        public void Serialize(BinarySerializer ser)
        {
            ser.Serialize(ref _global);
        }        


        internal Scope PackageScope
        {
            get { return _global; }
        }

        internal Scope TopScope
        {
            get { return _topScope; }
            set { _topScope = value; }
        }

        public ScopeManager()
        {
            OpenScope( ScopeType.Package, TokenPos.Invalid );
            _global = _topScope;
        }

        internal Scope OpenScope(ScopeType type, TokenPos pos)
        {
            var s = new Scope(_topScope, type, pos );
            _topScope = s;
            return s;
        }

        internal void CloseScope()
        {
            _topScope = _topScope.Outter;
        }

        internal Scope OpenClassScope(string name, TokenPos pos)
        {
            var exists = GetClassScope(name);
            if ( exists != null )
            {
                // 被函数提前定义， 所以这里补下主定义位置
                exists.CodePos = pos;
                return exists;
            }

            var s = OpenScope(ScopeType.Class, pos);
            s.ClassName = name;
            return s;
        }

        internal Scope GetClassScope(string name)
        {
            foreach( var s in _global.Child )
            {
                if ( s.Type == ScopeType.Class && s.ClassName == name )
                {
                    return s;
                }
            }

            return null;
        }

        internal static Symbol Declare(Node declareNode, Scope s, string name, TokenPos pos, SymbolUsage usage )
        {
            var pre = s.FindSymbol(name);
            if ( pre != null )
            {                
                throw new CompileException(string.Format("{0} redeclared, pre define: {1}", name, pre.DefinePos), pos);
            }

            Symbol data = new Symbol();
            data.Name = name;
            data.Decl = declareNode;
            data.DefinePos = pos;
            data.Usage = usage;
            

            s.Insert(data );

            if ( declareNode != null )
            {
                var ident = declareNode as Ident;

                if (ident == null)
                    return data;

                ident.Symbol = data;
            }

            return data;
        }

        internal void Resolve(Node x, Scope beginScope = null)
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
