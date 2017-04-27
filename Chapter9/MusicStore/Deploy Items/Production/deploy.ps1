param
(
	[ValidateNotNullOrEmpty()]
	#Container Image version
	[String]$Version
)

#clean up existing containers
if($(docker ps -a -q).length -gt 0)
{
	docker stop $(docker ps -a -q)
	docker rm $(docker ps -a -q)
}

#pull images from docker hub
docker pull microsoft/mssql-server-windows:latest
docker pull learningwsc/redis-server:latest
docker pull learningwsc/musicstore.api:$Version
docker pull learningwsc/musicstore:$Version

#Creating containers
docker run -d -p 1433:1433 --name musicstoresql -e sa_password=Password@123 -e ACCEPT_EULA=Y microsoft/mssql-server-windows
docker run -d -p 6379:6379 --name redis-server learningwsc/redis-server

Start-Sleep 20

docker run -d -p 5100:80 --name musicstoreapi1 -e ASPNETCORE_ENVIRONMENT=Production learningwsc/musicstore.api:$Version
docker run -d -p 5101:80 --name musicstoreapi2 -e ASPNETCORE_ENVIRONMENT=Production learningwsc/musicstore.api:$Version
docker run -d -p 5102:80 --name musicstoreapi3 -e ASPNETCORE_ENVIRONMENT=Production learningwsc/musicstore.api:$Version

docker run -d -p 5000:80 --name musicstore1 -e ASPNETCORE_ENVIRONMENT=Production learningwsc/musicstore:$Version
docker run -d -p 5001:80 --name musicstore2 -e ASPNETCORE_ENVIRONMENT=Production learningwsc/musicstore:$Version
docker run -d -p 5002:80 --name musicstore3 -e ASPNETCORE_ENVIRONMENT=Production learningwsc/musicstore:$Version

Start-Sleep 20

#Build NGinx Images
cd c:\musicstore
docker build -t learningwsc/nginx.musicstore.api:$Version ./NGinx.MusicStore.API
docker build -t learningwsc/nginx.musicstore:$Version ./NGinx.MusicStore

#Create NGinx Containers
docker run -d -p 81:81 --name musicstore.api.nginx learningwsc/nginx.musicstore.api:$Version
docker run -d -p 80:80 --name musicstore.nginx learningwsc/nginx.musicstore:$Version





