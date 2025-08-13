Write-Host "🚀 Iniciando DDD ASP.NET Core Services..." -ForegroundColor Green

Write-Host "📦 Iniciando containers Docker..." -ForegroundColor Yellow
docker-compose up -d

Write-Host "⏳ Aguardando serviços iniciarem..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

Write-Host "🗄️ Aplicando migrações do Auth Service..." -ForegroundColor Cyan
cd services/auth/src/Auth.Infrastructure
dotnet ef database update --startup-project ..\Auth.API
cd ..\..\..

Write-Host "🗄️ Aplicando migrações do Email Service..." -ForegroundColor Cyan
cd services/email/src/Email.Infrastructure
dotnet ef database update --startup-project ..\Email.API
cd ..\..\..

Write-Host "🔧 Iniciando Gateway..." -ForegroundColor Magenta
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd gateway/Gateway; dotnet run"

Write-Host "🔐 Iniciando Auth Service..." -ForegroundColor Blue
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd services/auth/src/Auth.API; dotnet run"

Write-Host "📧 Iniciando Email Service..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd services/email/src/Email.API; dotnet run"

Write-Host "✅ Todos os serviços foram iniciados!" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 Gateway: https://localhost:7000" -ForegroundColor Cyan
Write-Host "🔐 Auth Service: https://localhost:7001" -ForegroundColor Blue
Write-Host "📧 Email Service: https://localhost:7002" -ForegroundColor Green
Write-Host "🐰 RabbitMQ Management: http://localhost:15672 (guest/guest)" -ForegroundColor Yellow
Write-Host ""
Write-Host "📚 Swagger:" -ForegroundColor White
Write-Host "  - Auth: https://localhost:7001/swagger" -ForegroundColor Blue
Write-Host "  - Email: https://localhost:7002/swagger" -ForegroundColor Green
