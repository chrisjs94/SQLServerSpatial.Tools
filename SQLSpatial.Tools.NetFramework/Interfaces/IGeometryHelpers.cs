using Microsoft.SqlServer.Types;

namespace SQLSpatial.Tools.NetFramework.Interfaces
{
    public interface IGeometryHelpers
    {
        SqlGeometry ReprojectPoint(SqlGeometry point);
        SqlGeometry ReprojectLineString(SqlGeometry line, SqlGeometryBuilder builder);
        SqlGeometry ReprojectPolygon(SqlGeometry poly, SqlGeometryBuilder builder);
        SqlGeometry ReprojectMultiGeometry(SqlGeometry multiGeom, string type);
    }
}
