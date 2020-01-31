using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace TemCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var methodInfo in typeof(Temtem.Core.WildMonstersLogic).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(x =>
                x.ReturnType == typeof(void) &&
                x.GetParameters().Length == 1 &&
                x.GetParameters()[0].ParameterType == typeof(Temtem.World.SpawnZoneDefinition)
            ).ToArray())
            {
                Console.WriteLine(methodInfo.Name);
            }


            var asm = AssemblyDefinition.ReadAssembly(@"E:\SteamLibrary\steamapps\common\Temtem\Temtem_Data\Managed\Assembly-CSharp.dll");

            var roFields = typeof(Temtem.Network.oejjngkeoec).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (TypeDefinition t in asm.MainModule.Types)
            {
                if (t.Name == "oejjngkeoec")
                {
                    foreach (FieldDefinition f in t.Fields)
                    {
                        if (f.Constant != null)
                            f.Name = f.Constant.ToString();
                        else
                        {
                            var field = roFields.First(f2 => f2.Name == f.Name);
                            f.Name = field.GetValue(null).ToString();
                        }

                    }
                    t.Name = "NetworkStrings";
                }
            }
            foreach (TypeDefinition t in asm.MainModule.Types)
            {
                if (t.Name == "NetworkLogic" || t.Name == "BattleClient" || t.Name == "WildMonstersLogic" || t.Name == "NPCInteract")
                {
                    foreach (MethodDefinition m in t.Methods)
                    {
                        var ins = m.Body.Instructions;
                        var name = ins.LastOrDefault(i => i.Operand != null && i.Operand.ToString().Contains("NetworkStrings"));
                        if (name == null)
                            continue;
                        var operand = name.Operand.ToString();
                        if (operand.Contains("Trp"))
                            Console.WriteLine("");
                        m.Name = "Send" + operand.Replace("System.String Temtem.Network.NetworkStrings::", "").Replace(".", "");

                    }
                }
            }
            asm.Write(@"E:\SteamLibrary\steamapps\common\Temtem\Temtem_Data\Managed\Assembly-CSharp2.dll");
        }
    }
}
