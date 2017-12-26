using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LLBLGenKeygen
{
    public class BaseClass { }
    public interface IInterfaceA { }
    public interface IInterfaceB { }
    public class ClassT1 { }
    public class ClassT2 : BaseClass, IInterfaceA, IInterfaceB { }

    public class ReflectionTest
    {
        public void CreateGeneric()
        {
            //创建一个名“ReflectionTest”的动态程序集，这个程序集可以执行和保存。
            AppDomain myDomain = AppDomain.CurrentDomain;
            AssemblyName assemblyName = new AssemblyName("ReflectionTest");
            AssemblyBuilder assemblyBuilder = myDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            //在这个程序集中创建一个与程序集名相同的模块，接着创建一个类MyClass
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll");
            TypeBuilder myType = moduleBuilder.DefineType("MyClass", TypeAttributes.Public);

            //创建类型参数名，将达到这样的效果：public MyClass<TParam1, TParam2>
            string[] tNames = { "TName1", "TName2" };
            GenericTypeParameterBuilder[] gtps = myType.DefineGenericParameters(tNames);
            GenericTypeParameterBuilder tName1 = gtps[0];
            GenericTypeParameterBuilder tName2 = gtps[1];

            //为泛型添加约束，TName1将会被添加构造器约束和引用类型约束
            tName1.SetGenericParameterAttributes(GenericParameterAttributes.DefaultConstructorConstraint | GenericParameterAttributes.ReferenceTypeConstraint);
            //TName2达到的效果将是：where TName2：ValueType，IComparable，IEnumerable
            Type baseType = typeof(BaseClass);
            Type interfaceA = typeof(IInterfaceA);
            Type interfaceB = typeof(IInterfaceB);
            Type[] interfaceTypes = { interfaceA, interfaceB };
            tName2.SetBaseTypeConstraint(baseType);
            tName2.SetInterfaceConstraints(interfaceTypes);

            FieldBuilder fieldBuilder = myType.DefineField("name", typeof(string), FieldAttributes.Public);
            FieldBuilder fieldBuilder2 = myType.DefineField("tField1", tName1, FieldAttributes.Public);

            //为泛型类添加方法Hello
            Type listType = typeof(List<>);
            Type ReturnType = listType.MakeGenericType(tName1);
            Type[] parameter = { tName1.MakeArrayType() };
            MethodBuilder methodBuilder = myType.DefineMethod("Hello", MethodAttributes.Public | MethodAttributes.Static, ReturnType, parameter);

            //为方法添加方法体
            Type ienumof = typeof(IEnumerable<>);
            Type TFromListOf = listType.GetGenericArguments()[0];
            Type ienumOfT = ienumof.MakeGenericType(TFromListOf);
            Type[] ctorArgs = { ienumOfT };
            ConstructorInfo cInfo = listType.GetConstructor(ctorArgs);
            //最终的目的是要调用List<TName1>的构造函数：new List<TName1>(IEnumerable<TName1>);
            ConstructorInfo ctor = TypeBuilder.GetConstructor(ReturnType, cInfo);
            //设置IL指令
            ILGenerator msil = methodBuilder.GetILGenerator();
            msil.Emit(OpCodes.Ldarg_0);
            msil.Emit(OpCodes.Newobj, ctor);
            msil.Emit(OpCodes.Ret);
            //创建并保存程序集
            Type finished = myType.CreateType();
            assemblyBuilder.Save(assemblyName.Name + ".dll");
            //创建这个MyClass类
            Type[] typeArgs = { typeof(ClassT1), typeof(ClassT2) };
            Type constructed = finished.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(constructed);
            MethodInfo mi = constructed.GetMethod("Hello");
            ClassT1[] inputParameter = { new ClassT1(), new ClassT1() };
            object[] arguments = { inputParameter };
            List<ClassT1> listResult = (List < ClassT1 >) mi.Invoke(null, arguments);
            //查看返回结果中的数量和完全限定名
            Console.WriteLine(listResult.Count);
            Console.WriteLine(listResult[0].GetType().FullName);

            //查看类型参数以及约束
            foreach (Type t in finished.GetGenericArguments())
            {
                Console.WriteLine(t.ToString());
                foreach (Type c in t.GetGenericParameterConstraints())
                {
                    Console.WriteLine("  " + c.ToString());
                }
            }
        }
    }
}
