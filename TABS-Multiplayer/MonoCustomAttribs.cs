using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System;

namespace MonoMod
{
    [MonoModCustomMethodAttribute("InsertMethod")]
    public class InsertMethod : Attribute
    {
        public InsertMethod(Action TargetMethod, OpCode opCode, int count) {} // TargetMethod has to be in the same class!!!
    }

    static class MonoModRules
    {
        public static void InsertMethod(MethodDefinition method, CustomAttribute attrib)
        {
            if (!method.HasBody)
                return;

            Action TargetMethod = (Action) attrib.ConstructorArguments[0].Value;
            OpCode SearchOpCode = (OpCode) attrib.ConstructorArguments[1].Value;
            int count = (int) attrib.ConstructorArguments[2].Value;

            int ins = 0;
            foreach(Instruction i in method.Body.Instructions) {
                if(i.OpCode == SearchOpCode)
                {
                    if(ins == count)
                    {
                        ILProcessor ilp = method.Body.GetILProcessor();
                        Collection<MethodDefinition> methods = method.DeclaringType.Methods;

                        foreach(MethodDefinition func in methods)
                        {
                            if(func.Name.Equals(TargetMethod.Method.Name))
                            {
                                Instruction cIns = ilp.Create(OpCodes.Call, func);
                                ilp.InsertAfter(i, cIns);
                                break;
                            }
                        }
                        break;
                    }

                    ins++;
                }
            }

        }
    }
}