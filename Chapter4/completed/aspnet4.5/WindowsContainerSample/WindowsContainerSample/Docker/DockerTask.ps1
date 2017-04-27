param
(
#This switch builds the Windows Container Image
[switch]$Build,

#This switch removes all the containers created if any using the Windows Container Image created using the build option 'MusicStore' in this case.
[switch]$Clean,

#This switch creates a container 
[switch]$Run,

#This switch publishes the image to docker hub
[switch]$Publish,

[ValidateNotNullOrEmpty()]
#ProjectName will be used to create image name
[string]$ProjectName = "aspnet45_windowscontainer",

#Host Port which should be mapped to the container port
[ValidateNotNullOrEmpty()]
[String]$HostPort = 80,

#Container Port which maps to the Host Port
[ValidateNotNullOrEmpty()]
[String]$ContainerPort = 80,

[ValidateNotNullOrEmpty()]
#MusicStore image version
[String]$Version = "1.0.0" ,

[ValidateNotNullOrEmpty()]
#DockerHub Username
[String]$Username= "learningwsc",

[ValidateNotNullOrEmpty()]
#DockerHub Password, Replace **** with your password
[String]$Password = "***********" 
)

$ErrorActionPreference = "Stop"

function Build(){

	#Build Docker Image
	docker build -t $ImageName`:$Version -f ./Docker/Dockerfile . 
} 

function Run()
{
	#Get Containers running on Port 80
	$conflictingContainerIds = $(docker ps -a | select-string -pattern ":$HostPort->" | foreach { Write-Output $_.Line.split()[0] })

	#Stopping Containers running on Port 80
	if ($conflictingContainerIds) {
            $conflictingContainerIds = $conflictingContainerIds -Join ' '
            Write-Host "Stopping conflicting containers using port $HostPort"
            docker stop $conflictingContainerIds 
    }

	#Creates a Music Store Container
	docker run -p $HostPort`:$ContainerPort $ImageName`:$Version
}

function Clean(){
	
	#Regex for image name
	$ImageNameRegEx = "\b$ImageName\b"
	
	#Removes all images with name matching the ImageNameRegEx, Ex: musicstore
	docker images | select-string -pattern $ImageNameRegEx | foreach {
            $imageName = $_.Line.split(" ", [System.StringSplitOptions]::RemoveEmptyEntries)[0];
            $tag = $_.Line.split(" ", [System.StringSplitOptions]::RemoveEmptyEntries)[1];
            Write-Host "Removing image ${imageName}:$tag";
            docker rmi ${imageName}:$tag --force
    }
}

function Publish(){
	
	#Login to Docker Hub
	docker login --username $username --password $password 
	
	#Push the image to docker hub
	docker push $ImageName`:$Version
}

$ImageName = "learningwsc/${ProjectName}".ToLowerInvariant()

if($Build) {
    Build
}
if($Run){
	Run
}
if($Clean){
	Clean
}
if($Publish){
	Publish
}
