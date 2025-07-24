using Microsoft.SqlServer.Types;
using SQLSpatial.Tools.NetFramework.Interfaces;
using SQLSpatial.Tools.SqlFunctions;
using System.Data.SqlTypes;

namespace SQLSpatial.Tools.NetFramework.Helpers
{
    internal class GeometryHelpers : IGeometryHelpers
    {
        /// <summary>
        /// Reprojects a geographic point from its original coordinate system to the UTM coordinate system.
        /// </summary>
        /// <remarks>This method assumes that the input point is in a geographic coordinate system
        /// (latitude and longitude). The transformation to UTM is performed using the <c>TransformToUTM</c> method,
        /// which must return a  comma-separated string containing the UTM coordinates in the format
        /// "easting,northing".</remarks>
        /// <param name="point">A <see cref="SqlGeometry"/> object representing the geographic point to be reprojected.  The point must have
        /// valid latitude (Y) and longitude (X) coordinates.</param>
        /// <returns>A <see cref="SqlGeometry"/> object representing the reprojected point in the UTM coordinate system. The
        /// returned point uses the WGS 84 / UTM zone 16N spatial reference system (SRID 32616).</returns>
        public SqlGeometry ReprojectPoint(SqlGeometry point)
        {
            SqlString result = SpatialFunctions.TransformToUTM(point.STY, point.STX);
            string[] parts = result.Value.Split(',');
            return SqlGeometry.Point(double.Parse(parts[1]), double.Parse(parts[0]), 32616);
        }

        /// <summary>
        /// Reprojects a <see cref="SqlGeometry"/> LineString to a new coordinate system using UTM coordinates.
        /// </summary>
        /// <remarks>This method iterates through each point in the input LineString, converts its
        /// coordinates to UTM,  and constructs a new LineString in the target coordinate system using the provided <see
        /// cref="SqlGeometryBuilder"/>.</remarks>
        /// <param name="line">The <see cref="SqlGeometry"/> LineString to be reprojected. Must be a valid LineString geometry.</param>
        /// <param name="builder">The <see cref="SqlGeometryBuilder"/> used to construct the reprojected geometry.</param>
        /// <returns>A new <see cref="SqlGeometry"/> LineString representing the reprojected geometry.</returns>
        public SqlGeometry ReprojectLineString(SqlGeometry line, SqlGeometryBuilder builder)
        {
            builder.BeginGeometry(OpenGisGeometryType.LineString);
            int count = line.STNumPoints().Value;

            SqlGeometry pt = line.STPointN(1);
            builder.BeginFigure(SpatialFunctions.GetXFromUTM(pt).Value, SpatialFunctions.GetYFromUTM(pt).Value);
            for (int i = 2; i <= count; i++)
            {
                pt = line.STPointN(i);
                builder.AddLine(SpatialFunctions.GetXFromUTM(pt).Value, SpatialFunctions.GetYFromUTM(pt).Value);
            }
            builder.EndFigure();
            builder.EndGeometry();
            return builder.ConstructedGeometry;
        }

        /// <summary>
        /// Reprojects a polygon geometry to a new coordinate system using the provided geometry builder.
        /// </summary>
        /// <remarks>This method processes the exterior ring and all interior rings of the input polygon, 
        /// reprojecting each point using the <c>GetXFromUTM</c> and <c>GetYFromUTM</c> methods to transform  coordinates to the
        /// target coordinate system. The resulting geometry is constructed using  the provided <see
        /// cref="SqlGeometryBuilder"/>.</remarks>
        /// <param name="poly">The input polygon geometry to be reprojected. Must be a valid <see cref="SqlGeometry"/> object.</param>
        /// <param name="builder">The <see cref="SqlGeometryBuilder"/> used to construct the reprojected geometry.  The builder will be
        /// populated with the reprojected polygon's exterior and interior rings.</param>
        /// <returns>A <see cref="SqlGeometry"/> object representing the reprojected polygon.</returns>
        public SqlGeometry ReprojectPolygon(SqlGeometry poly, SqlGeometryBuilder builder)
        {
            builder.BeginGeometry(OpenGisGeometryType.Polygon);

            SqlGeometry ring = poly.STExteriorRing();
            int count = ring.STNumPoints().Value;
            SqlGeometry pt = ring.STPointN(1);
            builder.BeginFigure(SpatialFunctions.GetXFromUTM(pt).Value, SpatialFunctions.GetYFromUTM(pt).Value);
            for (int i = 2; i <= count; i++)
            {
                pt = ring.STPointN(i);
                builder.AddLine(SpatialFunctions.GetXFromUTM(pt).Value, SpatialFunctions.GetYFromUTM(pt).Value);
            }
            builder.EndFigure();

            int interiors = poly.STNumInteriorRing().Value;
            for (int r = 1; r <= interiors; r++)
            {
                ring = poly.STInteriorRingN(r);
                count = ring.STNumPoints().Value;
                pt = ring.STPointN(1);
                builder.BeginFigure(SpatialFunctions.GetXFromUTM(pt).Value, SpatialFunctions.GetYFromUTM(pt).Value);
                for (int i = 2; i <= count; i++)
                {
                    pt = ring.STPointN(i);
                    builder.AddLine(SpatialFunctions.GetXFromUTM(pt).Value, SpatialFunctions.GetYFromUTM(pt).Value);
                }
                builder.EndFigure();
            }

            builder.EndGeometry();
            return builder.ConstructedGeometry;
        }

        /// <summary>
        /// Reprojects a multi-geometry instance by reprojecting each individual geometry within it.
        /// </summary>
        /// <remarks>This method iterates through each geometry in the multi-geometry, reprojects it, and
        /// combines the results into a single geometry.  If the resulting geometry type does not match the specified
        /// <paramref name="type"/>, the method attempts to make the geometry valid.</remarks>
        /// <param name="multi">The <see cref="SqlGeometry"/> instance representing the multi-geometry to reproject. Must not be null or
        /// empty.</param>
        /// <param name="type">The expected geometry type of the result (e.g., "MultiPolygon", "MultiLineString").</param>
        /// <returns>A <see cref="SqlGeometry"/> instance representing the reprojected multi-geometry.  Returns <see
        /// cref="SqlGeometry.Null"/> if the input <paramref name="multi"/> is null or empty.</returns>
        public SqlGeometry ReprojectMultiGeometry(SqlGeometry multi, string type)
        {
            if (multi == null || multi.IsNull)
                return SqlGeometry.Null;

            SqlGeometry result = SqlGeometry.Null;

            int count = multi.STNumGeometries().Value;
            for (int i = 1; i <= count; i++)
            {
                SqlGeometry subGeom = multi.STGeometryN(i);
                SqlGeometry reprojected = SpatialFunctions.ReprojectGeometryToUTM(subGeom); // ← llamada recursiva

                result = result.IsNull ? reprojected : result.STUnion(reprojected);
            }

            // Aseguramos que el tipo final sea el mismo que el original
            if (result.STGeometryType().Value != type)
                result = result.MakeValid(); // opcional: fuerza tipo válido

            return result;
        }
    }
}
