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
Start-Sleep 10
docker run -d -p 81:80 --name musicstoreapi -e ASPNETCORE_ENVIRONMENT=Staging learningwsc/musicstore.api:$Version
docker run -d -p 80:80 --name musicstore -e ASPNETCORE_ENVIRONMENT=Staging learningwsc/musicstore:$Version



