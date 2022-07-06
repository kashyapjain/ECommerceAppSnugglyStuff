using System;

using SkiaSharp;

namespace SnugglyStuff.Transforms
{
    class TouchManipulationInfo
    {
        public SKPoint PreviousPoint { set; get; }

        public SKPoint NewPoint { set; get; }
    }
}