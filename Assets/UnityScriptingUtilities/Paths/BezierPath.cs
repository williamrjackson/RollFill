﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Wrj
{
    public class BezierPath : MonoBehaviour
    {
        public Transform slider;
        [Range(0, 1)]
        public float percent = 0f;
        [Range(2, 50)]
        public int res = 15;
        private PathGuide[] points;
        private Vector3[] curve;

        void Awake()
        {
            RefreshPath();
            foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
            {
                rend.enabled = false;
            }
        }

        void Update()
        {
            if (slider != null)
            {
                Vector3 look = Vector3.zero;
                slider.position = GetPointOnCurve(percent, ref look);
                slider.LookAt(look);
            }
        }

        // Build up a new curve with latest curveguides
        public void RefreshPath()
        {
            curve = CurvePath(res);
        }
        public Vector3 GetPointOnCurve(float t, ref Vector3 lookAt)
        {
            if (curve.Length < 2)
            {
                return Vector3.zero;
            }

            // Get total length of curve
            float posOnLine = GetCurveLength() * t;

            for (int i = 0; i < curve.Length - 1; i++)
            {
                Vector3 p0 = curve[i];
                Vector3 p1 = curve[i + 1];
                float currentDistance = Vector3.Distance(p1, p0);

                // If the remaining distance is greater than the distance between these vectors, subtract the current distance and proceed
                if (currentDistance < posOnLine)
                {
                    posOnLine -= currentDistance;
                    continue;
                }
                lookAt = Vector3.Lerp(p0, p1, posOnLine / currentDistance + .01f);
                return Vector3.Lerp(p0, p1, posOnLine / currentDistance);
            }
            // made it to the end
            return curve[curve.Length - 1];
        }

        // Returns total length of the curve
        public float GetCurveLength()
        {
            if (curve.Length < 1)
                return 0;

            float length = 0f;
            for (int i = 0; i < curve.Length - 1; i++)
            {
                length += Vector3.Distance(curve[i + 1], curve[i]);
            }
            return length;
        }

        // Cunstruct a curve path.
        public Vector3[] CurvePath(int resolution = 15)
        {
            // Collect PathGuides
            points = GetComponentsInChildren<PathGuide>();
            // Require at least 3, for start, influence, and end.
            if (points.Length < 3)
                return null;

            // Create vector array to hold the results (-2 is to accommodate the first and last)
            Vector3[] finalPoints = new Vector3[(points.Length - 2) * resolution];
            
            // Start at the front of the path
            Vector3 currentPos = points[0].transform.position;
            int finalPointIndex = 0;

            // Connect multiple quadratic curves from the mid-point between each PathGuide node
            for (int i = 1; i < points.Length - 2; i += 1)
            {
                Vector3 p0 = points[i].transform.position;
                Vector3 p1 = points[i + 1].transform.position;
                // Get the midpoint between p0 & p1
                Vector3 mid = Vector3.Lerp(p0, p1, .5f);
                finalPointIndex = (i * resolution) - resolution;
                foreach (Vector3 p in Wrj.Utils.QuadraticBezierCurve(currentPos, p0, mid, resolution))
                {
                    finalPoints[finalPointIndex] = p;
                    finalPointIndex++;
                }
                currentPos = mid;
            }
            finalPointIndex = (points.Length - 2) * resolution - resolution;
            foreach (Vector3 p in Wrj.Utils.QuadraticBezierCurve(currentPos, points[points.Length - 2].transform.position, points[points.Length - 1].transform.position, resolution))
            {
                finalPoints[finalPointIndex] = p;
                finalPointIndex++;
            }
            return finalPoints;
        }

        // Renumber curveguide children
        public void RefreshChildIndexes()
        {
            foreach (PathGuide cg in transform.GetComponentsInChildren<PathGuide>())
            {
                cg.name = "Node_" + cg.transform.GetSiblingIndex();
            }
        }
    }
}