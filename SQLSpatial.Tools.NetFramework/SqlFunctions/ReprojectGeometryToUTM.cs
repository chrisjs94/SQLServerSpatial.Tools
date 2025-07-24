using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;
using System;

namespace SQLSpatial.Tools.SqlFunctions
{
    public partial class SpatialFunctions
    {
        /// <summary>
        /// Reprojects the specified <see cref="SqlGeometry"/> instance to a new spatial reference system.
        /// </summary>
        /// <remarks>This method determines the geometry type of the input and applies the appropriate
        /// reprojection logic. The output geometry will have its SRID set to 32616.</remarks>
        /// <param name="inputGeom">The input geometry to reproject. Must be a valid <see cref="SqlGeometry"/> instance.</param>
        /// <returns>A new <see cref="SqlGeometry"/> instance reprojected to the spatial reference system with SRID 32616.
        /// Returns <see cref="SqlGeometry.Null"/> if <paramref name="inputGeom"/> is null or represents a null
        /// geometry.</returns>
        /// <exception cref="ArgumentException">Thrown if the geometry type of <paramref name="inputGeom"/> is not supported for reprojection. Supported
        /// types include Point, LineString, Polygon, MultiPoint, MultiLineString, and MultiPolygon.</exception>
        [SqlFunction]
        public static SqlGeometry ReprojectGeometryToUTM(SqlGeometry inputGeom)
        {
            if (inputGeom == null || inputGeom.IsNull)
                return SqlGeometry.Null;

            string geomType = inputGeom.STGeometryType().Value;
            SqlGeometryBuilder builder = new SqlGeometryBuilder();
            builder.SetSrid(32616);

            switch (geomType)
            {
                case "Point":
                    return geometryHelpers.ReprojectPoint(inputGeom);

                case "LineString":
                    return geometryHelpers.ReprojectLineString(inputGeom, builder);

                case "Polygon":
                    return geometryHelpers.ReprojectPolygon(inputGeom, builder);

                case "MultiPolygon":
                case "MultiLineString":
                case "MultiPoint":
                    return geometryHelpers.ReprojectMultiGeometry(inputGeom, geomType);

                default:
                    return SqlGeometry.Null;
            }
        }
    }
}
