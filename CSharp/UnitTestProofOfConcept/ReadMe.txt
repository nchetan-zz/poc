This solution provides a proof of concept for Unit and functional testing. The concepts can be extended further
to do End to end testing if required.

The list of projects are as follows

ProductionCode --> This is the library that needs to be tested
UnitTests --> This library contains unit tests.
Database --> Database schema. Can be deployed on top of Sql Express (can be downloaded from a link at http://www.hanselman.com/blog/DownloadSQLServerExpress.aspx)
DataAccessLayer --> This library contains data access layer abstraction on top of database project.

DatabaseACcessLayer
=====================

Interface folder contains all the interfaces that form the contract of work. This helps take dependencies over
multiple interface instead of concrete implementations

Implementation folder contains classes providing one implementation of project

ProductionCode
==============
This is a project that imitates functionality provided to user. 
DuplicateUserCommand --> Command pattern (https://en.wikipedia.org/wiki/Command_pattern) used to keep functionality of Duplicating user in a single code. This helps keeping code readable.

FunctionalTests
================

Solution contains an example of how we would do a functional test for a class (UserRepository). This class requires a database and we do use a database. Hence this
is a functional test.

UnitTestProofOfConcept
========================

