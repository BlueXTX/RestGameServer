FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY /PaidGame.Server/bin/Release/net5.0/publish /app
ENTRYPOINT ["dotnet", "PaidGame.Server.dll"]