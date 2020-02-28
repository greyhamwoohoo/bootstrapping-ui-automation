# Start Zalenium in Docker
Start Zalenium locally in Docker with  two containers running:

```
docker-compose up
```

NOTE: Be sure to allocate enough memory to Docker before launching Zalenium. Video recording is enabled by default (but disabled in the docker-compose file).

To see the Dashboard and to see videos of recorded tests:

1. docker-compose up
2. Run a test pointing the RemoteWebDriver to http://localhost:4444/wd/hub (see THEINTERNET_REMOTEWEBDRIVERSETTINGS_FILES or environment variables in the test projects for more information))
3. Browse to the Live View to see the tests running now. 
4. Browse to the Dashboard to see the results and watch the video.

After launching Zalenium, you can access the following locations:

| URL | Description |
| --- | ----------- |
| http://localhost:4444/grid/console | Zalenium Grid |
| http://localhost:4444/wd/hub | RemoteWebDriver URL for Zalenium Grid |
| http://localhost:4444/dashboard/# | Zalenium Dashboard (run at least one test or you will get a 'Forbidden' error) |
| http://localhost:4444/grid/admin/live | See a live preview of running tests |
