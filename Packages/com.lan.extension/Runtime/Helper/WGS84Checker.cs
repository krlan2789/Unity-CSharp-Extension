using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAN.Helper {
    /// <summary>
    /// World Geodetic System 1984 (WGS84) Coordinate System
    /// </summary>
    public static class WGS84Checker {
        /// <summary>
        /// Check if valid coordinate
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static bool Check(double longitude, double latitude) {
            //Debug.Log($"Coordinate: ({longitude}, {latitude})");
            bool status = -180 <= longitude && longitude <= 180;
            //Debug.Log($"Coordinate.longitude.isValid: " + status);
            status = status && -90 <= latitude && latitude <= 90;
            //Debug.Log($"Coordinate.latitude.isValid: " + status);
            status = status && longitude != 0 && latitude != 0;
            return status;
        }

        /// <summary>
        /// Check if valid coordinate
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool Check(LocationInfo location) {
            return Check(location.longitude, location.latitude);
        }

        /// <summary>
        /// Check if valid coordinate
        /// </summary>
        /// <param name="location">X = Longitude, Y = Latitude</param>
        /// <returns></returns>
        public static bool Check(Vector2 location) {
            return Check(location.x, location.y);
        }
    }
}
