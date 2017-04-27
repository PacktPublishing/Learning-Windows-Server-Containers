FROM microsoft/windowsservercore

LABEL Description="Nginx" Vendor=Nginx" Version="1.0.13"
RUN powershell -Command \
	$ErrorActionPreference = 'Stop'; \
	Invoke-WebRequest -Method Get -Uri http://nginx.org/download/nginx-1.9.13.zip -OutFile c:\nginx-1.9.13.zip ; \
	Expand-Archive -Path c:\nginx-1.9.13.zip -DestinationPath c:\ ; \
	Remove-Item c:\nginx-1.9.13.zip -Force
WORKDIR /nginx-1.9.13
COPY nginx.conf c:/nginx-1.9.13/conf/
CMD ["/nginx-1.9.13/nginx.exe"]