# Path Not Too Long

A command line program that allows the user to delete files/folders with long path names, that normally cause the explorer to throw a PathTooLongException message to the user.

It is a command line wrapper built upon [PathTooLong](https://github.com/lski/PathTooLong) designed to work with files/directories with long paths.

### Usage

The executable is copied to the deployment folder on building the solution, all the Dlls it requires are automatically embedded inside the .exe file so only one file is required to run.

To use it, you can either drag a file or folder on the .exe file or run it from the command line. It will ask the user to confirm they want to delete the file or folder before continuing.

From the command line:

```
pathtoolong.exe "c:\temp\folder to delete"
or
pathtoolong.exe "-path:c:\temp\folder to delete"
```

To run the application silently without the warning prompt, add the "-silent" flag to the command.

```
pathtoolong.exe "c:\temp\folder to delete" -silent
```

It is possible to create a batch file that performs the drag and drop feature silently

### Requirements

Microsoft .Net v4 or above

#### Trouble shooting

As with deleting files with the normal explorer window its possible you will not have access rights to delete a file or folder. If that happens you will be informed via the console window (unless in silent mode) that you didn't have access rights, try re-running the program as an Administrator.

##### Disclaimer

This program has been tested although understand its job is to delete files, I take no responsibility for any loss of files due to its usage.
