#!/bin/bash
## builds linux binary for local platform
## requires installation of
## mono-devel   (sudo apt-get install mono-devel)
## monodevelop  (sudo apt-get install monodevelop)


sudo rm -rf yoctobridgecalibration

mkdir --mode=755 yoctobridgecalibration/
mkdir --mode=755 yoctobridgecalibration/usr/
mkdir --mode=755 yoctobridgecalibration/usr/lib/
mkdir --mode=755 yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration

mkdir --mode=755 yoctobridgecalibration/usr/bin/

mkdir --mode=755 yoctobridgecalibration/usr/share/
mkdir --mode=755 yoctobridgecalibration/usr/share/applications
mkdir --mode=755 yoctobridgecalibration/usr/share/pixmaps
mkdir --mode=755 yoctobridgecalibration/DEBIAN

mkdir --mode=755 yoctobridgecalibration/usr/share/icons
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/16x16
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/16x16/apps
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/32x32
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/32x32/apps
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/48x48
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/48x48/apps
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor//128x128
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor//128x128/apps
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/256x256
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/256x256/apps
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/scalable
mkdir --mode=755 yoctobridgecalibration/usr/share/icons/hicolor/scalable/apps

mkdir --mode=755 yoctobridgecalibration/usr/share/doc
mkdir --mode=755 yoctobridgecalibration/usr/share/doc/yoctobridgecalibration


#mkdir yoctobridgecalibration/etc
#mkdir yocto-bridgecalibration/etc/udev
#mkdir yocto-bridgecalibration/etc/rules.d

#cp 51-yoctopuce_all.rules yocto-BridgeCalibration/etc/udev/rules.d
#chmod -R 644 yocto-bridgecalibration/etc


#copy copyright
cp copyright yoctobridgecalibration/usr/share/doc/yoctobridgecalibration
chmod 644 yoctobridgecalibration/usr/share/doc/yoctobridgecalibration/copyright
cp changelog yoctobridgecalibration/usr/share/doc/yoctobridgecalibration
gzip -n -9 yoctobridgecalibration/usr/share/doc/yoctobridgecalibration/changelog
chmod 644 yoctobridgecalibration/usr/share/doc/yoctobridgecalibration/changelog.gz

#copy debian control filE
cp control yoctobridgecalibration/DEBIAN
#cp conffiles yoctobridgecalibration/DEBIAN
chmod 644 yoctobridgecalibration/DEBIAN/*

#copy freedesktop stuff
cp YoctoBridgeCalibration.desktop yoctobridgecalibration/usr/share/applications
chmod 644 yoctobridgecalibration/usr/share/applications/YoctoBridgeCalibration.desktop
cp ../artwork/icon.svg yoctobridgecalibration/usr/share/icons/hicolor/scalable/apps/YoctoBridgeCalibration.svg
chmod 644 yoctobridgecalibration/usr/share/icons/hicolor/scalable/apps/YoctoBridgeCalibration.svg
cp icon_16.png yoctobridgecalibration/usr/share/icons/hicolor/16x16/apps/YoctoBridgeCalibration.png
chmod 644 yoctobridgecalibration/usr/share/icons/hicolor/16x16/apps/YoctoBridgeCalibration.png
cp icon_32.png yoctobridgecalibration/usr/share/icons/hicolor/32x32/apps/YoctoBridgeCalibration.png
chmod 644 yoctobridgecalibration/usr/share/icons/hicolor/32x32/apps/YoctoBridgeCalibration.png
cp icon_48.png yoctobridgecalibration/usr/share/icons/hicolor/48x48/apps/YoctoBridgeCalibration.png
chmod 644 yoctobridgecalibration/usr/share/icons/hicolor/48x48/apps/YoctoBridgeCalibration.png
cp icon_128.png yoctobridgecalibration/usr/share/icons/hicolor//128x128/apps/YoctoBridgeCalibration.png
chmod 644 yoctobridgecalibration/usr/share/icons/hicolor//128x128/apps/YoctoBridgeCalibration.png
cp icon_256.png yoctobridgecalibration/usr/share/icons/hicolor/256x256/apps/YoctoBridgeCalibration.png
chmod 644 yoctobridgecalibration/usr/share/icons/hicolor/256x256/apps/YoctoBridgeCalibration.png
cp icon_48.png yoctobridgecalibration/usr/share/pixmaps/YoctoBridgeCalibration.png
chmod 644 yoctobridgecalibration/usr/share/pixmaps/YoctoBridgeCalibration.png



# copy shell script in the path
cp Yocto-BridgeCalibration yoctobridgecalibration/usr/bin
chmod 755 yoctobridgecalibration/usr/bin/Yocto-BridgeCalibration

#copy linux libs
cp ../libyapi-amd64.so yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration
chmod 0644 yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration/libyapi-amd64.so
cp ../libyapi-armhf.so yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration/libyapi-armhf.so
chmod 0644 yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration/libyapi-armhf.so
cp ../libyapi-i386.so yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration
chmod 0644 yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration/libyapi-i386.so

#copy binary
cp  ../bin/Release/YoctoBridgeCalibration.exe yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration
#cp  YoctoBridgeCalibration.exe.config yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration
chmod 755 yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration/YoctoBridgeCalibration.exe
#chmod 644 yoctobridgecalibration/usr/lib/Yocto-BridgeCalibration/YoctoBridgeCalibration.exe.config

#set all file to root user
sudo chown -R root:root yoctobridgecalibration


dpkg-deb --build yoctobridgecalibration


lintian yoctobridgecalibration.deb