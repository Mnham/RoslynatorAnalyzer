FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /src

ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV PATH="$PATH:/root/.dotnet/tools"

COPY ./analyzer/Roslynator.Analyzer.csproj ./analyzer/
RUN dotnet restore "./analyzer/Roslynator.Analyzer.csproj"

COPY ./deployment/ /bin/
COPY ./analyzer/ .

# Установка системных пакетов
RUN apt-get update && \
    apt-get install -y mono-complete wget unzip && \
    wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Установка protolint
RUN wget https://github.com/yoheimuta/protolint/releases/download/v0.56.4/protolint_0.56.4_linux_amd64.tar.gz && \
    tar -xzf protolint_0.56.4_linux_amd64.tar.gz -C /usr/local/bin && \
    rm protolint_0.56.4_linux_amd64.tar.gz && \
    chmod +x /usr/local/bin/protolint

# Установка Roslynator
RUN dotnet tool install -g roslynator.dotnet.cli

# Установка NuGet-анализаторов
RUN mono nuget.exe install Roslynator.Analyzers -OutputDirectory /root/analyzers && \
    mono nuget.exe install Roslynator.Formatting.Analyzers -OutputDirectory /root/analyzers && \
    mono nuget.exe install Roslynator.CodeAnalysis.Analyzers -OutputDirectory /root/analyzers

# Сборка основного проекта
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Очистка
RUN rm -rf /src/*

RUN chmod +x /bin/entrypoint.sh
ENTRYPOINT [ "/bin/entrypoint.sh" ]