using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text;

namespace LLVM.ClangTidy
{
    /// <summary>
    /// This class asks implicitly ValidationTagger for text spans containing clang-tidy 
    /// validation warning in given text buffer (visible text in code).
    /// Usually single SnapshotSpan is one of visible lines or selections of text in text editor 
    /// and a Snapshot is the content of a whole file opened in editor window.
    /// </summary>
    class Classifier : IClassifier
    {
        private IClassificationType ClassificationType;
        private ITagAggregator<ValidationTag> Tagger;
        private static Classifier ActiveClassifier = null;
        private SnapshotSpan CurrentSpan;

        internal Classifier(ITagAggregator<ValidationTag> tagger, IClassificationType classificationType)
        {
            Tagger = tagger;
            ClassificationType = classificationType;
        }

        /// <summary>
        /// Get every ValidationTag instance within the given span. Generally, the span in 
        /// question is the displayed portion of the file currently open in the Editor
        /// </summary>
        /// <param name="span">The span of text that will be searched for validation tags</param>
        /// <returns>A list of every relevant tag in the given span</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            ActiveClassifier = this;

            // After clang-tidy returns new results, tags will be automatically created only for 
            //   newly appearing text lines or focused code window.
            // To force-refresh tags in current window, store single span for the whole file and 
            //   call Invalidate() on this span when clang-tidy results are ready.
            if (CurrentSpan == null || CurrentSpan.Snapshot != span.Snapshot)
            {
                CurrentSpan = new SnapshotSpan(span.Snapshot, 0, span.Snapshot.Length);
            }

            IList<ClassificationSpan> classifiedSpans = new List<ClassificationSpan>();

            var tags = Tagger.GetTags(span);

            foreach (IMappingTagSpan<ValidationTag> tagSpan in tags)
            {
                var validationSpan = tagSpan.Span.GetSpans(span.Snapshot).First();
                classifiedSpans.Add(new ClassificationSpan(validationSpan, ClassificationType));
            }

            return classifiedSpans;
        }

        /// <summary>
        /// Create an event for when the Classification changes
        /// </summary>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        /// <summary>
        /// Force refresh a span stored on last update (assume it corresponds to currently active document)
        /// </summary>
        public void Invalidate()
        {
            ClassificationChanged?.Invoke(this, new ClassificationChangedEventArgs(CurrentSpan));
        }

        static public void InvalidateActiveClassifier()
        {
            if (ActiveClassifier != null)
                ActiveClassifier.Invalidate();
        }
    }
}
