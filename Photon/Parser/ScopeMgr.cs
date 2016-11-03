using Photon.AST;
using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class ScriptParser
    {
        Scope _topScope;
        Scope _global;

        void InitScope( )
        {
            OpenScope( null, ScopeType.Global );
            _global = _topScope;
        }

        void OpenScope( Scope outter, ScopeType type)
        {
            var s = new Scope(outter, type );
            _topScope = s;
        }

        void CloseScope( )
        {
            _topScope = _topScope.Outter;
        }

        Symbol Declare(Node n, Scope s, string name)
        {
            if ( s.Lookup(name) != null )
            {
                Error( string.Format("{0} redeclared", name ));
            }

            Symbol data = new Symbol();
            data.Name = name;
            data.Decl = n;

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
    }
}
