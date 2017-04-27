FROM microsoft/windowsservercore



LABEL Description=”IIS” Vendor=Microsoft” Version=”10?

RUN powershell -Command Add-WindowsFeature Web-Server


COPY ./*.* c:/inetpub/wwwroot/

EXPOSE 80

CMD "ping localhost -t"




