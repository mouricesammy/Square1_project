FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["WebBlogApi.csproj", "/"]
RUN dotnet restore "WebBlogApi.csproj"
COPY . .
WORKDIR "/src/WebBlogApi"
RUN dotnet build "WebBlogApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebBlogApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet WebBlogApi.dll
