using CommandLine;

namespace ConfigTransformerCore
{
    public class Options
    {
        [Option('s', "sourceFile", Required = true, HelpText = "Base XML file to be converted.")]
        public string SourceFile { get; set; }

        [Option('t', "transformFile", Required = true, HelpText = "XML file containing transforms")]
        public string TransformFile { get; set; }

        [Option('d', "destinationFile", Required = true, HelpText = "Destination file name")]
        public string DestinationFile { get; set; }

        [Option('v', "verbose", Required = false, Default = false, HelpText = "Enable verbose logging")]
        public bool Verbose { get; set; }
    }
}
