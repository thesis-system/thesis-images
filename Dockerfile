FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /src

COPY . ./
ENV ASPNETCORE_URLS="http://+:5050"
EXPOSE 5050

ENTRYPOINT  ["dotnet", "/src/Thesis.Images.dll", "http://*:5050"]