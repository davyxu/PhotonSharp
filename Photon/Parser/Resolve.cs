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
            OpenScope( null );
            _global = _topScope;
        }

        void OpenScope( Scope outter )
        {
            _topScope = new Scope(outter);
        }

        void CloseScope( )
        {
            _topScope = _topScope.Outter;
        }

        void Declare( Node n, Scope s, string ident)
        {
            if ( s.Lookup(ident) != null )
            {
                Error( string.Format("{0} redeclared", ident ));
            }

            ScopeMeta data = new ScopeMeta();
            data.Name = ident;
            data.Decl = n;

            s.Insert(data);
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
