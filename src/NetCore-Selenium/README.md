# NetCore-Selenium
A raw implementation - using inline locators/selectors - to drive the pages on the http://the-internet.herokuapp.com website. 

## Framework Parameters
By default, the tests will run using the Chrome browser against http://the-internet.herokuapp.com

| Environment Variable | Default | Description |
| -------------------- | ------- | ----------- |
| THEINTERNET_BROWSERSETTINGS_FILES | chrome-default.json | Launches an incognito Chrome |
| THEINTERNET_REMOTEWEBDRIVERSETTINGS_FILES | localhost.json | Does not use a remote webdriver - launches locally |

### Advanced Parameters
The optional xxx_FILES environment variables support either a fully qualified path; or the name of a file that is expected to exist under the Runtime folder when the tests are executed; or a combination of both. 

This implementation uses the .Net Core ConfigurationBuilder() - this means configuration files can be 'overlayed' and specialized like appsettings. For example, to specify a base file and then overload just one or two properties, specify the original file and then a Json file containing the variations:

```
SET THEINTERNET_BROWSERSETTINGS_FILES=chrome-default.json;chrome-captureLogFile.json;otherspecializations
```

The .Net Core conventions for environment variable overrides are also supported. For example: to change the RemoteWebDriverSettings RemoteUri property in the JSON, do something like this:

```
SET THEINTERNET_REMOTEWEBDRIVERSETTINGS:REMOTEURI="https://localhost.com/overriddenUri"
```
