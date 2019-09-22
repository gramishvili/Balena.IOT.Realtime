#STAGE 1 BUILD BACKEND
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


#STAGE 2  build frontend
FROM node:latest as angular-build
WORKDIR /app
COPY dashboard/package.json /app/
RUN npm install
COPY dashboard/ /app/
ARG configuration=production
RUN npm install -g @angular/cli
RUN ng build --configuration=$configuration --prod --output-path=dist



FROM microsoft/dotnet:aspnetcore-runtime
EXPOSE 5000
EXPOSE 80
WORKDIR /app
#backend host configuration
ENV ASPNETCORE_URLS="http://*:5000"

#copy build artefacts for api
COPY --from=build-env /app/Balena.IOT.RealTimeMonitor.Api/out /app/api
#copy build artefacts for simulator

COPY --from=build-env /app/Belane.IOT.Simulator/out /app/sumulator

#install supervisor to run api,simulator and nginx as service
RUN apt-get update && apt-get install -y  supervisor

#copy supervisor service files
COPY --from=build-env /app/supervisor_api.conf /etc/supervisor/conf.d/supervisor_api.conf
COPY --from=build-env /app/supervisor_simulator.conf /etc/supervisor/conf.d/supervisor_simulator.conf

#install nginx
RUN apt-get update && apt-get install -y  nginx
#copy nginx supervisor config
COPY --from=build-env /app/supervisor_nginx.conf /etc/supervisor/conf.d/supervisor_nginx.conf

#configure frontend nginx
COPY --from=angular-build /app/dist /usr/share/nginx/html
COPY nginx/nginx.conf /etc/nginx/nginx.conf

CMD ["/usr/bin/supervisord"]