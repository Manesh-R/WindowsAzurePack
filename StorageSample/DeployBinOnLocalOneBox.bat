SET ROOT_DIR=%~dp0

XCOPY /KERCHIQY "%ROOT_DIR%\AdminExtension\bin\Debug\*StorageSample*.dll" C:\inetpub\MgmtSvc-AdminSite\bin

XCOPY /KERCHIQY "%ROOT_DIR%\Api\bin\*StorageSample*.dll" C:\inetpub\MgmtSvc-StorageSample\bin

XCOPY /KERCHIQY "%ROOT_DIR%\TenantExtension\bin\Debug\*StorageSample*.dll" C:\inetpub\MgmtSvc-TenantSite\bin

XCOPY /KERCHIQY "%ROOT_DIR%\TenantExtension\Manifests" C:\inetpub\MgmtSvc-TenantSite\Manifests

XCOPY /KERCHIQY "%ROOT_DIR%\TenantExtension\Content"   C:\inetpub\MgmtSvc-TenantSite\Content\StorageSampleTenant

XCOPY /KERCHIQY "%ROOT_DIR%\AdminExtension\Manifests"  C:\inetpub\MgmtSvc-AdminSite\Manifests

XCOPY /KERCHIQY "%ROOT_DIR%\AdminExtension\Content"    C:\inetpub\MgmtSvc-AdminSite\Content\StorageSampleAdmin

IISRESET
