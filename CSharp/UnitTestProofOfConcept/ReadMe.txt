This solution provides a proof of concept for Unit and functional testing. The concepts can be extended further
to do End to end testing if required.

The list of projects are as follows

ProductionCode --> This is the library that needs to be tested
UnitTests --> This library contains unit tests.
Database --> Database schema. Can be deployed on top of Sql Express (can be downloaded from a link at http://www.hanselman.com/blog/DownloadSQLServerExpress.aspx)
DataAccessLayer --> This library contains data access layer abstraction on top of database project.

ProductionCode
=====================

Interface folder contains all the interfaces that form the contract of work. This helps take dependencies over
multiple interface instead of concrete implementations

Implementation folder contains classes providing one implementation of project
