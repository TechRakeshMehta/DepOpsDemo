rem MSBuild.exe MyProj.csproj /property:Configuration=Debug  

del build.log /F /Q

C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.FeedsFramework.BulkDataUploader\LPSFS.FeedsFramework.BulkDataUploader.csproj /property:Configuration=Debug >>build.log
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.FeedsFramework.BAL\LPSFS.FeedsFramework.BAL.csproj /property:Configuration=Debug >>build.log
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.FeedsFramework.DAL\LPSFS.FeedsFramework.DAL.csproj /property:Configuration=Debug >>build.log
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.FeedsFramework.Entity\LPSFS.FeedsFramework.Entity.csproj /property:Configuration=Debug >>build.log
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.FeedsFramework.Contracts\LPSFS.FeedsFramework.Contracts.csproj /property:Configuration=Debug >>build.log
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.FeedsFramework.FeedUtils\LPSFS.FeedsFramework.FeedUtils.csproj /property:Configuration=Debug >>build.log

C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.Entity.LegacyArchive\LPSFS.Entity.LegacyArchive.csproj /property:Configuration=Debug >>build.log
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.Entity\LPSFS.Entity.csproj /property:Configuration=Debug >>build.log

rem C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  LPSFS.SYSX.sln /property:Configuration=Debug >>build.log
echo "Check log file (build.log) for compilation details."
pause














