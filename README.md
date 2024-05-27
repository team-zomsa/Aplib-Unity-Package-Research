This readme should be updated later.

Short instructions:
1. Make sure git is installed on your machine.
2. With your unity project of choice opened, press on the "Window" tab on the top of the screen.
3. Press on "Package Manager".
4. Press on the "+" sign on the top left corner of the window.
5. Press on "Add package from git URL".
6. Paste the following link: `https://github.com/team-zomsa/Aplib-Unity-Package.git`
7. Press on "Add" or "Install". Wait for the package to be installed.
8. After installation, open the Test Runner tab under Window -> General -> Test Runner.
9. Go to PlayMode and press create a new test assembly, if you do not have one yet, if you do, skip this step.
10. Under Assembly Definition References, add the team-zomsa.aplib-unity.Runtime.asmdef file.
11. Under Assembly References, add Aplib.Core.dll.
12. Go to https://github.com/team-zomsa/aplib.net/wiki for instructions on how to use the library and set up your first test.
13. After setting up tests, under the Test Runner tab, press "Run All" to run all tests.

Possible errors:
- If the test scene can not be found, make sure it is added in the build settings.

# Licensing

This project has no license defined yet.

## Used packages

This package uses several packages. For the packages below, we refer you to
their respective licenses, located in the `plugins` folder:

- Castle.Core
- Moq
- System.Diagnostics.EventLog