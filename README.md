# bootstrapping-ui-automation
Automation of https://the-internet.herokuapp.com/ - all kinds of UI Automation Nastiness

# WORK IN PROGRESS!
| Status      | Milestone |
| Done        | Infrastructure folder for docker-compose and minikube (two environments / targets) |
| Not Started | .Net Project Bootstrapping (Browsers) |

## Infrastructure
You can run 'the-internet' yourself in the following ways: 

| Target | Description |
| infra/docker-localhost | Uses a docker-compose file to spin up the-internet locally |
| infra/minikube | Uses minikube / kubectl to start the-internet locally and expose it via an external 'LoadBalancer' port on your own machine |
