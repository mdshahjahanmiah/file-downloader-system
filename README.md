Language:  C#
Framework: .Net Core 3.1
Tools:
Visual Studio 2019, SQL Server Management Studio 18, NodeJS v12.16.1

#How to build and run application:
#Step 1:
Click the “FileDownloaderSystem.sln”. Make sure project is running and have loaded all projects correctly. 
 

#Step 2:
After all projects successfully loading, select single startup project as a startup project “Agoda.FileDownloaderSystem.DataAccess” and then apply and ok.
 
#Step 3:
Make sure our Data Access Project appsettings.json is open and correctly spelled connection strings is valid. Please specify here valid SQL Server Connection String where database will be created.
 

#Step 4:
As I told before our application is implemented by CQRS, we must make sure us we are running below commands for creating database (Please select in Package Manager Console Default Project Agoda.FileDownloaderSystem.DataAccess).Now time to Run Migration Command :
add-migration filedownloadersystem -Context FileDownloaderCommandsContext 
After successful migration add then run again
update-database filedownloadersystem -Context FileDownloaderCommandsContext 

 Please make sure database is correctly created in SQL Server with default seed value.
 

#Step 5:
All right! If everything is ok, then please go to solution properties again and select multiple startup project as below.
 
#Step 6: 
Now time to place all configuration value for API under “Agoda.FileDownloaderSystem.Api” appsettings.json . Please specify all credentials like below.
 

#Step 7: 
Please specify API  running hosting url in Application appsettings.json too (Under “Agoda.FileDownloaderSystem.App”). This is ReactJS front-end application.

 #Step 8: 
Yes, all are set. Now time run application and see the result. Make sure both projects API and Application are running properly. 
 
#Test Cases: 
We can test all functionality too. 
N.B. 2 Case fails here because I don’t have any valid FTP and SFTP Server.
 


#API Specification:

#API Purpose:  Download file from multiple sources and protocols to local disk.
#Method: Post
#Request Type: FromBody
#Service Consume Type : application/json
#Request URL : http://localhost:54327/FileDownloader
#Request Body: 
#{
#   "Source”: "http://www.techcoil.com/ph/img/logo.png"
#}

#Response: 
#{
#   "respCode": 200, #   "respDesc": null
#}

#API Purpose:  Validation for POST method.
#Method: Post
#Request Type: FromBody
#Service Consume Type : application/json
#Request URL : http://localhost:54327/FileDownloader
#Request Body: 
#{
#   "Source”: " ht://www.techcoil.com/ph/img/logo.png"
#}

#Response: 
#{
#    "respCode": 400,
#    "respDesc": “Please specify the url protocol.”
#}



#API Purpose:  Show list of downloads and their status.
#Method: Get
#Request URL : http://localhost:54327/FileDownloader
#Response: 
#[
#    {
        "fileId": 1,
        "source": "http://www.techcoil.com/ph/img/logo.png",
        "destination": "C:\\Hasan\\",
        "downloadStartedDate": "2020-03-11T22:35:11.5782251",
        "downloadEndedDate": "2020-03-11T22:35:11.5785616",
        "protocol": "http",
        "isLargeData": "True",
        "isSlow": "False",
        "percentageOfFailure": 0,
        "elapsedTime": 0.0003365,
        "downloadSpeed": 11.682156113600483,
        "statusId": 1,
        "protocolId": 1,
        "status": "Completed"
#    },
#    {
        "fileId": 2,
        "source": "https://nvd.nist.gov/download/nvd-rss.xml",
        "destination": "C:\\Hasan\\",
        "downloadStartedDate": "2020-03-11T22:35:10.9638792",
        "downloadEndedDate": "2020-03-11T22:35:11.513427",
        "protocol": "https",
        "isLargeData": "True",
        "isSlow": "True",
        "percentageOfFailure": 0,
        "elapsedTime": 0.5495478,
        "downloadSpeed": 0.3051422953555803,
        "statusId": 1,
        "protocolId": 2,
        "status": "Completed"
#   }
#]
