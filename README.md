# Learning Windows Server Containers
This is the code repository for [Learning Windows Server Containers](https://www.packtpub.com/virtualization-and-cloud/learning-windows-server-containers?utm_source=github&utm_medium=repository&utm_content=9781785887932), published by Packt. It contains all the supporting project files necessary to work through the book from start to finish.

## About the Book


Windows Server Containers are independent, isolated, manageable and portable application environments which are light weight and shippable. Decomposing your application into smaller manageable components or MicroServices helps in building scalable and distributed application environments. Windows Server Containers have a significant impact on application developers, development operations (DevOps) and infrastructure management teams. Applications can be built, shipped and deployed in a fast-paced manner on an easily manageable and updatable environment.

Learning Windows Server Containers teaches you to build simple to advanced production grade container based application using Asp.Net Core, Visual Studio, Azure, Docker and PowerShell technologies. The book teaches you to build and deploy simple web applications as Windows and Hyper-V containers on Windows 10 and Windows Server 2016 on Azure. You will learn to build on top of Windows Container Base OS Images, integrate with existing images from Docker Hub, create custom images and publish to Hub. You will also learn to work with storage containers built using Volumes and SQL Server as container, create and configure custom networks, integrate with Redis Cache containers, configure continuous integration and deployment pipelines using VSTS and Git Repository. Further you can also learn to manage resources for a container, setting up monitoring and diagnostics, deploy composite container environments using Docker Compose on Windows and manage container clusters using Docker Swarm. The last chapter of the book focuses on building applications using Microsoft’s new and thinnest server platform – Nano Servers.

## Instructions and Navigations
All of the code is organized into folders. Each folder starts with a number followed by the application name. For example, Chapter2

The code will look like the following:
       
       static int ngpios;
       static int gpios[2] = { -1 , -1 };
       module_param_array(gpios, int, &ngpios, S_IRUSR | S_IWUSR | S_IRGRP | S_IWGRP);
       MODULE_PARM_DESC(gpios, "Defines the GPIOs number to be used as a list of"
                        " numbers separated by commas.");

       /* Logging stuff */
       #define __message(level, fmt, args...)                                  \
                       printk(level "%s: " fmt "\n" , NAME , ## args)

There are no code files for the following chapters:

- **Chapter 01**  - Exploring Virtualization
- **Chapter 10**  - Manage Resource Allocation and REST API

### Software requirements:

Regarding the software you should have a little knowledge of a non graphical text editor as
vi, emacs or nano. Even if you can connect an LCD display, a keyboard and a mouse
directly to embedded kits and then use the graphical interface, in this book we assume that
you is able to do little modifications to text files by using a text only editor.
The host computer, that is the computer you will use to cross-compile the code and/or to
manage your embedded systems, is assumed running a GNU/Linux based distribution. My
host PC is running an Ubuntu 15.10 but you can use also a newer Ubuntu Long Term
Support (LTS) or a Debian based system too with little modifications or you may use
another GNU/Linux distribution but with a little effort from you mainly regarding the
cross-compiling tools installation, libraries dependencies and packages management.
Foreign systems such as Windows, MacOS or similar are not covered by this book due the
fact you should not use low technology systems to develop code for high technology
system!
Knowing how a C compiler works and how to manage a Makefile is required.
This book will present some kernel programming techniques but these must cannot be
taken as a kernel programming course. You need a proper book for such topic! However each
example is well documented and you will find several suggested resources. Regarding the
kernel I'd like to state that the version used into this book is 4.4.x.
As a final note I suppose that you known how to connect a GNU/Linux based board on the
Internet in order to download a package or a generic file.

### Hardware requirements:

In this book all code is developed for BeagleBone Black board revision C, for SAMA5D3
Xplained revision A or for the WandBoard revision C1 (depending on the board used) but
you can use an older revision without any issues, in fact the code is portable and it should
work on other systems too (but the DTS files whose must be considered apart)!
Regarding the computer peripherals used in this book I reported in each chapter where I
got the hardware and where you can buy it but, of course, you can decide to surf the
Internet in order to find a better and cheaper offer. A note where to find the datasheet is
also present.
You should not have any difficulties in order to connect the hardware presented in this
book with the embedded kits since the connections are very simple and well documented.
They don't require any particular hardware skills to be performed from you (apart knowing
how to use a solder), however having a minor knowledge in electronics may help.


## Related Products:

* [Using Yocto Project with BeagleBone Black]( https://www.packtpub.com/hardware-and-creative/yocto-beaglebone?utm_source=github&utm_medium=repository&utm_content=9781785289736 )

* [ARM® Cortex® M4 Cookbook]( https://www.packtpub.com/hardware-and-creative/arm-cortex-m4-cookbook?utm_source=github&utm_medium=repository&utm_content=9781782176503 )

* [BeagleBone Home Automation Blueprints]( https://www.packtpub.com/hardware-and-creative/beaglebone-home-automation-blueprints?utm_source=github&utm_medium=repository&utm_content=9781783986026 )

* [Building Networks and Servers Using BeagleBone]( https://www.packtpub.com/hardware-and-creative/building-networks-and-servers-using-beaglebone?utm_source=github&utm_medium=repository&utm_content=9781784390204 )

### Suggestions and Feedback
[Click here]( https://docs.google.com/forms/d/e/1FAIpQLSe5qwunkGf6PUvzPirPDtuy1Du5Rlzew23UBp2S-P3wB-GcwQ/viewform ) if you have any feedback or suggestions.


