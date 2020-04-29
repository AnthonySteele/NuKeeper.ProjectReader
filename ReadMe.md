# NuKeeper file system scanner

## Notes

I expect this part to be the first to stabilise.

The location of shared parts that cross the interface (e.g. `INuKeeperLogger` ) is difficult, but then it always was.

This is NetStandard 2.1 so that it can use C#8, and the null checking features.

File system could be made pluggable, if there's a need for that (e.g. use a remote repo as a file system)

Any other parts would simply use this at the public api `Reader.FindAllNuGetPackages` call, and not worry about the implementation or tests.

## Testing

Internally there of course will be tests. The plan is for test coverage to be high, but mostly not at the "one class, on test class, and mock the rest" level.

We test at the api: scanning projects on a file systems, using a set of test data projects, on a file system.
So tests are less coupled, but covering all useful cases, and still very much fast enough. Therefore easier to refactor.

Although there will be some lower level tests as well.

Needs test case examples for:

* .NET full framework project
* .vbproj, .fsproj, etc.
* Directory.Build.props include files
* multiple projects in a solution
* And some "expected bad data" cases
