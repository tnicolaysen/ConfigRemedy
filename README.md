ConfigRemedy / Configuratron (TBD)
==================================

*Continuous Delivery*, hosting in *the cloud* and *enterprise software development* tease out some interesting problems related to **configuration management**.  Configuratron tries to mitigate them.

> Configuratron lets you take back control of your configuration

TODO: Selling points here
TODO: Explain the problems

One of the _progressive ideas_ we propose is to differentiate between **environment** and **application** configuration variables. 
We aim to extend what [Octopus Deploy](http://octopusdeploy.com/) already [provides](http://docs.octopusdeploy.com/display/OD/Variables). We believe that Octopus Deploy is very well suited to handle and maintain environment variables. Environment variables are very often related to server names, database connection strings etc. Application variables, on the other hand, are variables that change the way to application behaves. E.g. `ReplyToAddress`, `EnableSendingOutsideDomain`, `UseLegacyCalculation` etc. 
In many cases, you will have different values for settings in different environments.

Goals
------

* Easy to install and deploy
* Easy to use
* Extendable
* Flexible
* Secure
* TODO: Refine

_Inspired by [Escape](https://code.google.com/p/escservesconfig/)_
