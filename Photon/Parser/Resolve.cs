using Photon.AST;
using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class Parser
    {
        Scope _topScope;
        Scope _global;

        void InitScope( )
        {
            OpenScope( null, "global" );
            _global = _topScope;
        }

        void OpenScope( Scope outter, string name )
        {
            _topScope = new Scope(outter, name);
        }

        void CloseScope( )
        {
            _topScope = _topScope.Outter;
        }

        ScopeMeta Declare(Node n, Scope s, string name)
        {
            if ( s.Lookup(name) != null )
            {
                Error( string.Format("{0} redeclared", name ));
            }

            ScopeMeta data = new ScopeMeta();
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

            Error(string.Format("undeclared symbol {0}", ident.Name));

        }
    }
}
