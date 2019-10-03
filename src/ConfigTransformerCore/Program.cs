using System;
using System.Collections.Generic;
using CommandLine;
using Microsoft.Web.XmlTransform;

namespace ConfigTransformerCore
{
    public static class Program
    {
        private static IXmlTransformationLogger _logger;

        public static IXmlTransformationLogger Logger
        {
            get => _logger ?? (_logger = new ConsoleLogger());
            set => _logger = value;
        }

        private static IFilesystemAdapter _filesystemAdapter;

        public static IFilesystemAdapter FilesystemAdapter
        {
            get => _filesystemAdapter ?? (_filesystemAdapter = new FilesystemAdapter());
            set => _filesystemAdapter = value;
        }

        public static void Main(string[] args)
        {
            var initialBackgroundColor = Console.BackgroundColor;
            var initialForegroundColor = Console.ForegroundColor;

            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(RunTransformation)
                .WithNotParsed(HandleParseError);

            Console.BackgroundColor = initialBackgroundColor;
            Console.ForegroundColor = initialForegroundColor;


            void RunTransformation(Options opts)
            {
                var success = new Transformer(Logger, FilesystemAdapter)
                    .Run(opts);

                if (!success) { Environment.ExitCode = -1;  }

            }

            void HandleParseError(IEnumerable<Error> errs)
            {
                Environment.ExitCode = -2;
            }
        }

    }
}
