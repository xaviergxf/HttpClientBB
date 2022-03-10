# .NET HttpClient best practices

Comparing different ways of using HttpClient to better understand the problems of some approaches.


The solution contains the following projects:

## WeatherAPI
A simple static rest API with random weather previsions

## HttpClientWithDispose
Console app that fires several sequential HTTP GET requests using HttpClient class **and disposes it for every request**

## HttpClientWithStatic 
Console app that fires several sequential HTTP GET requests using a **static HttpClient class for every request**

## HttpClientWithDI
Console app that fires several sequential HTTP GET requests using a HttpClient from **dependency injection for every request**

## ConcurrentHttpReqPerf
A BenchmarkDotNet project comparing the approches above against WeatherAPI