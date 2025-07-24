USE master;
GO
ALTER DATABASE master SET TRUSTWORTHY ON;

CREATE CERTIFICATE AssemblyCert  
FROM FILE = 'C:\Workspace\CLR\SQL Server Project\SQLSpatial.Tools.NetFramework\Csilva.cer';

CREATE LOGIN AssemblyLogin FROM CERTIFICATE AssemblyCert;
GRANT UNSAFE ASSEMBLY TO AssemblyLogin;
GO

CREATE ASSEMBLY [ALMA.SQLSpatial.Tools]
FROM 'C:\Assemblies\SQLServerSpatialTools\SQLSpatial.Tools.NetFramework.dll'
WITH PERMISSION_SET = UNSAFE;
GO

/*
CREATE FUNCTION dbo.TransformToUTM(@Lat FLOAT, @Lon FLOAT)
RETURNS NVARCHAR(100)
AS EXTERNAL NAME [AssemblyName].[Namespace.ClassName].[Function];
GO
*/

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


SELECT dbo.TransformToUTM(12.34, -86.78);