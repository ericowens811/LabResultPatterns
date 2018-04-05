## STEP 0
# load the secrets

. "C:\Users\ericowens\Desktop\GitHub\cloud\ClusterSetup\ClusterSetup\Development\Secret\AzureIdentityVars.ps1"

## STEP 1
# Export the server certificate 
# The $Pwd password should be $Pwd = ConvertTo-SecureString -String ‘certPassword’ -Force -AsPlainText
# Get-ChildItem -Path cert:\localMachine\my\SomeThumb | Export-PfxCertificate -FilePath C:\Certificates -Password $Pwd

Get-ChildItem -Path $CertStorePath | Export-PfxCertificate -FilePath $ServerCertPath -Password $Pwd

## STEP 2
# Import the ServiceFabricRPHelpers.psm1 module
# Find it here: https://github.com/ChackDan/Service-Fabric
# this module includes the Invoke-AddCertToKeyVault routines

Import-Module $ModulePath

## STEP 3
# add the key to the vault

Login-AzureRmAccount
Get-AzureRmSubscription
Select-AzureRmSubscription -SubscriptionId $subscriptionId

### !!!
### !!! Note that the password should be plain text, not 'ConvertTo-SecureString'
### !!!

Invoke-AddCertToKeyVault -SubscriptionId $SubscriptionId -ResourceGroupName $KeyVaultResourceGroup -Location $KeyVaultRegion -VaultName $VaultName `
-CertificateName $ServerCertName -Password $PlainTextPassword -UseExistingCertificate -ExistingPfxFilePath $ServerCertPath

# Cert notes

# http://blog.davidchristiansen.com/2016/09/howto-create-self-signed-certificates-with-powershell/
# https://www.experts-exchange.com/questions/29020388/Use-New-SelfSignedCertificate-to-create-a-Trusted-Root-certificate.htm
# https://www.youtube.com/watch?v=hqIJM-Rutic

# Create Root certificate
# $RootCertName = "cogitocertificateroot"
# $RootDnsName = "Cogito Certificate Authority"
# $RootCertPath = ($CertFolder + $RootCertName + ".pfx")
# $RootPublicKeyPath = ($CertFolder + $RootCertName + ".crt")
# New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname $RootDnsName -KeyusageProperty All -KeyUsage CertSign, CRLSign, DigitalSignature
# Output: A8D5E404DE0F4921ACD928D9C0A1CBD9908EF4DF  CN=Cogito Certificate Authority

# Export Root certificate (cert:\localMachine\my\rootThumbprint)
# Get-ChildItem -Path cert:\localMachine\my\A8D5E404DE0F4921ACD928D9C0A1CBD9908EF4DF | Export-PfxCertificate -FilePath $RootCertPath -Password $Pwd

# Export public key of root certificate (cert:\localMachine\my\rootThumbprint)
# Get-ChildItem -Path cert:\localMachine\my\A8D5E404DE0F4921ACD928D9C0A1CBD9908EF4DF | Export-Certificate -FilePath $RootPublicKeyPath

# Load the root authority certificate (cert:\localMachine\my\rootThumbprint)
# $Rootcert = ( Get-ChildItem -Path cert:\localMachine\my\A8D5E404DE0F4921ACD928D9C0A1CBD9908EF4DF )
# Create a server certificate signed by the authority
# New-SelfSignedCertificate -dnsname $ServerDnsName -certstorelocation cert:\localmachine\my -Signer $Rootcert
# Output: 283629AF2B68F4461A316031D12CC4FF5ACE24D1  CN=cogitocluster.westus.cloudapp.azure.com

# Export the server certificate (cert:\localMachine\my\serverThumbprint)
# Get-ChildItem -Path cert:\localMachine\my\283629AF2B68F4461A316031D12CC4FF5ACE24D1 | Export-PfxCertificate -FilePath $ServerCertPath -Password $Pwd

# to delete certificates locally
# cd cert:\localmachine\my
# dir
# Get-ChildItem Cert:\localmachine\my\thumbprint | Remove-Item

# this lists all the cert details
# Get-ChildItem | FORMAT-LIST -PROPERTY *

