using GeoAPI.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using SQLSpatial.Tools.NetFramework.Helpers;
using SQLSpatial.Tools.NetFramework.Interfaces;

namespace SQLSpatial.Tools.NetFramework.Core
{
    abstract public class SpatialClassCore
    {
        protected static readonly CoordinateTransformationFactory _ctFactory = new CoordinateTransformationFactory();
        protected static readonly ICoordinateTransformation _transformation;
        protected static readonly ICoordinateTransformation _inverseTransformation;
        protected static IGeometryHelpers geometryHelpers;

        static SpatialClassCore()
        {
            geometryHelpers = new GeometryHelpers();

            // Definir el sistema de coordenadas de origen (EPSG:4326 - WGS 84)
            var wgs84 = GeographicCoordinateSystem.WGS84;

            // Definir el sistema de coordenadas de destino (EPSG:32616 - UTM Zona 16N)
            var utm16n = ProjectedCoordinateSystem.WGS84_UTM(16, true);

            // Crear la transformación
            _transformation = _ctFactory.CreateFromCoordinateSystems(wgs84, utm16n);

            _inverseTransformation = _ctFactory.CreateFromCoordinateSystems(utm16n, wgs84);
        }
    }
}
