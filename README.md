<p align="center"><img src="/screenshots/worksoft-logo.png" width="50%" alt="Worksoft Logo" /></p>

# NeoLoad Integration for Worksoft Certify

## Overview

C# extension to integrate [Worksoft Certify](https://www.worksoft.com/) with [NeoLoad](https://www.neotys.com/neoload/overview) for Script maintenance.
It allows you to interact with the NeoLoad [Design API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#11265.htm) to convert a Worksoft Certify Process to a NeoLoad User Path or update an existing User Path.

| Property | Value |
| ----------------    | ----------------   |
| Maturity | Experimental |
| Author | Neotys |
| License           | [BSD 2-Clause "Simplified"](https://github.com/Neotys-Labs/Worksoft-Certify/blob/master/LICENSE) |
| NeoLoad Licensing | License FREE edition, or Enterprise edition, or Professional with Integration & Advanced Usage|
| Supported versions | Tested with Worksoft Certify 10 and NeoLoad from version [6.6.0](https://www.neotys.com/support/download-neoload) (version 32 bits for SAP GUI)
| Download Binaries | See the [latest release](https://github.com/Neotys-Labs/Worksoft-Certify/releases/latest)|

## Setting up the NeoLoad Integration for Worksoft Certify

1. Download the [latest release](https://github.com/Neotys-Labs/Worksoft-Certify/releases/latest)

2. In the Navigation pane, click Interfaces. In the Interfaces Summary pane, right-click on the Interfaces node, then Import and select the **neoload-interface.xml** file from the release.

3. Unzip the **wsTransferToNeoLoad.zip** file in a folder nammed NeoLoad of the Worksoft\Certify\Interface Client\Worksoft\wsTest directory of the installation directory of Worksoft Certify(for example: C:\Program Files (x86)\Worksoft\Certify\Interface Client\Worksoft\wsTest\NeoLoad).

4. Unblock "wsTransferToNeoLoad.dll" (Right click the DLL > Properties and tick **Unblock**).

5. Register the Assembly for COM interop by executing the following command: regasm.exe" /codebase **wsTransferToNeoLoad.dll** (regasm.exe can be found in %SystemRoot%\Microsoft.NET\Framework\v2.0.50727)

6. Edit the file **Worksoft\Certify\Interface Client\Worksoft\wsTest\wsTest.exe.config** in the installation directory of Worksoft Certify:

* Add the following node at the end of the configuration node:
```xml
    <runtime>
        <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
          <dependentAssembly>
            <assemblyIdentity name="Microsoft.Data.OData" publicKeyToken="31bf3856ad364e35" culture="neutral" />
            <bindingRedirect oldVersion="0.0.0.0-5.8.4.0" newVersion="5.8.4.0" />
          </dependentAssembly>
    	  <dependentAssembly>
            <assemblyIdentity name="Microsoft.Data.Edm" publicKeyToken="31bf3856ad364e35" culture="neutral" />
            <bindingRedirect oldVersion="0.0.0.0-5.8.4.0" newVersion="5.8.4.0" />
          </dependentAssembly>
    	  <dependentAssembly>
            <assemblyIdentity name="System.Spatial" publicKeyToken="31bf3856ad364e35" culture="neutral" />
            <bindingRedirect oldVersion="0.0.0.0-5.8.4.0" newVersion="5.8.4.0" />
          </dependentAssembly>
        </assemblyBinding>
      </runtime>
```
7. Relaunch the Worksoft Certify.

## Modify a Worksoft Certify Process to convert it to a NeoLoad User Path.
