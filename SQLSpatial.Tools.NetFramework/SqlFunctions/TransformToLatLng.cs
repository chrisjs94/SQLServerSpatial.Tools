using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;

namespace SQLSpatial.Tools.SqlFunctions
{
    public partial class SpatialFunctions
    {
        /// <summary>
        /// Transforms the specified X and Y coordinates into a latitude and longitude string representation.
        /// </summary>
        /// <remarks>This method applies an inverse transformation to the provided coordinates and returns
        /// the result as a comma-separated string. Ensure that the input coordinates are valid for the
        /// transformation.</remarks>
        /// <param name="X">The X coordinate to transform. Must not be null.</param>
        /// <param name="Y">The Y coordinate to transform. Must not be null.</param>
        /// <returns>A <see cref="SqlString"/> containing the latitude and longitude in the format "latitude,longitude".</returns>
        /// <exception cref="ArgumentException">Thrown if either <paramref name="X"/> or <paramref name="Y"/> is null.</exception>
        [SqlFunction]
        public static SqlString TransformToLatLng(SqlDouble X, SqlDouble Y)
        {
            if (X.IsNull || Y.IsNull)
                throw new ArgumentException("X,Y no pueden ser nulos");

            // Aplicar la transformación
            double[] latlngCoords = _inverseTransformation.MathTransform.Transform(new double[] { X.Value, Y.Value });
            return new SqlString($"{latlngCoords[0]},{latlngCoords[1]}");
        }
    }
}
