$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2("C:\Workspace\CLR\SQL Server Project\SQLSpatial.Tools.NetFramework\Csilva.pfx", "1234567890")
[System.IO.File]::WriteAllBytes("C:\Workspace\CLR\SQL Server Project\SQLSpatial.Tools.NetFramework\Csilva.cer", $cert.RawData)



Get-FileHash "SQLSpatial.Tools.NetFramework.dll" -Algorithm SHA512



Algorithm       Hash                                                                   Path
---------       ----                                                                   ----
SHA512          3862B6C72E83DDC7F1F25D362EEE29BE4BDC572635D4A11DA7966DBA119C6640BA3... C:\Workspace\CLR\SQL Server P...