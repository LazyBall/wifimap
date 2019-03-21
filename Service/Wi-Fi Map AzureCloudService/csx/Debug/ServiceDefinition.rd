<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Wi_Fi_Map_AzureCloudService" generation="1" functional="0" release="0" Id="2ff3c211-84f9-4e92-9803-38a07fd806aa" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Wi_Fi_Map_AzureCloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WCFService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/LB:WCFService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="WCFService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/MapWCFService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WCFServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/MapWCFServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WCFService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/WCFService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapWCFService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/WCFService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWCFServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/WCFServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WCFService" generation="1" functional="0" release="0" software="D:\Sanek\Wi-Fi Map\Service\Wi-Fi Map AzureCloudService\csx\Debug\roles\WCFService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WCFService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WCFService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/WCFServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/WCFServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/WCFServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WCFServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="WCFServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WCFServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="1a5db636-a8fd-4f91-9658-06702cd61fa6" ref="Microsoft.RedDog.Contract\ServiceContract\Wi_Fi_Map_AzureCloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="93f1c235-bb25-4c0d-ae13-cd9e3714a1e4" ref="Microsoft.RedDog.Contract\Interface\WCFService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Wi_Fi_Map_AzureCloudService/Wi_Fi_Map_AzureCloudServiceGroup/WCFService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>