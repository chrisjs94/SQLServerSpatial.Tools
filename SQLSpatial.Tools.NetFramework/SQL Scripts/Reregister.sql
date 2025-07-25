USE master;

DROP FUNCTION IF EXISTS dbo.ToUTM;
GO
DROP FUNCTION IF EXISTS dbo.ToGeomUTM;
GO
DROP FUNCTION IF EXISTS dbo.ToLatLng;
GO

DROP ASSEMBLY IF EXISTS [ALMA.SQLSpatial.Tools];
CREATE ASSEMBLY [ALMA.SQLSpatial.Tools] 
FROM 'C:\Assemblies\SQLServerSpatialTools\SQLSpatial.Tools.NetFramework.dll'
WITH PERMISSION_SET = UNSAFE;
GO

CREATE FUNCTION dbo.ToUTM(@Lat FLOAT, @Lon FLOAT)
RETURNS NVARCHAR(100)
AS EXTERNAL NAME [ALMA.SQLSpatial.Tools].[SQLSpatial.Tools.SqlFunctions.SpatialFunctions].[TransformToUTM];
GO

CREATE FUNCTION dbo.ToLatLng(@X FLOAT, @Y FLOAT)
RETURNS NVARCHAR(100)
AS EXTERNAL NAME [ALMA.SQLSpatial.Tools].[SQLSpatial.Tools.SqlFunctions.SpatialFunctions].[TransformToLatLng];
GO

CREATE FUNCTION dbo.ToGeomUTM(@geom geometry)
RETURNS GEOMETRY
AS EXTERNAL NAME [ALMA.SQLSpatial.Tools].[SQLSpatial.Tools.SqlFunctions.SpatialFunctions].[ReprojectGeometryToUTM];
GO
