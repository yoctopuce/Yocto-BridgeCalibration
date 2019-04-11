# Yocto-BridgeCalibration

Yocto-BridgeCalibration is a C# .Net application  allowing to compute  temperature compensation for load cells in a empirical way. It is mainly designed for use with  the Yoctopuce Yocto-Bridge load cell interface. You can found more information about Yocto-BridgeCalibration inner-working on [Yoctopuce web site](https://www.yoctopuce.com/EN/article/load-cell-temperature-drift-compensation). 

![Screenshot example](https://www.yoctopuce.com/pubarchive/2019-04/compensation-error-improvement_1.png)

The application is basically a big wizard that will guide you through the 3 steps required to compute the temperature compensation parameters:
- Yocto-bridge configuration and calibration
- Data acquisition
- Compensation calculation.


Yocto-BridgeCalibration is designed to take advantage of some .NET 4.5 optimizations, but it can be compiled for .NET 3.5 if Windows XP compatibility is required. 

The Windows, Linux and MacOS installers are available on   [Yoctopuce web site](http://www.yoctopuce.com/EN/tools.php)



