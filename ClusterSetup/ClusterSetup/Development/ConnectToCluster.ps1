#
# ConnectToCluster.ps1
#
Connect-ServiceFabricCluster -ConnectionEndpoint lrpcluster.westus.cloudapp.azure.com:19000 `
          -KeepAliveIntervalInSec 10 `
          -X509Credential -ServerCertThumbprint 11243CC74E1BEEB3EAD091773CE1C9AE644DDCE4 `
          -FindType FindByThumbprint -FindValue 11243CC74E1BEEB3EAD091773CE1C9AE644DDCE4 `
          -StoreLocation CurrentUser -StoreName My
		  

Get-ServiceFabricClusterHealth

