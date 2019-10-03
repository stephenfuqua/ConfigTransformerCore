using Microsoft.Web.XmlTransform;
using System;

namespace ConfigTransformerCore
{
    public class Transformer
    {
        private readonly IXmlTransformationLogger _logger;
        private readonly IFilesystemAdapter _filesystemAdapter;

        public Transformer(IXmlTransformationLogger logger, IFilesystemAdapter filesystemAdapter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _filesystemAdapter = filesystemAdapter ?? throw new ArgumentNullException(nameof(filesystemAdapter));
        }

        public bool Run(Options opts)
        {
            if (opts == null) { throw new ArgumentNullException(nameof(opts)); }

            _logger.LogMessage($"Applying {opts.TransformFile} to {opts.SourceFile} with output to {opts.DestinationFile}");

            if (!_filesystemAdapter.FileExist(opts.SourceFile))
            {
                _logger.LogError($"Source file {opts.SourceFile} does not exist.");
                return false;
            }

            if (!_filesystemAdapter.FileExist(opts.TransformFile))
            {
                _logger.LogError($"Transform file {opts.TransformFile} does not exist.");
                return false;
            }

            var src = new XmlTransformableDocument { PreserveWhitespace = true };
            src.Load(opts.SourceFile);

            var transform = new XmlTransformation(opts.TransformFile, _logger);

            bool success = transform.Apply(src);
            if (success)
            {
                src.Save(opts.DestinationFile);

                return true;
            }

            return false;
        }
    }
}
