FROM microsoft/iis

SHELL ["powershell"]

RUN Install-WindowsFeature NET-Framework-45-ASPNET

RUN Install-WindowsFeature Web-Asp-Net45

COPY ./publishoutput c:/inetpub/wwwroot

EXPOSE 80

CMD Write-Host IIS Started....; \

	While ($true) { Start-Sleep -Seconds 3600 }
