FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY . /app
WORKDIR /app

RUN dotnet restore
RUN dotnet build

RUN mkdir -p /app/Uploads
RUN mkdir -p /app/Uploads/Avatars

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker

CMD ["dotnet", "ef", "--project", "/app", "database", "update"]

ENTRYPOINT dotnet run