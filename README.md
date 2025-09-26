# IotHub Hello World

IoTHub Hello World consists of 3 projects:

## IotHubDeviceSim

Simulation of an IoT device that connects to IoT Hub and sends telemetry every 20 seconds. 
The device also listens for the reception of a direct method called "myMethod". 
Authentication can be configured to use either SAS or Self-signed certificates.

PREREQUISITE: Device01 or Device02 must be provisioned on IoT Hub before running the code.

## IotHubClientSim

Simulation of a client that connects to IoT Hub and sends a direct method "myMethod" to Device01.

## IotHubClientConsumerSim

Simulation of a message consumer that connects to an Event Hub or to the built-in Event Hub in IoT Hub.