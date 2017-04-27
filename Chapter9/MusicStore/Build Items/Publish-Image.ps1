param(
	
	[ValidateNotNullOrEmpty()]
	#Image Name
	[string]$ImageName,
	
	[ValidateNotNullOrEmpty()]
	#Container Image version
	[String]$Version,

	[ValidateNotNullOrEmpty()]
	#Docker Hub Username
	[string]$username,
	
	[ValidateNotNullOrEmpty()]
	#Docker Hub Password
	[String]$password
)

function Publish-Image(){
	#Login to Docker Hub
	docker login --username $username --password $password 
	
	#Push the image to docker hub
	docker push $ImageName`:$Version
}

Publish-Image