![Logo of the project](https://www.balena.io/avatar.png)

# Balena IOT device real-time monitoring

A company has several drones flying around the country.  
The Purpose of the platform is to track the location of every drone in real-time.  
The system's dashboard displays the device telemetry in realtime.

Each drone is associated with a unique identifier: Serial Number.

For Testing purposes, we use Simulator to report its geolocation coordinates to the central server in real-time.

As there are diverse models of the devices(drones),
they operate at a different speed.
That is why device firmware decides the frequency of telemetry reporting.
Devices use HTTP/2 to communicate with the monitoring platform.

Major versions of drones report it's telemetry to the central server 6 times per minute. depending on their speed limits.

Having in mind that project is the MVP stage, this reflects the architecture.
Architecture is built on SOLID and well testable principle.
Code fragments are separated in modules, potentially shippable packages.

Drones are abstracted as Device entity, so when new versions of drones arrive, the platform is not dependant on it.

Devices send it's telemetries, like latitude, longitude and other info to the central server.

The server creates the entry for the telemetry,
broadcasts an event that new telemetry is received and returns
HTTP 201 code to the drone.

The subscriber subscribes to the telemetry event, known as HostedService and listens to the events.

Once the telemetry event arrives in the subscriber it calculates data for and updates device entity accordingly.

Real-time dashboard calls API/v1/device endpoint once in a 10 second to get the latest data available on the devices.

## Installing / Getting started

Balena IOT device real-time monitoring platform has docker in mind.
run the following commands to spin up a container.

```shell
#pull the container from the registry
docker pull gioasap/balena_iot_realtime_monitor

#run the container
docker run --rm  -p 5000:5000 -p 80:80 --name balena_iot_realtime_monitor gioasap/balena_iot_realtime_monitor

```

> p/s  
> ctrl+c(command+c on osx) to exit/stop the container  
> container usually takes 5/10s to spin up

#### Endpoints

> Dashboard is exposed on the host machine, port: 80 > http://localhost  
> Api is exposed on the host mashince, port 5000 > http://localhost:5000/swagger/index.html

#### Swagger

API uses swagger to expose developer-friendly documentation

> http://localhost:5000/swagger/index.html

## Build from Dockerfile

Here's a brief intro about what a developer must do to build container locally

```shell
#build the container
docker build -t balena_iot_realtime_monitor .

#run the container
docker run --rm  -p 5000:5000 -p 80:80 --name balena_iot_realtime_monitor balena_iot_realtime_monitor
```

## Dev dependencies

> `dotnet core 2.2`  
> `docker`  
> `angular-cli`

## Unit Test

Project provides unit tests on some levels.  
Dockerfile handles automated tests.  
If tests are failing, the container will fail to build.

To run tests manually, run the following command:

```shell
#restore dependencies
dotnet restore

#run tests
dotnet test
```

## Project structure

Belane IOT realtime monitoring uses angular 8 for frontend and net core for the backend.

### Frontend

The project can be found under `dashboard/` directory

It is a simple angular application, the single table which displays device analytical data.

Table component could be found under `dashboard/src/app/device-list`

Service integration could be found under `dashboard/src/app/device.service.ts`

Pipelines for data transformation could be found under `dashboard/src/app/pipes`

### Api - backend

Api project is location at `Balena.IOT.RealTimeMonitor.Api/` directory

API endpoint controllers

> `Balena.IOT.RealTimeMonitor.Api/Controllers/DeviceController.cs` Endpoint  
> `/api/v1/device` - responsible for device(Drone) list and get

> `Balena.IOT.RealTimeMonitor.Api/Controllers/DeviceTelemetryController.cs`  
> Endpoint  
> `/api/v1/device/telemetry` - responsible for receiving telemetry from the device(Drone), realtime location

Once the telemetry is received, API dispatches the internal event.
Event handler, which is responsible for the distance and speed calculations, is located under `Balena.IOT.RealTimeMonitor.Api/HostedService/TelemetryProcessorHostedService.cs`

### Dependency libraries

> `Balena.Geolocation/` library provides geolocation helper methods, distance and speed calculations. Could be used in future projects.

> `Balena.IOT.Entity/` library provides a business model (Entity) descriptions. Could be used in future projects.

> `Balena.IOT.Repository/` library implements a Repository pattern for accessing the data layer. Dependency is abstract, implementation is InMemeory for testing purposes.
> Could be used in future projects.

> `Balena.IOT.Tests/` Unit tests that coverst libraries

> `Belane.IOT.Simulator/` Simple drone simulator

### Docker integration and hosting

Dockerfile covers 3 stages.

#### Stage 1 - INTERMEDIATARY container

Runs unit tests and only after that builds API and Simulator saves it as artifact `build-env`

#### Stage 2 - INTERMEDIATARY container

Runs under `node` docker image, install angular-cli, restores packages and builds a dashboard, artifacts are passed to `angular-build`

#### Stage 3 - shippable container

Runs under `dotnet-runtime` docker image.

#### Docker Dependencies

> `supervisor` is used for process managment.  
> `nginx` frontend reverse proxy.  
> `dotnet` backend runtime.  
> `supervisor_api.conf` api supervisor service file.  
> `supervisor_simulator.conf` simulator supervisor service file.  
> `supervisor_nginx.conf` nginx supervisor service file.  
> `nginx/nginx.conf` nginx hosting configuration file for dashboard.
