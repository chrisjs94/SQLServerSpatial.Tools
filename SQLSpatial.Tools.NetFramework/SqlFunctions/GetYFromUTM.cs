using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;

namespace SQLSpatial.Tools.SqlFunctions
{
    public partial class SpatialFunctions
    {
        /// <summary>
        /// Converts the given <see cref="SqlGeometry"/> point to UTM coordinates and retrieves the Y (northing) value.
        /// </summary>
        /// <param name="point">The <see cref="SqlGeometry"/> point to be converted. Must represent a valid geographic point.</param>
        /// <returns>The Y (northing) coordinate of the point in the UTM coordinate system as a <see cref="SqlDouble"/>.</returns>
        [SqlFunction]
        public static SqlDouble GetYFromUTM(SqlGeometry point)
        {
            SqlString result = TransformToUTM(point.STY, point.STX);
            string[] parts = result.Value.Split(',');
            return double.Parse(parts[1]);
        }
    }
}
