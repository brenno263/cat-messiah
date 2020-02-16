using UnityEngine;

namespace _General._Scripts.Utils
{

    public static class Utils
    {
        public static void GizmosDrawRect2D(float top, float right, float bottom, float left, Color color)
        {

            if (bottom >= top || left >= right)
            {
                color = Color.red;
            }
            
            var corners = new Vector2[4];
            
            corners[0] = new Vector2(left, top);
            corners[1] = new Vector2(right, top);
            corners[2] = new Vector2(right, bottom);
            corners[3] = new Vector2(left, bottom);

            Gizmos.color = color;
            for (int i = 1; i <= 4; i++)
            {
                Gizmos.DrawLine(corners[i - 1], corners[i % 4]);
            }
        }

        public static void GizmosDrawRect2D(float top, float right, float bottom, float left)
        {
            GizmosDrawRect2D(top, right, bottom, left, Color.white);
        }
    }
}