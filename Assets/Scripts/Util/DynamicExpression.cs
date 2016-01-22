//using System;
//using System.CodeDom.Compiler;
//using System.Reflection;
//using System.Text;
//using Mono.CSharp;
//
//public class DynamicExpression {
//	Type type;
//	Object obj;
//	MethodInfo methodInfo;
//
//
//	public DynamicExpression() {
//	}
//
//	
//	void BuildExpression(string expression) {
//		CSharpCodeProvider c = new CSharpCodeProvider();
//		CompilerParameters cp = new CompilerParameters();
//		
//		cp.ReferencedAssemblies.Add("system.dll");
//		
//		cp.CompilerOptions = "/t:library";
//		cp.GenerateInMemory = true;
//		
//		StringBuilder sb = new StringBuilder("");
//		sb.Append("using System;\n");
//		sb.Append("namespace CSCodeEvaler{ \n");
//		sb.Append("public class CSCodeEvaler{ \n");
//		sb.Append("public object EvalCode(){\n");
//		sb.Append("return " + expression + "; \n");
//		sb.Append("} \n");
//		sb.Append("} \n");
//		sb.Append("}\n");
//		
//		CompilerResults cr = c.CompileAssemblyFromSource(cp, sb.ToString());
//		if (cr.Errors.Count > 0)
//		{
//			throw new Exception(
//				string.Format("Error ({0}) evaluating: {1}", 
//			              cr.Errors[0].ErrorText, expression));
//		}
//		
//		System.Reflection.Assembly a = cr.CompiledAssembly;
//
//		obj = a.CreateInstance("CSCodeEvaler.CSCodeEvaler");
//		type = obj.GetType();
//		methodInfo = type.GetMethod("EvalCode");
//	}
//
//
//	public void Eval(object[] args = null) {
//		object s = methodInfo.Invoke(obj, args);
//		return s;
//	}
//}
//
