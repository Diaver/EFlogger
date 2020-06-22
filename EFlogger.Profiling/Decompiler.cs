using System;
using System.IO;
using System.Linq;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Ast;
using ICSharpCode.Decompiler.Ast.Transforms;
using Mono.Cecil;

namespace EFlogger.Profiling
{
    public static class Decompiler
    {
        public static string GetSourceCode(string pathToAssembly, string className, string methodName)
        {
            try
            {
                var assemblyDefinition = AssemblyDefinition.ReadAssembly(pathToAssembly);

                TypeDefinition assembleDefenition = assemblyDefinition.MainModule.Types.First(type => type.Name == className);
                MethodDefinition methodDefinition = assembleDefenition.Methods.First(method => method.Name == methodName);
                var output = new StringWriter();
                var plainTextOutput = new PlainTextOutput(output);
                DecompileMethod(methodDefinition, plainTextOutput);
                return output.ToString();
            }
            catch (Exception exception)
            {

                return
                    string.Format(
                        "Exception in decompling. \r\n Message:{0}, \r\n Inner Exception:{1}, \r\n StackTrace:{2}",
                        exception.Message, exception.InnerException, exception.StackTrace);
            }
            
        }

        private static void DecompileMethod(MethodDefinition method, ITextOutput output)
        {
            AstBuilder codeDomBuilder = CreateAstBuilder(currentType: method.DeclaringType, isSingleMember: true);
            if (method.IsConstructor && !method.IsStatic && !method.DeclaringType.IsValueType)
            {
                AddFieldsAndCtors(codeDomBuilder, method.DeclaringType, method.IsStatic);
                RunTransformsAndGenerateCode(codeDomBuilder, output);
            }
            else
            {
                codeDomBuilder.AddMethod(method);
                RunTransformsAndGenerateCode(codeDomBuilder, output);
            }
        }

        private static AstBuilder CreateAstBuilder(ModuleDefinition currentModule = null, TypeDefinition currentType = null, bool isSingleMember = false)
        {
            if (currentModule == null)
                currentModule = currentType.Module;
            var settings = new DecompilerSettings();
            if (isSingleMember)
            {
                settings = settings.Clone();
                settings.UsingDeclarations = false;
            }
            return new AstBuilder(
                new DecompilerContext(currentModule)
                {
                    CurrentType = currentType,
                    Settings = settings
                });
        }

        private static void AddFieldsAndCtors(AstBuilder codeDomBuilder, TypeDefinition declaringType, bool isStatic)
        {
            foreach (var field in declaringType.Fields)
            {
                if (field.IsStatic == isStatic)
                    codeDomBuilder.AddField(field);
            }
            foreach (var ctor in declaringType.Methods)
            {
                if (ctor.IsConstructor && ctor.IsStatic == isStatic)
                    codeDomBuilder.AddMethod(ctor);
            }
        }

        private static void RunTransformsAndGenerateCode(AstBuilder astBuilder, ITextOutput output, IAstTransform additionalTransform = null)
        {
            astBuilder.GenerateCode(output);
        }
    }
}
