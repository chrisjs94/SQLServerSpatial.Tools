using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;

namespace SQLSpatial.Tools.SqlFunctions
{
    public partial class SpatialFunctions
    {
        /// <summary>
        /// Transforms geographic coordinates (latitude and longitude) into UTM (Universal Transverse Mercator)
        /// coordinates.
        /// </summary>
        /// <remarks>This method performs a coordinate transformation from geographic coordinates (WGS84)
        /// to UTM. The resulting UTM coordinates are returned as a comma-separated string in the format
        /// "Easting,Northing".</remarks>
        /// <param name="latitude">The latitude in decimal degrees. Must be between -80 and 84.</param>
        /// <param name="longitude">The longitude in decimal degrees. Must be between -180 and 180.</param>
        /// <returns>A <see cref="SqlString"/> containing the UTM coordinates in the format "Easting,Northing".</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="latitude"/> or <paramref name="longitude"/> is null,  or if the values are outside
        /// the valid ranges (-80 to 84 for latitude, -180 to 180 for longitude).</exception>
        [SqlFunction]
        public static SqlString TransformToUTM(SqlDouble latitude, SqlDouble longitude)
        {
            if (latitude.IsNull || longitude.IsNull)
                throw new ArgumentException("Las coordenadas no pueden ser nulas.");

            // Validar latitud y longitud dentro de los rangos permitidos
            if (latitude < -80 || latitude > 84)
                throw new ArgumentException("La latitud debe estar entre -80 y 84 grados.");
            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("La longitud debe estar entre -180 y 180 grados.");

            // Aplicar la transformación
            double[] utmCoords = _transformation.MathTransform.Transform(new double[] { longitude.Value, latitude.Value });
            return new SqlString($"{utmCoords[0]},{utmCoords[1]}");
        }
    }
}
