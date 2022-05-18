using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using NTypewriter;
using NTypewriter.CodeModel;
using NTypewriter.CodeModel.Functions;
using NTypewriter.CodeModel.Roslyn;

namespace Atrasti.TypeWriter
{
class Program
    {
        static async Task Main()
        {
            // 1) Load project
            AnalyzerManager manager = new AnalyzerManager();
            IProjectAnalyzer analyzer = manager.GetProject(@"../../../../Atrasti.API/Atrasti.API.csproj");
            AdhocWorkspace workspace = analyzer.GetWorkspace(false);
            var project = workspace.CurrentSolution.Projects.Where(x => x.Name == "Atrasti.API").First();

            // 2) Add xml documentation
            project = AddXmlDocumentation(project, typeof(ICodeModel));
            project = AddXmlDocumentation(project, typeof(ActionFunctions));
            project = AddXmlDocumentation(project, typeof(Scriban.Functions.StringFunctions));

            // 3) Compile
            var compilation = await project.GetCompilationAsync();

            // 4) Create CodeModel
            var codeModelConfiguration = new CodeModelConfiguration() { OmitSymbolsFromReferencedAssemblies = false };
            var codeModel = new CodeModel(compilation, codeModelConfiguration);

            // 5) Load template
            string template = File.ReadAllText(@"../../../CodeModel.nt");

            // 6) Add custom functions
            var ntypewriterConfig = new Configuration();
            //ntypewriterConfig.AddCustomFunctions(typeof(NTEConfig));

            // 7) Render
            var result = await NTypeWriter.Render(template, codeModel, ntypewriterConfig);

            if (!result.HasErrors)
            {
                foreach (var renderedItem in result.Items)
                {
                    var path = Path.Combine(@"../../../", renderedItem.Name);
                    File.WriteAllText(path, renderedItem.Content);
                }
            }
            else
            {
                foreach (var msg in result.Messages)
                {
                    Console.WriteLine(msg.Message);
                }
            }        
        }
        
        private static Project AddXmlDocumentation(Project project, Type type)
        {
            var assemblyPath = type.Assembly.Location;
            var assemblyXmlDocPath = Path.ChangeExtension(assemblyPath, "xml");
            var assemblyDocProvider = XmlDocumentationProvider.CreateFromFile(assemblyXmlDocPath);
            project = project.AddMetadataReference(MetadataReference.CreateFromFile(assemblyPath, documentation: assemblyDocProvider));
            return project;
        }
    }
}