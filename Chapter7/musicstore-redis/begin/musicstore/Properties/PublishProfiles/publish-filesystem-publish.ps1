try{
	    
}
catch{
    "An error occurred during publish.`n{0}" -f $_.Exception.Message | Write-Error
}