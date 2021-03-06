﻿# Overview

This is the application server for AuditLog. It serves up the user interfaces and 
the versioned event and list APIs.

# Dependencies

These are the local tools so far:

* Visual Studio 2017 
* ASP.Net Core 2
* .Net Core 2
* SQL Server

# Getting Started

* Setup your database, make sure you set snapshot isolation and name it "AuditLogDev"
* Run `UpdateLocalDB.cmd` from Database folder
* Do a build, ensure automatic `gulp build` runs successfully (look at Task Runner Explorer)

# Running

* F5 to build
* `gulp build` should run once before build, `gulp local` should run in the background the whole time

# Errors Applying DB Script

* `/database/DatabaseUpdate.sql` is the raw SQL (and safe to re-run in SSMS againt local DB)
* Fix problem in `/database/Changes/...` and re-run `UpdateLocalDB.cmd`
* Profit

# Add sample data

To add some sample data to the system:

* cd samples
* node runSample _host_ _port_ _authpair_ _numDaysToAdd_

The _authpair_ value is a colon seperated Consumer Id and Secret from the /configure/apikeys screen.

The _numDaysToAdd_ is a number of day to add to event times (negative numbers work best).

# Misc

## Customer vs Client

* Customers are people that pay money to use AuditLog
* Clients are their customers/users

## Events vs Entries

* Events are things that customers record
* Entries are things we have recorded
