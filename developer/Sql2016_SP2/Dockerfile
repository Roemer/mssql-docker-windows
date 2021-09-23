# escape=`

# The windows version to use, can be any of the tags of https://hub.docker.com/_/microsoft-windows-servercore
# Most used are: ltsc2016, ltsc2019, ltsc2022, 1709, 1803, 1809, 1903, 1909, 2004, 20H2
ARG winversion=1909

FROM mcr.microsoft.com/windows/servercore:$winversion

# Download Links:
ENV exe "https://download.microsoft.com/download/4/1/A/41AD6EDE-9794-44E3-B3D5-A1AF62CD7A6F/sql16_sp2_dlc/en-us/SQLServer2016SP2-FullSlipstream-DEV-x64-ENU.exe"
ENV box "https://download.microsoft.com/download/4/1/A/41AD6EDE-9794-44E3-B3D5-A1AF62CD7A6F/sql16_sp2_dlc/en-us/SQLServer2016SP2-FullSlipstream-DEV-x64-ENU.box"

ENV sa_password="_" `
    attach_dbs="[]" `
    ACCEPT_EULA="_" `
    sa_password_path="C:\ProgramData\Docker\secrets\sa-password"

# Set the default shell to powershell
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# make install files accessible
COPY start.ps1 /
WORKDIR /

RUN Invoke-WebRequest -Uri $env:box -OutFile SQL.box; `
    Invoke-WebRequest -Uri $env:exe -OutFile SQL.exe; `
    Start-Process -Wait -FilePath .\SQL.exe -ArgumentList /qs, /x:setup; `
    .\setup\setup.exe /q /ACTION=Install /INSTANCENAME=MSSQLSERVER /FEATURES=SQLEngine /UPDATEENABLED=0 /SQLSVCACCOUNT='NT AUTHORITY\NETWORK SERVICE' /SQLSYSADMINACCOUNTS='BUILTIN\ADMINISTRATORS' /TCPENABLED=1 /NPENABLED=0 /IACCEPTSQLSERVERLICENSETERMS; `
    Remove-Item -Recurse -Force SQL.exe, SQL.box, setup

RUN Stop-Service MSSQLSERVER ; `
    Set-ItemProperty -path 'HKLM:\software\microsoft\microsoft sql server\mssql13.MSSQLSERVER\mssqlserver\supersocketnetlib\tcp\ipall' -name tcpdynamicports -value ''; `
    Set-ItemProperty -path 'HKLM:\software\microsoft\microsoft sql server\mssql13.MSSQLSERVER\mssqlserver\supersocketnetlib\tcp\ipall' -name tcpport -value 1433; `
    Set-ItemProperty -path 'HKLM:\software\microsoft\microsoft sql server\mssql13.MSSQLSERVER\mssqlserver\' -name LoginMode -value 2;

HEALTHCHECK CMD [ "sqlcmd", "-Q", "select 1" ]

CMD .\start -sa_password $env:sa_password -ACCEPT_EULA $env:ACCEPT_EULA -attach_dbs \"$env:attach_dbs\" -Verbose
