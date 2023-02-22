To demonstrate the importance of reusing a gRPC channel, three different types of clients will be set up in the application.

1- A wrapper class; where a new client object is created every time a new call is made (the channel remains active until the wrapper object is disposed of).
2- 