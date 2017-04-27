FROM microsoft/windowsservercore

MAINTAINER srikanth@live.com



LABEL Description=”IIS” Vendor=Microsoft” Version=”10?

RUN powershell -Command Add-WindowsFeature Web-Server


COPY index.htm /inetpub/wwwroot/


EXPOSE 80

CMD [ “ping localhost -t” ]

