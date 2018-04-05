# load the secrets

. "C:\Users\ericowens\Desktop\GitHub\cloud\ClusterSetup\ClusterSetup\Development\Secret\AzureIdentityVars.ps1"

# The $Pwd password should be $Pwd = ConvertTo-SecureString -String ‘certPassword’ -Force -AsPlainText

Import-PfxCertificate -Exportable -CertStoreLocation Cert:\LocalMachine\My `
        -FilePath C:\Users\ericowens\Desktop\SFCertificates\cogitocertificate100.pfx `
        -Password $Pwd