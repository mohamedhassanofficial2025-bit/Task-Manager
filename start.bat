@echo off
echo ========================================================
echo Starting TaskManager (Backend + Frontend)
echo Press Ctrl+C at any time to gracefully stop BOTH servers.
echo ========================================================

npx concurrently -n "BACKEND,FRONTEND" -c "green,blue" "cd Backend\TaskManager.API && dotnet run" "cd FrontEnd\task-Manager-ui && npm start"
