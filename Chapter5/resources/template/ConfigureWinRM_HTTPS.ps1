#File containing the PowerShell which runs on Remote VM 
 
# POWERSHELL TO EXECUTE ON REMOTE SERVER BEGINS HERE
 
param($DNSName)
 
# Ensure PS remoting is enabled, although this is enabled by default for Azure VMs
Enable-PSRemoting -Force
  
# Create rule in Windows Firewall
New-NetFirewallRule -Name 'WinRM HTTPS' -DisplayName 'WinRM HTTPS' -Enabled True -Profile Any -Action Allow -Direction Inbound -LocalPort 5986 -Protocol TCP -ErrorAction SilentlyContinue

# Create Self Signed certificate and store thumbprint
$thumbprint = (New-SelfSignedCertificate -DnsName $DNSName -CertStoreLocation Cert:\LocalMachine\My).Thumbprint
  
# Run WinRM configuration on command line. DNS name set to computer hostname, you may wish to use a FQDN
$cmd = "winrm create winrm/config/Listener?Address=*+Transport=HTTPS @{Hostname=""$DNSName""; CertificateThumbprint=""$thumbprint""}"
cmd.exe /C $cmd
  
# POWERSHELL TO EXECUTE ON REMOTE SERVER ENDS HERE
  
