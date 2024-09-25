using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace LansUILib
{
    public class InjectUtils
    {
        // 注入一个委托到IL中，委托在IL指令流的当前位置调用
        public static void InjectDelegate(ILContext il, Action action)
        {
            // 创建一个IL游标，用于操作IL指令
            var c = new ILCursor(il);

            // 在当前IL指令流位置插入调用指定委托的方法
            c.Emit(OpCodes.Call, action.GetMethodInfo());
        }

        // 注入一个委托到IL中，委托在IL指令流的末尾调用
        public static void InjectDelegateEnd(ILContext il, Action action)
        {
            // 创建一个IL游标，用于操作IL指令
            var c = new ILCursor(il);
            // 移动游标到IL指令流的最后
            c.Goto(c.Instrs.Last());
            // 在IL指令流末尾插入调用指定委托的方法
            c.Emit(OpCodes.Call, action.GetMethodInfo());
        }

        // 根据一个布尔值委托决定是否跳过当前的IL指令流（跳过即返回）
        public static void InjectSkipOnBoolean(ILContext il, Func<bool> action)
        {
            // 创建一个IL游标，用于操作IL指令
            var c = new ILCursor(il);
            // 插入调用布尔值委托的方法
            c.Emit(OpCodes.Call, action.GetMethodInfo());
            // 定义一个标签，用于标记跳过的位置
            var after = c.DefineLabel();
            // 如果布尔值委托返回false，则跳转到标记位置
            c.Emit(OpCodes.Brfalse_S, after);
            // 返回（结束当前的IL指令流）
            c.Emit(OpCodes.Ret);
            // 标记跳过的位置
            c.MarkLabel(after);
        }

        // 根据一个布尔值委托决定是否跳过当前的IL指令流，并在跳过时调用返回值委托
        public static void InjectSkipOnBooleanWithReturnValue(ILContext il, Func<bool> action, Func<object> retValue)
        {
            // 创建一个IL游标，用于操作IL指令
            var c = new ILCursor(il);
            // 插入调用布尔值委托的方法
            c.Emit(OpCodes.Call, action.GetMethodInfo());
            // 定义一个标签，用于标记跳过的位置
            var after = c.DefineLabel();
            // 如果布尔值委托返回false，则跳转到标记位置
            c.Emit(OpCodes.Brfalse_S, after);
            // 在跳过时调用返回值委托的方法
            c.Emit(OpCodes.Call, retValue.GetMethodInfo());
            // 返回（结束当前的IL指令流）
            c.Emit(OpCodes.Ret);
            // 标记跳过的位置
            c.MarkLabel(after);
        }
    }
}
