using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace LLVM.ClangTidy
{
    /// <summary>
    /// QuickInfoSourceProvider creates QuickInfoSource for text buffers currently 
    /// processed by Intellisense quick info. In this case the text buffer is provided 
    /// by QuickInfoController.
    /// </summary>
    [Export(typeof(IQuickInfoSourceProvider))]
    [ContentType("code")]
    [Name("ClangTidy QuickInfo Source")]
    //[Order(Before = "Default Quick Info Presenter")]
    internal class QuickInfoSourceProvider : IQuickInfoSourceProvider
    {
        [Import]
        IBufferTagAggregatorFactoryService AggService = null;

        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new QuickInfoSource(textBuffer, AggService.CreateTagAggregator<ValidationTag>(textBuffer));
        }
    }
}
