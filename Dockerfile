FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy and restore
COPY . ./
RUN dotnet restore 

# Run tests
RUN dotnet test

# build api
RUN dotnet publish Balena.IOT.RealTimeMonitor.Api/Balena.IOT.RealTimeMonitor.Api.csproj  -c Release -o out

#build simulator
RUN dotnet publish Belane.IOT.Simulator/Belane.IOT.Simulator.csproj  -c Release -o out


FROM microsoft/dotnet:aspnetcore-runtime
EXPOSE 5000
WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5000"
COPY --from=build-env /app/Balena.IOT.RealTimeMonitor.Api/out /app/api
COPY --from=build-env /app/Belane.IOT.Simulator/out /app/sumulator
RUN apt-get update && apt-get install -y  supervisor
COPY --from=build-env /app/supervisor_api.conf /etc/supervisor/conf.d/supervisor_api.conf
COPY --from=build-env /app/supervisor_simulator.conf /etc/supervisor/conf.d/supervisor_simulator.conf
CMD ["/usr/bin/supervisord"]