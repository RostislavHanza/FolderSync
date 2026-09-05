# FolderSync

Folder synchronization console application written in C#.

## Features

- One-way synchronization from source to replica
- Periodic synchronization
- Creates missing files and directories
- Updates changed files
- Removes files and directories that no longer exist in source
- Logs operations to console and log file

## Usage

```bash
FolderSync --source <path> --replica <path> --interval <seconds> --log <filePath>