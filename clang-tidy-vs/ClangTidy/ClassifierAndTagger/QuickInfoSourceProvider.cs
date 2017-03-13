using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace LLVM.ClangTidy
{
    [Export(typeof(IQuickInfoSourceProvider))]
    [ContentType("code")]
    [Name("ClangTidy QuickInfo Source")]
    //[Order(Before = "Default Quick Info Presenter")]
    internal class TestQuickInfoSourceProvider : IQuickInfoSourceProvider
    {
        [Import]
        IBufferTagAggregatorFactoryService AggService = null;

        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new TestQuickInfoSource(textBuffer, AggService.CreateTagAggregator<ValidationTag>(textBuffer));
        }
    }
}
