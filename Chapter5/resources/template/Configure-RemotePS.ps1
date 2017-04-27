param(
[String][parameter(Mandatory=$true)]$VMName,
[String][parameter(Mandatory=$true)]$ResourceGroupName,
[String][parameter(Mandatory=$true)]$StorageAccountName
)

  function Configure-AzureWinRMHTTPS {
  <#
  .SYNOPSIS
  Configure WinRM over HTTPS inside an Azure VM.
  .DESCRIPTION
  1. Creates a self signed certificate on the Azure VM.
  2. Creates and executes a custom script extension to enable Win RM over HTTPS and opens 5986 in the Windows Firewall
  3. Creates a Network Security Rules for the Network Security Group attached the the first NIC attached the the VM allowing inbound traffic on port 5986
  .EXAMPLE
   Configure-AzureWinRMHTTPS -ResourceGroupName "TestGroup" -VMName "TestVM"
  .EXAMPLE
  Give another example of how to use it
  .PARAMETER ResourceGroupName
  Name of the resource group that the VM exists in
  .PARAMETER VMName
  The name of the virtual machine you wish to enable Win RM on.
   .PARAMETER DNSName
  DNS name you will use to connect to the VM. If not provided defaults to the computer name.
  .PARAMETER SourceAddressPrefix
  Provide an CIDR value to restrict connections to a specific IP range
  #>
 
   
  Param
  (
            [parameter(Mandatory=$true)]
            [String]
            $VMName,
             
            [parameter(Mandatory=$true)]
            [String]
            $ResourceGroupName,    
			  
			[parameter(Mandatory=$true)]
            [String]
            $storageaccountname,
			   
            [parameter()]
            [String]
            $DNSName = $env:COMPUTERNAME,
              
            [parameter()]
            [String]
            $SourceAddressPrefix = "*"			
 
          ) 
 
# define a temporary file in the users TEMP directory
$file = "$PSScriptRoot\ConfigureWinRM_HTTPS.ps1"
  
# Get the VM we need to configure
$vm = Get-AzureRmVM -ResourceGroupName $ResourceGroupName -Name $VMName
 
# get storage account key
$key = (Get-AzureRmStorageAccountKey -Name $storageaccountname -ResourceGroupName $ResourceGroupName)[0].Value
  
# create storage context
$storagecontext = New-AzureStorageContext -StorageAccountName $storageaccountname -StorageAccountKey $key
  
# create a container called scripts
New-AzureStorageContainer -Name "scripts" -Context $storagecontext -ErrorAction SilentlyContinue
  
#upload the file
Set-AzureStorageBlobContent -Container "scripts" -File $file -Blob "ConfigureWinRM_HTTPS.ps1" -Context $storagecontext -force
 
# Create custom script extension from uploaded file
Set-AzureRmVMCustomScriptExtension -ResourceGroupName $ResourceGroupName -VMName $VMName -Name "EnableWinRM_HTTPS" -Location $vm.Location -StorageAccountName $storageaccountname -StorageAccountKey $key -FileName "ConfigureWinRM_HTTPS.ps1" -ContainerName "scripts" -RunFile "ConfigureWinRM_HTTPS.ps1" -Argument $DNSName
  
# Get the name of the first NIC in the VM
$nic = Get-AzureRmNetworkInterface -ResourceGroupName $ResourceGroupName -Name (Get-AzureRmResource -ResourceId $vm.NetworkInterfaceIDs[0]).ResourceName
 
# Get the network security group attached to the NIC
$nsg = Get-AzureRmNetworkSecurityGroup  -ResourceGroupName $ResourceGroupName  -Name (Get-AzureRmResource -ResourceId $nic.NetworkSecurityGroup.Id).Name 
  
# Add the new NSG rule, and update the NSG
$nsg | Add-AzureRmNetworkSecurityRuleConfig -Name "WinRM_HTTPS" -Priority 1101 -Protocol TCP -Access Allow -SourceAddressPrefix $SourceAddressPrefix -SourcePortRange * -DestinationAddressPrefix * -DestinationPortRange 5986 -Direction Inbound   | Set-AzureRmNetworkSecurityGroup
 
# get the NIC public IP
$ip = Get-AzureRmPublicIpAddress -ResourceGroupName $ResourceGroupName -Name (Get-AzureRmResource -ResourceId $nic.IpConfigurations[0].PublicIpAddress.Id).ResourceName 
 
 
Write-Host "To connect to the VM using the IP address while bypassing certificate checks use the following command:" -ForegroundColor Green
Write-Host "Enter-PSSession -ComputerName " $ip.IpAddress  " -Credential <admin_username> -UseSSL -SessionOption (New-PsSessionOption -SkipCACheck -SkipCNCheck)" -ForegroundColor Green
 
}
        
  Configure-AzureWinRMHTTPS -VMName $VMName -ResourceGroupName $ResourceGroupName -storageaccountname $StorageAccountName

