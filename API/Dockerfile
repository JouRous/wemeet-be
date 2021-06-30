FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY . /app
WORKDIR /app

RUN dotnet restore
RUN dotnet build

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef migrations add Init

RUN mkdir -p /app/Uploads
RUN mkdir -p /app/Uploads/Avatars

# RUN dotnet tool install --global dotnet-ef

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker

ENTRYPOINT dotnet run