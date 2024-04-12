Novel Approach to Webservices - Fast, Compact, Maintainable, Flexible
---------------------------------------------------------------------

1. Client & Server both share a common INTERFACE project (or cross-platform equivalant)
	- Custom Interface (e.g IPushUpgrade)
	- Methods Enum (one for each interface method)
	- Dtos, with serialisation specifics (e.g Serialisable, DataContract, or Protobuf annotations)

2a. ONE magic webservice method implements the ENTIRE interface - see IPassthru for specification
	- Blob of binary data in (function parameters, serialised)
	- Blob of binary data out (return type, serialised)
	- Enum value to specify the method to call
	
2b. Magic method also includes other 4 enums, specifing how to read/format the input/output blobs
	- Encryption {None,Xor,Rij} - App-scoped: Client/Server share common encryption key (config-driven) *alternatively can use ssl
	- Format in/out {Protobuf,Binary,DataContract,Json,Xml} - Typically consistent for a Project (but can mix)
	- Compression {None,In,Out,Both} - Varies by method (eg. Jpeg/avi does not need additional gzip layer, other dtos usually do)

2c. Magic method can be implemented using:
    - Http (HttpClient calls aspx or ashx)
	- Wcf (single method to configure)
	- Rest Api (to manage authentication, rate-limiting etc)
	- Call to an Exe (eg. C++, reading/writing binary files to exchange protobuf data)

3. Client implements the interface (client project)
	- calls generic helper method for each interface method
	- provides method name as enum (first param)
	- provides gzip settings as enum (optional second param)
	- provides list of function parameters (auto-serialised by a helper function)
	- deserialises the output into the appropriate type
	- In/out formats are typically fixed/defaulted for any given project (consistent serialisation scheme)
	- encryption key and switch are typically config-driven (ssl is an option)
	- host name typically provided to constructor at runtime, base url is often a constant
	- Async versions of methods are optional (interface is sync only), take care to ensure same gzip settings

4. Server implements the interface (server project)
	- switch statement manually resolves method enum into method calls - see ExecuteMethod(int methodName, CReader params)
	- reader deserialises the input parameters eg. p.Str, p.Bool, p.Int, p.Bool, p.Unpack(Of CFilesList)
	- return type is serialised into binary blob, and returned to client (null for Subs)
	- unhandled exceptions are serialised into an exception dto, serialised, rethrown on the client

5.  Three-tiered project scheme (collapsed into a single project for Comms.PushUpgrade example)
	- Client apps reference client project (eg Comms.PushUpgradeClient) and interface project (eg. Comms.PushUpgrade)
	- Server app references server project (eg Comms.PushUpgradeServer) and interface project (eg. Comms.PushUpgrade)
	- Both apps have config settings, and helper class (to read/write config settings) eg. encryption key/alg (or use-ssl), hostname
