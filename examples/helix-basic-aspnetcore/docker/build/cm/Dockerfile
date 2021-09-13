# escape=`

ARG BASE_IMAGE
ARG TOOLING_IMAGE
ARG MANAGEMENT_SERVICES_IMAGE
ARG HEADLESS_SERVICES_IMAGE
ARG EDGE_CONNECTOR_IMAGE
ARG SOLUTION_IMAGE

FROM ${SOLUTION_IMAGE} as solution
FROM ${HEADLESS_SERVICES_IMAGE} AS headless_services
FROM ${EDGE_CONNECTOR_IMAGE} as edge_connector
FROM ${MANAGEMENT_SERVICES_IMAGE} AS management_services
FROM ${TOOLING_IMAGE} as tooling
FROM ${BASE_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR C:\inetpub\wwwroot

# Copy developer tools and entrypoint
COPY --from=tooling C:\tools C:\tools

# Copy the Sitecore Management Services Module
COPY --from=management_services C:\module\cm\content C:\inetpub\wwwroot

# Copy the Experience Edge Connector module
COPY --from=edge_connector C:\module\cm\content C:\inetpub\wwwroot
COPY --from=edge_connector C:\module\cm\xdt C:\module\xdt

# Copy and init the JSS / Headless Services Module
COPY --from=headless_services C:\module\cm\content C:\inetpub\wwwroot
COPY --from=headless_services C:\module\tools C:\module\tools
RUN C:\module\tools\Initialize-Content.ps1 -TargetPath C:\inetpub\wwwroot; `
    C:\module\tools\xdt\Invoke-XdtTransformations.ps1 -TargetPath C:\inetpub\wwwroot -XdtPath C:\module\xdt; `
    Remove-Item -Path C:\module -Recurse -Force;

# Copy solution website files
COPY --from=solution \artifacts\sitecore\ .\