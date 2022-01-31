dotnet sonarscanner `
         begin `
         /k:"mongofun" `
         /d:sonar.sources="src" `
         /d:sonar.host.url="$env:SONAR_ADDRESS" `
         /d:sonar.login="$env:SONAR_TOKEN" `
         /d:sonar.cs.xunit.reportsPaths="**/TestResults/TestResults.xml" `
         /d:sonar.coverageReportPaths="../sonarqubecoverage/SonarQube.xml" `
         /d:sonar.dotnet.excludeTestProjects=true `
         /d:sonar.exclusions="Playground/**"

dotnet build

dotnet test --no-build --collect:"XPlat Code Coverage"  --logger:xunit

reportgenerator "-reports:src\*.Tests\TestResults\*\coverage.cobertura.xml" "-targetdir:sonarqubecoverage" "-reporttypes:SonarQube"
 
dotnet sonarscanner end /d:sonar.login="$env:SONAR_TOKEN"