
namespace Photon
{
    // S: 栈  R: 寄存器 K:常量表　I:索引 F: 函数表 Top: 栈顶 M: 类成员
    enum Opcode
    {
        NOP = 0,

        // 双目元操作
        ADD,    // S[Top] = S[I] + S[I+1]
        SUB,    // S[Top] = S[I] - S[I+1]
        MUL,    // S[Top] = S[I] * S[I+1]
        DIV,    // S[Top] = S[I] / S[I+1]
        GT,     // S[Top] = S[I] > S[I+1]
        GE,     // S[Top] = S[I] >= S[I+1]
        LT,     // S[Top] = S[I] < S[I+1]
        LE,     // S[Top] = S[I] <= S[I+1]
        EQ,     // S[Top] = S[I] == S[I+1]
        NE,     // S[Top] = S[I] != S[I+1]

        // 单目元操作
        MINUS,
        NOT,
        LEN,
        

        // 存取操作
        LOADK,  // S[Top] = K[I]
        LOADR,  // S[Top] = R[I+ RegBase] 
        LOADG,  // S[Top] = R[I]
        LOADU,  // S[Top] = & R[I]           // Upvalue
        LOADF,  // S[Top] = F[PackageID + CmdSetID]        
        LOADM,  // S[Top] = Class[C].Member[I]
        SETR,   // R[I+RegBase] = S[I+1]
        SETG,   // R[I] = S[I+1]
        SETU,   // R[I] = * S[I+1]
        SETM,   // Class[C].Member[I] = S[I+1]
        IDXM,   // S[Top] = S[Top].Member[I]
        IDX,    // S[Top] = S[I][ S[I+1] ]  key为非字符串
        SEL,    // S[Top] = S[I][ P1 ] 显式字符串key调用
        LINKU,  // 让Upvalue与寄存器建立引用
        CLOSURE, //  创建闭包
        NEW,    // C[I] -> ClassInstance

        // 流程控制        
        CALL,  // S[I](S[I+1], ... ) 栈交换
        RET,    // 
        JZ,    // S[Top] != 0
        JMP,    // 无条件
        EXIT,


        MAX,
    }





}
