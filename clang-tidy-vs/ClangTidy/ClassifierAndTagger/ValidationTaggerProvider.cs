using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace LLVM.ClangTidy
{
    /// <summary>
    /// Export a <see cref="ITaggerProvider"/>
    /// This class creates ValidationTagger for given text buffer allowing other 
    /// MEF components to use ValidationTagger's functionality of searching for text 
    /// spans containing clang-tidy warnings.
    /// </summary>
    [Export(typeof(ITaggerProvider))]
    [ContentType("code")]
    [TagType(typeof(ValidationTag))]
    class ValidationTaggerProvider : ITaggerProvider
    {
        /// <summary>
        /// Creates an instance of our custom ValidationTagger for a given buffer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer">The buffer we are creating the tagger for.</param>
        /// <returns>An instance of our custom ValidationTagger.</returns>
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            return new ValidationTagger(buffer) as ITagger<T>;
        }
    }
}
