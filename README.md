# ToDoList  
A simple console application for managing a ToDo list.  

# Installing and running the application  
## 1. Prerequisites  
.NET SDK or Runtime: Depending on whether you need to compile or just run the application.  
For building: install the .NET SDK.  
For running: install the .NET Runtime.  
Download from https://dotnet.microsoft.com/download.  
## 2. Clone this repository or download zip file  
Either use git clone https://github.com/Himske/ToDoList.git or download as a zip file from github.  
## 3. Build the application  
* Navigate to the project directory  
cd path\to\your\project  
* Restore dependencies  
dotnet restore  
* Build the project  
dotnet build -c Release  
## 4. Run the application  
Output executable will usually be in: bin\Release\netX.Y\YourApp.exe  
Double click the exe file to run the application.  

# How the application works
When starting the application it will load any previously saved tasks and then show the main menu.  
<img width="424" height="223" alt="image" src="https://github.com/user-attachments/assets/4c20c555-555a-4246-b006-072a20b899a6" />  

## 1. Show Task List  
The tasks currently in the system can be shown either by due date or by project.  
<img width="681" height="439" alt="image" src="https://github.com/user-attachments/assets/074ea9cb-d550-4a3b-9422-835e1203b02f" />  

## 2. Add New Task  
You add a new task by entering title, due date and project name.  
<img width="317" height="187" alt="image" src="https://github.com/user-attachments/assets/25c00aef-f72f-4416-aa4d-d9dea20ba7a4" />  
New tasks will always be added with status set to "Not Started".  

## 3. Edit Task  
This will give you the option to update title, due date or project name. Or change task status or remove a task.  
<img width="402" height="140" alt="image" src="https://github.com/user-attachments/assets/e0a4cafd-07f2-4af6-a6dc-ce6eb20dccf9" />  

Update task will ask for an Id and if a task is found it will be shown. When updating a task you only have to enter the updated information. If nothing is entered, the old information will be kept.  
<img width="620" height="400" alt="image" src="https://github.com/user-attachments/assets/33801f96-2a5a-462a-ba8f-befac66e6190" />

Change task status will ask for an Id and if a task is found it will be shown. A status list will be shown and you can enter a new status.  
<img width="659" height="466" alt="image" src="https://github.com/user-attachments/assets/c297e20e-4ed7-401d-8fe2-480c7d040680" />  

Remove task will ask for an Id and if a task is found it will be shown. 
<img width="662" height="351" alt="image" src="https://github.com/user-attachments/assets/87fd946b-3d89-4890-ae02-96822416f12c" />  

## 4. Save and Quit  
The tasks currently in the application will be saved to a JSON file and the program will be closed.
