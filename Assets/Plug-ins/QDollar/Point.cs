using System;

namespace QDollar
{
    // Implements a 2D Point that exposes X, Y, and StrokeID properties.
    // StrokeID is the stroke index the point belongs to (e.g., 0, 1, 2, ...) that is filled by counting pen down/up events.
    public class Point
    {
        public float X, Y;       // point coordinates
        public int StrokeID;     // the stroke index to which this point belongs
        public int intX, intY;   // integer coordinates for LUT indexing (used by $Q's lower bounding optimization; see QPointCloudRecognizer.cs)

        public Point(float x, float y, int strokeId)
        {
            this.X = x;
            this.Y = y;
            this.StrokeID = strokeId;
            this.intX = 0;
            this.intY = 0;
        }
    }
}
