using Photon.AST;
using Photon.Model;
using SharpLexer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Photon.Parser
{
    public partial class ScriptParser
    {
        Scope _topScope;
        Scope _global;

        void InitScope( )
        {
            OpenScope( ScopeType.Global );
            _global = _topScope;
        }

        Scope OpenScope( ScopeType type)
        {
            var s = new Scope(_topScope, type);
            _topScope = s;
            return s;
        }

        void CloseScope( )
        {
            _topScope = _topScope.Outter;
        }

        Symbol Declare(Node n, Scope s, string name, TokenPos pos )
        {
            var pre = s.Lookup(name);
            if ( pre != null )
            {
                Error(string.Format("{0} redeclared, pre define: {1}", name, pre.DefinePos), pos);
            }

            Symbol data = new Symbol();
            data.Name = name;
            data.Decl = n;
            data.DefinePos = pos;

            s.Insert(data);

            var ident = n as Ident;

            if (ident == null)
                return data;

            ident.ScopeInfo = data;

            return data;
        }

        void Resolve( Node x )
        {
            var ident = x as Ident;

            if (ident == null)
                return;


            for (var s = _topScope; s != null; s = s.Outter)
            {
                var data = s.Lookup(ident.Name);
                if ( data != null )
                {
                    ident.ScopeInfo = data;
                    return;
                }
            }

            Error(string.Format("undeclared symbol {0}", ident.Name), ident.DefinePos );

        }

        public void PrintScopeSymbol( )
        {

            Debug.WriteLine("symbols:");

            _global.DebugPrint("");

            Debug.WriteLine("");
        }
    }
}
