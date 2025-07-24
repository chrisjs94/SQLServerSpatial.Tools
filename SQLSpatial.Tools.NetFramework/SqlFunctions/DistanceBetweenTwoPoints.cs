using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;
using System;
using System.Data.SqlTypes;

namespace SQLSpatial.Tools.SqlFunctions
{
    public partial class SpatialFunctions
    {
        /// <summary>
        /// Calculates the distance between two points represented as SqlGeometry objects.
        /// </summary>
        /// <param name="point1">The first point as SqlGeometry.</param>
        /// <param name="point2">The second point as SqlGeometry.</param>
        /// <param name="unit">The unit of measurement for the distance ('km' for kilometers, 'm' for meters).</param>
        /// <returns>The distance between the two points in the specified unit.</returns>
        /// <exception cref="ArgumentException">Thrown if the geometries are null or not of type Point, or if the unit is invalid.</exception>
        [SqlFunction]
        public static SqlDouble CalculateDistance(SqlGeometry point1, SqlGeometry point2, SqlString unit)
        {
            if (point1 == null || point2 == null || point1.IsNull || point2.IsNull)
            {
                throw new ArgumentException("Las geometrías no pueden ser nulas.");
            }

            // Verificar que ambas geometrías sean puntos
            if (point1.STGeometryType() != "Point" || point2.STGeometryType() != "Point")
            {
                throw new ArgumentException("Ambas geometrías deben ser de tipo Point.");
            }

            // Calcular la distancia entre los dos puntos
            double distance = point1.STDistance(point2).Value;

            // Convertir la distancia a la unidad solicitada
            switch (unit.Value.ToLower())
            {
                case "km":
                    return distance / 1000; // Convertir metros a kilómetros
                case "m":
                    return distance; // Devolver la distancia en metros
                default:
                    throw new ArgumentException("Unidad no válida. Use 'km' para kilómetros o 'm' para metros.");
            }
        }
    }
}
