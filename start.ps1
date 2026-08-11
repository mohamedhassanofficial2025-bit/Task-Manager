Write-Host "========================================================" -ForegroundColor Cyan
Write-Host "Starting TaskManager (Backend + Frontend)" -ForegroundColor Cyan
Write-Host "Press Ctrl+C at any time to gracefully stop BOTH servers." -ForegroundColor Yellow
Write-Host "========================================================" -ForegroundColor Cyan

npx concurrently -n "BACKEND,FRONTEND" -c "green,blue" "cd Backend\TaskManager.API; dotnet run" "cd FrontEnd\task-Manager-ui; npm start"
