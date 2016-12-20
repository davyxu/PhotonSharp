using System.Collections.Generic;
using MarkSerializer;

namespace Photon
{

    public partial class Package : IMarkSerializable
    {        
        string _name;
     
        int _id;

        // 源码
        List<CodeFile> _code = new List<CodeFile>();

        // 父级
        Executable _exe;

        // 包的作用域        
        ScopeManager _scopeManager;

        // 第一次pass无法搞定的node
        List<CompileContext> _secondPass = new List<CompileContext>();
        
        internal ValuePhoFunc InitEntry = ValuePhoFunc.Empty;        

        internal List<CodeFile> FileList
        {
            get { return _code; }
        }

        internal Executable Exe
        {
            get { return _exe; }
            set { _exe = value; }
        }

        internal int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
        }

        internal Scope TopScope
        {
            get { return _scopeManager.TopScope; }
        }

        internal Scope PackageScope
        {
            get { return _scopeManager.PackageScope; }
        }

        internal ScopeManager ScopeMgr
        {
            get { return _scopeManager; }
        }

        public Package( )
        {        
            _scopeManager = new ScopeManager();
        }

        internal Package( string name )
        {
            _name = name;            
            _scopeManager = new ScopeManager();
        }


        public void Serialize(BinarySerializer ser)
        {
            ser.Serialize<string>(_name);
            ser.Serialize<int>(_id);
            ser.Serialize<ScopeManager>(_scopeManager);
            ser.Serialize<ValuePhoFunc>(InitEntry);
        }

        public void Deserialize(BinaryDeserializer ser)
        {
            _name = ser.Deserialize<string>();
            _id = ser.Deserialize<int>();
            _scopeManager = ser.Deserialize<ScopeManager>();
            InitEntry = ser.Deserialize<ValuePhoFunc>();
        }

        int _closureCount;
        internal ObjectName GenClosureName()
        {
            _closureCount++;

            return new ObjectName(this.Name, "closure" + _closureCount.ToString());
        }

        // 第二次pass
        internal void ResolveNode()
        {
            foreach (var ctx in _secondPass)
            {
                ctx.node.Resolve(ctx.parameter);
            }

        }

        internal void AddCode(CodeFile code)
        {
            _code.Add(code);
        }

        internal void Compile(CompileParameter param)
        {
            foreach (var f in _code)
            {
                f.AST.Compile(param);
            }
        }

        public override string ToString()
        {
            return _name;
        }

        internal bool ContainSecondPassNode( Node n )
        {
            foreach( var c in _secondPass )
            {
                if (c.node == n)
                    return true;
            }

            return false;
        }

        internal void AddSecondPass( Node n, CompileParameter cp )
        {
            CompileContext ctx;
            ctx.node = n;
            ctx.parameter = cp;

            _secondPass.Add(ctx);
        }

        static void PrintAST(Node n, string indent = "")
        {
            Logger.DebugLine(indent + n.ToString());

            foreach (var c in n.Child())
            {
                PrintAST(c, indent + "\t");
            }
        }

        internal void PrintAST()
        {
            foreach (var f in FileList)
            {
                // 语法树
                Logger.DebugLine(string.Format("'{0}' AST:", f.Source.Name));
                PrintAST(f.AST);
                Logger.DebugLine("");
            }
        }

        internal void PrintSymbols(  )
        {            
            if (TopScope != null )
            {
                // 符号
                Logger.DebugLine("Symbols:");
                TopScope.DebugPrint("");
                Logger.DebugLine("");
            }

        }

        internal void PrintInitEntry()
        {
            if (InitEntry == null)
                return;

            Logger.DebugLine(InitEntry.DebugString());
            InitEntry.DebugPrint(Exe);
        }
    }
}
