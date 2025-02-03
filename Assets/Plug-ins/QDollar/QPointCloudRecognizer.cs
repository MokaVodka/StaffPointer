using System;
using UnityEngine;

namespace QDollar
{
	public struct Result {

		public string GestureClass;
		public float Score;
	}

    public class QPointCloudRecognizer
    {
        // $Q's two major optimization layers (Early Abandoning and Lower Bounding)
        public static bool UseEarlyAbandoning = true;
        public static bool UseLowerBounding   = true;

        // Classifies a candidate gesture against a set of templates.
        // Returns the class of the closest neighbor in the template set.
        public static Result Classify(Gesture candidate, Gesture[] templateSet)
        {
            float minDistance = float.MaxValue;
            string gestureClass = "";
            foreach (Gesture template in templateSet)
            {
                float dist = GreedyCloudMatch(candidate, template, minDistance);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    gestureClass = template.Name;
                }
            }

            //--Return Result--//
            //Get accuracy confidence by minDistance
            //Bigger minDistance -> Worse the prediction
            float score = Mathf.Abs((minDistance));
            string result = score > 32.0f ? "No match" : gestureClass; //32.0f seems to be good

            return new Result() {GestureClass = result, Score = score};
        }

        // Greedy search for a minimum-distance matching between two point clouds.
        // Implements Early Abandoning and Lower Bounding (LUT) optimizations.
        private static float GreedyCloudMatch(Gesture gesture1, Gesture gesture2, float minSoFar)
        {
            int n = gesture1.Points.Length;       // the two clouds should have the same number of points by now
            float eps = 0.5f;                     // controls the number of greedy search trials (eps is in [0..1])
            int step = (int)Math.Floor(Math.Pow(n, 1.0f - eps));

            if (UseLowerBounding)
            {
                float[] LB1 = ComputeLowerBound(gesture1.Points, gesture2.Points, gesture2.LUT, step);  // direction of matching: gesture1 --> gesture2
                float[] LB2 = ComputeLowerBound(gesture2.Points, gesture1.Points, gesture1.LUT, step);  // direction of matching: gesture2 --> gesture1
                for (int i = 0, indexLB = 0; i < n; i += step, indexLB++)
                {
                    if (LB1[indexLB] < minSoFar) minSoFar = Math.Min(minSoFar, CloudDistance(gesture1.Points, gesture2.Points, i, minSoFar));  // direction of matching: gesture1 --> gesture2 starting with index point i
                    if (LB2[indexLB] < minSoFar) minSoFar = Math.Min(minSoFar, CloudDistance(gesture2.Points, gesture1.Points, i, minSoFar));  // direction of matching: gesture2 --> gesture1 starting with index point i   
                }
            }
            else
            {
                for (int i = 0; i < n; i += step)
                {
                    minSoFar = Math.Min(minSoFar, CloudDistance(gesture1.Points, gesture2.Points, i, minSoFar));  // direction of matching: gesture1 --> gesture2 starting with index point i
                    minSoFar = Math.Min(minSoFar, CloudDistance(gesture2.Points, gesture1.Points, i, minSoFar));  // direction of matching: gesture2 --> gesture1 starting with index point i   
                }
            }

            return minSoFar;
        }

        /// Computes lower bounds for each starting point and the direction of matching from points1 to points2 
        private static float[] ComputeLowerBound(Point[] points1, Point[] points2, int[][] LUT, int step)
        {
            int n = points1.Length;
            float[] LB = new float[n / step + 1];
            float[] SAT = new float[n];

            LB[0] = 0;
            for (int i = 0; i < n; i++)
            {
                int index = LUT[points1[i].intY / Gesture.LUT_SCALE_FACTOR][points1[i].intX / Gesture.LUT_SCALE_FACTOR];
                float dist = Geometry.SqrEuclideanDistance(points1[i], points2[index]);
                SAT[i] = (i == 0) ? dist : SAT[i - 1] + dist;
                LB[0] += (n - i) * dist;
            }

            for (int i = step, indexLB = 1; i < n; i += step, indexLB++)
                LB[indexLB] = LB[0] + i * SAT[n - 1] - n * SAT[i - 1];
            return LB;
        }

        /// Computes the distance between two point clouds by performing a minimum-distance greedy matching
        /// starting with point startIndex
        private static float CloudDistance(Point[] points1, Point[] points2, int startIndex, float minSoFar)
        {
            int n = points1.Length;                // the two point clouds should have the same number of points by now
            int[] indexesNotMatched = new int[n];  // stores point indexes for points from the 2nd cloud that haven't been matched yet
            for (int j = 0; j < n; j++)
                indexesNotMatched[j] = j;

            float sum = 0;                // computes the sum of distances between matched points (i.e., the distance between the two clouds)
            int i = startIndex;           // start matching with point startIndex from the 1st cloud
            int weight = n;               // implements weights, decreasing from n to 1
            int indexNotMatched = 0;      // indexes the indexesNotMatched[..] array of points from the 2nd cloud that are not matched yet
            do
            {
                int index = -1;
                float minDistance = float.MaxValue;
                for (int j = indexNotMatched; j < n; j++)
                {
                    float dist = Geometry.SqrEuclideanDistance(points1[i], points2[indexesNotMatched[j]]);  // use the squared Euclidean distance
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        index = j;
                    }
                }
                indexesNotMatched[index] = indexesNotMatched[indexNotMatched];  // point indexesNotMatched[index] of the 2nd cloud is now matched to point i of the 1st cloud
                sum += (weight--) * minDistance;           // weight each distance with a confidence coefficient that decreases from n to 1

                if (UseEarlyAbandoning)
                {
                    if (sum >= minSoFar) 
                        return sum;       // implement early abandoning
                }

                i = (i + 1) % n;                           // advance to the next point in the 1st cloud
                indexNotMatched++;                         // update the number of points from the 2nd cloud that haven't been matched yet
            } while (i != startIndex);
            return sum;
        }
    }
}