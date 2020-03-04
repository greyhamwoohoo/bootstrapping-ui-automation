# netcore-selenium-framework 
Bootstrapping of a bare minimum, opinionated .Net Core Selenium Framework using MsTest, the in-built .Net Core DI Container, Serilog, .runsettings and Visual Studio.  

This framework supports a 'hot-reload' workflow that allows you to run your tests against an existing browser instance: see the NetCore-Selenium/README.md file for more information.

Optional use of Zalenium (dockerized Selenium Grid) via docker or Kubernetes (Minikube only at the moment). 

## Versions
The baseline framework is the same - but the attachable driver / hot-reload workflow implementation is very dependent on the version of Selenium in use. 

| Folder | Description | 
| ------ | ----------- |
| src/NetCore-Selenium | Baseline framework and attachable drivers using Selenium 3.141  (IE, Chrome, Edge) |
| src/NetCore-Selenium-4.0-alpha3 | Baseline framework and attachable drivers using Selenium 4.0.0.0-alpha-03  (IE, Chrome, Edge) |

## Infrastructure
You can run 'the-internet' yourself in the following ways: 

| Target | Description |
| ------ | ----------- |
| infra/docker-localhost | Uses a docker-compose file to spin up the-internet locally |
| infra/minikube | Uses minikube / kubectl to start the-internet locally and expose it via an external 'LoadBalancer' port on your own machine |
