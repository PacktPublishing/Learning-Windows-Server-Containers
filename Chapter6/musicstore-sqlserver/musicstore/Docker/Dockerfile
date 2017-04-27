from microsoft/windowsservercore

COPY ./Docker /Windows/Temp/Setups

RUN powershell -executionpolicy unrestricted C:\Windows\Temp\Setups\Install-DotNetCore.ps1

WORKDIR /app

ENV ASPNETCORE_URLS http://*:80

EXPOSE 80

COPY ./publishoutput C:/app






