# Dapper Context
This library, leveraging Dapper, handles read and write operations with SSMS.

By exploiting the Generic Repository and Unit Of Work patterns, it manages to be not only high-performance, but also generic and robust since it also takes into account transactional contexts with generic data types.

The logic within this library includes three macro areas: operations via TVP and stored procedures, only via stored procedures and finally via literal queries.

# TVP
#### ExecuteStoredProcedureAsyncByTvp
It takes as input the name of the stored procedure, the name of the TVP parameter specified within the stored procedure, the name of the TVP, the list of parameters to pass to fill the TVP and finally the type of command which in any case is of the Stored Procedure type. 

**It returns a boolean** variable that certifies whether or not the correct execution has taken place.

# Stored Procedure
#### GetAllAsyncWithStoredProcedure
It takes as input the name of the stored procedure and the type of command which in any case is of the Stored Procedure type.

**It returns an IEnumerable** That is all the records obtained by the SP.

#### GetAsyncWithStoredProcedure
It takes as input the name of the stored procedure, an object (anonymous type)  and the type of command which in any case is of the Stored Procedure type.

**It returns an IEnumerable** That is all the records obtained by the SP. 

#### DeleteAsyncWithStoredProcedure
It takes as input the name of the stored procedure, an object (anonymous type)  and the type of command which in any case is of the Stored Procedure type.

**It returns a boolean** variable that certifies whether or not the correct execution has taken place.

#### AddAsyncWithStoredProcedure
It takes as input the name of the stored procedure, an object (anonymous type)  and the type of command which in any case is of the Stored Procedure type.

**It returns a boolean** variable that certifies whether or not the correct execution has taken place.

#### UpdateAsyncWithStoredProcedure
It takes as input the name of the stored procedure, an object (anonymous type)  and the type of command which in any case is of the Stored Procedure type.

**It returns a T object** that is the object modified with the new values.

# Query string
#### GetAsyncWithQuery
It takes as input, in format string, the query and the parameters.
**It returns an IEnumerable** That is all the records obtained by the query. 

#### ExecuteAsyncWithQuery
It takes as input, in format string, the query and the parameters.
**It returns a boolean** variable that certifies whether or not the correct execution has taken place.

#### UpdateAsyncWithQuery
It takes as input, in format string, the query and the parameters.

**It returns a T object** that is the object modified with the new values.