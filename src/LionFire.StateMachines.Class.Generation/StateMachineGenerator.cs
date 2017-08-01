﻿using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Validation;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Runtime.Loader;
using System.Diagnostics;
using LionFire.ExtensionMethods.CodeAnalysis;

// Tool: http://roslynquoter.azurewebsites.net/

namespace LionFire.StateMachines.Class.Generation
{
    public class StateMachineGenerator : ICodeGenerator
    {
        AttributeData attributeData;

        public StateMachineGenerator(AttributeData attributeData)
        {
            Requires.NotNull(attributeData, nameof(attributeData));
            this.attributeData = attributeData;


            AssemblyLoadContext.Default.Resolving += Default_Resolving;
        }

        private Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
        {
            Log("Resolving - " + arg2);
            return null;
        }


        public static HashSet<string> DefaultAfterPrefixes = new HashSet<string>()
            {
                "After"
            };

        public static HashSet<string> DefaultEnteringPrefixes = new HashSet<string>()
            {
                "On",
                "Enter",
                "OnEnter",
                "OnEntering",
            };
        public static HashSet<string> DefaultLeavingPrefixes = new HashSet<string>()
            {
                "Leave",
                "OnLeave",
                "OnLeaving",
            };

        public HashSet<string> AfterPrefixes
        {
            get => afterPrefixes ?? DefaultAfterPrefixes;
            set => afterPrefixes = value;
        }
        private HashSet<string> afterPrefixes;

        public HashSet<string> EnteringPrefixes
        {
            get => enteringPrefixes ?? DefaultEnteringPrefixes;
            set => enteringPrefixes = value;
        }
        private HashSet<string> enteringPrefixes;

        public HashSet<string> LeavingPrefixes
        {
            get => leavingPrefixes ?? DefaultLeavingPrefixes;
            set => leavingPrefixes = value;
        }
        private HashSet<string> leavingPrefixes;

        //public class TransitionInfo
        //{
        //    public Enum EnumValue { get; set; }
        //    public string PresentTense { get; set; }
        //    public string PastTense { get; set; }
        //}

        public StreamWriter log;

        public const BindingFlags bf = BindingFlags.Static | BindingFlags.Public;
        const string unusedIndicator = " - ";
        const string usedIndicator = " * ";

        [Conditional("DEBUG")]
        public void Log(string msg = null)
        {
            log?.WriteLine(msg);
        }



        public async Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {

            if (context.ProcessingMember == null) throw new ArgumentNullException("context.ProcessingMember");
            var dClass = (ClassDeclarationSyntax)context.ProcessingMember;
            if (context.SemanticModel == null) throw new Exception("no SemanticModel");
            var sm = context.SemanticModel;
            var typeInfo = context.SemanticModel.GetTypeInfo(dClass);

            using (log = new StreamWriter(new FileStream(@"C:\src\Core\obj\codegen." + dClass.Identifier + ".txt", FileMode.Create)))
            {
                //Log("Location: " + Assembly.GetEntryAssembly().Location);
                //Log("BaseDir: " + AppContext.BaseDirectory);
                //Log("Compilation: " + (context.Compilation.SourceModule.Name));
                //Log("Compilation: " + (context.Compilation.SourceModule.Locations.Select(l => l.ToString()).Aggregate((x, y) => x + ", " + y)));
                foreach (var r in context.Compilation.References)
                {
                    if (r.Display.Contains(".nuget")) continue;
                    Log("#r \"" + r.Display + "\"");

                    assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(r.Display));
                }
                Log();

                return await part2(context, progress, cancellationToken);
            }
        }

        List<Assembly> assemblies = new System.Collections.Generic.List<Assembly>();

        int i = 0;

        //public string GetFullName(INamedTypeSymbol nts)
        //{
        //    return nts.GetFullMetadataName();
        //    //List<string> list = new System.Collections.Generic.List<string>();

        //    //list.Add(nts.Name);
        //    //ISymbol s = nts;
        //    //while (s.ContainingNamespace != null && !string.IsNullOrWhiteSpace(s.Name))
        //    //{
        //    //    s = s.ContainingNamespace;
        //    //    list.Add(s.Name);
        //    //}
        //    //list.Reverse();
        //    //return list.Aggregate((x, y) => x + "." + y).TrimStart('.');
        //}
        private Type ResolveType(TransformationContext context, object o)
        {
            var result = TryResolveType(context, o);
            if (result != null) return result;

            var combined = ((INamedTypeSymbol)o).GetFullMetadataName();
            foreach (var a in assemblies)
            {
                Log(" - looking in " + a.FullName);
            }
            throw new Exception("Failed to resolve " + combined);
        }
        private Type TryResolveType(TransformationContext context, object o)
        {
            var combined = ((INamedTypeSymbol)o).GetFullMetadataName();

            var type = Type.GetType(combined);

            if (type == null)
            {
                foreach (var a in assemblies)
                {
                    type = a.GetType(combined);

                    if (type != null) break;
                    else
                    {
                        if (a.FullName.StartsWith("LionFire.Execution.Abstractions"))
                        {
                            foreach (var t in a.GetTypes())
                            {
                                Log(".. " + t.FullName);
                            }
                        }
                    }
                }
            }

            if (type != null) Log("Resolved " + type.FullName);
            else
            {
                Log("Failed to resolve " + combined);

                //type = context.Compilation.References
                //    .Select(context.Compilation.GetAssemblyOrModuleSymbol)
                //    .OfType<IAssemblySymbol>()
                //    .Select(assemblySymbol => assemblySymbol.GetTypeByMetadataName(combined));

                //if (type != null)
                //{
                //    Log("resolved using 2nd approach" + combined);
                //}
            }


            return type;
            //if (i++ == 0) return stateMachineAttribute.StateType;
            //else return stateMachineAttribute.TransitionType;

            /*  REVIEW -- See this for other options  https://github.com/dotnet/roslyn/issues/3864
              
              @robintheilade The GetTypeByMetadataName API exists on both the Compilation and the IAssemblySymbol types. Therefore, if you have a Compilation, you can get the interesting IAssemblySymbols from that, and then call IAssemblySymbol.GetTypeByMetadataName.

            compilation.References
            .Select(compilation.GetAssemblyOrModuleSymbol)
            .OfType<IAssemblySymbol>()
            .Select(assemblySymbol => assemblySymbol.GetTypeByMetadataName(typeMetadataName))
            Depending on your use case, you might also want to include Compilation.Assembly in the list of searched IAssemblySymbols.


                */
        }
        private Task<SyntaxList<MemberDeclarationSyntax>> part2(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            var dClass = (ClassDeclarationSyntax)context.ProcessingMember;

            StateMachineAttribute stateMachineAttribute;

            if (attributeData.ConstructorArguments.Any())
            {
                if (attributeData.ConstructorArguments[0].Value is int o)
                {
                    stateMachineAttribute = (StateMachineAttribute)Activator.CreateInstance(typeof(StateMachineAttribute), (StateMachineOptions)o);
                }
                else
                {
                    stateMachineAttribute = (StateMachineAttribute)Activator.CreateInstance(typeof(StateMachineAttribute),
                        ResolveType(context, attributeData.ConstructorArguments[0].Value),
                        ResolveType(context, attributeData.ConstructorArguments[1].Value),
                        (StateMachineOptions)attributeData.ConstructorArguments[2].Value
                        );
                }
            }
            else
            {
                stateMachineAttribute = (StateMachineAttribute)Activator.CreateInstance(typeof(StateMachineAttribute));
            }

            Log();
            Log("StateMachine: ");
            Log(" - StateType: " + stateMachineAttribute.StateType.FullName);
            Log(" - TransitionType: " + stateMachineAttribute.TransitionType.FullName);
            Log(" - Options: " + stateMachineAttribute.Options.ToString());
            Log();
            foreach (var na in attributeData.NamedArguments)
            {
                var pi = typeof(StateMachineAttribute).GetProperty(na.Key);
                pi.SetValue(stateMachineAttribute, na.Value.Value);
            }

            if (stateMachineAttribute.Options.HasFlag(StateMachineOptions.DisableGeneration)) { return Task.FromResult(new SyntaxList<MemberDeclarationSyntax>()); }

            Type stateType = stateMachineAttribute.StateType;
            Type transitionType = stateMachineAttribute.TransitionType;

            var allStates = new HashSet<string>(stateType.GetFields(bf).Select(fi => fi.Name));
            var allTransitions = new HashSet<string>(transitionType.GetFields(bf).Select(fi => fi.Name));
            HashSet<string> usedStates;
            HashSet<string> usedTransitions;

            if (stateMachineAttribute.Options.HasFlag(StateMachineOptions.DisablePruneUnusedTransitions))
            {
                usedTransitions = allTransitions;
            }
            else
            {
                usedTransitions = new HashSet<string>();
                foreach (var md in dClass.Members.OfType<MethodDeclarationSyntax>())
                {
                    foreach (var transition in allTransitions)
                    {
                        if (md.Identifier.Text.EndsWith(transition))
                        {
                            var prefix = md.Identifier.Text.Substring(0, md.Identifier.Text.Length - transition.Length);

                            if (!usedTransitions.Contains(transition))
                            {
                                //Log(" - method " + md.Identifier.Text + $"() for {transition}");
                                usedTransitions.Add(transition);
                            }
                        }
                    }
                }
            }



            if (stateMachineAttribute.Options.HasFlag(StateMachineOptions.DisablePruneUnusedStates))
            {
                usedStates = allStates;
            }
            else
            {
                usedStates = new HashSet<string>();
                foreach (var md in dClass.Members.OfType<MethodDeclarationSyntax>())
                {
                    foreach (var state in allStates)
                    {
                        if (md.Identifier.Text.EndsWith(state))
                        {
                            var prefix = md.Identifier.Text.Substring(0, md.Identifier.Text.Length - state.Length);

                            if (!usedStates.Contains(state))
                            {
                                //Log(" - method " + md.Identifier.Text + $"() for state '{state}'");
                                usedStates.Add(state);
                            }
                        }
                    }
                }
                foreach (var transition in usedTransitions)
                {
                    var info = StateMachine.GetTransitionInfo(stateType, transitionType, Enum.Parse(stateMachineAttribute.TransitionType, transition));

                    if (info != null)
                    {
                        if (!usedStates.Contains(info.From.ToString())) usedStates.Add(info.From.ToString());
                        if (!usedStates.Contains(info.To.ToString())) usedStates.Add(info.To.ToString());
                    }
                }
            }

            Log("States:");

            foreach (var n in stateMachineAttribute.StateType.GetFields(bf).Select(fi => fi.Name))
            {
                //bool isUsed = ;
                Log((usedStates.Contains(n) ? usedIndicator : unusedIndicator) + n);
            }
            Log();

            Log("Transitions:");
            foreach (var n in stateMachineAttribute.TransitionType.GetFields(bf).Select(fi => fi.Name))
            {
                //bool isUsed = ;
                Log((usedTransitions.Contains(n) ? usedIndicator : unusedIndicator) + n);
            }
            Log();

            //CompilationUnitSyntax cu = SF.ClassDeclaration()
            //       .AddUsings(SF.UsingDirective(SF.IdentifierName("System")))
            //       .AddUsings(SF.UsingDirective(SF.IdentifierName("LionFire.StateMachines.Class")))
            //    ;

            ClassDeclarationSyntax c = SF.ClassDeclaration(dClass.Identifier).AddModifiers(SF.Token(SyntaxKind.PartialKeyword));

            //foreach (var used in usedStates)
            //{
            //    var m = SF.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), used);
            //    //m = m.WithBody((BlockSyntax)BlockSyntax.DeserializeFrom(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{}"))));
            //    var block = SF.Block();
            //    m = m.WithBody(block);
            //    c = c.AddMembers(m);
            //}
            foreach (var used in usedTransitions)
            {
                var md = MethodDeclaration(
                            PredefinedType(
                                Token(SyntaxKind.VoidKeyword)),
                            Identifier(used))
                        .WithModifiers(
                            TokenList(
                                Token(SyntaxKind.PublicKeyword)))
                        .WithExpressionBody(
                            ArrowExpressionClause(
                                        //SingletonList<StatementSyntax>(
                                        //    ExpressionStatement(
                                        InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName(stateMachineAttribute.StateMachineStatePropertyName),
                                                IdentifierName(nameof(StateMachineState<object, object, object>.ChangeState))))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList<ArgumentSyntax>(
                                                    Argument(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName(transitionType.Name),
                                                            IdentifierName(used))))))))
                                                            //))
                                                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                ;




                //var m = SF.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), used);
                //m = m.WithBody((BlockSyntax)BlockSyntax.DeserializeFrom(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{}"))));
                //var block = SF.Block();

                ////StateMachine.ChangeState(ExecutionTransition.Initialize);
                //block.AddStatements(
                //    SF.ExpressionStatement(SyntaxFactory.InvocationExpression(
                //            SF.MemberAccessExpression(
                //                SyntaxKind.SimpleMemberAccessExpression,
                //                SF.IdentifierName("StateMachine"),
                //                SF.IdentifierName("ChangeState")
                //            )),
                //        SF.SeparatedList<SyntaxNode>(
                //            //SF.Argument(SF.MemberAccessExpression(SyntaxKind.Enum))
                //            )
                //            )
                //    )
                //    );

                //m = m.WithBody(block);
                c = c.AddMembers(md);

            }

            //foreach (var used in usedStates)
            //{
            //    var md = MethodDeclaration(
            //                PredefinedType(
            //                    Token(SyntaxKind.VoidKeyword)),
            //                Identifier(used))
            //            .WithModifiers(
            //                TokenList(
            //                    Token(SyntaxKind.PublicKeyword)))
            //            .WithExpressionBody(
            //                ArrowExpressionClause(
            //                            //SingletonList<StatementSyntax>(
            //                            //    ExpressionStatement(
            //                            InvocationExpression(
            //                                MemberAccessExpression(
            //                                    SyntaxKind.SimpleMemberAccessExpression,
            //                                    IdentifierName("stateMachine"),
            //                                    IdentifierName("ChangeState")))
            //                            .WithArgumentList(
            //                                ArgumentList(
            //                                    SingletonSeparatedList<ArgumentSyntax>(
            //                                        Argument(
            //                                            MemberAccessExpression(
            //                                                SyntaxKind.SimpleMemberAccessExpression,
            //                                                IdentifierName(transitionType.Name),
            //                                                IdentifierName(used))))))))
            //                                                //))
            //                                                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
            //    ;
            //    c = c.AddMembers(md);

            //}
            //NamespaceDeclarationSyntax ns = SF.NamespaceDeclaration(SF.IdentifierName(typeInfo.Type.ContainingNamespace.Name));
            //cu = cu.AddMembers(ns);

            //ClassDeclarationSyntax c = SF.ClassDeclaration(typeInfo.Type.Name)
            //    //.AddModifiers(SF.Token(SyntaxKind.PrivateKeyword))
            //    .AddModifiers(SF.Token(SyntaxKind.PartialKeyword))
            //    ;
            //ns = ns.AddMembers(c);

            var results = SyntaxFactory.List<MemberDeclarationSyntax>();
            results = results.Add(c);
            return Task.FromResult(results);



            //// Our generator is applied to any class that our attribute is applied to.



            //ns = ns.AddMembers(c);


            //// Apply a suffix to the name of a copy of the class.
            ////var copy = applyToClass .WithIdentifier(SyntaxFactory.Identifier(applyToClass.Identifier.ValueText));

            //applyToClass

            //// Return our modified copy. It will be added to the user's project for compilation.
            //results = results.Add(copy);
            //return Task.FromResult<SyntaxList<MemberDeclarationSyntax>>(results);
        }
    }

}
