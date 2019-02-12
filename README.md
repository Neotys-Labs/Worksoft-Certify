<p align="center"><img src="/screenshots/worksoft-logo.png" width="50%" alt="Worksoft Logo" /></p>

# NeoLoad Integration for Worksoft Certify

## Overview

C# extension to integrate [Worksoft Certify](https://www.worksoft.com/) with [NeoLoad](https://www.neotys.com/neoload/overview) for Script maintenance.
It allows you to interact with the NeoLoad [Design API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#11265.htm) to convert an SAP GUI or Web Worksoft Certify Process to a NeoLoad User Path or update an existing User Path.

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

2. In the Navigation pane, click **Interfaces**. In the **Interfaces Summary** pane, right-click the **Interfaces** node, then Import and select the **neoload-interface.xml** file from the release.

<p align="center"><img src="/screenshots/interfacesimport.png" alt="Import NeoLoad interface" /></p>

3. Unzip the **wsTransferToNeoLoad.zip** file in a folder named "NeoLoad" in the Worksoft\Certify\Interface Client\Worksoft\wsTest directory of the installation directory of Worksoft Certify(for example: C:\Program Files (x86)\Worksoft\Certify\Interface Client\Worksoft\wsTest\NeoLoad).

4. Unblock **wsTransferToNeoLoad.dll** (Right click the DLL > Properties and tick **Unblock**).

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

## Modify the Worksoft Application to allow the conversion with the NeoLoad interface.

1. In the Navigation pane, click **Applications**.

2. In the Applications pane, on the version of the application that contains the process that you want to convert to a NeoLoad User Path, right-click and select **Edit**.

<p align="center"><img src="/screenshots/applicationsedit.png" alt="Edit Application" /></p>

3. In Edit Application Version window, select the **NeoLoad** interface in the Interfaces section and click OK.

<p align="center"><img src="/screenshots/applicationneoload.png" alt="Select NeoLoad" /></p>

4. Right click again on the version and select **New Window**.

5. In the New Window dialog, enter a Name, a Physical Name and select the NeoLoad Interface. Click OK

<p align="center"><img src="/screenshots/addwindow.png" alt="Add Window" /></p>

## Modify a Worksoft Certify Process to convert it to a NeoLoad User Path.

1. In the Navigation pane, click **Processes**.

2. In the Applications pane, select the process that you want to convert to a NeoLoad User Path, right-click and select **Edit**.

3. At the beginning of the Process, add a new Step associated to the NeoLoad window with the **StartRecording** Action.

<p align="center"><img src="/screenshots/startrecording.png" alt="Start Recording" /></p>

4. If identification is required by the NeoLoad API, use the **apiKey** parameter.

5. Enter the User Path name, you can use the **Process Name** built-in System Variable.

6. Enter the record Mode:
    * **SAP GUI & WEB** to record both SAP GUI and the Web traffic (the System proxy will be updated)
    * **SAP GUI** to record only SAP GUI.
    * **WEB** to record only Web traffic (the System proxy will be updated)

7. At the end of the Process, add a new Step associated to the NeoLoad window with the **StopRecording** Action.

<p align="center"><img src="/screenshots/process.png" alt="Process" /></p>

**Tip** : Once the conversion is done, you can comment the Steps associated to the NeoLoad interface with the **Comment** button.

<p align="center"><img src="/screenshots/comment.png" alt="Comment" /></p>

## Add NeoLoad Transactions.

In the Edit Process dialog, add a new Step associated to the NeoLoad window with the **StartTransaction** Action each time you want to start a NeoLoad Transaction.

<p align="center"><img src="/screenshots/transaction.png" alt="Transaction" /></p>

## Advanced Configuration

The **advanced** parameter of the **StartRecording** Action allows to define the following options:
* **designApiUrl**: the URL of the NeoLoad design API, by default it is http://localhost:7400/Design/v1/Service.svc/.
* **updateUserPath**: Used to automatically update the User Path with the same name. Default value is true.
* **isHttp2**: Used to record or not HTTP/2. Default value is true.
* **addressToExclude**: List of addresses separated by a semicolon. Requests and responses through these addresses will not be taken into account by Neaoload when recording (example : 10.0.0.5:7400;10.3.1.15;localhost:9100)
* **userAgent**: Used to specify the user agent.

If you want to define multiple options, the options must be separated by carriage return. For example:<br />

<p align="center"><img src="/screenshots/advanced.png" alt="Advanced" /></p>

The **advanced** parameter of the **StopRecording** Action allows to define the following options:
* **frameworkParameterSearch**: Default value is true.
* **genericParameterSearch**: Default value is false.
* **deleteRecording**: Default value is true.
* **includeVariablesInUserPathUpdate**: Default value is true.
* **updateSharedContainers**: Default value is false.
* **matchingThreshold**: Default value is 60.

## ChangeLog

* Version 0.1.0 (February 12, 2019): Initial release.
