using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;
using SQLSpatial.Tools.NetFramework.Core;
using System.Data.SqlTypes;

namespace SQLSpatial.Tools.SqlFunctions
{
    public partial class SpatialFunctions
    {
        /// <summary>
        /// Converts the given <see cref="SqlGeometry"/> point to its UTM X coordinate.
        /// </summary>
        /// <param name="point">The <see cref="SqlGeometry"/> point to be converted. Must contain valid latitude and longitude values.</param>
        /// <returns>A <see cref="SqlDouble"/> representing the UTM X coordinate of the specified point.</returns>
        [SqlFunction]
        public static SqlDouble GetXFromUTM(SqlGeometry point)
        {
            SqlString result = TransformToUTM(point.STY, point.STX);
            string[] parts = result.Value.Split(',');
            return double.Parse(parts[0]);
        }
    }
}
