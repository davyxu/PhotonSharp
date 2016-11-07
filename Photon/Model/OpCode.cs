
namespace Photon.Model
{
    // S: 栈  R: 寄存器 G: 全局表  C:常量表　I:索引 Top: 栈顶
    public enum Opcode
    {
        Nop = 0,
        Add,    // S[Top] = S[I] + S[I+1]
        Sub,    // S[Top] = S[I] - S[I+1]
        Mul,    // S[Top] = S[I] * S[I+1]
        Div,    // S[Top] = S[I] / S[I+1]
        GT,     // S[Top] = S[I] > S[I+1]
        GE,     // S[Top] = S[I] >= S[I+1]
        LT,     // S[Top] = S[I] < S[I+1]
        LE,     // S[Top] = S[I] <= S[I+1]
        EQ,     // S[Top] = S[I] == S[I+1]
        NE,     // S[Top] = S[I] != S[I+1]

        LoadC,  // S[Top] = C[I]
        LoadR,  // S[Top] = R[I]
        LoadG,  // S[Top] = G[I]
        SetR,   // R[I] = S[I+1]
        SetG,   // G[I] = S[I+1]
        IndexR, // S[Top] = S[I][ S[I+1] ]  key为非字符串
        SelectR, // S[Top] = S[I][ P1 ] 显式字符串key调用
        
        Call,   // S[I](S[I+1], ... )
        Ret,    // 
        Jnz,    // S[Top] != 0
        Jmp,    // 无条件
        
        Exit,
        MAX,
    }





}
