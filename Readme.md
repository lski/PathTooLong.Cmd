# Path Not Too Long

A command line program that allows the user to delete or copy files/folders with long path names, that normally cause the explorer to throw a PathTooLongException message to the user.

It is a command line wrapper built upon [PathTooLong](https://github.com/lski/PathTooLong).

### Usage

#### Delete

You can either drag a file or folder on the .exe file or run it from the command line. It will ask the user to confirm they want to delete the file or folder before continuing.

```
pathtoolong.exe "c:\temp\item to delete"
//or
pathtoolong.exe -delete "c:\temp\item to delete"
```

To run the application silently without the warning and confirmation prompt, add the `-silent` flag to the command

```
pathtoolong.exe "c:\temp\item to delete" -silent
```

#### Copy

As well as deleting files/folders you can copy them.

```
pathtoolong.exe -copy "c:\test\source" -dest "c:\test\destination"
```

By default if the destination exists the file/folder will not be copied. So to overwrite the destination add the `-overwrite` flag. If it is a file it will simply replace the destination, if it is a folder then it will merge the folders together, replacing any files that match the original folder but leaving other files in the destination untouched.

```
pathtoolong.exe -copy "c:\test\source" -dest "c:\test\destination" -overwrite
```

Like with deleting you can add the `-silent` flag so that it closes automatically when finished.

```
pathtoolong.exe -copy "c:\test\source" -dest "c:\test\destination" -overwrite -silent
```

### Build Exe

Open the soluton and build the code using the `Release` configuration. A single exe file is created and copied to the 'deployment' folder. __Note__ All dlls required by the exe are embedded directly into the exe itself, so only the exe file is needed.

### Requirements

Microsoft .Net v4 or above

#### Trouble shooting

As with deleting files with the normal explorer window its possible you will not have access rights to delete a file or folder. If that happens you will be informed via the console window (unless in silent mode) that you didn't have access rights, try re-running the program as an Administrator.

##### Disclaimer

This program has been tested although please remember one of its primary jobs is to delete files, I take no responsibility for any loss of files due to its usage.