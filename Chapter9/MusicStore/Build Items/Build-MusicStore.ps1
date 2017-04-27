param
(
	[ValidateNotNullOrEmpty()]
	#Image Name
	[string]$ImageName,
	
	[ValidateNotNullOrEmpty()]
	#Container Image version
	[String]$Version,

	[ValidateNotNullOrEmpty()]
	#Docker file Path
	[String]$DockerfilePath,

	[ValidateNotNullOrEmpty()]
	#Docker build context
	[String]$BuildContext,

	[ValidateNotNullOrEmpty()]
	#Project Name
	[String]$ProjectName

)

function Build-Image()
{
	#Clean up extraction if already present	
	if(Test-Path "$BuildContext/$ProjectName")
	{
		Remove-Item "$BuildContext/$ProjectName" -Recurse -Force
	}

	#Unzip the build artifacts
	Add-Type -AssemblyName System.IO.Compression.FileSystem
	[System.IO.Compression.ZipFile]::ExtractToDirectory("$BuildContext/$ProjectName.zip","$BuildContext/$ProjectName")
	
	#Build Docker Image
	docker build -t $ImageName`:$Version -f $DockerfilePath $BuildContext
}

Build-Image

