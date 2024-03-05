using UnityEngine;

namespace QuantumClock {
    public static class PathRescaner {
        public static void Rescan(Bounds bounds) => AstarPath.active.UpdateGraphs(bounds);

        public static void Rescan(Vector2 pos, Vector2 size) => Rescan(new Bounds(pos, size));

        public static void Rescan(Collider2D collider) => Rescan(collider.bounds);
    }
}