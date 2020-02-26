# Start The-Internet locally in Minikube
This assumes you have installed Minikube on your system and it is up and running. 

To start a deployment on your local system:

```
kubectl config set-context minikube
kubectl create -f deployment-definition.yml
kubectl create -f service-definition.yml
```

Wait until the deployment and service is configured - perhaps with this:

```
kubectl get all
```

As we are using Minikube - and not a Cloud Provider - to find the-internal URL exposed via the LoadBalancer:

```
minikube service the-internet --url=true
```

Use that URL for testing.

## References
| Reference | Link |
| --------- | ---- |
| Install Minikube | https://kubernetes.io/docs/setup/ |
