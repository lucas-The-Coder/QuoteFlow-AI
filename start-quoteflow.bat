@echo off
title QuoteFlow Auto-Starter
echo Starting QuoteFlow API...
start "QuoteFlow API" cmd /k "cd /d "T:\QuoteFlow AI\QuoteFlow AI" && dotnet watch run --urls=http://localhost:5284"

timeout /t 5 >nul

echo Starting QuoteFlow UI...
start "QuoteFlow UI" cmd /k "cd /d "T:\QuoteFlow AI\QuoteFlow AI" && npm start"

echo Both services launching... Press any key to close this window.
pause >nul
