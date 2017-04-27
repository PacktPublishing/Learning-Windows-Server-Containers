FROM microsoft/windowsservercore  

SHELL ["powershell"]  

RUN $ErrorActionPreference = 'Stop'; \  
    wget https://github.com/MSOpenTech/redis/releases/download/win-3.2.100/Redis-x64-3.2.100.zip -OutFile Redis-x64-3.2.100.zip ; \
    Expand-Archive Redis-x64-3.2.100.zip -dest 'C:\\Program Files\\Redis\\' ; \
    Remove-Item Redis-x64-3.2.100.zip -Force

RUN setx PATH '%PATH%;C:\\Program Files\\Redis\\'  

WORKDIR 'C:\\Program Files\\Redis\\'  

# Change to unprotected mode and open the daemon to listen on all interfaces.
RUN Get-Content redis.windows.conf | Where { $_ -notmatch 'bind 127.0.0.1' } | Set-Content redis.openport.conf ; \  
  Get-Content redis.openport.conf | Where { $_ -notmatch 'protected-mode yes' } | Set-Content redis.unprotected.conf ; \
  Add-Content redis.unprotected.conf 'protected-mode no' ; \
  Add-Content redis.unprotected.conf 'bind 0.0.0.0' ; 

RUN (Get-Content redis.unprotected.conf).replace('dbfilename dump.rdb', 'dbfilename redisdata.rdb') | Set-Content redis.unprotected.conf
RUN (Get-Content redis.unprotected.conf).replace('dir ./', 'dir c:\redisdatastore') | Set-Content redis.unprotected.conf
RUN (Get-Content redis.unprotected.conf).replace('dir ./', 'dir c:\redisdatastore') | Set-Content redis.unprotected.conf

EXPOSE 6379  

CMD .\\redis-server.exe .\\redis.unprotected.conf --port 6379 ; \  
    Write-Host Redis Started... ; \
    while ($true) { Start-Sleep -Seconds 3600 }