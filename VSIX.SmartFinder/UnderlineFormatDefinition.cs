using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Geeks.GeeksProductivityTools
{
    #region Classification type/format exports

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UnderlineClassification")]
    [Name("UnderlineClassificationFormat")]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    internal sealed class UnderlineFormatDefinition : ClassificationFormatDefinition
    {
        public UnderlineFormatDefinition()
        {
            this.DisplayName = "Underline";
            this.TextDecorations = System.Windows.TextDecorations.Underline;
            this.ForegroundColor = Colors.Blue;
        }
    }

    #endregion

    #region Provider definition
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("projection")]
    [TagType(typeof(ClassificationTag))]
    internal class UnderlineClassifierProvider : IViewTaggerProvider
    {
        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UnderlineClassification")]
        internal static ClassificationTypeDefinition underlineClassificationType = null;

        static IClassificationType UnderlineClassification;
        public static UnderlineClassifier GetClassifierForView(ITextView view)
        {
            if (UnderlineClassification == null)
                return null;

            return view.Properties.GetOrCreateSingletonProperty(() => new UnderlineClassifier(view, UnderlineClassification));
        }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            if (UnderlineClassification == null)
                UnderlineClassification = ClassificationRegistry.GetClassificationType("UnderlineClassification");

            if (textView.TextBuffer != buffer)
                return null;

            return GetClassifierForView(textView) as ITagger<T>;
        }
    }
    #endregion

    internal class UnderlineClassifier : ITagger<ClassificationTag>
    {
        IClassificationType _classificationType;
        ITextView _textView;
        SnapshotSpan? _underlineSpan;

        internal UnderlineClassifier(ITextView textView, IClassificationType classificationType)
        {
            _textView = textView;
            _classificationType = classificationType;
            _underlineSpan = null;
        }

        #region Private helpers

        void SendEvent(SnapshotSpan span)
        {
            var temp = this.TagsChanged;
            if (temp != null)
                temp(this, new SnapshotSpanEventArgs(span));
        }

        #endregion

        #region UnderlineClassification public members

        public SnapshotSpan? CurrentUnderlineSpan => _underlineSpan; 

        public void SetUnderlineSpan(SnapshotSpan? span)
        {
            var oldSpan = _underlineSpan;
            _underlineSpan = span;

            if (!oldSpan.HasValue && !_underlineSpan.HasValue)
                return;

            else if (oldSpan.HasValue && _underlineSpan.HasValue && oldSpan == _underlineSpan)
                return;

            if (!_underlineSpan.HasValue)
            {
                this.SendEvent(oldSpan.Value);
            }
            else
            {
                var updateSpan = _underlineSpan.Value;
                if (oldSpan.HasValue)
                    updateSpan = new SnapshotSpan(updateSpan.Snapshot,
                        Span.FromBounds(Math.Min(updateSpan.Start, oldSpan.Value.Start),
                                        Math.Max(updateSpan.End, oldSpan.Value.End)));

                this.SendEvent(updateSpan);
            }
        }

        #endregion

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (!_underlineSpan.HasValue || spans.Count == 0)
                yield break;

            var request = new SnapshotSpan(spans[0].Start, spans[spans.Count - 1].End);
            var underline = _underlineSpan.Value.TranslateTo(request.Snapshot, SpanTrackingMode.EdgeInclusive);
            if (underline.IntersectsWith(request))
            {
                yield return new TagSpan<ClassificationTag>(underline, new ClassificationTag(_classificationType));
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}

